using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager
{
    public static Action OnObjectiveOn;
    public static Action OnObjectiveCompleted;

    public Objective objectiveActive;

    public void AddObjective(Objective objective)
    {
        objectiveActive = objective;
        

        OnObjectiveOn?.Invoke();
    }

    public void CompleteObjective()
    {
        objectiveActive = null;
        OnObjectiveCompleted?.Invoke();
    }

    public Objective GetObjective()
    {
        return objectiveActive;
    }

    public void SetObjective(Objective objective)
    {
        objectiveActive = objective;
    }
}
