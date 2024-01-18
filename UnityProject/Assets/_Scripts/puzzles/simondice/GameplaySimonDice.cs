using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySimonDice : MonoBehaviour
{
    [SerializeField] PuzzleScriptable PuzzleScipt;

    [SerializeField] int totalNumberSerie;
    [SerializeField] int totalButtons;
    [SerializeField] Material[] visualButtons;
    [SerializeField] Material[] LightBulbPass;

    private List<int> numberSerie;
    private List<int> usernumberSerie = new List<int>();

    Coroutine IdleCoroutine;

    private void Awake()
    {
        PuzzleScipt.isDone = false;
        ResetSimonDice();
        IdleCoroutine = StartCoroutine(ButtonParpadeoInicial());
        Debug.Log(numberSerie[0]);
    }

    private void OnDisable()
    {
        ResetColors();
        ResetLighBulbsWin();
    }

    public void EnterSimonInput(int number) {
        StopCoroutine(IdleCoroutine);
        ResetColors();

        if (PuzzleScipt.isDone)
        {
            return;
        }

        if (number > totalButtons)
        {
            Debug.LogWarning("estas introduciendo un numero superior al que puede soportar el simon.");
            return;
        }

        if(totalNumberSerie < numberSerie.Count)
        {
            return;
        }

        usernumberSerie.Add(number);
        int index = usernumberSerie.Count - 1;

        if (numberSerie[index] != usernumberSerie[index])
        {
            ErrorSimonDice();
        }

        if(usernumberSerie.Count == numberSerie.Count)
        {
            CorrectSimonDice();
        }
    }

    private void ErrorSimonDice()
    {
        usernumberSerie = new List<int>();


        Debug.Log("Fallaste");
        ResetSimonDice();
        IdleCoroutine = StartCoroutine(Parpadear());
        Debug.Log(numberSerie[numberSerie.Count - 1]);
    }

    private void CorrectSimonDice()
    {
        if (numberSerie.Count + 1 > totalNumberSerie)
        {
            Debug.Log("Te pasaste el juego");
            LightBulbPass[numberSerie.Count - 1].SetFloat("_Intensity", 1);
            PuzzleScipt.isDone = true;
            return;
        }
        usernumberSerie = new List<int>();

        Debug.Log("Acertaste");
        numberSerie.Add(Random.Range(1, totalButtons + 1));
        LightBulbPass[numberSerie.Count - 2].SetFloat("_Intensity", 1);
        Debug.Log(numberSerie[numberSerie.Count - 1]);
        IdleCoroutine = StartCoroutine(Parpadear());
    }

    private void ResetSimonDice()
    {
        numberSerie = new List<int>();

        numberSerie.Add(Random.Range(1, totalButtons + 1));
        ResetLighBulbsWin();
    }

    private void ResetColors()
    {
        int index = 0;
        // Hacer que ponga los botones bien
        foreach(Material buttonvisual in visualButtons)
        {
            buttonvisual.SetFloat("_Intensity", 0);
            index++;
        }
    }

    private void ResetLighBulbsWin()
    {
        foreach (Material buttonvisual in LightBulbPass)
        {
            buttonvisual.SetFloat("_Intensity", 0);
        }
    }

    IEnumerator Parpadear()
    {
        yield return new WaitForSeconds(0.3f);
        foreach (int index in numberSerie)
        {
            visualButtons[index - 1].SetFloat("_Intensity", 1);
            yield return new WaitForSeconds(0.6f);
            visualButtons[index - 1].SetFloat("_Intensity", 0);
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator ButtonParpadeoInicial()
    {
        bool Enable = true;
        while (true)
        {
            Enable = !Enable;

            if (Enable)
            {
                visualButtons[numberSerie[0] - 1].SetFloat("_Intensity", 1);
            }
            else
            {
                visualButtons[numberSerie[0] - 1].SetFloat("_Intensity", 0);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
