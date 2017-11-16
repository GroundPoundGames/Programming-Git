﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// A PawnModule can be held by a Pawn in order to gain abilities and logic.
/// Each PawnModule can be initialised, updated and destroyed.
/// </summary>
[Serializable]
public abstract class PawnModule : UnityEngine.ScriptableObject
{
    protected Pawn ModulePawn; // Pawn this module affects.
    bool Destroyed; // Is this Module destroyed ? Used to determine when the Pawn should
    // remove its reference to this module.
    public abstract void Initialise(Pawn pawn);
    public abstract void UpdateModule(float deltaTime);
    public void Destroy()
    {
        Destroyed = true;
        OnDestroy();
    }
    public abstract void OnDestroy();
}
