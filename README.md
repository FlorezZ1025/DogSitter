Este bloque contiene la estructura necesaria para construir un proyecto con net8 de tipo Domain Centric (Hex, Clean, Onion).

Los principales patrones y estilos de arquitectura que guían este bloque son

## Arquitectura de Puertos y Adaptadores
La idea de Puertos y Adaptadores es que la aplicación(Dominio) sea el centro del sistema. Todas las entradas y salidas alcanzan o dejan el dominio a traves de un puerto. Este puerto aisla el dominio de las tecnologias externas, herramientas y mecánicas de entrega.

El dominio mismo nunca deberia tener ningún conocimiento de quien envía o recibe la entrada y salida. Esto le permite al sistema estar asegurado contra la evolución de la tecnología y los requerimientos del negocio.

Más información [aquí](https://www.thinktocode.com/2018/07/19/ports-and-adapters-architecture/)

## CQRS (Commmand Query Responsability Segregation):
Patrón con el cual dividimos nuestro modelo de objetos en dos, un modelo para consulta(Query) y un modelo para comando(Command). Este patrón es recomendado cuando se va desarrollar lógica de negocio compleja porque nos ayuda a separar las responsabilidades y a mantener un modelo de negocio consistente.

* Consulta: modelo a través del cual se divide la responsabilidad para presentar datos en la interfaz de usuario, los objetos se modelan basado en lo que se va a presentar y no en la lógica de negocio, ejm: ver facturas, consultar clientes. Para las consultas en esta plantilla usamos Dapper.
* Comando: son todas las operaciones que cambian el estado del sistema, ejm: (facturar, aplicar descuento), este modelo se construye todo el modelo de objetos basado en la lógica de negocio de la aplicación. Las operaciones de cambio de estado del sistema las hacemos a traves de EntityFrameworkCore.

Más información [aquí](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs)

## HealthCheck
Health checks son expuestos por una aplicación como endpoints http. Los endpoints pueden ser configurados para una variedad de escenarios de monitoreo en tiempo real:

Pruebas de Salud : pueden ser usados por orquestadores de contenedores y balanceadores de carga para verificar el estado de salud de una aplicacion.
Uso de memoria, disco, y otros recursos fisicos del servidor que pueden ser monitoreados por estado de saludable.
Health checks pueden testear dependencias de una aplicación, como bases de datos y servicios externos para confirmar la disponibilidad de los mismos.
Más información [aquí](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-3.1)

## Especificaciones técnicas:
* Plantilla de microservicios con Net8 (Top level statements, minimal apis, mapgroup - for endpoints,  global usings, records, more...)
* Listo para contenerizar con Docker
* Entity Framework Core 8(MSSql: database + schema) Code First 
* FluentValidation
* Dapper 
* Repositorio Genérico (muy útil con el manejo de agregados) y extendido (usado para ocultar caracteristicas no necesarias del generico)
* Shadow Properties en entidades : Propiedades de que se añaden a las entidades de dominio sin "envenenar" la definicion propia de la entidad en esa capa
* Inyeccion automática de Domain Services usando anotacion "[DomainService]" y de repositorios "[Repository]"
* MediaTR : registra manejadores de commands y queries de forma automática (via reflexion hace scan del assembly)
* Manejador de Excepciones Global
* Pruebas Unitarias (Domain) con Xunit
* NSubstitute para Mocking
* Pruebas de Integración (Api) con XUnit
* Logs : disco y ElasticSearch
* Swagger
* HealthCheck (para base de datos, endpoint "/healthz") 
* Ejemplo de Comand + Query + Handlers
* Exposición de metricas con prometheus

### Estructura del proyecto:
Solucion para VisualStudio(.sln) compuesta de los siguientes carpetas :

* Api : Api Rest, punto de entrada de la aplicación
* Api.Tests : Integration Tests para la Api Rest
* Application : Capa de orquestacion de servicios de dominio; Ports, Commands, Queries, Handlers
* Infrastructure : Adapters
* Domain : Entities, Value Objects, Ports, Domain Services, Aggregates
* Domain.Tests : Unit Tests para Domain Services

Estructura del proyecto

Importar el proyecto:
Para empezar a usar la plantilla solo se debe instalar el paquete de Nuget con el siguiente comando : 

> dotnet new install xm-net-hex-api

Solo se debe instalar una sola vez, y para crear una nueva solucion : 

> dotnet new xm-net-hex-api -S [Sigla de la aplicación registrado en INAP] -N [Nombre o sigla para el proyecto de código] -U [Parámetro para el uso de autorización]

Este template contiene migraciones que debe ejecutar en caso de generar entidades partiendo de modelado con Code First, actualice el appsettings.json del proyecto api con la cadena de conexión a su sql server.

## Nuget de Auditoría:
La solución cuenta con un paquete nuget instalado que puede registrar eventos de Auditoria o Monitoreo de la aplicación según se requiera.

La documentación que se presenta a continuación, tiene como complemento la [documentación inicial del Nuget de Auditoria](https://dev.azure.com/XM-Mercado/WIKI%20EQUIPO%20TI/_wiki/wikis/Wiki%20XM/2932/Nuget-Auditor%C3%ADa). 

### Registro eventos Auditoría - Configuración local
Para hacer uso del Nuget de Auditoría tal que el Api Key de consumo del servicio sea almacenado localmente se deben de seguir los siguientes pasos:

1. Varificar que al appsettings.config cuente con los siguientes campos > _BCT:IdAplicacion_, _BCT:Auditoria:Servicio_, _BCT:Auditoria:Modulo_.

 Los cuales deben estar configurados de la siguiente manera: 

```
"BCT": {
  "IdAplicacion": "{ID_APLICACION}",
  "Auditoria": {
    "Servicio": "{URL_SERVICIO_AUDITORIA}",
    "Modulo": "{MODULO_APLICACION}"
  }
}
```
En donde:

* {ID_APLICACION}: Obligatorio. Es el ID de la aplicación a la cual se referencian todos los logs guardados.
* {URL_SERVICIO_AUDITORIA}: Obligatorio. Nombre del módulo o componente de la aplicación de INAP, los logs quedarán asociados a dicho módulo.
* {MODULO_APLICACION}: Opcional. Nombre del módulo o componente de la aplicación de INAP, los logs quedarán asociados a dicho módulo.

Un ejemplo de una configuración de pruebas es el siguiente 

```
"BCT": {
  "IdAplicacion": 225,
  "Auditoria": {
    "Servicio": "https://api-prb.xm.com.co/bct/",
    "Modulo": "Modulo_test"
  }
}
```

2. En el program.cs se debe agregar la siguiente configuración:

```
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuditoriaAdapterSupport(config);
```

3. En los secretos _secrets.json_, se debe de agregar el Api Key del servicio de auditoría, según el ambiente que se esté consumiendo

```
{
  "BCTAuditoriaApimKey": "SECRETO-API-KEY"
}
```

4. Hacer uso del método que expone el Nuget para registrar las trazas: _RegistrarAuditoria()_ usando la interfaz _IAuditoriaAdapter_

```
auditoriaAdapter.RegistrarAuditoria(traza);
```
Donde _traza_ es el parámetro que se quiere registrar en el componente de auditoría.

### Registro eventos Auditoría - Configuración Key Vault con Identidad Administrada
Para hacer uso del Nuget de Auditoría tal que el Api Key de consumo del servicio sea almacenado en el Key Vaukt de Azure, se deben de seguir los siguientes pasos:
* Tener instalado el paquete _Azure.Extensions.AspNetCore.Configuration.Secrets_ 

* Configurar los parámetros "IdentidadAdministradaClientId" y "BCT:KeyVault" en el _appsettings.json_ de la siguiente manera:

```
{
  "BCT": {
  "KeyVault": "{NOMBRE-DEL-KEY-VAULT-APLICACION}",
  "IdAplicacion": "{ID_APLICACION}",
  "Auditoria": {
    "Servicio": "{URL_SERVICIO_AUDITORIA}",
    "Modulo": "{MODULO_APLICACION}"
    }
  },
  "IdentidadAdministradaClientId": "{ID_IDENTIDAD_ADMINISTRADA}"
}
```

El parámetro _ID_IDENTIDAD_ADMINISTRADA_ representa el Identificador de la identidad administrada que se va a usar para acceder al keyvault, por lo cual debe tener permisos de lectura de secretos sobre este recurso.

El parámetro {NOMBRE-DEL-KEY-VAULT-APLICACION}: Representa el nombre del key vault de la aplicación.

Con esta configuración, si el secreto no está almacenado localmente, el sistema lo busca en el Key Vault que haya sido configurado.

### Registro eventos Auditoría - Uso del componente

Un ejemplo del uso del componente de auditoría se puede observar a continuación, en donde se inyecta la interfaz _IAuditoriaAdapter_ y se usa el método expuesto _RegistrarAuditoria()_

```
public static class VoterApi
{
    public static RouteGroupBuilder MapVoter(this IEndpointRouteBuilder routeHandler)
    {       
        routeHandler.MapGet("/{id}", async (IMediator mediator, Guid id, IAuditoriaAdapter auditoriaAdapter) =>
        {
            await auditoriaAdapter.RegistrarAuditoria("Traza a registrar");

            return Results.Ok(await mediator.Send(new VoterQuery(id)));
        })
        .Produces(StatusCodes.Status200OK, typeof(VoterDto));         

        return (RouteGroupBuilder)routeHandler;
    }
}
```

## Registro eventos Monitoreo - Configuración ILogger
Una vez configurado el componente de Auditoría, podemos agregar la configuración para el ILogger que también nos enviará trazas de información al contenedor de Monitoreo. En caso de que se requiera configurar, seguir [la guía de configuración](https://dev.azure.com/XM-Mercado/WIKI%20EQUIPO%20TI/_wiki/wikis/Wiki%20XM/2932/Nuget-Auditor%C3%ADa?anchor=configuraci%C3%B3n-y-uso-de-ilogger-personalizado-para-monitoreo).

**Notas Adicionales**
* Ya este proyecto ha pasado por integración continua y análisis de codigo estático en Sonar, con las exclusiones válidas y los tests al codigo demo incuido tiene un nivel de coverage aceptable. 
* Al usar esta plantilla ustede debe encargarse de eliminar todo el codigo asociado a los demos y producir el suyo y las pruebas para estos.
* Esta plantilla incluye un pipeline de CI para AzureDevOps, por favor ajustela a sus necesidades en caso de usar esta plataforma en su ALM.
* Como parte de las politicas de la compañía, en el pipeline se incluyen la tarea de ejecución de test de mutacion [Stryker](https://stryker-mutator.io/docs/) , solo debe cerciorarse que estas se ejecuten. En esencia, como lo describe el pipeline : 
1. Se debe instalar stryker en el pipeline 
   > update dotnet-stryker --tool-path $(Agent.BuildDirectory)/tools
2. Se ejecutan las pruebas 
   > dotnet stryker --reporters "['html','json']"

## Configuración de sonar
Para configurar SonarQube correctamente, es necesario reemplazar los valores de projectKey y projectName en la configuración del proyecto con los valores correspondientes. Por ejemplo: Xm.XmDogSitter.XmDogWalker

## Nuget de Autorización:
Si el proyecto fue construido con la opción de **UsarAutorizacion** se adicionará a la solución un paquete nuget para el uso de autorización en los endpoints del api

La documentación que se presenta a continuación tiene como complemento la [documentación inicial del Nuget de Autorizacion]([Manual de Uso](https://dev.azure.com/XM-Mercado/WIKI%20EQUIPO%20TI/_wiki/wikis/Wiki%20XM/2933/Nuget-Autorizaci%C3%B3n#)). 

### Configuración 
Para hacer uso del Nuget de Autorización se deben de seguir los siguientes pasos:

1. Verificar que al appsettings.config cuente con los siguientes campos > _BCT:Autorizacion:Servicio_

Ejemplo: 

```
"BCT": {
  "Autorizacion": {
    "Servicio": "{URL_SERVICIO_AUTORIZACION}",
  }
}
```
En donde:

* {URL_SERVICIO_AUTORIZACION}: Obligatorio. Url del api de servicio de autorización.

Un ejemplo de una configuración de pruebas es el siguiente 

```
"BCT": {
  "Autorizacion": {
    "Servicio": "https://api-prb.xm.com.co/bct/",
  }
}
```

2. En los secretos _secrets.json_, se debe de agregar el Api Key del servicio de autorización, según el ambiente que se esté consumiendo

```
{
  "BCTAutorizacionApimKey": "SECRETO-API-KEY"
}
```

3. Hacer uso del método que expone el Nuget para restringir el uso de un endpoint o controlador

en el siguiente ejemplo vemos un minimal api la cual tiene una restricción adicionada por medio del metodo **AgregarAutorizacionPorPermiso** 

```
private const string PERMISO_LECTURA = "Read voter";

public static RouteGroupBuilder MapVoter(this IEndpointRouteBuilder routeHandler)
{
    routeHandler.MapGet("/{id}", async (IMediator mediator, Guid id) =>
    {
        return Results.Ok(await mediator.Send(new VoterQuery(id)));
    })
    .Produces(StatusCodes.Status200OK, typeof(VoterDto))
    .AgregarAutorizacionPorPermiso(PERMISO_LECTURA);

    return (RouteGroupBuilder)routeHandler;
}
```
para mayor información sobre los métodos para el uso de autorización [ir](https://dev.azure.com/XM-Mercado/WIKI%20EQUIPO%20TI/_wiki/wikis/Wiki%20XM/2933/Nuget-Autorizaci%C3%B3n#)

