namespace TodoApp.Exceptions;

public class ResourceAlreadyExistException : Exception
{
    public ResourceAlreadyExistException(string message) : base(message)
    {
    }
}