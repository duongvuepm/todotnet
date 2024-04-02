namespace TodoApp.Exceptions;

public class ActionNotAllowedException : Exception
{
    public ActionNotAllowedException(string s) : base(s)
    {
    }
}