using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// An AIState is a set of linear instructions and data which determine an AI's immediate behavior.
/// It is able to Enter, Update, and Exit. It has a reference to the StateMachineAI holding it.
/// </summary>
public abstract class AIState
{
    private WeakReference AIWeakRef = new WeakReference(null); // Assigned through the "LinkAI(StateMachineAI ai)" function. Not visible in child classes.
    protected StateMachineAI GetAI()
    {
        if (AIWeakRef.Target == null)
        {
            throw new Exception("This State is not linked to an AI !");
        }
        else
        {
            return (StateMachineAI)AIWeakRef.Target;
        }
    } // Returns the StateMachineAI using this State. Accessible to child classes. Throws an exception if no AI is linked.
    public void LinkAI(StateMachineAI ai)
    {
        AIWeakRef.Target = ai;
    } // Links a StateMachineAI to this State. Throws an exception if this State is already linked.

    abstract public void OnEntry();

    abstract public void Update();

    abstract public void OnExit();

}
