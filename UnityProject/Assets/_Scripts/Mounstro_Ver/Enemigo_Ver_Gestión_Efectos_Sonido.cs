using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo_Ver_Gestión_Efectos_Sonido : MonoBehaviour
{
    string tagObjetivo = "Enemigo";  // El tag del objeto que desencadenará el sonido
    public AudioClip sonido;  // El sonido que se reproducirá

    private void OnTriggerEnter(Collider collision)
    {

        AudioSource.PlayClipAtPoint(sonido, transform.position);
        Debug.Log("Alcachofa");
        // Verificar si el objeto que entró tiene el tag deseado
        if (collision.gameObject.tag == tagObjetivo)
        {
            // Reproducir el sonido
            
        }
    }
}
