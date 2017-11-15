using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// A PatternElement is one part of a pattern. It can have a "child" PatternElement representing the next part of the pattern. It has a "wait time"
/// indicating how much time in seconds an AI should wait near this PatternElement before proceeding to the next one.
/// </summary>
public class PatternElement : MonoBehaviour
{
    public float WaitTime = 1f;
    public PatternElement Next;
    PatternElement Previous = null; // Parent PatternElement. Can be null. Used when reaching the end of a "line". Set by the father.

    public PatternElement GetPrevious()
    {
        return Previous;
    }

    private void Start()
    {
        if (Next != null) // Set this Element's child's parent to this Element.
        {
            Next.Previous = this;
        }

        if (Next == null)
        {
            Next = this; // By default, the Next element is this element (to allow one-element patterns).
        }
    }

    private void Update()
    {
        if (Next == null)
        {
            Next = this;
        }

        if (Next != this && Next.Previous != this)
        {
            Next.Previous = this;
        }

        if (Previous != null && Previous.Next != this)
        {
            Previous = null;
        }
    }
}
