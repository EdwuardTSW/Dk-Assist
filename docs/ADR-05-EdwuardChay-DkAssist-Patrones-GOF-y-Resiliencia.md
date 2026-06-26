# ADR-05: Integración de patrones GOF y resiliencia en DkAssist

| Campo | Valor |
|-------|-------|
| Autor | Edwuard Chay |
| Fecha | 25/06/2026 |
| Estado | Aceptado |

---

## Contexto

DkAssist ya cuenta con una arquitectura en capas documentada en el ADR-03 y con una integración REST externa para consultar el catálogo de resurtido documentada en el ADR-04. Conforme el sistema crece en módulos como clientes, citas, proveedores, pedidos, pagos y movimientos de stock, aparecen nuevas necesidades de mantenibilidad y tolerancia a fallos.

Antes de este cambio, el registro de repositorios se hacía directamente en `Program.cs`, las operaciones de clientes no dejaban trazas específicas de logging, la confirmación de una cita no tenía un mecanismo desacoplado para disparar acciones posteriores y la consulta al catálogo externo dependía de una llamada HTTP directa desde el controlador de proveedores.

Estos puntos no impedían que el sistema funcionara, pero sí hacían más difícil evolucionarlo sin mezclar responsabilidades. Por eso se decidió integrar patrones GOF y mecanismos de resiliencia reales, verificables en código y pruebas automatizadas.

---

## Decisión

Se integran tres patrones GOF en DkAssist y se refuerza la integración externa del catálogo de proveedores con resiliencia:

- **Factory Method** para centralizar la creación del repositorio base de clientes mediante `ClienteRepositoryFactory`.
- **Decorator** para agregar logging a `IClienteRepository` mediante `LoggingClienteRepository`, sin modificar `ClienteRepository`.
- **Observer** para reaccionar cuando una cita queda confirmada mediante `ICitaObserver` y observadores registrados en la capa de presentación.
- **Resiliencia HTTP** para el catálogo externo de proveedores mediante `ProveedorCatalogoClient`, con timeout, reintentos, logging y fallback controlado.
- **Logging persistente legible** mediante un proveedor propio de `ILogger`, configurable desde `appsettings.json`.

La arquitectura base se mantiene como arquitectura en capas:

```text
Presentation → Application → Domain ← Infrastructure
```

Los cambios se integran sobre esa estructura sin reemplazarla.

---

## Factory Method

### Problema

El arranque de la aplicación conocía directamente la implementación concreta del repositorio de clientes. Aunque actualmente se usa Entity Framework Core con SQLite, centralizar esa decisión evita que futuras variantes de persistencia obliguen a modificar varias partes de la aplicación.

### Decisión aplicada

Se creó `IClienteRepositoryFactory` y `ClienteRepositoryFactory` en `DkAssist.Infrastructure/Factories`. La fábrica crea la implementación base `ClienteRepository` usando el `DkAssistDbContext` configurado por inyección de dependencias.

### Evidencia en código

- `DkAssist.Infrastructure/Factories/IClienteRepositoryFactory.cs`
- `DkAssist.Infrastructure/Factories/ClienteRepositoryFactory.cs`
- `DkAssist.Presentation/Program.cs`
- `DkAssist.Tests/Infrastructure.Tests/ClienteRepositoryFactoryTests.cs`

### Justificación

Factory Method permite encapsular la creación del repositorio. Si en el futuro se requiere otra implementación para clientes, la decisión queda centralizada en la fábrica y no mezclada en controladores o servicios.

---

## Decorator

### Problema

Agregar logs directamente dentro de `ClienteRepository` mezclaría dos responsabilidades: persistencia y trazabilidad. El repositorio debe seguir enfocado en acceder a datos, no en decidir cómo registrar operaciones.

### Decisión aplicada

Se creó `LoggingClienteRepository`, que implementa `IClienteRepository`, recibe un repositorio interno y agrega logs antes o después de delegar cada operación.

El flujo real queda así:

```text
ClienteService
→ IClienteRepository
→ LoggingClienteRepository
→ ClienteRepository
→ DkAssistDbContext
```

### Evidencia en código

- `DkAssist.Infrastructure/Repositories/LoggingClienteRepository.cs`
- `DkAssist.Presentation/Program.cs`
- `DkAssist.Tests/Infrastructure.Tests/LoggingClienteRepositoryTests.cs`

### Justificación

Decorator permite agregar comportamiento sin modificar la clase original. En este caso, el comportamiento agregado son logs verificables de consulta, creación, actualización y eliminación de clientes.

---

## Observer

### Problema

Cuando una cita queda confirmada, pueden ejecutarse varias acciones relacionadas: preparar una notificación por correo, preparar una notificación SMS o actualizar métricas del dashboard. Si `CitaService` llamara directamente a cada acción concreta, quedaría acoplado a detalles de presentación o infraestructura.

### Decisión aplicada

Se creó la interfaz `ICitaObserver` en la capa de aplicación y se registraron observadores concretos en la capa de presentación:

- `EmailCitaObserver`
- `SmsCitaObserver`
- `DashboardCitaObserver`

Cuando `CitaService.ActualizarAsync` recibe una cita con `Estado = "Confirmada"`, persiste la actualización y notifica a todos los observadores registrados.

### Evidencia en código

- `DkAssist.Application/Observers/ICitaObserver.cs`
- `DkAssist.Application/Services/CitaService.cs`
- `DkAssist.Presentation/Observers/EmailCitaObserver.cs`
- `DkAssist.Presentation/Observers/SmsCitaObserver.cs`
- `DkAssist.Presentation/Observers/DashboardCitaObserver.cs`
- `DkAssist.Presentation/Program.cs`
- `DkAssist.Tests/Application.Tests/CitaServiceObserverTests.cs`

### Justificación

Observer permite que `CitaService` publique el evento de confirmación sin depender directamente de cada acción posterior. Así se pueden agregar nuevos observadores sin reescribir el servicio principal.

---

## Resiliencia en catálogo externo

### Problema

El ADR-04 documentó la integración con FakeStoreAPI para consultar productos externos. Esa integración funcionaba, pero la llamada HTTP estaba en el controlador y solo capturaba errores de solicitud HTTP. Faltaba manejar mejor fallos temporales, lentitud del proveedor y respuestas no exitosas.

### Decisión aplicada

Se creó `ProveedorCatalogoClient` en la capa de presentación para encapsular la llamada externa con:

- Timeout configurable.
- Reintentos configurables.
- Logs por intento y por fallback.
- Fallback controlado con lista vacía y mensaje amigable.
- Configuración en `appsettings.json` bajo la sección `ProveedorCatalogo`.

El controlador `ProveedorController` deja de construir la llamada HTTP directamente y delega esa responsabilidad en `IProveedorCatalogoClient`.

### Evidencia en código

- `DkAssist.Presentation/Services/IProveedorCatalogoClient.cs`
- `DkAssist.Presentation/Services/ProveedorCatalogoClient.cs`
- `DkAssist.Presentation/Services/ProveedorCatalogoOptions.cs`
- `DkAssist.Presentation/Services/ProveedorCatalogoResult.cs`
- `DkAssist.Presentation/Controllers/ProveedorController.cs`
- `DkAssist.Presentation/appsettings.json`
- `DkAssist.Tests/Presentation.Tests/ProveedorCatalogoClientTests.cs`

### Justificación

La integración externa es el punto más natural para aplicar resiliencia porque depende de un proveedor fuera del control de DkAssist. Con timeout, reintentos y fallback, la vista de catálogo puede seguir respondiendo de forma controlada aunque la API externa falle.

---

## Logging legible en archivo

### Problema

Los logs generados por los patrones y la resiliencia eran visibles en consola, pero no quedaban persistidos para evidencia posterior. Para la actividad se requiere que los cambios sean comprobables, por lo que era necesario conservar las trazas de ejecución en un archivo revisable y fácil de leer.

### Decisión aplicada

Se creó un proveedor propio de `ILogger` que escribe logs en formato de texto legible. La configuración vive en `appsettings.json` bajo la sección `CsvLogging` y permite activar/desactivar el logger, definir la ruta del archivo y establecer el nivel mínimo.

El archivo configurado por defecto es:

```text
logs/dkassist-logs.txt
```

El archivo contiene líneas con este estilo:

```text
[2026-06-23 08:26:37] ObtenerTodos - inicio
[2026-06-23 08:26:37] ObtenerTodos - 4 registros
[SMS] Recordatorio enviado al cliente 2 - cita el 01/06/2026 a las 10:00
[EMAIL] Confirmación enviada al cliente 2 - motivo: Revisión - estado: Confirmada
```

### Evidencia en código

- `DkAssist.Presentation/Logging/CsvLoggerOptions.cs`
- `DkAssist.Presentation/Logging/CsvLoggerProvider.cs`
- `DkAssist.Presentation/Logging/CsvLogger.cs`
- `DkAssist.Presentation/Program.cs`
- `DkAssist.Presentation/appsettings.json`
- `DkAssist.Tests/Presentation.Tests/CsvLoggerTests.cs`

### Justificación

Persistir logs en un archivo legible permite comprobar operaciones reales del sistema sin depender únicamente de la consola. También facilita entregar evidencia de ejecución para clientes, citas, observadores y resiliencia del catálogo externo.

---

## Alternativas consideradas

| Alternativa | Por qué se descartó |
|-------------|---------------------|
| Registrar directamente `ClienteRepository` en `Program.cs` | Funciona, pero mantiene la creación de la implementación concreta dispersa en el arranque de la aplicación. |
| Agregar logs dentro de `ClienteRepository` | Mezcla persistencia con trazabilidad y dificulta reutilizar el repositorio sin logging. |
| Llamar notificaciones directamente desde `CitaService` | Aumenta el acoplamiento entre el caso de uso de citas y acciones concretas como email, SMS o dashboard. |
| Mantener la llamada HTTP en `ProveedorController` | El controlador queda con demasiada responsabilidad y la resiliencia es más difícil de probar aisladamente. |
| Agregar una librería externa de resiliencia | Para el alcance actual, `HttpClient`, `CancellationToken`, configuración y pruebas cubren la necesidad sin añadir otra dependencia. |
| Guardar logs solo en consola | Sirve durante desarrollo, pero no deja evidencia persistente para auditoría o entrega académica. |
| Agregar Serilog o NLog | Son opciones maduras, pero para el alcance actual un proveedor propio de `ILogger` mantiene el cambio pequeño, visible y fácil de explicar. |

---

## Consecuencias

**Lo que gano:**

- El proyecto demuestra patrones GOF reales y verificables en el flujo de DkAssist.
- La creación de repositorios de clientes queda centralizada.
- El logging de clientes queda separado del repositorio base.
- La confirmación de citas puede disparar acciones desacopladas.
- El catálogo externo tolera mejor fallos temporales y proveedores lentos.
- Los logs quedan persistidos en un archivo legible para revisión posterior.
- Los cambios tienen pruebas automatizadas que sirven como evidencia funcional.

**Lo que sacrifico o asumo:**

- Se agregan más clases e interfaces al proyecto.
- El flujo de resolución de `IClienteRepository` es más indirecto por el uso conjunto de Factory Method y Decorator.
- Los observadores actuales simulan acciones mediante logs; integraciones reales de email, SMS o dashboard requerirían implementaciones posteriores.
- La resiliencia implementada usa reintentos simples; si el sistema creciera, podría evaluarse una política más avanzada con una librería especializada.
- El logger escribe en archivo local; en despliegues productivos con varias instancias habría que evaluar almacenamiento centralizado de logs.

---

## Validación

Los cambios se validan con pruebas automatizadas específicas:

- `ClienteRepositoryFactoryTests` comprueba que la fábrica crea un repositorio de clientes real.
- `LoggingClienteRepositoryTests` comprueba que el decorador delega en el repositorio interno.
- `CitaServiceObserverTests` comprueba que los observadores se notifican solo cuando la cita está confirmada.
- `ProveedorCatalogoClientTests` comprueba reintentos ante fallos temporales y fallback cuando todos los intentos fallan.
- `CsvLoggerTests` comprueba creación del archivo, formato legible, mensajes con prefijo y filtro por nivel/categoría.

También deben ejecutarse las verificaciones generales del proyecto:

```bash
dotnet build
dotnet test
```

---

## Declaración de uso de IA

Para la elaboración de este ADR se utilizó inteligencia artificial como apoyo para estructurar la documentación y revisar la correspondencia entre los patrones GOF y los cambios implementados. Las decisiones documentadas reflejan cambios reales realizados en el proyecto DkAssist y verificables en código, configuración y pruebas automatizadas.
