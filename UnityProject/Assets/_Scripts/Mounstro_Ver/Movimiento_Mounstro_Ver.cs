using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movimiento_Mounstro_Ver : MonoBehaviour
{
    public float velocidadDesplazamiento = 1.0f;
    public float alturaMaxima = -5.0f;
    public float tiempoLimite = 3f;
    public float tiempoDetenido = 5f;
    public int tiempoSinMirarMinimo = 3;

    public Transform posicionInicial;  // Nueva variable para la posición inicial

    public List<Transform> posicionesIntermedias;  // Lista de Transform que representan las posiciones intermedias
    private int posicionActualIndex = 0;
    private bool enMovimientoAscendente = true;

    private bool playerMirando = false;
    private float tiempoDetectado = 0f;
    private float tiempoDetenidoActual = 0f;

    private bool detenerMovimiento = false;

    void Start()
    {
        if (posicionesIntermedias.Count == 0)
        {
            Debug.LogError("Agrega al menos un Transform de posición intermedia en la lista.");
            return;
        }

        // Verificar si se ha asignado la posición inicial
        if (posicionInicial == null)
        {
            Debug.LogError("Asigna una posición inicial al objeto.");
            return;
        }

        StartCoroutine(DesplazarHaciaPosicionesIntermedias());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player_Mirando") && posicionActualIndex == posicionesIntermedias.Count - 1)
        {
            playerMirando = true;
            tiempoDetectado = Time.time;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player_Mirando"))
        {
            playerMirando = false;
            tiempoDetectado = 0f; // Reiniciar el temporizador al dejar de mirar
        }
    }

    IEnumerator DesplazarHaciaPosicionesIntermedias()
    {
        while (posicionActualIndex >= 0 && posicionActualIndex < posicionesIntermedias.Count)
        {
            if (!detenerMovimiento)
            {
                Vector3 destino = posicionesIntermedias[posicionActualIndex].position;
                float distancia = Vector3.Distance(transform.position, destino);

                while (distancia > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, destino, velocidadDesplazamiento * Time.deltaTime);
                    distancia = Vector3.Distance(transform.position, destino);
                    yield return null;
                }
            }

            if (enMovimientoAscendente)
            {
                posicionActualIndex++;
                if (posicionActualIndex == posicionesIntermedias.Count)
                {
                    // Alcanzó la última posición intermedia, detenerse
                    enMovimientoAscendente = false;
                    posicionActualIndex = posicionesIntermedias.Count - 1;

                    yield return new WaitForSeconds(tiempoDetenido);
                    tiempoDetenidoActual = Time.time;

                    // Verificar si la tag "Player_Mirando" no ha sido detectada durante el tiempo mínimo requerido
                    while (Time.time - tiempoDetenidoActual < tiempoSinMirarMinimo)
                    {
                        if (!playerMirando)
                        {
                            tiempoDetenidoActual = 0f;
                            break;
                        }
                        yield return null;
                    }

                    // Desactivar el objeto si la tag "Player_Mirando" no se detecta durante el tiempo mínimo
                    if (!playerMirando)
                    {
                        gameObject.SetActive(false);
                        yield break;
                    }
                }
            }
            else
            {
                posicionActualIndex--;
                if (posicionActualIndex < 0)
                {
                    // Volvió a la primera posición, reiniciar el movimiento ascendente
                    enMovimientoAscendente = true;
                    posicionActualIndex = 0;

                    // Detener el movimiento al volver a la primera posición
                    detenerMovimiento = true;
                }
            }

            yield return null;
        }

        // Llegó al final del recorrido, volver a la posición inicial
        StartCoroutine(DesplazarAPosicionInicial());
    }

    IEnumerator DesplazarAPosicionInicial()
    {
        float tiempoInicio = Time.time;
        Vector3 posicionActual = transform.position;

        while (Time.time - tiempoInicio < tiempoLimite)
        {
            float t = (Time.time - tiempoInicio) / tiempoLimite;
            transform.position = Vector3.Lerp(posicionActual, posicionInicial.position, t);
            yield return null;
        }

        transform.position = posicionInicial.position;
        tiempoDetectado = 0f; // Reiniciar el temporizador al volver a la posición inicial

        // Reiniciar la dirección y recorrer las posiciones en sentido ascendente
        enMovimientoAscendente = true;
        detenerMovimiento = false; // Permitir el movimiento nuevamente

        StartCoroutine(DesplazarHaciaPosicionesIntermedias());
    }
}
