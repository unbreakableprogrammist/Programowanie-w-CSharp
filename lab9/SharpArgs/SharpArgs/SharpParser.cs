using System.Reflection;

namespace SharpArgs;

public class SharpParser<T>
    where T : SharpOptions, new() // do sharp parser wstawiamy cos typu SharpOptions i ma to byc zrobione poprzez T model = new T()
{
    public SharpParser()
    {
    }

    public ParseResult<T> Parse(string[] args)
    {
        var errors = new List<string>(); // tu bedda erory 
        var obiekt = Activator.CreateInstance(typeof(T)); // tworzymy obiekt typu T
        if (obiekt is not T options) // jesli stworzony obiekt jakos nie bedzie typu T to blad ( takie upewnienie sie) 
        {
            throw new InvalidOperationException();
        }
        // robimy pusta liste par wlasciwosc , atrybut - flaga
        var flago_wlasciwosci = new List<(PropertyInfo Prop, FlagAttribute Attr)>();
        // bierzemy wszystkie wlasciwosci z obiektu T
        var wszytstkie = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (var wlasc in wszytstkie)
        {
            // bierzemy atrybut kazdej wlasciwosci ktory jest typu flag ( jak takiego nie ma to atrybut == null)
            var atrybut = wlasc.GetCustomAttribute<FlagAttribute>(inherit: true);
            if (atrybut != null)
            {
                flago_wlasciwosci.Add((wlasc,atrybut));
            }
        }
        
        // teraz mamy te wlasciwosci ktore maja atrybut flag i zbieramy krotkie i dlugie flagi
        var krotkie_flagi = new Dictionary<char, PropertyInfo>();
        var dlugie_flagi = new Dictionary<string, PropertyInfo>(StringComparer.Ordinal);        
        foreach (var (wlasc,atryb) in flago_wlasciwosci)
        {
            krotkie_flagi[atryb.Short] = wlasc;
            dlugie_flagi[atryb.Long] = wlasc;
        }

        foreach (var arg in args)
        {
            if (string.IsNullOrWhiteSpace(arg))  // ignorujemy biale znaki
            {
                continue;
            }
            // jesli nie zaczyna sie od - to nie wiadomo
            if (!arg.StartsWith("-"))
            {
                errors.Add($"Unknown option: {arg}.");
                continue;
            }

            if (arg.StartsWith("--"))
            {
                var name = arg.Substring(2); // obcinamy "--"
                // jesli po "--" nic nie ma albo nie znamy takiej flagi 
                if (string.IsNullOrEmpty(name) || !dlugie_flagi.TryGetValue(name, out var wlasc))// wyciagamy ze slownika wartosc przy kluczu o nazwie wlasciwosci
                {
                    errors.Add($"Unknown option: {arg}.");
                    continue;
                } 
                wlasc.SetValue(options, true);  // przelaczamy wlasciwosc przed ktora jest atrybut na true
            }else if (arg.StartsWith("-"))
            {
                char c = arg[1];
                if (!krotkie_flagi.TryGetValue(c, out var wlasc))
                {
                    // nie znamy takiej flagi
                    errors.Add($"Unknown option: {arg}.");
                    continue;
                }
                wlasc.SetValue(options, true);
            }
            
            
            
        }
        if (errors.Count > 0) // jesli byly jakies bledy
        {
            return new ParseResult<T>(errors);
        }
        return new ParseResult<T>(options);
    }
}