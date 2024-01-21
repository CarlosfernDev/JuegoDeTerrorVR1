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
    public Animator[] modelosAnimados; // Animadores de los modelos en 3D
    private int posicionActualIndex = 0;
    private bool enMovimientoAscendente = true;

    private bool playerMirando = false;
    private float tiempoDetectado = 0f;
    private float tiempoDetenidoActual = 0f;

    private bool detenerMovimiento = false;

    private Coroutine secondCoroutine; // Nueva corutina
    private bool seCompletoRecorridoInicial = false; // Nueva variable
    private bool reproduciendoSecuencia = false; // Variable para controlar la reproducción de la secuencia

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

        StartCoroutine(DesplazarHaciaPosicionesIntermedias());
    }

    void OnTriggerEnter(Collider other)
    {
        if (enMovimientoAscendente && !detenerMovimiento)
        {
            // Verificar si el objeto está en la última posición
            if (posicionActualIndex == posicionesIntermedias.Count - 1)
            {
                // Referenciar la corutina de otro script (ReproducirSiguienteAnimacion) aquí
                StartCoroutine(ControladorSecuencia.Instance.ReproducirSiguienteAnimacion());
            }
        }
    }

    IEnumerator ReproducirSecuencia()
    {
        reproduciendoSecuencia = true;

        // Lógica para reproducir la secuencia de animaciones
        foreach (var modeloAnimado in modelosAnimados)
        {
            // Obtén el componente Animator del modelo actual
            Animator animatorActual = modeloAnimado.GetComponent<Animator>();

            // Activa el modelo actual
            modeloAnimado.GetComponent<Renderer>().enabled = true;

            // Comienza su animación
            modeloAnimado.GetComponent<Animator>().SetTrigger("IniciarAnimacion");

            // Espera hasta que la animación actual haya terminado
            yield return new WaitForSeconds(animatorActual.GetCurrentAnimatorStateInfo(0).length);

            // Desactiva el modelo actual
            modeloAnimado.GetComponent<Renderer>().enabled = false;
        }

        // Finalizar la reproducción de la secuencia
        reproduciendoSecuencia = false;

        // Esperar un tiempo antes de reactivar las colisiones con el tag "Player_Mirando"
        yield return new WaitForSeconds(2f);

        // Restablecer la detección de jugador mirando
        playerMirando = false;
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
                        // Si ha pasado el tiempo mínimo sin mirar y el jugador no está presente, desactivar el objeto
                        gameObject.SetActive(false);
                        yield break;
                    }

                    // Se ha completado el recorrido inicial
                    seCompletoRecorridoInicial = true;
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
        tiempoDetectado = 0f;

        enMovimientoAscendente = true;
        detenerMovimiento = false;

        StartCoroutine(DesplazarHaciaPosicionesIntermedias());
    }

    void Update()
    {
        // Activar la segunda corutina si se ha completado el recorrido inicial
        if (seCompletoRecorridoInicial && secondCoroutine == null)
        {
            secondCoroutine = StartCoroutine(RecorridoDescendente());
        }
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
                // Si el jugador no está mirando, desactivar el objeto
                gameObject.SetActive(false);
                yield break;
            }
        }

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
