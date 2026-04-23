[AttributeUsage(AttributeTargets.Property)]
public class OptionAttribute : Attribute
{
    public string Id { get; }
    public char Short { get; }
    public string? Long { get; set; }
    public string? Default { get; set; }
    public bool Required { get; set; }
    public string? Help { get; set; }
    public OptionAttribute(string id, char @short)
    {
        Id = id;
        Short = @short;
    }
}