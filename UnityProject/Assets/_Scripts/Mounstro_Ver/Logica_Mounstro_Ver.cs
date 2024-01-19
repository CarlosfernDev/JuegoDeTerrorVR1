using UnityEngine;

public class Logica_Mounstro_Ver : MonoBehaviour
{
    // Variable pública para almacenar el objeto en la escena
    public GameObject objetoParaActivar;

    void Start()
    {
        // Verificar si se asignó un objeto a la variable pública
        if (objetoParaActivar != null)
        {
            // Activar el objeto en la escena
            objetoParaActivar.SetActive(true);

            // Puedes realizar más acciones con el objetoParaActivar si es necesario
            // Por ejemplo, cambiar su escala, rotación, etc.
        }
        else
        {
            Debug.LogError("Objeto para activar no asignado. Asigna un objeto en el Inspector.");
        }
    }
}
