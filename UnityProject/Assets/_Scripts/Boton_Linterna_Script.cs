using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Boton_Linterna_Script : MonoBehaviour
{
    public float velocidadSubida = 2f;
    public float velocidadBajada = 2f;
    public float tiempoDeDesplazamiento = 5f;
    public List<Material> materiales;
    public GameObject objetoADesactivar;

    private float tiempoPasado = 0f;
    private Vector3 posicionInicial;

    private enum EstadoDesplazamiento
    {
        Subiendo,
        Esperando,
        Bajando
    }

    private EstadoDesplazamiento estado = EstadoDesplazamiento.Subiendo;
    private Renderer objetoRenderer;

    // Nuevo evento para encapsular la funcionalidad
    [System.Serializable]
    public class DesplazamientoCompletoEvent : UnityEvent { }

    public DesplazamientoCompletoEvent desplazamientoCompletoEvent;

    void Start()
    {
        posicionInicial = transform.position;
        objetoRenderer = GetComponent<Renderer>();

        if (materiales.Count >= 1)
        {
            objetoRenderer.material = materiales[0];
        }
        else
        {
            Debug.LogError("La lista de materiales debe contener al menos 1 material.");
        }
    }

    void Update()
    {
        switch (estado)
        {
            case EstadoDesplazamiento.Subiendo:
                DesplazamientoHaciaArriba();
                break;

            case EstadoDesplazamiento.Esperando:
                EsperarYBajar();
                break;

            case EstadoDesplazamiento.Bajando:
                DesplazamientoHaciaAbajo();
                break;
        }
    }

    public void DesplazamientoHaciaArriba()
    {
        float desplazamientoY = velocidadSubida * Time.deltaTime;
        transform.position += new Vector3(0f, desplazamientoY, 0f);

        tiempoPasado += Time.deltaTime;

        if (tiempoPasado >= tiempoDeDesplazamiento)
        {
            estado = EstadoDesplazamiento.Esperando;

            if (materiales.Count >= 2)
            {
                objetoRenderer.material = materiales[1];
            }

            if (objetoADesactivar != null)
            {
                objetoADesactivar.SetActive(false);
            }

            tiempoPasado = 0f;

            // Disparar el evento cuando se completa el desplazamiento hacia arriba
            desplazamientoCompletoEvent.Invoke();
        }
    }

    void EsperarYBajar()
    {
        if (Input.GetMouseButtonDown(1))
        {
            tiempoPasado = 0f;
            estado = EstadoDesplazamiento.Bajando;

            if (objetoADesactivar != null)
            {
                objetoADesactivar.SetActive(true);
            }
        }
    }

    void DesplazamientoHaciaAbajo()
    {
        float desplazamientoY = velocidadBajada * Time.deltaTime;
        transform.position -= new Vector3(0f, desplazamientoY, 0f);

        if (transform.position.y <= posicionInicial.y)
        {
            transform.position = posicionInicial;
            estado = EstadoDesplazamiento.Subiendo;

            if (materiales.Count >= 1)
            {
                objetoRenderer.material = materiales[0];
            }

            tiempoPasado = 0f;
        }
    }
}
