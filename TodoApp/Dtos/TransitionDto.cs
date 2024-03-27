using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TodoApp.Dtos;

public record TransitionDto([BindRequired] long ToState)
{
}
