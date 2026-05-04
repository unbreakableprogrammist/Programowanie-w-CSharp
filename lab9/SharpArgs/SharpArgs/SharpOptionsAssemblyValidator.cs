using System.Reflection;
using SharpArgs.Exceptions;

namespace SharpArgs;

public static class SharpOptionsAssemblyValidator
{
    public static void ValidateAssembly(Assembly assembly)
    {
        var errors = new List<SharpArgsException>(); // tablica obiektow typu SharpArgsException
        
        var opcje = new List<Type>();
        var wszystkie_typy = assembly.GetTypes(); // bierzemy wszytskie typy ktore znajdziemy w plikuu assembly
        foreach (var t in wszystkie_typy)
        {
            if (!t.IsAbstract && typeof(SharpOptions).IsAssignableFrom(t))  // t nie jest klasa abstrakcyjna i t jest dziedziczace lub typu SharpOptions
            {
                opcje.Add(t);
            }
        }
        // teraz idziemy po tych opcjach i 
        foreach (var type in opcje)
        {
            try
            {
                // staramy sie skonstruowac obiekt tego typu z opcji
                var obiekt = Activator.CreateInstance(type);

                // sprawdzamy czy obiekt da sie zrzutowac na obiekt klasy SharpOptions ( options)
                if (obiekt is SharpOptions options)
                {
                    options.ValidateModel(); // walidujemy flagi i opcje na obiekcie typu opiekt
                }
            }
            // lapiemy tylko wyjatki ktore dziedzicza po SharpArgsExceptions
            catch (SharpArgsException ex)
            {
                errors.Add(ex);
            }
        }
        // jesli zlapalismy jakies takie wyjatki zwracamy AggregateException
        if (errors.Count > 0)
        {
            throw new AggregateException(errors);
        }
    }
}