using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Has a speed attribute.
/// Able to move a Pawn from one point to another.
/// </summary>
[Serializable]
public class BasicMover : PawnModule
{
    public float Speed = 1f;
    public string lol;
    UnityEngine.Vector3 Destination;
    bool DestinationReached = false;

    public override void Initialise(Pawn pawn)
    {
        ModulePawn = pawn;
    }

    public override void UpdateModule(float deltaTime)
    {
        if (!DestinationReached)
        {
            MoveTowardsDestination(deltaTime);
        }
    }

    public override void OnDestroy()
    {
        
    }

    public void SetDestination(float x, float y, float z)
    {
        if (Destination.x != x || Destination.y != y || Destination.z != z)
        {
            Destination = new UnityEngine.Vector3(x, y, z);
            DestinationReached = false;
        }
    }

    public UnityEngine.Vector3 GetDestination()
    {
        return Destination;
    }

    void MoveTowardsDestination(float deltaTime)
    {
        float dist = Speed * deltaTime;
        float distToDest = (ModulePawn.transform.position - Destination).sqrMagnitude; // Keep it a square to avoid the costly square root operation.
        if (dist * dist > distToDest)
        {
            // Destination reached ! Move directly onto the dest point.
            ModulePawn.transform.position = Destination;
            DestinationReached = true;
        }
        else
        {
            ModulePawn.transform.Translate((Destination - ModulePawn.transform.position).normalized * dist);
        }
    }

    public bool ReachedDestination()
    {
        return DestinationReached;
    }
}

