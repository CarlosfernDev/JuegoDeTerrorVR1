using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Logica_Calculadora_UI : MonoBehaviour
{
    public GameObject objetoCorrecto; // Asigna este objeto en el Inspector de Unity
    public GameObject objetoIncorrecto; // Asigna este objeto en el Inspector de Unity

    private string clavePredeterminada = "123456789";
    private string entradaUsuario = "";

    public TMP_InputField pantalla;

    void Start()
    {
        // Asegúrate de que la pantalla no sea editable directamente
        pantalla.readOnly = true;
    }

    public void OnButtonPress(int digito)
    {
        if (entradaUsuario.Length < 9)
        {
            entradaUsuario += digito.ToString();
            ActualizarPantalla();

            if (entradaUsuario.Length == 9)
            {
                VerificarClave();
            }
        }
    }

    public void OnDeleteButtonPress()
    {
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
            Debug.Log("¡Clave correcta!");
            ActivarObjeto(objetoCorrecto);
        }
        else
        {
            Debug.Log("Clave incorrecta. Intenta de nuevo.");
            ActivarObjeto(objetoIncorrecto);
        }

        // Limpiar la entrada después de verificar
        entradaUsuario = "";
        ActualizarPantalla();
    }

    void ActivarObjeto(GameObject objeto)
    {
        if (objeto != null)
        {
            objeto.SetActive(true);
        }
        else
        {
            Debug.LogError("El objeto no está asignado en el Inspector.");
        }
    }
}
