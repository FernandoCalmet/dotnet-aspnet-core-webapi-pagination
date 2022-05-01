# C# ASPNET Core 6 Web API Pagination

[![Github][github-shield]][github-url]
[![Kofi][kofi-shield]][kofi-url]
[![LinkedIn][linkedin-shield]][linkedin-url]
[![Khanakat][khanakat-shield]][khanakat-url]

## 🔥 ACERCA DEL PROYECTO

Este proyecto es una muestra de paginación, utilizando ASP.NET Core 6 Web API.

## ⚙️ INSTALACIÓN

Clonar el repositorio.

```bash
gh repo clone FernandoCalmet/dotnet-6-aspnet-core-webapi-pagination
```

Migración de datos

```bash
dotnet ef database update
```

Generar una nueva migración en el directorio de migraciones

```bash
dotnet ef migrations add NewMigration --context ApplicationDbContext --output-dir Data/Migrations/
```

## 📓 RESUMEN TEÓRICO

### ¿Qué es paginación/paginación? ¿Por qué es importante?

Imagine que tiene un punto final en su API que potencialmente podría devolver millones de registros con una sola solicitud. Digamos que hay cientos de usuarios que explotarán este punto final solicitando todos los datos de una sola vez al mismo tiempo. Esto casi mataría a su servidor y provocaría varios problemas, incluida la seguridad.

Un punto final de API ideal permitiría a sus consumidores obtener solo una cantidad específica de registros de una sola vez. De esta forma, no estamos dando carga a nuestro Servidor de Base de Datos, la CPU en la que se aloja la API, o el ancho de banda de la red. Esta es una característica muy importante para cualquier API. especialmente las API públicas.

> Paginación o paginación en un método en el que obtiene una respuesta de paginación. Esto significa que solicita un número de página y un tamaño de página, y ASP.NET Core WebApi devuelve exactamente lo que solicitó, nada más.

Al implementar la Paginación en sus API, sus Desarrolladores Front-end se sentirían muy cómodos construyendo IU que no se retrasen. Dichas API son buenas para la integración por parte de otros consumidores (MVC, aplicaciones React.js) ya que los datos ya vienen paginados.

### Configuración del proyecto

Para este tutorial, trabajaremos en una WebAPI ASP.NET Core 3.1 junto con Entity Framework Core que incluye un controlador de cliente que devuelve todos los datos y datos por ID de cliente.

Saltaré hacia adelante y llegaré a la parte donde tengo un controlador que puede devolver todos los clientes (`../api/customer/`) y también devolver un cliente por id (`../api/customer/{id}`).

```csharp
public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Contact { get; set; }
    public string Email { get; set; }
}
```

Después de hacer todas las migraciones requeridas y actualizar mi base de datos, todavía me falta una parte crucial. Los datos del cliente😀 Usualmente `generatedata.com` para generar datos de muestra para demostraciones rápidas. Genera fragmentos de inserción SQL / XlS / recopilación de datos con formato CSV. A continuación se explica cómo utilizar esta práctica utilidad.

Después de insertar los datos en la tabla del cliente, ejecutemos la aplicación. Usaré el navegador Chrome para probar los datos, ya que es suficiente para nuestro escenario.

### Envoltorios para puntos finales de API

Siempre es una buena práctica agregar envoltorios a su respuesta API. ¿Qué es un envoltorio? En lugar de solo devolver los datos en la respuesta, tiene la posibilidad de devolver otros parámetros como mensajes de error, estado de respuesta, número de página, datos, tamaño de página, etc. Tú entiendes. Entonces, en lugar de devolver `List<Customer>`, devolveremos `Response<List<Customer>>`. Esto nos daría más flexibilidad y datos con los que trabajar, ¿verdad?

Cree una nueva clase, `Wrappers/Response.cs`.

```csharp
public class Response<T>
{
    public Response()
    {
    }
    public Response(T data)
    {
        Succeeded = true;
        Message = string.Empty;
        Errors = null;
        Data = data;
    }
    public T Data { get; set; }
    public bool Succeeded { get; set; }
    public string[] Errors { get; set; }
    public string Message { get; set; }
}
```

Esta es una clase contenedora bastante sencilla. Puede mostrarle el estado, los mensajes o errores si los hay, y los datos en sí (T). Así es como idealmente querría exponer sus puntos finales de API. Modifiquemos nuestro método `CustomerController/GetById`.

```csharp
[HttpGet("{id}")]
public async Task<IActionResult> GetById(int id)
{
    var customer = await context.Customers.Where(a => a.Id == id).FirstOrDefaultAsync();
    return Ok(new Response<Customer>(customer));
}
```

La línea 4 obtiene el registro del cliente de nuestra base de datos para una identificación en particular.
Línea 5 Devuelve una nueva clase contenedora con datos del cliente.

Puede ver lo útil que es este tipo de enfoque. Response.cs será nuestra clase base. Ahora, desde nuestra API, tenemos 2 posibilidades de respuestas, datos paginados (Lista de Clientes) o un solo registro sin datos paginados (Cliente por Id).

Ampliaremos la clase base agregando propiedades de paginación. Cree otra clase, `Wrappers/PagedResponse.cs`.

```csharp
public class PagedResponse<T> : Response<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public Uri FirstPage { get; set; }
    public Uri LastPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public Uri NextPage { get; set; }
    public Uri PreviousPage { get; set; }

    public PagedResponse(T data, int pageNumber, int pageSize)
    {
        this.PageNumber = pageNumber;
        this.PageSize = pageSize;
        this.Data = data;
        this.Message = null;
        this.Succeeded = true;
        this.Errors = null;
    }
}
```

Esas son muchas propiedades con las que trabajar. Tenemos tamaño de página, número, Uris de la primera página, última página, recuento total de páginas y mucho más. Comencemos a trabajar en nuestro controlador de clientes.

### Controlador de clientes – GetAll

```csharp
[HttpGet]
public async Task<IActionResult> GetAll()
{
    var response = await context.Customer.ToListAsync();
    return Ok(response);
}
```

Así es como se veía nuestro CustomerController . Estaremos modificando este método para acomodar la paginación. Para empezar, idealmente necesitaríamos los parámetros de página requeridos en la cadena de consulta de la solicitud, por lo que la solicitud sería `https://localhost:44312/api/customer? número de página = 3 y tamaño de página = 10` . Llamaremos a este modelo como PaginationFilter.

### Filtro de paginación

Crear una nueva clase, `Filter/PaginationFilter.cs`

```csharp
public class PaginationFilter
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public PaginationFilter()
    {
        this.PageNumber = 1;
        this.PageSize = 10;
    }
    public PaginationFilter(int pageNumber, int pageSize)
    {
        this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
        this.PageSize = pageSize > 10 ? 10 : pageSize;
    }
}
```

La línea 12 establece que el número de página mínimo siempre se establece en 1.
Línea 13: para esta demostración, configuraremos nuestro filtro de modo que el tamaño de página máximo que un usuario puede solicitar sea 10. Si solicita un tamaño de página de 1000 , por defecto volvería a 10.

Agreguemos este filtro a nuestro método de controlador.

```csharp
[HttpGet]
public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
{
    var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
    var response = await context.Customer.ToListAsync();
    return Ok(response);
}
```

Línea 2: lea la cadena de consulta en la solicitud de propiedades de filtro de página.
Línea 3: valida el filtro a un objeto de filtro válido (de manera predeterminada, vuelve a los valores permitidos). Ps, probablemente querrás usar un mapeador aquí. Pero este enfoque actual está bien para nuestra guía.

### Paginación con Entity Framework Core

Esta es la función central de toda la implementación, la paginación real. Es bastante fácil con EFCore. En lugar de consultar la lista completa de datos de la fuente. EFCore hace que sea muy fácil consultar solo un conjunto particular de registros, ideal para la paginación. Así es como lo lograrías.

```csharp
var pagedData = await context.Customers
               .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
               .Take(validFilter.PageSize)
               .ToListAsync();
```

La línea 1 accede a la tabla de clientes.
Línea 2 Salta un determinado conjunto de registros, por el número de página * tamaño de página.
Línea 3 Solo toma la cantidad requerida de datos, establecida por tamaño de página.

Modifique el controlador como se muestra a continuación.

```csharp
[HttpGet]
public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
{
    var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
    var pagedData = await context.Customers
        .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
        .Take(validFilter.PageSize)
        .ToListAsync();
    var totalRecords = await context.Customers.CountAsync();
    return Ok(new PagedResponse<List<Customer>>(pagedData, validFilter.PageNumber, validFilter.PageSize));
}
```

Línea 9: contaremos los registros totales para su uso posterior.
Línea 10: envuelve los datos paginados en nuestro contenedor PagedResponse.

¡Genial! Ya hemos implementado la paginación básica en nuestra API ASP.NET Core. Intentemos solicitar con un tamaño de página superior a 10.

El valor predeterminado es 10 😀Ahora, comencemos a agregar algunas funciones avanzadas como la URL de la página siguiente, etc.

### Generación de URL de paginación

#### ¿Qué son las URL de paginación?

Las URL de paginación ayudan al consumidor a navegar a través de los datos disponibles de API Endpoint con mucha facilidad. Los enlaces de la primera página, la última página, la página siguiente y la página anterior suelen ser las URL de paginación. Aquí hay una respuesta de muestra.

```csharp
"firstPage": "https://localhost:44312/api/customer?pageNumber=1&pageSize=10",
"lastPage": "https://localhost:44312/api/customer?pageNumber=10&pageSize=10",
"nextPage": "https://localhost:44312/api/customer?pageNumber=3&pageSize=10",
"previousPage": "https://localhost:44312/api/customer?pageNumber=1&pageSize=10",
```

Para implementar esto en nuestro proyecto, necesitaremos un servicio que tenga una sola responsabilidad, construir URL basadas en el filtro de paginación pasado. Llamémoslo UriService.

Cree una nueva interfaz, Services/IUriService.cs

```csharp
public interface IUriService
{
    public Uri GetPageUri(PaginationFilter filter, string route);
}
```

En esta interfaz, tenemos una definición de función que toma el filtro de paginación y una cadena de ruta (api/cliente). PD, tendremos que construir dinámicamente esta cadena de ruta, ya que la estamos construyendo de manera que pueda ser utilizada por cualquier controlador (Producto, Factura, Proveedores, etc., etc.) y en cualquier host (localhost, api.com, etc.) ). ¡La codificación limpia y eficiente es en lo que debe concentrarse!😀

Agregue una clase concreta, `Services/UriServics.cs` para implementar la interfaz anterior.

```csharp
public class UriService : IUriService
{
    private readonly string _baseUri;
    public UriService(string baseUri)
    {
        _baseUri = baseUri;
    }
    public Uri GetPageUri(PaginationFilter filter, string route)
    {
        var _enpointUri = new Uri(string.Concat(_baseUri, route));
        var modifiedUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "pageNumber", filter.PageNumber.ToString());
        modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());
        return new Uri(modifiedUri);
    }
}
```

Línea 3: obtendremos la URL base (localhost, api.com, etc.) en esta cadena a través de la inyección de dependencia de la clase de inicio. Lo mostraré más adelante en este artículo.

Línea 10: crea un nuevo Uri a partir del uri base y la cadena de ruta. (`api.com + /api/customer = api.com/api/customer`)
Línea 11: mediante la clase QueryHelpers (integrada), agregamos una nueva cadena de consulta, "pageNumber" a nuestro Uri. (`api.com/api/customer?pageNumber={i}`)
Línea 12: de manera similar, agregamos otra cadena de consulta, "pageSize"

### Configuración del servicio para obtener la URL base

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(
            Configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
    services.AddHttpContextAccessor();
    services.AddSingleton<IUriService>(o =>
    {
        var accessor = o.GetRequiredService<IHttpContextAccessor>();
        var request = accessor.HttpContext.Request;
        var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
        return new UriService(uri);
    });
    services.AddControllers();
}
```

Aquí obtenemos la URL base de la aplicación (http(s)://www.api.com) de la solicitud HTTP y el contexto.

Ahora que nuestra clase de servicio está lista, usemos esto en una clase de ayuda para generar los enlaces de punto final requeridos.

### Ayudante de paginación

Dado el hecho de que nuestro código de controlador está aumentando bastante con el tiempo, agreguemos una nueva clase auxiliar de paginación para que podamos segregar el código mucho mejor.

Agregue una nueva clase, Helpers/PaginationHelper.cs . En esta clase, tendremos una función estática que tomará parámetros y devolverá una nueva PagedResponse<List<T>> donde T puede ser cualquier clase. Reutilización de código, ¿recuerdas?

```csharp
public static PagedResponse<List<T>> CreatePagedReponse<T>(List<T> pagedData, PaginationFilter validFilter, int totalRecords, IUriService uriService, string route)
{
    var respose = new PagedResponse<List<T>>(pagedData, validFilter.PageNumber, validFilter.PageSize);
    var totalPages = ((double)totalRecords / (double)validFilter.PageSize);
    int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
    respose.NextPage =
        validFilter.PageNumber >= 1 && validFilter.PageNumber < roundedTotalPages
        ? uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber + 1, validFilter.PageSize), route)
        : null;
    respose.PreviousPage =
        validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= roundedTotalPages
        ? uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber - 1, validFilter.PageSize), route)
        : null;
    respose.FirstPage = uriService.GetPageUri(new PaginationFilter(1, validFilter.PageSize), route);
    respose.LastPage = uriService.GetPageUri(new PaginationFilter(roundedTotalPages, validFilter.PageSize), route);
    respose.TotalPages = roundedTotalPages;
    respose.TotalRecords = totalRecords;
    return respose;
}
```

Línea 3: toma los datos paginados de EFCore, el filtro, el recuento total de registros, el objeto de servicio URI y la cadena de ruta del controlador. (/api/customer/)
Línea 5: inicializa el objeto de respuesta con los parámetros necesarios.
Línea 6-7 Algunas funciones matemáticas básicas para calcular el total de páginas. (total de registros/tamaño de página)

Línea 8: tenemos que generar la URL de la página siguiente solo si existe una página siguiente, ¿no? Verificamos si el número de página solicitado es menor que el total de páginas y generamos el URI para la página siguiente. si el número de página solicitado es igual o mayor que el número total de páginas disponibles, simplemente devolvemos nulo.
Línea 12 – Del mismo modo, generamos la URL de la página anterior.
Línea 16-17: generamos URL para la primera y la última página utilizando nuestro URIService.
Línea 18-19: configuración de la página total y los registros totales para el objeto de respuesta.
Línea 20: devuelve el objeto de respuesta.

Ahora hagamos los últimos cambios en nuestro controlador. Inicialmente tendremos que inyectar el IUriService al constructor de CustomerController.

```csharp
private readonly ApplicationDbContext context;
private readonly IUriService uriService;
public CustomerController(ApplicationDbContext context, IUriService uriService)
{
    this.context = context;
    this.uriService = uriService;
}
[HttpGet]
public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
{
    var route = Request.Path.Value;
    var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
    var pagedData = await context.Customers
        .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
        .Take(validFilter.PageSize)
        .ToListAsync();
    var totalRecords = await context.Customers.CountAsync();
    var pagedReponse = PaginationHelper.CreatePagedReponse<Customer>(pagedData, validFilter, totalRecords, uriService, route);
    return Ok(pagedReponse);
}
```

Línea 6 – Inyectando el objeto IUriService al constructor.
Línea 11: mencioné que necesitamos obtener la ruta del método de acción del controlador actual (api/cliente). Request.Path.Value lo hace por usted. Es esta cadena la que vamos a pasar a nuestro método de clase auxiliar. Supongo que puede haber mejores formas de generar la ruta de la solicitud actual. Házmelo saber en la sección de comentarios.
Línea 18: llama a la clase Helper con los parámetros necesarios.
Línea 19: devuelve la respuesta paginada.

Eso es todo con el desarrollo! 😀Construyamos nuestra aplicación y ejecútela.

Hemos solicitado la primera página, es decir, número de página = 1, es decir, la URL de la página anterior es nula, como esperábamos. Vayamos a la última página de este punto final y verifiquemos. Con la extensión JSONFormatter para Chrome, es fácil navegar a través de estos datos. Simplemente haga clic en la URL de paginación y ¡funciona!

## 📄 LICENCIA

Este proyecto está bajo la Licencia (Licencia MIT) - mire el archivo [LICENSE](LICENSE) para más detalles.

## ⭐️ DAME UNA ESTRELLA

Si esta Implementación le resultó útil o la utilizó en sus Proyectos, déle una estrella. ¡Gracias! O, si te sientes realmente generoso, [¡Apoye el proyecto con una pequeña contribución!](https://ko-fi.com/fernandocalmet).

<!--- reference style links --->
[github-shield]: https://img.shields.io/badge/-@fernandocalmet-%23181717?style=flat-square&logo=github
[github-url]: https://github.com/fernandocalmet
[kofi-shield]: https://img.shields.io/badge/-@fernandocalmet-%231DA1F2?style=flat-square&logo=kofi&logoColor=ff5f5f
[kofi-url]: https://ko-fi.com/fernandocalmet
[linkedin-shield]: https://img.shields.io/badge/-fernandocalmet-blue?style=flat-square&logo=Linkedin&logoColor=white&link=https://www.linkedin.com/in/fernandocalmet
[linkedin-url]: https://www.linkedin.com/in/fernandocalmet
[khanakat-shield]: https://img.shields.io/badge/khanakat.com-brightgreen?style=flat-square
[khanakat-url]: https://khanakat.com