using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Logica_Calculadora_UI : MonoBehaviour
{
    [SerializeField] PuzzleScriptable PuzzleScipt;

    [SerializeField] private string clavePredeterminada;
    [SerializeField] private int LongitudPassword;
    private string entradaUsuario = "";

    [SerializeField] private TMP_Text pantalla;
    [SerializeField] private AudioSource AudioWrong;
    [SerializeField] private AudioClip WrongSound;
    [SerializeField] private AudioClip CompleteSound;

    [SerializeField] Material CorrectMaterial;
    [SerializeField] Material WrongMaterial;

    private bool ButtonEnable = true;
    private Coroutine singlecoroutine;

    private void Awake()
    {
        ActualizarPantalla();
    }

    private void OnDisable()
    {
        WrongMaterial.SetFloat("_Intensity", 0);
        CorrectMaterial.SetFloat("_Intensity", 0);
    }

    void Start()
    {
        //// Asegúrate de que la pantalla no sea editable directamente
        //pantalla.readOnly = true;
    }

    public void OnButtonPress(int digito)
    {
        if (PuzzleScipt.isDone)
        {
            return;
        }

        if (!ButtonEnable)
        {
            return;
        }

        if (entradaUsuario.Length >= LongitudPassword)
            return;


        entradaUsuario += digito.ToString();
        ActualizarPantalla();

        if (entradaUsuario.Length == 5)
        {
            ButtonEnable = false;
            VerificarClave();
        }

    }

    public void OnDeleteButtonPress()
    {
        if (PuzzleScipt.isDone)
        {
            return;
        }

        if (!ButtonEnable)
        {
            return;
        }

        if (entradaUsuario.Length > 0)
        {
            entradaUsuario = entradaUsuario.Substring(0, entradaUsuario.Length - 1);
            ActualizarPantalla();
        }
    }

    void ActualizarPantalla()
    {
        pantalla.text = entradaUsuario;
    }

    void VerificarClave()
    {
        if (entradaUsuario == clavePredeterminada)
        {
            AudioWrong.clip = CompleteSound;
            AudioWrong.Play();

            PuzzleScipt.isDone = true;
            pantalla.color = Color.green;
            CorrectMaterial.SetFloat("_Intensity", 1);
            return;
            // ActivarObjeto(objetoCorrecto);
        }

        singlecoroutine = StartCoroutine(Incorrecto());

        // Limpiar la entrada después de verificar
        entradaUsuario = "";
    }

    IEnumerator Incorrecto()
    {
        AudioWrong.clip = WrongSound;
        AudioWrong.Play();

        pantalla.color = Color.red;
        WrongMaterial.SetFloat("_Intensity", 1);

        yield return new WaitForSeconds(1.5f);

        WrongMaterial.SetFloat("_Intensity", 0);
        pantalla.color = Color.white;
        ActualizarPantalla();
        ButtonEnable = true;
    }

    //public void AgregarDigito(string digito)
    //{
    //    if (entradaUsuario.Length > LongitudPassword)
    //        return;

    //    entradaUsuario += digito;
    //    ActualizarPantalla();
    //}

    //public void BorrarDigito()
    //{
    //    if (entradaUsuario.Length < 0)
    //    {
    //        return;
    //    }

    //    entradaUsuario = entradaUsuario.Substring(0, entradaUsuario.Length - 1);
    //    ActualizarPantalla();
    //}
}
