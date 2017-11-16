using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Stores the Pawn's HP.
/// Reacts to taking damage (destroys the Pawn if HP goes below 0, has callback for when it takes damage).
/// </summary>
public class HealthModule : PawnModule
{
    public float MaxHP;
    public float CurrentHP;

    public override void Initialise(Pawn pawn)
    {
        ModulePawn = pawn;
        CurrentHP = MaxHP;
    }

    public override void UpdateModule(float deltaTime)
    {
        
    }

    public override void OnDestroy()
    {
        
    }

    public void TakeDamage(float amount)
    {
        CurrentHP -= amount;
        if (CurrentHP <= 0)
        {
            Destroy(ModulePawn);
        }
    }
}
