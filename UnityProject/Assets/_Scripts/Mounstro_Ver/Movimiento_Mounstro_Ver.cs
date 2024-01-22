using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movimiento_Mounstro_Ver : MonoBehaviour
{
    public float velocidadDesplazamiento = 1.0f;
    public float alturaMaxima = -5.0f;
    public float tiempoLimite = 3f;
    public float tiempoDetenido = 5f;
    public float tiempoSinMirarMinimo = 2f;

    public Transform posicionInicial;
    public List<Transform> posicionesIntermedias;
    public GameObject objetoADesactivar; // Nuevo objeto a desactivar

    private int posicionActualIndex = 0;
    private bool enMovimientoAscendente = true;

    private bool playerMirando = false;
    private float tiempoDetectado = 0f;
    private float tiempoDetenidoActual = 0f;

    private bool detenerMovimiento = false;

    private Coroutine secondCoroutine; // Nueva corutina
    private bool seCompletoRecorridoInicial = false; // Nueva variable

    private Animator myAnim;

    void Start()
    {
        if (posicionesIntermedias.Count == 0)
        {
            Debug.LogError("Agrega al menos un Transform de posición intermedia en la lista.");
            return;
        }

        if (posicionInicial == null)
        {
            Debug.LogError("Asigna una posición inicial al objeto.");
            return;
        }
        if (myAnim == null)
        {
            myAnim = GetComponent<Animator>();
        }
        StartCoroutine(DesplazarHaciaPosicionesIntermedias());
    }

    void Update()
    {
        // Activar la segunda corutina si se presiona la tecla F y se ha completado el recorrido inicial
        if (Input.GetKeyDown(KeyCode.F) && seCompletoRecorridoInicial && secondCoroutine == null)
        {
            secondCoroutine = StartCoroutine(RecorridoDescendente());
        }

        // Verificar si ha pasado el tiempo mínimo sin mirar y el jugador no está presente
        if (seCompletoRecorridoInicial && Time.time - tiempoDetectado >= tiempoSinMirarMinimo && !playerMirando)
        {
            // Desactivar el objeto
            if (objetoADesactivar != null)
            {
                objetoADesactivar.SetActive(false);
            }
        }
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
            tiempoDetectado = 0f;
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
                    enMovimientoAscendente = false;
                    posicionActualIndex = posicionesIntermedias.Count - 1;

                    // Activar el primer trigger solo si no se ha completado el recorrido inicial antes
                    if (!seCompletoRecorridoInicial)
                    {
                        Debug.Log("InvokeAnimation");
                        myAnim.SetTrigger("Invoke_Animation");
                        yield return new WaitForSeconds(tiempoDetenido);
                        tiempoDetenidoActual = Time.time;

                        while (Time.time - tiempoDetenidoActual < tiempoSinMirarMinimo)
                        {
                            if (!playerMirando)
                            {
                                tiempoDetenidoActual = 0f;
                                break;
                            }
                            yield return null;
                        }

                        if (Time.time - tiempoDetectado >= tiempoSinMirarMinimo && !playerMirando)
                        {
                            // Desactivar el objeto
                            if (objetoADesactivar != null)
                            {
                                objetoADesactivar.SetActive(false);
                            }
                            yield break;
                        }

                        // Se ha completado el recorrido inicial
                        seCompletoRecorridoInicial = true;
                    }

                    myAnim.SetTrigger("Leaving");
                    seCompletoRecorridoInicial = false;
                    break;


                }
            }
            else
            {
                posicionActualIndex--;
                if (posicionActualIndex < 0)
                {
                    enMovimientoAscendente = true;
                    posicionActualIndex = 0;
                    detenerMovimiento = true;
                }
            }

            yield return null;
        }



        StartCoroutine(DesplazarAPosicionInicial());
    }

    IEnumerator DesplazarAPosicionInicial()
    {

        posicionActualIndex = 0;
        Debug.Log("DesplazarPosicionInicial");
        transform.position = posicionInicial.position;
        tiempoDetectado = 0f;

        enMovimientoAscendente = true;
        detenerMovimiento = false;

        StartCoroutine(DesplazarHaciaPosicionesIntermedias());

        yield return null;  
    }

    IEnumerator RecorridoDescendente()
    {
        int lastIndex = posicionesIntermedias.Count;

        // Recorrer desde la posición actual hasta la penúltima posición
        for (int i = 0; i < lastIndex; i++)
        {
            Vector3 destino = posicionesIntermedias[i].position;
            float distancia = Vector3.Distance(transform.position, destino);

            while (distancia > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, destino, velocidadDesplazamiento * Time.deltaTime);
                distancia = Vector3.Distance(transform.position, destino);
                yield return null;
            }
        }

        // Detenerse en la última posición durante 5 segundos
        yield return new WaitForSeconds(5f);

        // Verificar si está en contacto con el tag durante al menos 3 segundos
        float tiempoInicio = Time.time;
        while (Time.time - tiempoInicio < 3f)
        {
            if (playerMirando)
            {
                yield return null;
            }
            else
            {
                // Desactivar el otro objeto si no cumple la condición
                if (objetoADesactivar != null)
                {
                    objetoADesactivar.SetActive(false);
                }
                yield break;
            }
        }

        // Activar el trigger "Invoke_Animation" cuando el objeto esté en la última posición

        // Recorrer desde la última posición hasta la posición inicial
        for (int i = lastIndex - 1; i >= 0; i--)
        {
            Vector3 destino = posicionesIntermedias[i].position;
            float distancia = Vector3.Distance(transform.position, destino);

            while (distancia > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, destino, velocidadDesplazamiento * Time.deltaTime);
                distancia = Vector3.Distance(transform.position, destino);
                yield return null;
            }
        }

        // Reiniciar la segunda corutina para futuros usos
        // (o realizar cualquier otra acción que necesites después del recorrido)
        secondCoroutine = null;
    }
}
