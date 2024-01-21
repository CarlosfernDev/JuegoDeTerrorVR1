using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Cronometro : MonoBehaviour
{
    public static Cronometro Instance;

    [SerializeField] private TMP_Text[] Textos;

    bool IsCounting;

    public float duracionEnMinutos = 10.0f;

    private float tiempoInicio;

    UnityEvent ActivarTrasAcabarTiempo;

    void Start()
    {
        ActualizarCronometro(0);

        Instance = this;
    }

    void Update()
    {
        if (!IsCounting)
            return;

        float tiempoTranscurrido = Time.time - tiempoInicio;
        float tiempoRestante = Mathf.Max(0, duracionEnMinutos * 60 - tiempoTranscurrido);
        ActualizarCronometro(tiempoRestante);

        if (tiempoRestante <= 0)
        {
            if(ActivarTrasAcabarTiempo != null)
            {
                ActivarTrasAcabarTiempo.Invoke();
            }
        }
    }

    void ActualizarCronometro(float tiempo)
    {
        int minutos = Mathf.FloorToInt(tiempo / 60);
        int segundos = Mathf.FloorToInt(tiempo % 60);
        float milisegundos = (tiempo * 100) % 100;

        foreach(TMP_Text texto in Textos)
        {
            texto.text = string.Format("{0:00}:{1:00}:{2:00}", minutos, segundos, milisegundos);
        }
    }

    public void ReiniciarCronometro()
    {
        IsCounting = true;

        tiempoInicio = Time.time;
    }


}


