using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo_Ver_Gestión_Efectos_Sonido : MonoBehaviour
{
    public AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemigo_Ver"))
        { 
            audioSource.volume = 0.5f;
            audioSource.pitch = 0.5f;
            audioSource.Play();
        }
    }

}
