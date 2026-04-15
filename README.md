# 🐾 DogSitter API

> Microservicio RESTful para la gestión de un servicio de cuidado de perros, construido con **.NET 8 Minimal APIs** y arquitectura hexagonal.

---

## 📑 Tabla de contenido

- [Arquitectura](#-arquitectura)
- [Stack Tecnológico](#️-stack-tecnológico)
- [Estructura del proyecto](#-estructura-del-proyecto)
- [Recursos de la API](#-recursos-de-la-api)
- [Configuración local](#️-configuración-local)
- [Pruebas](#-pruebas)
- [CI/CD](#-cicd--github-actions)

---

## 📐 Arquitectura

![Arquitectura Multicloud](img/Arquitectura_Multicloud.jpeg)

Los principales patrones y estilos de arquitectura que guían este proyecto son:

### Puertos y Adaptadores (Hexagonal)

La aplicación (Dominio) es el centro del sistema. Todas las entradas y salidas alcanzan o dejan el dominio a través de un **puerto**. Este puerto aisla el dominio de las tecnologías externas, herramientas y mecánicas de entrega.

El dominio nunca tiene conocimiento de quién envía o recibe la entrada/salida, lo que le permite estar asegurado contra la evolución de la tecnología y los requerimientos del negocio.

### CQRS (Command Query Responsibility Segregation)

Patrón con el cual se divide el modelo de objetos en dos responsabilidades:

| Tipo | Descripción |
|---|---|
| **Query** | Presenta datos en la interfaz; los objetos se modelan según lo que se va a mostrar, no según la lógica de negocio 
| **Command** | Operaciones que cambian el estado del sistema (registrar, editar, eliminar) |

### Especificaciones técnicas destacadas

- ✅ **EF Core 8 Code First** con migraciones versionadas
- ✅ **Repositorio Genérico** (`GenericRepository<T>`) para manejo de agregados
- ✅ **Shadow Properties** — propiedades de infraestructura sin contaminar las entidades de dominio
- ✅ **Inyección automática** de Domain Services (`[DomainService]`) y repositorios (`[Repository]`) via reflexión
- ✅ **MediatR** — registra handlers de Commands y Queries automáticamente via scan del assembly
- ✅ **ValidationBehavior** — pipeline de validación con FluentValidation integrado en MediatR
- ✅ **Manejador de Excepciones Global** via middleware
- ✅ **Listo para contenerizar** con Docker

### Capas del proyecto

| Proyecto | Responsabilidad |
|---|---|
| `DogSitter.Api` | API REST, punto de entrada, Swagger, Middleware, Prometheus |
| `DogSitter.Api.Tests` | Pruebas de integración para la API REST |
| `DogSitter.Application` | Orquestación de servicios de dominio; Commands, Queries, Handlers |
| `DogSitter.Domain` | Entidades, Servicios de Dominio, Ports, DTOs, Excepciones |
| `DogSitter.Domain.Tests` | Pruebas unitarias para servicios de dominio y entidades |
| `DogSitter.Infrastructure` | EF Core, DataContext, Adapters y Migraciones |

---

## 🛠️ Stack Tecnológico

| Tecnología | Versión | Uso |
|---|---|---|
| **.NET** | 8 | Framework principal |
| **PostgreSQL + Npgsql** | — | Base de datos relacional |
| **Entity Framework Core** | 8.0.11 | ORM, migraciones, Code First |
| **MediatR** | — | Patrón CQRS |
| **FluentValidation** | — | Validación de DTOs |
| **Swagger / OpenAPI** | — | Documentación interactiva + auth JWT |
| **xUnit** | — | Pruebas unitarias y de integración |
| **NSubstitute** | — | Mocking en pruebas unitarias |
| **EF Core InMemory** | 8.0.11 | Base de datos en memoria para tests |

---

## 📁 Estructura del proyecto

```
DogSitter/
├── 📂 .github/
│   └── workflows/
│       ├── ci-prb.yml          # CI rama develop (quality gate 60%)
│       ├── cd-prb.yml          # Deploy a PRB en Render
│       ├── ci-prd.yml          # CI rama main (quality gate 85%)
│       └── cd-prd.yml          # Deploy a PRD en Render
├── 📂 UDEM.DEVOPS.DogSitter.Api/
│   ├── ApiHandlers/            # Endpoints Minimal API (Cuidador, Perro, Raza)
│   ├── Filters/                # ValidationFilter para FluentValidation
│   ├── Middleware/             # AppExceptionHandlerMiddleware
│   └── Program.cs
├── 📂 UDEM.DEVOPS.DogSitter.Application/
│   ├── Cuidador/Commands|Queries|Handlers
│   ├── Perro/Commands|Queries|Handlers
│   └── Raza/Commands|Queries|Handlers
├── 📂 UDEM.DEVOPS.DogSitter.Domain/
│   ├── Entities/               # Cuidador, Perro, Raza, DomainEntity
│   ├── Services/               # Servicios de dominio con [DomainService]
│   ├── Ports/                  # Interfaces de repositorios
│   ├── Dtos/                   # DTOs de entrada y salida
│   ├── Mappings/               # Extensiones de mapeo entre entidades y DTOs
│   └── Exceptions/             # NotFoundEntityException, DeleteRestrictionException
├── 📂 UDEM.DEVOPS.DogSitter.Infrastructure/
│   ├── Adapters/               # GenericRepository<T>, CuidadorRepo, PerroRepo, RazaRepo
│   ├── DataSource/             # DataContext, ModelConfig (Fluent API)
│   ├── Extensions/             # AutoLoadServices (reflexión)
│   └── Migrations/
├── 📂 UDEM.DEVOPS.DogSitter.Domain.Tests/
└── 📂 UDEM.DEVOPS.DogSitter.Api.Tests/
```

---

## 🐶 Recursos de la API

### Cuidador — `/api/cuidador`

| Método | Endpoint | Descripción |
|---|---|---|
| `GET` | `/api/cuidador` | Lista todos los cuidadores |
| `GET` | `/api/cuidador/{id}` | Obtiene un cuidador por ID |
| `POST` | `/api/cuidador` | Registra un nuevo cuidador |
| `PUT` | `/api/cuidador` | Actualiza un cuidador completo |
| `PATCH` | `/api/cuidador` | Actualiza parcialmente un cuidador |
| `DELETE` | `/api/cuidador/{id}` | Elimina un cuidador *(falla si tiene perros asociados)* |

### Perro — `/api/perro`

| Método | Endpoint | Descripción |
|---|---|---|
| `GET` | `/api/perro` | Lista todos los perros |
| `GET` | `/api/perro/{id}` | Obtiene un perro por ID |
| `POST` | `/api/perro` | Registra un nuevo perro |
| `PUT` | `/api/perro` | Actualiza un perro completo |
| `PATCH` | `/api/perro` | Actualiza parcialmente un perro |
| `DELETE` | `/api/perro/{id}` | Elimina un perro |

### Raza — `/api/raza`

| Método | Endpoint | Descripción |
|---|---|---|
| `GET` | `/api/raza` | Lista todas las razas |
| `GET` | `/api/raza/{id}` | Obtiene una raza por ID |
| `POST` | `/api/raza` | Registra una nueva raza |
| `PUT` | `/api/raza` | Actualiza una raza completa |
| `PATCH` | `/api/raza` | Actualiza parcialmente una raza |
| `DELETE` | `/api/raza/{id}` | Elimina una raza *(falla si tiene perros asociados)* |

### Observabilidad

| Endpoint | Descripción |
|---|---|
| `/` | Swagger UI (documentación interactiva) |
| `/healthz` | Health check de la aplicación y la base de datos |
| `/metrics` | Métricas Prometheus |

---

## ⚙️ Configuración local

### Prerrequisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/) corriendo localmente

### 1. Clonar el repositorio

```bash
git clone https://github.com/FlorezZ1025/DogSitter.git
cd DogSitter
```

### 2. Configurar la cadena de conexión

Edita `UDEM.DEVOPS.DogSitter.Api/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
	"db": "Host=localhost;Port=5432;Database=dogsitter;Username=postgres;Password=tu_password"
  }
}
```

O define la variable de entorno (recomendado para producción/Docker):

```bash
export DB_CONNECTION_STRING="Host=localhost;Port=5432;Database=dogsitter;Username=postgres;Password=tu_password"
```

### 3. Aplicar migraciones

```bash
dotnet ef database update \
  --project UDEM.DEVOPS.DogSitter.Infrastructure \
  --startup-project UDEM.DEVOPS.DogSitter.Api
```

### 4. Ejecutar la API

```bash
cd UDEM.DEVOPS.DogSitter.Api
dotnet run
```

La documentación Swagger estará disponible en `http://localhost:<puerto>` directamente en la raíz.

---

## 🧪 Pruebas

```bash
dotnet test --configuration Release \
  --collect:"XPlat Code Coverage" \
  --results-directory ./coverage \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.ExcludeByFile="**/Migrations/*.cs"
```

| Suite | Proyecto | Herramientas |
|---|---|---|
| Unitarias (dominio) | `DogSitter.Domain.Tests` | xUnit + NSubstitute |
| Integración (API) | `DogSitter.Api.Tests` | xUnit + EF InMemory |

---

## 🚀 CI/CD — GitHub Actions

El proyecto tiene pipelines automáticos separados por ambiente:

| Workflow | Rama | Trigger | Quality Gate |
|---|---|---|---|
| `ci-prb.yml` | `develop` | push / PR | ≥ **60%** cobertura de líneas |
| `cd-prb.yml` | `develop` | CI-PRB exitoso | Deploy a **PRB** en Render |
| `ci-prd.yml` | `main` | push / PR | ≥ **85%** cobertura de líneas |
| `cd-prd.yml` | `main` | CI-PRD exitoso | Deploy a **PRD** en Render |

### Flujo completo

```
Push develop ──► CI-PRB (build + test + coverage ≥ 60%) ──► CD-PRB (Render PRB)

Push main    ──► CI-PRD (build + test + coverage ≥ 85%) ──► CD-PRD (Render PRD)
```

---

## 👤 Autor - Santiago Arango Flórez

Desarrollado como proyecto de la materia **DevOps** en 
**Universidad de Medellín (UDEM)**