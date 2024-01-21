using UnityEngine;
using UnityEngine.UI;

public class Cronometro : MonoBehaviour
{
    public float duracionEnMinutos = 10.0f;

    private Text textoCronometro;
    private float tiempoInicio;

    void Start()
    {
        textoCronometro = GetComponentInChildren<Text>();
        ReiniciarCronometro();
    }

    void Update()
    {
        float tiempoTranscurrido = Time.time - tiempoInicio;
        float tiempoRestante = Mathf.Max(0, duracionEnMinutos * 60 - tiempoTranscurrido);
        ActualizarCronometro(tiempoRestante);

        if (tiempoRestante <= 0)
        {
            // Aquí puedes realizar acciones adicionales cuando se agote el tiempo, como finalizar el juego.
            Debug.Log("¡Tiempo agotado!");
        }
    }

    void ActualizarCronometro(float tiempo)
    {
        int minutos = Mathf.FloorToInt(tiempo / 60);
        int segundos = Mathf.FloorToInt(tiempo % 60);
        float milisegundos = (tiempo * 100) % 100;

        textoCronometro.text = string.Format("{0:00}:{1:00}:{2:00}", minutos, segundos, milisegundos);
    }

    public void ReiniciarCronometro()
    {
        tiempoInicio = Time.time;
    }
}


