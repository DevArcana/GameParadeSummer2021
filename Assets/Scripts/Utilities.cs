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
        "John",
        "Bob",
        "Alice",
        "Alex",
        "Norman",
        "Leo",
        "Kiri",
        "Klu",
        "Mex",
        "Mi",
        "Xiu",
        "Xi",
        "He",
        "Witek",
        "Piotr",
        "Damian",
        "Arek"
    };
        
    private static readonly string[] LastNames =
    {
        "Sha",
        "Lu",
        "Wei",
        "Wei Wei",
        "Flu",
        "Smith",
        "Herb",
        "Boxing",
        "Martyr"
    };
    
    public static string RandomizeName()
    {
        var firstName = FirstNames.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
        var lastName = LastNames.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
        return $"{firstName} {lastName}";
    }
}