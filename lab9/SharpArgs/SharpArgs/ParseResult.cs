namespace SharpArgs;

public sealed class ParseResult<T> 
    where T : SharpOptions
{
    public T? Target { get; }

    public IReadOnlyList<string> Errors { get; } = [];

    public bool Success => !this.Errors.Any();

    public ParseResult(T target)
    {
        this.Target = target;
    }

    public ParseResult(IReadOnlyList<string> errors)
    {
        this.Target = null;
        this.Errors = errors;
    }
}