using UnityEngine;

public class Logica_Mounstro_Ver : MonoBehaviour
{
    // Variable p�blica para almacenar el objeto en la escena
    public GameObject objetoParaActivar;

    void Start()
    {
            objetoParaActivar.SetActive(true);
    }
}
