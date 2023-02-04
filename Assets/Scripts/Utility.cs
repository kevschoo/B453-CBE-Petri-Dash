using System.Reflection;
using UnityEngine;
// trait
using EnumHolder;

public class Utility
{
    #if UNITY_EDITOR
    public static void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
    #endif


    public static void DebugLog(string message)
    {
        if (Debug.isDebugBuild)
            Debug.Log(message);
    }


    public static Trait PickTrait(float luck)
    {
        Trait trait = (Trait)Random.Range(0, System.Enum.GetValues(typeof(Trait)).Length);

        if (trait == Trait.Clumsy ||
            trait == Trait.Cowardly ||
            trait == Trait.Unlucky ||
            trait == Trait.Weak ||
            trait == Trait.Leisurely)
        {
            if (Random.Range(0.0f, 1.0f) < luck)
                trait = PickTrait(0.0f);
        }

        return trait;
    }
}