using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Utilities
{
    public static string ToPercentage(this float number, bool addPercentSign = true)
    {
        return (number * 100).ToString("F0") + (addPercentSign ? "%" : "");
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
    
    public static IEnumerator WaitAllCoroutine(this MonoBehaviour script, List<IEnumerator> coroutineList, [CanBeNull] Action onComplete) {
        foreach (var coroutine in coroutineList)
        {
            yield return script.StartCoroutine(coroutine);
        }
        
        onComplete?.Invoke();
    }
}