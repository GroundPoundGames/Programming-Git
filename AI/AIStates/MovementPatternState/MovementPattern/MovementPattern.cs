using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// A MovementPattern contains an array of PatternElements which form the pattern itself.
/// </summary>
public class MovementPattern : MonoBehaviour
{
    PatternElement[] Pattern = new PatternElement[0]; // Empty array by default.

    private void Awake()
    {
        // At Start, check all children for "PatternElement" components.
        Pattern = gameObject.GetComponentsInChildren<PatternElement>();

        if (Pattern.Length == 0)
        {
            Debug.LogError("ERROR : No elements in pattern !");
        }
        else
        {
            Debug.Log("Pattern built ! " + Pattern.Length + " elements.");
        }
    }

    public PatternElement[] GetPattern()
    {
        return Pattern;
    }
}
