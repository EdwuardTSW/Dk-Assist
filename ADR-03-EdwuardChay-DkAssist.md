# ADR-03: Estilo arquitectónico de DkAssist

| Campo  | Valor |
|--------|-------|
| Autor  | Edwuard Chay |
| Fecha  | 12/06/2026 |

---

## Contexto

DkAssist es un sistema web de gestión para emprendedores y negocios pequeños que manejan pedidos personalizados (clientes, productos, cotizaciones, pedidos, pagos, proveedores, movimientos de stock). Está construido con ASP.NET Core MVC, Entity Framework Core y SQLite.

Desde las primeras etapas del proyecto, el código se organizó en cuatro proyectos: `DkAssist.Domain`, `DkAssist.Application`, `DkAssist.Infrastructure` y `DkAssist.Presentation`. Cada módulo (Cliente, Producto, Proveedor, Pedido, Cotización, MovimientoStock) sigue el mismo patrón: modelo de dominio → interfaz de repositorio → repositorio → servicio → controlador/vistas, con pruebas unitarias en cada capa.

Ahora corresponde formalizar y justificar esa organización como una decisión de estilo arquitectónico, considerando las restricciones del proyecto: desarrollo individual, tiempo limitado por carga académica y laboral, y un dominio del problema esencialmente transaccional (CRUD sobre entidades de negocio).

---

## Decisión

Se adopta el **estilo de arquitectura en capas (layered architecture)** como estilo arquitectónico principal de DkAssist, organizado en cuatro capas con dependencias en una sola dirección:

```
Presentation → Application → Domain ← Infrastructure
```

- **Domain**: modelos de negocio (Cliente, Pedido, Producto, etc.) e interfaces de repositorio. No depende de ninguna otra capa.
- **Application**: servicios que implementan la lógica de negocio, dependen de Domain.
- **Infrastructure**: implementa las interfaces de Domain (repositorios) y maneja el acceso a datos vía EF Core/SQLite.
- **Presentation**: controladores y vistas MVC, depende de Application e Infrastructure (esta última solo para inyección de dependencias en `Program.cs`).

---

## ¿Por qué este estilo?

- **Separación de responsabilidades**: la lógica de negocio (Domain/Application) queda independiente de cómo se almacenan los datos (Infrastructure) y de cómo se presentan (Presentation). Esto permite cambiar la base de datos o la interfaz sin reescribir las reglas de negocio.
- **Facilita las pruebas**: al depender de interfaces (`IClienteRepository`, `IPedidoRepository`, etc.) en lugar de implementaciones concretas, los servicios pueden probarse de forma aislada, como ya ocurre en `DkAssist.Tests`.
- **Escala bien con el patrón ya usado**: cada nuevo módulo (Etapas 2 a 7) replicó el mismo patrón de capas sin friction, lo que confirma que el estilo es sostenible para seguir creciendo.
- **Proporcional al alcance del proyecto**: DkAssist es un sistema de gestión para un solo negocio, con un solo desarrollador y un solo entorno de despliegue. Un estilo simple, predecible y fácil de mantener es más valioso que uno que añada complejidad operativa sin necesidad real.

---

## Alternativas consideradas

| Alternativa | Por qué la descarté |
|-------------|---------------------|
| **Microservicios** | Sobreingeniería para un proyecto individual con una sola base de datos SQLite y un solo despliegue. Añadiría complejidad de comunicación entre servicios, infraestructura adicional (orquestación, descubrimiento de servicios) sin ningún beneficio real a este tamaño de proyecto. |
| **Arquitectura hexagonal / Clean Architecture estricta** | Comparte el espíritu de separar el dominio del resto del sistema, pero impone más ceremonia (puertos y adaptadores explícitos, mapeos adicionales) que no aporta valor adicional dado el tamaño del proyecto. La arquitectura en capas ya logra la separación necesaria con menos overhead. |
| **Cliente-servidor simple (2 capas: UI + base de datos)** | No separa la lógica de negocio de la interfaz ni de los datos. A medida que se agregan módulos (cotizaciones, pagos, stock), las reglas de negocio quedarían mezcladas con controladores o consultas SQL directas, dificultando las pruebas y el mantenimiento. |
| **Arquitectura basada en eventos (event-driven)** | El dominio del problema (gestión de clientes, pedidos, inventario) es esencialmente transaccional y CRUD, no requiere comunicación asíncrona entre componentes ni reacción a eventos en tiempo real. Adoptar este estilo introduciría infraestructura de mensajería innecesaria. |

---

## Consecuencias

**Lo que gano:**

- **Técnico**: cada capa puede evolucionar de forma relativamente independiente — por ejemplo, cambiar de SQLite a otro motor de base de datos solo afectaría a `DkAssist.Infrastructure`, sin tocar Domain ni Application.
- **Proceso**: el patrón ya probado en las Etapas 2-7 (modelo → interfaz → repositorio → servicio → controlador → vistas + tests) sirve como plantilla repetible para futuros módulos, reduciendo el tiempo de diseño en cada nueva funcionalidad.

**Lo que sacrifico o asumo:**

- **Limitación técnica**: la arquitectura en capas puede generar cierto código repetitivo (boilerplate) al añadir un módulo nuevo, ya que cada entidad requiere su propio modelo, interfaz, repositorio, servicio y controlador.
- **Deuda o riesgo**: si el sistema creciera mucho más allá del alcance actual (por ejemplo, múltiples negocios o necesidad de procesamiento asíncrono pesado), este estilo podría volverse limitante y requerir una revisión arquitectónica posterior.

---

## Diagramas

### Vista lógica — ¿qué hace el sistema?

<img width="622" height="427" alt="Vista lógica drawio" src="https://github.com/user-attachments/assets/12b32e22-899b-4738-b2c9-a0ff62108523" />

---

### Vista de desarrollo — ¿cómo está organizado el código?

```
DkAssist.sln
├── DkAssist.Domain          ← Cliente, Pedido, Producto, Cotizacion, Pago, Cita, Proveedor, IClienteRepository, IPedidoRepository...
├── DkAssist.Application     ← ClienteService, PedidoService, ProductoService, CotizacionService...
├── DkAssist.Infrastructure  ← DkAssistDbContext, ClienteRepository, PedidoRepository, ProductoRepository...
└── DkAssist.Presentation    ← ClienteController, PedidoController, ProductoController... Views, Program.cs
```

---

### Vista de procesos — ¿cómo se comporta en ejecución?

<img width="531" height="551" alt="Vista de procesos drawio" src="https://github.com/user-attachments/assets/caa93fb8-9164-4e31-bf08-a1387711d0f9" />

---

### Vista de despliegue — ¿dónde vive físicamente?

<img width="542" height="932" alt="Vista de despliegue drawio" src="https://github.com/user-attachments/assets/49ffe850-fb55-4a44-a71d-adf881a5843b" />

> **Nota:** El entorno de producción está planeado pero no implementado todavía.

---

## Declaración de uso de IA

Para la elaboración de este ADR y el diagrama del estilo arquitectónico se utilizó Claude (Anthropic) como herramienta de apoyo para estructurar el contenido. Todas las decisiones arquitectónicas documentadas aquí son propias y reflejan el diseño real del proyecto DkAssist.
