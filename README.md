# C# ASPNET Core Web API Pagination

[![Github][github-shield]][github-url]
[![Kofi][kofi-shield]][kofi-url]
[![LinkedIn][linkedin-shield]][linkedin-url]
[![Khanakat][khanakat-shield]][khanakat-url]

## 🔥 ABOUT THE PROJECT
This project is a demonstration of pagination using ASP.NET Core Web API with NET 8.

## ⚙️ INSTALLATION
Clone the repository.

```bash
gh repo clone FernandoCalmet/dotnet-aspnet-core-webapi-pagination
```

Data migration:

```bash
dotnet ef database update
```
Generate a new migration in the migrations directory:

```bash
dotnet ef migrations add NewMigration --context ApplicationDbContext --output-dir Data/Migrations/
```

## 📓 THEORETICAL OVERVIEW
What is Pagination? Why is it Important?
Imagine having an API endpoint that could return millions of records in a single request. Suppose hundreds of users hit this endpoint simultaneously, requesting all the data at once. This could severely overload your server, causing various problems, including security risks.

An ideal API endpoint should allow its consumers to retrieve only a specific number of records at a time. By doing so, we avoid overloading our Database Server, the CPU hosting the API, and network bandwidth. This feature is crucial for any API, especially public ones.

Pagination is a method where you receive a paginated response. This means you request a page number and a page size, and the ASP.NET Core WebApi returns precisely what you asked for, nothing more.

By implementing Pagination in your APIs, Front-end Developers will find it comfortable to build UIs that aren't laggy. Such APIs are great for integration by other consumers (MVC, React.js apps) as the data is already paginated.

## 📄 LICENSE
This project is under the MIT License - see the [LICENSE](LICENSE) file for more details.

## ⭐️ STAR THE PROJECT
If you found this implementation helpful or used it in your projects, consider giving it a star. Thank you! Or, if you're feeling extra generous, support the project with a [small contribution!](https://ko-fi.com/fernandocalmet).

<!--- reference style links --->
[github-shield]: https://img.shields.io/badge/-@fernandocalmet-%23181717?style=flat-square&logo=github
[github-url]: https://github.com/fernandocalmet
[kofi-shield]: https://img.shields.io/badge/-@fernandocalmet-%231DA1F2?style=flat-square&logo=kofi&logoColor=ff5f5f
[kofi-url]: https://ko-fi.com/fernandocalmet
[linkedin-shield]: https://img.shields.io/badge/-fernandocalmet-blue?style=flat-square&logo=Linkedin&logoColor=white&link=https://www.linkedin.com/in/fernandocalmet
[linkedin-url]: https://www.linkedin.com/in/fernandocalmet
[khanakat-shield]: https://img.shields.io/badge/khanakat.com-brightgreen?style=flat-square
[khanakat-url]: https://khanakat.com