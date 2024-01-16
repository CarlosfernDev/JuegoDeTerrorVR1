using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleScriptable : ScriptableObject
{
    public int ID;
    [HideInInspector] public bool isDone;
}
