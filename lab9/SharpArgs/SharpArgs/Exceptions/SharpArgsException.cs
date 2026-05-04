namespace SharpArgs.Exceptions;

public class SharpArgsException : Exception
{
    public SharpArgsException() { }

    public SharpArgsException(string message) 
        : base(message) { }

    public SharpArgsException(string message, Exception inner) 
        : base(message, inner) { }
}