namespace SharpArgs;
[AttributeUsage(AttributeTargets.Property)] // atrybut bedzie wskazywal na propety
public class FlagAttribute : Attribute{
    public string Id { get; }
    public char Short { get; set; }
    public string? Long { get; set; }
    public string? Help { get; set; }
    public FlagAttribute(string id, char @short)
    {
        Id = id;
        Short = @short;
    }
}