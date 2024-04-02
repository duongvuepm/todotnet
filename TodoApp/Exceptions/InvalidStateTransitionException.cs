namespace TodoApp.Exceptions;

public class InvalidStateTransitionException : Exception
{
    public InvalidStateTransitionException(string s) : base(s)
    {
    }
}