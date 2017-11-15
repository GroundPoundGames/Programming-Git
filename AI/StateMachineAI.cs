    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// The StateMachine AI is the simplest "complex" game AI that can be made : at all times, it is in one "State".
/// A State is a set of instructions and data that determines the AI's immediate behavior. It knows about all the other
/// states and always has conditions to switch to those States.
/// 
/// Of course, should the AI become too complex, the amount of States will increase and the number of links between
/// states will increase exponentially : if we feel AI should be more complex, let's not hesitate to go for a behavior tree, for example.
/// </summary>
public class StateMachineAI : MonoBehaviour
{
    // State Management
    AIState CurrentState; // Current state the AI is in.
    public void ChangeState(AIState newState)
    {
        if (newState == CurrentState || newState == null) throw new Exception("Invalid state assignment !");
        if (CurrentState != null) CurrentState.OnExit(); // Give a chance to the current state to do whatever it needs to do on exit (since this might be triggered by something other than the State itself)
        CurrentState = newState; // Set the current state to the new state
        CurrentState.LinkAI(this); // Link this AI to the new state.
        CurrentState.OnEntry(); // Trigger the state's Entry behavior.
    }
    private void Update()
    {
        if (CurrentState != null)
        {
            CurrentState.Update();
            CurrentState.CheckTransitions(); // TODO : As checking transitions can be CPU intensive, don't do this every frame (every fixed time in seconds or number of frames).
        }
    } // Updates the current state and check transitions

    private void Start()
    {
        // Set the initial State to "Wandering".
        ChangeState(new MovementPatternState());
    }

    // AI Blackboard : link a Unity GameObject to a String entry in a dictionnary

    public BlackboardElement[] Blackboard;

    /// <summary>
    /// Returns the GameObject corresponding to the given elementName in the Blackboard array.
    /// If the element does not exist, "null" is returned.
    /// </summary>
    public GameObject ReadBlackboard(string elementName)
    {
        BlackboardElement element = null;
        int elementID = 0;
        while (element == null && elementID < Blackboard.Length)
        {
            if (Blackboard[elementID].Name == elementName)
            {
                element = Blackboard[elementID];
            }
            else
            {
                elementID++;
            }
        }

        if (element == null)
        {
            return null;
        }
        else
        {
            return element.GO;
        }
    }

    // Pawn actions TODO : Create a "Entity" or "Pawn" class defining the "physical" attributes and capabilities of a character.

    // Attributes :

    float Speed = 2f;

    /// <summary>
    /// Moves the GameObject towards the dest by a distance of Speed * time.deltaTime. If the point is reached (Speed * 0.1f < dest-transform.position),
    /// move on the dest point directly.
    /// Returns weither the destination was reached.
    /// </summary>
    /// <param name="dest"> Destination to move towards. </param>
    public bool MoveTowards(Vector3 dest)
    {
        float dist = Speed * Time.deltaTime;
        float distToDest = (transform.position - dest).sqrMagnitude; // Keep it a square to avoid the costly square root operation.
        if (dist * dist > distToDest)
        {
            // Destination reached ! Move directly onto the dest point.
            transform.position = dest;
            return true;
        }
        else
        {
            transform.Translate((dest - transform.position).normalized * dist);
            return false;
        }
    }
}
