using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective
{
    public string TaskDescription { get; set; }
    public bool IsCompleted { get; set; }

    public Objective(string taskDescription)
    {
        this.TaskDescription = taskDescription;
    }

    public override string ToString()
    {
        return $"{TaskDescription} [{(IsCompleted ? "x" : " ")}]";
    }
}
