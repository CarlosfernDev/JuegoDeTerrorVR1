using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstruoSabotajeIA : MonoBehaviour
{
    public Transform destino; // El destino al que el monstruo se moverá
    public float velocidadMovimiento = 2.0f;
    public float tiempoEspera = 2.0f;
    public GameObject objetoActivar; // Objeto a activar cuando el monstruo llega al destino

    private Vector3 posicionInicial;

    void Start()
    {
        // Almacena la posición inicial del monstruo
        posicionInicial = transform.position;

        // Inicia la corutina para el comportamiento del monstruo
        StartCoroutine(ComportamientoMonstruoCoroutine());
    }

    IEnumerator ComportamientoMonstruoCoroutine()
    {
        while (true)
        {
            // Mueve el monstruo hacia el destino usando Lerp
            float elapsedTime = 0f;
            while (elapsedTime < 1f)
            {
                transform.position = Vector3.Lerp(posicionInicial, destino.position, elapsedTime);
                elapsedTime += Time.deltaTime * velocidadMovimiento;
                yield return null;
            }

            // Monstruo ha llegado al destino
            transform.position = destino.position;

            // Activa el objeto si está asignado
            if (objetoActivar != null)
            {
                objetoActivar.SetActive(true);
            }

            // Espera un tiempo determinado en el destino
            yield return new WaitForSeconds(tiempoEspera);

            // Mueve el monstruo de regreso a la posición inicial usando Lerp
            elapsedTime = 0f;
            while (elapsedTime < 1f)
            {
                transform.position = Vector3.Lerp(destino.position, posicionInicial, elapsedTime);
                elapsedTime += Time.deltaTime * velocidadMovimiento;
                yield return null;
            }

            // Monstruo ha vuelto a la posición inicial
            transform.position = posicionInicial;

            // Desactiva el objeto si está asignado
            if (objetoActivar != null)
            {
                objetoActivar.SetActive(false);
            }

            // Espera un tiempo determinado en la posición inicial
            yield return new WaitForSeconds(tiempoEspera);
        }
    }
}
