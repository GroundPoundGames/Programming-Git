using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// In this State, the AI will follow a specific movement pattern. Every time it reaches a PatternElement, it waits for the amount of second
/// specified in the "WaitTime" attribute of this PatternElement, then moves on to the next PatternElement indicated by the current PatternElement.
/// If none, then it returns to the PatternElement's father.
/// 
/// </summary>
public class MovementPatternState : AIState
{

    PatternElement LastElement = null; // The last Element this AI reached.
    PatternElement DestElement = null; // Element this AI is going towards.

    /// <summary>
    /// Tries to find a "Pattern" element on the AI's blackboard. If it fails, or if the linked GameObject does not have a "MovementPattern"
    /// component, it tries to find the nearest object with a "MovementPattern" component. If that fails, an error is thrown.
    /// 
    /// Once a suitable GameObject is found, the nearest PatternElement within that pattern is choosen as the first DestElement.
    /// </summary>
    public override void OnEntry()
    {
        UnityEngine.GameObject patternGO = GetAI().ReadBlackboard("Pattern"); // Read the "Pattern" element.
        if (patternGO == null || patternGO.GetComponent<MovementPattern>() == null)
        {
            // No Pattern entry on the blackboard... search for the closest pattern.
            MovementPattern[] patterns = UnityEngine.GameObject.FindObjectsOfType<MovementPattern>();
            if (patterns.Length > 0)
            {
                UnityEngine.Vector3 myPosition = GetAI().gameObject.transform.position;

                float shortestDist = UnityEngine.Mathf.Infinity;
                MovementPattern closest = null;
                foreach(MovementPattern pattern in patterns)
                {
                    float dist = (pattern.transform.position - myPosition).sqrMagnitude;
                    if (closest == null ||  dist < shortestDist)
                    {
                        closest = pattern;
                        shortestDist = dist;
                    }
                }

                SearchClosestPatternElement(closest);
            }
            else
            {
                // No patterns in the world. Error !
                UnityEngine.Debug.LogError("ERROR : No MovementPatterns found !");
            }
        }
        else
        {
            MovementPattern pattern = patternGO.GetComponent<MovementPattern>();
            SearchClosestPatternElement(pattern);
        }
    }

    /// <summary>
    /// Searches for the closest PatternElement within the given MovementPattern object.
    /// Throws an exception when the MovementPattern does not have any PatternElement object.
    /// </summary>
    /// <param name="pattern"></param>
    void SearchClosestPatternElement(MovementPattern pattern)
    {
        if (pattern.GetPattern().Length == 0)
        {
            throw new Exception("MovementPattern does not have any Elements !");
        }
        else
        {
            UnityEngine.Vector3 myPosition = GetAI().gameObject.transform.position;

            float shortestDist = UnityEngine.Mathf.Infinity;
            PatternElement closest = null;
            foreach (PatternElement element in pattern.GetPattern())
            {
                float dist = (element.transform.position - myPosition).sqrMagnitude;
                if (closest == null || dist < shortestDist)
                {
                    closest = element;
                    shortestDist = dist;
                }
            }

            DestElement = closest;
        }
    }

    /// <summary>
    /// Move to the nearest 
    /// </summary>
    public override void Update()
    {
        
    }

    public override void OnExit()
    {
        
    }
}
