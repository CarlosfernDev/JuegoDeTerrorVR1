using UnityEngine;

public class Logica_Mounstro_Ver : MonoBehaviour
{
    // Variable p�blica para almacenar el objeto en la escena
    public GameObject objetoParaActivar;

    void Start()
    {
        // Verificar si se asign� un objeto a la variable p�blica
        if (objetoParaActivar != null)
        {
            // Activar el objeto en la escena
            objetoParaActivar.SetActive(true);

            // Puedes realizar m�s acciones con el objetoParaActivar si es necesario
            // Por ejemplo, cambiar su escala, rotaci�n, etc.
        }
        else
        {
            Debug.LogError("Objeto para activar no asignado. Asigna un objeto en el Inspector.");
        }
    }
}
