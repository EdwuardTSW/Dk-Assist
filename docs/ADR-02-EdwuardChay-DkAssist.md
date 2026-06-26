# ADR-02: Vistas arquitectónicas de DkAssist

| Campo  | Valor |
|--------|-------|
| Autor  | Edwuard Chay |
| Fecha  | 04/06/2026 |


---

## Contexto

DkAssist es un sistema web de gestión para emprendedores y negocios pequeños, construido con ASP.NET Core MVC, Entity Framework Core y SQLite. El sistema está organizado en cuatro capas: Domain, Application, Infrastructure y Presentation.

En esta etapa del proyecto es necesario documentar cómo se ve el sistema desde cuatro perspectivas distintas — funcional, estructural, de ejecución y de infraestructura — para que cualquier persona involucrada pueda entender su parte sin necesitar conocer el sistema completo.

Las restricciones que influyeron en estas decisiones son: proyecto individual, tiempo limitado por carga académica y laboral, y la necesidad de mantener una arquitectura simple pero escalable.

---

## Decisión

Se adopta el modelo de **cuatro vistas arquitectónicas** para documentar DkAssist:

- **Vista lógica** — muestra los módulos funcionales del sistema y sus responsabilidades
- **Vista de desarrollo** — muestra la organización del código en capas y proyectos
- **Vista de procesos** — muestra el flujo de una operación relevante en tiempo de ejecución
- **Vista de despliegue** — muestra dónde y cómo correrá el sistema en infraestructura real

### ¿Por qué?

Un solo diagrama no puede responderle a todas las personas involucradas en un sistema. El emprendedor que usa DkAssist necesita entender qué hace el sistema. El desarrollador que lo mantiene necesita entender cómo está organizado el código. El arquitecto necesita entender cómo fluyen los procesos. El administrador necesita saber dónde desplegarlo.

Documentar las cuatro vistas garantiza que cada audiencia tenga su respuesta sin necesitar leer todo el sistema.

### Alternativas consideradas

| Alternativa | Por qué la descarté |
|-------------|---------------------|
| Documentar solo el diagrama C4 | El C4 cubre bien el contexto y los contenedores, pero no documenta explícitamente los flujos de proceso ni la infraestructura de despliegue planeada. |
| Un solo diagrama de arquitectura general | Un diagrama único intenta responder todo a la vez y termina siendo demasiado complejo para cualquier audiencia. |
| No documentar vistas y solo mantener el código | El código no comunica intención arquitectónica — alguien nuevo no sabría por dónde arrancar ni cómo está pensado el sistema. |

---

## Consecuencias

** Lo que gano:**

- **Técnico:** Tener las cuatro vistas documentadas facilita agregar nuevos módulos al sistema porque cualquier desarrollador puede entender la estructura sin leer todo el código. También sirve como referencia cuando el sistema escale y sea necesario tomar nuevas decisiones arquitectónicas.

- **Proceso:** La documentación de vistas obliga a pensar el sistema de forma completa — no solo el código sino también dónde va a vivir y cómo va a comportarse en ejecución. Eso reduce decisiones improvisadas más adelante.

** Lo que sacrifico o asumo:**

- **Limitación técnica:** Las vistas documentadas en esta etapa reflejan el sistema en su estado inicial. Conforme DkAssist crezca, las vistas deberán actualizarse — si no se mantienen al día, pierden utilidad como documentación.

- **Deuda o riesgo:** La vista de despliegue documenta una infraestructura planeada pero no implementada todavía. Existe el riesgo de que la implementación real difiera de lo documentado si cambian las restricciones de tiempo o presupuesto.

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

Para la elaboración de este ADR y los diagramas de vistas arquitectónicas se utilizó Claude (Anthropic) como herramienta de apoyo para estructurar el contenido. Todas las decisiones arquitectónicas documentadas aquí son propias y reflejan el diseño real del proyecto DkAssist.
