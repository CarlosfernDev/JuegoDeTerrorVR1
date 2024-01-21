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
    [SerializeField] private AudioSource SimonAudio;
    [SerializeField] private AudioSource WrongAudio;

    [SerializeField] private AudioClip WrongSound;
    [SerializeField] private AudioClip CompleteSound;

    private bool canISendInput = true;
    private List<int> numberSerie;
    private List<int> usernumberSerie = new List<int>();

    Coroutine IdleCoroutine;

    private void Awake()
    {
        ResetSimonDice();
        IdleCoroutine = StartCoroutine(ButtonParpadeoInicial());
        Debug.Log(numberSerie[0]);
    }

    private void OnDisable()
    {
        ResetColors();
        ResetLighBulbsWin();
    }

    public void EnterSimonInput(int number)
    {
        if (!canISendInput)
        {
            return;
        }

        if (PuzzleScipt.isDone)
        {
            return;
        }

        if (number > totalButtons)
        {
            Debug.LogWarning("estas introduciendo un numero superior al que puede soportar el simon.");
            return;
        }

        if (usernumberSerie.Count >= numberSerie.Count)
        {
            return;
        }

        if (totalNumberSerie < numberSerie.Count)
        {
            return;
        }

        StopCoroutine(IdleCoroutine);
        ResetColors();

        usernumberSerie.Add(number);

        SimonAudio.pitch = 0.9f + (0.1f * number);
        SimonAudio.Play();
        IdleCoroutine = StartCoroutine(ParpadearSingle(number));
    }

    private void ErrorSimonDice()
    {
        usernumberSerie = new List<int>();

        StartCoroutine(WrongLight());

        Debug.Log("Fallaste");
        ResetSimonDice();
        Debug.Log(numberSerie[numberSerie.Count - 1]);
    }

    private void CorrectSimonDice()
    {
        if (numberSerie.Count + 1 > totalNumberSerie)
        {
            WrongAudio.clip = CompleteSound;
            WrongAudio.Play();

            foreach (Material buttonvisual in visualButtons)
            {
                buttonvisual.SetFloat("_Intensity", 1);
            }

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
        foreach (Material buttonvisual in visualButtons)
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

    IEnumerator ParpadearSingle(int Number)
    {
        int index = Mathf.Clamp(usernumberSerie.Count - 1, 0, totalNumberSerie - 1);
        if (numberSerie[index] != usernumberSerie[index])
        {
            canISendInput = false;
        }

        SimonAudio.pitch = 0.9f + (0.1f * Number);
        SimonAudio.Play();
        visualButtons[Number - 1].SetFloat("_Intensity", 1);
        yield return new WaitForSeconds(0.6f);
        visualButtons[Number - 1].SetFloat("_Intensity", 0);
        yield return new WaitForSeconds(0.3f);

        if (numberSerie[index] != usernumberSerie[index])
        {
            canISendInput = false;
            ErrorSimonDice();
        }

        if (numberSerie[index] == usernumberSerie[index] && numberSerie.Count <= usernumberSerie.Count)
        {
            canISendInput = false;
            CorrectSimonDice();
        }
    }

    IEnumerator WrongLight()
    {
        yield return new WaitForSeconds(0.8f);
        //SimonAudio.pitch = 0.9f + (0.1f * index);
        //SimonAudio.Play();
        WrongAudio.clip = WrongSound;
        WrongAudio.Play();
        visualButtons[0].SetFloat("_Intensity", 1);
        visualButtons[2].SetFloat("_Intensity", 1);
        visualButtons[4].SetFloat("_Intensity", 1);
        visualButtons[6].SetFloat("_Intensity", 1);
        visualButtons[8].SetFloat("_Intensity", 1);
        yield return new WaitForSeconds(0.6f);
        visualButtons[0].SetFloat("_Intensity", 0);
        visualButtons[2].SetFloat("_Intensity", 0);
        visualButtons[4].SetFloat("_Intensity", 0);
        visualButtons[6].SetFloat("_Intensity", 0);
        visualButtons[8].SetFloat("_Intensity", 0);
        yield return new WaitForSeconds(0.3f);

        IdleCoroutine = StartCoroutine(Parpadear());
    }

    IEnumerator Parpadear()
    {
        yield return new WaitForSeconds(0.8f);
        foreach (int index in numberSerie)
        {
            SimonAudio.pitch = 0.9f + (0.1f * index);
            SimonAudio.Play();
            visualButtons[index - 1].SetFloat("_Intensity", 1);
            yield return new WaitForSeconds(0.6f);
            visualButtons[index - 1].SetFloat("_Intensity", 0);
            yield return new WaitForSeconds(0.3f);
            canISendInput = true;
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
                //SimonAudio.pitch = 0.9f + (0.1f * numberSerie[0]);
                //SimonAudio.Play();

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
