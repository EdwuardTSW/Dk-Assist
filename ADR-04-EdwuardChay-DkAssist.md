# ADR-04: Integración con API REST externa para catálogo de resurtido

| Campo  | Valor |
|--------|-------|
| Autor  | Edwuard Chay |
| Fecha  | 20/06/2026 |

---

## Contexto

DkAssist gestiona proveedores internos del negocio, pero no tenía forma de consultar disponibilidad de insumos externos sin salir del sistema. Para el módulo de cotizaciones y pedidos personalizados (especialmente de playeras), el equipo necesita verificar qué productos y precios ofrece un proveedor externo antes de armar una cotización para un cliente.

Se decidió conectar el sistema con un catálogo de proveedor externo mediante una API REST. Para esta etapa del proyecto se utiliza **FakeStoreAPI** (`https://fakestoreapi.com/`) como proveedor simulado, dado que permite desarrollar y validar la integración sin depender de un proveedor real ni de credenciales externas.

---

## Decisión

Se integra el catálogo externo mediante una llamada **HTTP GET** a `https://fakestoreapi.com/products` desde el propio servidor ASP.NET Core, usando `IHttpClientFactory` inyectado en el controlador de proveedores. La respuesta JSON se deserializa con `System.Text.Json` y se presenta al usuario en una vista de tarjetas Bootstrap accesible desde el módulo de Proveedores.

Los campos mapeados del JSON son: `id`, `title`, `price`, `description`, `category` e `image`.

La integración vive en la capa de presentación porque:
- Es una consulta de solo lectura (no modifica datos propios).
- No contiene lógica de negocio — solo obtiene y muestra información externa.
- Requiere `IHttpClientFactory`, que es un servicio de infraestructura HTTP de ASP.NET Core.

---

## ¿Por qué REST?

| Criterio | Justificación |
|----------|---------------|
| **Estándar de la industria** | La mayoría de catálogos y APIs de proveedores exponen sus datos vía HTTP/REST. Adoptarlo prepara el sistema para integrarse con APIs reales en el futuro. |
| **Simplicidad** | Una llamada `GET` con `HttpClient` requiere mínimo código, sin librerías adicionales ni contratos binarios. |
| **Sin estado (stateless)** | Cada consulta al catálogo es independiente; no hay necesidad de mantener sesión ni estado entre peticiones al proveedor. |
| **Formato JSON** | Es el formato de intercambio dominante en APIs modernas y `System.Text.Json` (incluido en .NET) lo deserializa de forma eficiente y sin dependencias externas. |
| **Interoperabilidad** | Al usar HTTP estándar, la integración funciona con cualquier proveedor que exponga una API REST, facilitando el cambio de FakeStoreAPI a un proveedor real sin reescribir la arquitectura. |

---

## Alternativas consideradas

| Alternativa | Por qué la descarté |
|-------------|---------------------|
| **GraphQL** | Añade complejidad de cliente (esquemas, queries tipadas) innecesaria para una consulta simple de listado. FakeStoreAPI tampoco expone un endpoint GraphQL. |
| **gRPC** | Requiere contratos `.proto` y genera código binario; es ideal para comunicación interna de alta frecuencia entre servicios, no para consultar catálogos externos de proveedores. |
| **Scraping web** | Frágil, no escalable y técnicamente incorrecto cuando el proveedor ya ofrece una API. |
| **Base de datos del proveedor** | Acceder directamente a la BD de un proveedor externo viola principios de encapsulamiento y no es posible con proveedores reales. |
| **Importación manual (CSV/Excel)** | No es en tiempo real; requiere intervención manual cada vez que el catálogo cambia, lo que elimina el beneficio de consultar insumos actualizados. |

---

## Consecuencias

**Lo que gano:**

- El equipo puede consultar disponibilidad de insumos y precios del proveedor directamente desde DkAssist, sin salir del sistema.
- La integración es fácilmente reemplazable: cambiar FakeStoreAPI por un proveedor real solo requiere actualizar la URL base y el modelo `ProveedorProducto` si los campos cambian.
- Se establece el patrón de integración con APIs externas para futuras integraciones (pagos, envíos, etc.).

**Lo que sacrifico o asumo:**

- **Dependencia de disponibilidad externa**: si la API del proveedor cae o responde lento, la vista del catálogo muestra un error amigable, pero el usuario no puede consultar sin conexión.
- **Sin caché**: cada visita al catálogo hace una llamada nueva a la API externa. Si el proveedor tiene límites de tasa (*rate limiting*), esto podría ser un problema a escala. En esta etapa académica no se implementa caché, pero sería el siguiente paso lógico.
- **Datos simulados**: FakeStoreAPI no es un proveedor real de insumos para playeras; los datos son de prueba y no reflejan un catálogo comercial real.

---

# Diagramas

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

Para la elaboración de este ADR se utilizó IA como herramienta de apoyo para estructurar el contenido. Todas las decisiones documentadas aquí son propias y reflejan la integración real implementada en el proyecto DkAssist.
