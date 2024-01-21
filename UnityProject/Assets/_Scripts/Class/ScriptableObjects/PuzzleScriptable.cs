using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PuzzleData", menuName = "ScriptableObjects/PuzzleData", order = 1)]
public class PuzzleScriptable : ScriptableObject
{
    public int ID;
    [HideInInspector] public bool isDone;
    [HideInInspector] public bool isEnabled;
}
