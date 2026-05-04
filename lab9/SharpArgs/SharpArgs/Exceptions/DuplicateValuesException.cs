namespace SharpArgs.Exceptions;

public sealed class DuplicateValuesException<T> : SharpArgsException
{
    public IReadOnlyCollection<T> DuplicateValues { get; }

    public DuplicateValuesException(IReadOnlyCollection<T> duplicateValues)
    {
        this.DuplicateValues = duplicateValues;
    }

    public DuplicateValuesException(IReadOnlyCollection<T> duplicateValues, string message)
        : base(message)
    {
        this.DuplicateValues = duplicateValues;
    }

    public DuplicateValuesException(IReadOnlyCollection<T> duplicateValues, string message, Exception inner)
        : base(message, inner)
    {
        this.DuplicateValues = duplicateValues;
    }
}