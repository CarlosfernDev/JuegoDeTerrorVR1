using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Instance Things
    public static GameManager Instance { get; private set; }

    [SerializeField] private List<PuzzleScriptable> _PuzzleScripts;
    [SerializeField] private List<GameObject> Interactuables;

    private int _PuzzleCompleted;
    [SerializeField] Animator BoardAnimator;
    //Clase para puzzles o scriptable object idk

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

        //EnableAllPuzzle(false);
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

    public void EnableAllPuzzle(bool value)
    {
        Interactuables[0].SetActive(false);
        Interactuables[1].SetActive(true);
        Interactuables[2].SetActive(true);

        BoardAnimator.SetBool("Compuerta1", false);
        BoardAnimator.SetBool("Compuerta2", true);
        BoardAnimator.SetBool("Compuerta3", true);
        BoardAnimator.SetBool("Compuerta4", true);
        Cronometro.Instance.ReiniciarCronometro();

        foreach (PuzzleScriptable Puzzle in _PuzzleScripts)
        {
            Puzzle.isEnabled = value;
        }

        EnemigoDeLuzRemake.instance.StarTimer();
    }

    //Revisas la cantidad de Puzzles que faltan
    public int HowManyPuzzleIsDone()
    {
        return Mathf.Clamp(_PuzzleCompleted,0 ,4);
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
}
