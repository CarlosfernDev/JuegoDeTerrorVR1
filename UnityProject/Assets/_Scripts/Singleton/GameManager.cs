using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Instance Things
    public GameManager Instance { get; private set; }

    [SerializeField] private List<PuzzleScriptable> _PuzzleScripts;
    private int _PuzzleCompleted;
    //Clase para puzzles o scriptable object idk

    [SerializeField] private float _CountDownGameTime;
    private float _GameTimer;
    private float _TimeReference;

    #endregion
    #region UnityFunctions
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }

        foreach(PuzzleScriptable Puzzle in _PuzzleScripts)
        {
            Puzzle.isDone = false;
        }

    }


    #endregion

    #region GameLoop

    public void Win()
    {
        Debug.Log("Ganaste");
        // Logica de ganar
    }

    public void Lose()
    {
        // Logica de perder
    }

    #endregion

    #region PuzzleFunctions

    //Revisas la cantidad de Puzzles que faltan
    public int HowManyPuzzleIsDone()
    {
        return Mathf.Clamp(_PuzzleCompleted,0 ,4);
    }

    public void AddPuzzleQuantity(PuzzleScriptable value)
    {
        if (!_PuzzleScripts.Contains(value))
        {
            Debug.LogError("Este puzzle no esta en la lista: " + value.ID);
            return;
        }

        _PuzzleCompleted = Mathf.Clamp(_PuzzleCompleted + 1,0, _PuzzleScripts.Count + 1);
    }

    public bool CheckPuzzleCompleted()
    {
        if (_PuzzleCompleted == _PuzzleScripts.Count + 1)
        {
            Win();
            return true;
        }
        return false;
    }

    #endregion

    #region TemporizadorGlobal

    public void SetCountDown()
    {
        _TimeReference = Time.time;
    }

    public float GetCountDown()
    {
        return  Mathf.Clamp(_CountDownGameTime - (Time.time - _TimeReference),0, _CountDownGameTime);
    }

    public bool CheckCountDown()
    {
        if(GetCountDown() == 0)
        {
            Lose();
            return false;
        }
        return true;
    }

    #endregion
}
