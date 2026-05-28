# ADR-01: Arquitectura base de DkAssist

| Campo  | Valor |
|--------|-------|
| Autor  | Edwuard Chay |
| Fecha  | 15/05/2025 |
| Estado | `Propuesto` |

---

## Contexto

DkAssist es un sistema web de gestión diseñado para emprendedores y dueños de negocios pequeños que manejan pedidos personalizados, como venta de ropa, artesanías o productos por encargo. El problema central es que estos negocios operan de forma desorganizada: llevan sus pedidos por WhatsApp, su inventario en papel y sus citas de entrega de memoria, lo que genera errores, pérdidas de información y falta de control financiero.

El sistema busca centralizar en un solo lugar: registro de clientes, gestión de pedidos, control de inventario, generación de cotizaciones, administración de proveedores y agendamiento de entregas.

Las restricciones que influyeron en las decisiones fueron: trabajo individual, tiempo limitado por carga académica y laboral, conocimiento previo en C# y .NET, y la necesidad de entregar un sistema funcional al cierre del cuatrimestre.

---

## Decisión

Se adopta ASP.NET Core como tecnología base para el desarrollo del sistema, dado que es el framework principal que se maneja en el curso y con el que tengo experiencia previa en C#.

### ¿Por qué?

- ASP.NET Core es un framework web maduro y bien documentado que permite construir sistemas de gestión como DkAssist de forma estructurada. Su ecosistema cubre desde la lógica del servidor hasta la conexión con bases de datos, lo que evita depender de múltiples tecnologías externas desde el inicio.

- La decisión de usar .NET también responde a una restricción práctica: es la tecnología que conozco y que el proyecto académico contempla, lo que reduce el riesgo de invertir tiempo aprendiendo algo nuevo en lugar de construir.

- Decisiones sobre base de datos, patrón de acceso a datos y arquitectura interna se definirán en etapas posteriores, una vez que los requerimientos del sistema estén más claros.

### Alternativas consideradas

| Alternativa | Por qué la descarté |
|-------------|---------------------|
| ASP.NET Core Web API + React | Requiere mantener dos proyectos separados (backend y frontend), lo que duplica la complejidad de configuración y despliegue para un proyecto individual con tiempo limitado. |
| Blazor Server | Es una tecnología más reciente con menos recursos de apoyo disponibles. La curva de aprendizaje en el contexto de este cuatrimestre representa un riesgo para la entrega. |
| MySQL en lugar de SQL Server | SQL Server tiene integración nativa más madura con el ecosistema .NET y con Entity Framework Core, lo que reduce fricciones durante el desarrollo. |
| ADO.NET sin ORM | Escribir SQL manual para todas las entidades de DkAssist (9 modelos con relaciones) consumiría demasiado tiempo y aumentaría el riesgo de errores en consultas complejas. |

---

## Consecuencias

**Lo que gano:**

- **Técnico:** La arquitectura MVC con Repository permite agregar nuevas entidades o módulos (por ejemplo, reportes o notificaciones) sin reescribir el código existente. Cada capa tiene una responsabilidad clara, lo que hace el sistema más fácil de depurar y extender.

- **Proceso:** Al usar Entity Framework Core con migraciones, los cambios en el modelo de datos se aplican de forma controlada y documentada. Esto facilita trabajar de forma iterativa sin perder el estado de la base de datos entre sesiones de desarrollo.

**Lo que sacrifico o asumo:**

- **Limitación técnica:** Las vistas Razor de MVC generan HTML en el servidor, lo que limita la capacidad de construir interfaces altamente interactivas o en tiempo real (como actualizaciones de estado sin recargar la página) sin agregar JavaScript adicional.

- **Deuda o riesgo:** Entity Framework Core puede generar consultas SQL ineficientes en escenarios complejos con múltiples joins. Si DkAssist escala a un volumen alto de pedidos, será necesario revisar y optimizar consultas críticas manualmente o implementar vistas almacenadas en SQL Server.

---

## Diagrama

<img width="1168" height="1128" alt="Untitled-2026-05-13-0720" src="https://github.com/user-attachments/assets/b40350ad-758f-4da8-9a84-d9294d084cfc" />

