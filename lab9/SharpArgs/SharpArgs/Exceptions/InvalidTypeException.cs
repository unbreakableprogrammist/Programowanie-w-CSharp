namespace SharpArgs.Exceptions;

public sealed class InvalidTypeException : SharpArgsException
{
    public Type InvalidType { get; }

    public InvalidTypeException(Type invalidType)
    {
        this.InvalidType = invalidType;
    }

    public InvalidTypeException(Type invalidType, string message)
        : base(message)
    {
        this.InvalidType = invalidType;
    }

    public InvalidTypeException(Type invalidType, string message, Exception inner)
        : base(message, inner)
    {
        this.InvalidType = invalidType;
    }
}