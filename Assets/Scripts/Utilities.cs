using System;
using System.Linq;
using Random = UnityEngine.Random;

public static class Utilities
{
    public static string ToPercentage(this float number)
    {
        return (number * 100).ToString("F0") + "%";
    }
    
    private static readonly string[] FirstNames =
    {
        "Witold",
        "Piotr",
        "Damian",
        "Arek",
        "Gerwazy",
        "Elon",
        "Captain",
        "Tony"
    };
        
    private static readonly string[] LastNames =
    {
        "Musk",
        "Bu",
        "Ba",
        "Be",
        "Bezos",
        "Mt. Sugar",
        "Smith",
        "America",
        "Stark"
    };
    
    public static string RandomizeName()
    {
        var firstName = FirstNames.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
        var lastName = LastNames.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
        return $"{firstName} {lastName}";
    }
}