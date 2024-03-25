using System.Net;

namespace TodoApp.Dtos;

public record ExceptionResponse(HttpStatusCode StatusCode, string Description);