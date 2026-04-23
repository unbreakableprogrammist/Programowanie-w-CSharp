

using System.Reflection;
using SharpArgs.Exceptions;

namespace SharpArgs;

/// <summary>
/// An abstract base class for defining command-line option models.
/// Provides validation logic for flag and options.
/// </summary>
public abstract class SharpOptions
{
    private readonly List<PropertyInfo> _properties = [];

    public SharpOptions()
    {
    }

    /// <summary>
    /// Validates all properties marked with the <see cref="FlagAttribute"/>.
    /// </summary>
    /// <exception cref="InvalidTypeException">Thrown if a flag property is not of type <see cref="bool"/>.</exception>
    /// <exception cref="DuplicateValuesException{T}">Thrown if duplicate short names are found among flags.</exception>
    public virtual void ValidateFlags()
    {
        var flago_wlasciwosci = new List<PropertyInfo>(); //tworzymy pusta liste gdzie bedziemy dawac PropertyInfo( taki opis metadanowy wlasciwosci)
        var wszystkie = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic); // wszystkie wlasciwosci jakie mamyw tej klasie i w klasach dziedziczacych prywatne i publiczne
        foreach (var prop in wszystkie) // dodajemy wszystkie wlasciwosci ktore spelnaija Flag
        {
            var attr = prop.GetCustomAttribute<FlagAttribute>(inherit: true); // wez oznaczone atrybutem flag
            if (attr != null)
            {
                flago_wlasciwosci.Add(prop);
            }
           
        }
        // teraz rzucamy wyjatki jesli znajdziemy jakas wartosc nie-bool
        foreach (var prop in flago_wlasciwosci)
        {
            if (prop.PropertyType != typeof(bool)) // chcemy zeby wlasciwosci oznaczone Flag byly typu bool 
            {
                throw new InvalidTypeException(prop.PropertyType);
            }
        }
        /*
          GroupBy dziala tak : 
          'v' → ['v','v']
           'c' → ['c']
           'd' → ['d']
           po count zostaje 
           'v' → ['v', 'v']
           zostawiamy klucz = v
           konwertujemy do listy
         */
        var krotkie = flago_wlasciwosci.Select(prop => prop.GetCustomAttribute<FlagAttribute>(inherit: true).Short); //bierzemy krotkie nazwy we flagach
        var duplikaty = krotkie.FindDuplicates();
        if (duplikaty.Count > 0) //duplicaty to lista charow
        {
            throw new DuplicateValuesException<char>(duplikaty);            
        }
    }

    /// <summary>
    /// Validates all properties marked with the <see cref="OptionAttribute"/>
    /// </summary>
    /// <exception cref="InvalidTypeException">Thrown if an option property's type is not a <see cref="string"/> and does not implement <see cref="IParsable{TSelf}"/>.</exception>
    public virtual void ValidateOptions()
    {
        var option_wlasciwosci = new List<PropertyInfo>(); //tworzymy pusta liste gdzie bedziemy dawac PropertyInfo( taki opis metadanowy wlasciwosci)
        var wszystkie = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic); // wszystkie wlasciwosci jakie mamyw tej klasie i w klasach dziedziczacych prywatne i publiczne
        foreach (var prop in wszystkie) // dodajemy wszystkie wlasciwosci ktore maja atrybut option
        {
            var attr = prop.GetCustomAttribute<OptionAttribute>(inherit: true); // wez oznaczone atrybutem option
            if (attr != null)
            {
                option_wlasciwosci.Add(prop); 
            }
           
        }
        /*
         bierzemy sobie te wsyztskie wlasciwosci ktore maja przed soba atrybut Options i wtedy jesli to 
         jest wlasciwosc typu string to continue a jesli to ejst wartosc innego typu to sprawdzamy czy jest to typ taki 
         ktory implementuje Iparsable
         */
        foreach (var prop in option_wlasciwosci)
        {
            Type t = prop.PropertyType; // bierzemy sobie typ wlasciwowsci
            if(t == typeof(string)) // jesli ta wlasciwosc jesst typu string to git
                continue;
            // teraz tworzymy obiekt typu Iparsable ( obiekt generyczny)
            Type czy_iparsable = typeof(IParsable<>).MakeGenericType(t);
            if (!czy_iparsable.IsAssignableFrom(t)) // sprawdzamy czy ta wlasciwosc t moze implementowac IParsable
            {
                throw new InvalidTypeException(t);
            }
        }
    }

    /// <summary>
    /// Validates the entire options model by calling all individual validation methods.
    /// </summary>
    public void ValidateModel()
    {
        ValidateFlags();
        ValidateOptions();
    }
}