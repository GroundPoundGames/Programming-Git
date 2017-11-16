using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// A Pawn is a physical entity controlled by an AI or the Player.
/// It contains two collections :
///     - Items : Much like the AI's blackboard, it links a GameObject (prefab or on scene) to a name.
///     Used to define how the Pawn uses other objects (objects it "owns / wears" like weapons, armor, gadgets...
///     but also other objects it might use but doesn't own or wear).
///     - Modules : Array of PawnModule. See description of PawnModule to know more.
/// </summary>
[Serializable] // Makes this class Serializable, which makes it visible in the Unity Editor.
public class Pawn : MonoBehaviour {

    [Header("Pawn Modules")]
    public List<PawnModule> Modules; // Array of modules. Make sure only one module of a given type
    // exist within this array !
    /// <summary>
    /// Initialises all modules.
    /// </summary>
    public void InitialiseModules()
    {
        foreach(PawnModule module in Modules)
        {
            module.Initialise(this);
        }
    }

    /// <summary>
    /// Runs an update with the given deltaTime on all modules.
    /// </summary>
    public void UpdateModules(float deltaTime)
    {
        foreach(PawnModule module in Modules)
        {
            module.UpdateModule(deltaTime);
        }
    }

    /// <summary>
    /// Destroy the first module of the given type found in this Pawn if it exists.
    /// </summary>
    public void DestroyModule<T>() where T : PawnModule
    {
        PawnModule mod = GetModule<T>();
        if (mod != null)
        {
            mod.Destroy();
        }
    }

    public void DestroyAllModules()
    {
        foreach(PawnModule module in Modules)
        {
            module.Destroy();
        }
    }

    /// <summary>
    /// Returns the first module of the given type within this Pawn.
    /// If no module of that type is present within this entity, returns null.
    /// </summary>
    public T GetModule<T>() where T : PawnModule
    {
        PawnModule mod = null;
        int moduleID = 0;
        while (mod == null && moduleID < Modules.Count)
        {
            if (Modules[moduleID] is T)
            {
                mod = Modules[moduleID];
            }
            else
            {
                moduleID++;
            }
        }

        if (mod != null)
        {
            return (T)mod;
        }
        else
            return null;
    }

    private void Start()
    {
        InitialiseModules();
    }

    private void Update()
    {
        UpdateModules(Time.deltaTime);
    }

    private void OnDestroy()
    {
        DestroyAllModules();
    }

}
