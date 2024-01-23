using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Instance Things
    public static GameManager Instance { get; private set; }

    [SerializeField] private List<PuzzleScriptable> _PuzzleScripts;
    [SerializeField] private List<GameObject> Interactuables;

    bool isFinish = false;

    private int _PuzzleCompleted;
    [SerializeField] Animator BoardAnimator;
    [SerializeField] private AudioSource Screamer;
    [SerializeField] private GameObject GameOverObject;
    [SerializeField] private GameObject WinObject;
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

        WinObject.SetActive(false);
        GameOverObject.SetActive(false);
        //EnableAllPuzzle(false);
    }


    #endregion

    #region GameLoop

    public void Win()
    {
        if (isFinish)
            return;

        isFinish = true;

        EnemigoDeLuzRemake.instance.StopTimer();
        Cronometro.Instance.PararCronometro();
        WinObject.SetActive(true);

        Debug.Log("Ganaste");
        // Logica de ganar
    }

    public void Lose()
    {
        if (isFinish)
            return;

        isFinish = true;

        Screamer.Play();
        GameOverObject.SetActive(true);
        StartCoroutine(LoseCorutine());
        // Logica de perder
    }

    IEnumerator LoseCorutine()
    {
        yield return new WaitForSeconds(3f);
        Application.Quit();

        yield return null;
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
        BoardAnimator.SetBool("Compuerta4", false);
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
        if (_PuzzleCompleted == _PuzzleScripts.Count)
        {
            Win();
            return true;
        }
        return false;
    }

    public void AddPuzzleCompleted()
    {
        if(_PuzzleCompleted == 0)
        {
            Monsruo_Ver_Script.Instance.TriggearMosntruo();
        }
        _PuzzleCompleted += 1;

        CheckPuzzleCompleted();
    }

    #endregion  
}
