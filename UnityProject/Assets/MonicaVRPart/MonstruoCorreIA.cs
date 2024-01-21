using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstruoIA : MonoBehaviour
{
    public float tiempoEsperaSonido = 5f;
    public float tiempoEsperaAtaque = 2f;
    public float tiempoEsperaPararse = 1f;
    public float margenAtaque = 2f;

    private bool atacando = false;

    private Vector3 posicionInicial;

    void Start()
    {
        StartCoroutine(MonstruoComportamiento());
    }

    IEnumerator MonstruoComportamiento()
    {
        while (true)
        {
            // Esperar hasta el próximo ataque
            yield return new WaitForSeconds(tiempoEsperaAtaque);

            // Reproducir sonido
            ReproducirSonido();

            // Esperar un tiempo antes de atacar
            yield return new WaitForSeconds(tiempoEsperaSonido);

            // Ponerse a correr (simulación de ataque)
            StartCoroutine(Atacar());

            // Esperar un tiempo antes de pararse
            yield return new WaitForSeconds(tiempoEsperaPararse);

            // Detener el ataque
            PararAtaque();
        }
    }

    void ReproducirSonido()
    {
        // Agrega aquí la lógica para reproducir el sonido del monstruo
        Debug.Log("Monstruo: ¡Grrr! Sonido aterrador");
    }

    IEnumerator Atacar()
    {
        Debug.Log("Monstruo: ¡Atacando!");

        // Calcular el tiempo de ataque
        float tiempoDeAtaque = Time.time + margenAtaque;

        // Posición actual del monstruo
        Vector3 posicionInicial = transform.position;

        // Posición del jugador (simulación, debes obtener la posición real del jugador)
        Vector3 posicionJugador = new Vector3(10f, 0f, 0f);

        while (Time.time < tiempoDeAtaque)
        {
            // Calcular el progreso del movimiento
            float progreso = 1 - ((tiempoDeAtaque - Time.time) / margenAtaque);

            // Mover el monstruo hacia la posición del jugador utilizando Lerp
            transform.position = Vector3.Lerp(posicionInicial, posicionJugador, progreso);

            // Pausa para la siguiente frame
            yield return null;
        }

        atacando = true;
    }

    void PararAtaque()
    {
        // Agrega aquí la lógica para detener el ataque del monstruo
        Debug.Log("Monstruo: ¡Parando ataque!");

        // Posición actual del monstruo
        Vector3 posicionActual = transform.position;

        // Lerp para volver a la posición inicial
        StartCoroutine(MoverMonstruo(posicionActual, posicionInicial, tiempoEsperaPararse));
    }

    IEnumerator MoverMonstruo(Vector3 posicionInicial, Vector3 posicionFinal, float tiempo)
    {
        float tiempoInicio = Time.time;

        while (Time.time < tiempoInicio + tiempo)
        {
            float progreso = (Time.time - tiempoInicio) / tiempo;
            transform.position = Vector3.Lerp(posicionInicial, posicionFinal, progreso);
            yield return null;
        }

        // Asegurarse de que la posición sea exactamente la final
        transform.position = posicionFinal;
    }

    // Puedes llamar a esta función desde otros scripts para detener la corutina
    public void DetenerMonstruo()
    {
        StopCoroutine("MonstruoComportamiento");
    }
}
