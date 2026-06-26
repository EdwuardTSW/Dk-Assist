# DkAssist

> Sistema web de gestión para emprendedores y negocios pequeños.

---

## ¿Qué es DkAssist?

DkAssist es una aplicación web diseñada para centralizar la operación de negocios pequeños que manejan pedidos personalizados. En lugar de gestionar clientes por WhatsApp, el inventario en papel y las citas de memoria, DkAssist reúne todo en un solo lugar.

## Problema que resuelve

Muchos emprendedores pierden información, cometen errores y pierden tiempo porque no tienen un sistema que los ayude a organizarse. DkAssist atiende esa necesidad ofreciendo una plataforma simple, funcional y pensada para el día a día de un negocio real.

## Funcionalidades previstas

- Registro y seguimiento de pedidos
- Gestión de clientes y proveedores
- Control de inventario y stock de productos
- Generación de cotizaciones
- Agendamiento de citas y entregas
- Registro de pagos
- Consulta de catálogo externo de resurtido
- Registro de logs operativos en archivo

## Tecnología

- **Framework:** ASP.NET Core MVC (.NET 10)
- **Base de datos:** SQLite con Entity Framework Core
- **Arquitectura:** arquitectura en capas (`Domain`, `Application`, `Infrastructure`, `Presentation`)
- **Patrones aplicados:** Repository, Service Layer, Factory Method, Decorator y Observer
- **Resiliencia:** timeout, reintentos y fallback para el catálogo externo de proveedores
- **Logging:** proveedor propio de `ILogger` con salida persistente en `DkAssist.Presentation/logs/dkassist-logs.txt`
- **Pruebas:** xUnit, Moq y EF Core InMemory

## Estado del proyecto

🟡 En desarrollo — módulos principales implementados, arquitectura por capas documentada, patrones GOF integrados y resiliencia aplicada al catálogo externo.

## Arquitectura

DkAssist está organizado en cuatro proyectos principales:

```text
DkAssist.Domain          -> Entidades e interfaces de repositorio
DkAssist.Application     -> Servicios de aplicación, casos de uso y observadores
DkAssist.Infrastructure  -> EF Core, DbContext, repositorios y factories
DkAssist.Presentation    -> ASP.NET Core MVC, controladores, vistas, logging y servicios externos
```

Flujo principal:

```text
Controller -> Service -> Repository Interface -> Repository EF Core -> SQLite
```

## Patrones GOF y resiliencia

- **Factory Method:** `ClienteRepositoryFactory` centraliza la creación del repositorio base de clientes.
- **Decorator:** `LoggingClienteRepository` agrega trazas operativas sin modificar `ClienteRepository`.
- **Observer:** `ICitaObserver` permite reaccionar cuando una cita queda confirmada.
- **Resiliencia:** `ProveedorCatalogoClient` encapsula la integración con FakeStoreAPI usando timeout, reintentos, logs y fallback.

## Ejecución y validación

```bash
dotnet build
dotnet test
dotnet run --project DkAssist.Presentation
```

Al ejecutar la aplicación y usar módulos como clientes, citas o catálogo externo, los logs se escriben en:

```text
DkAssist.Presentation/logs/dkassist-logs.txt
```

## Documentación

[`ADR-03-EdwuardChay-DkAssist.md`](docs/ADR-03-EdwuardChay-DkAssist.md)

[`ADR-04-EdwuardChay-DkAssist.md`](docs/ADR-04-EdwuardChay-DkAssist.md)

[`ADR-05-EdwuardChay-DkAssist-Patrones-GOF-y-Resiliencia.md`](docs/ADR-05-EdwuardChay-DkAssist-Patrones-GOF-y-Resiliencia.md)

---

Desarrollado por **Edwuard Chay**
