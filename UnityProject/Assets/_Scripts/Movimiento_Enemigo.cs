using UnityEngine;

public class Movimiento_Enemigo : MonoBehaviour
{
    // Variable pública para el prefab del objeto a mover
    public GameObject prefabObjetoAMover;

    // Variables públicas para las posiciones
    public GameObject posicionInicial;
    public GameObject posicion1;
    public GameObject posicion2;
    public GameObject posicion3;

    // Variable privada para el objeto a mover
    private GameObject objetoAMover;

    void Start()
    {
        // Instanciar el objeto a mover desde el prefab
        objetoAMover = Instantiate(prefabObjetoAMover, posicionInicial.transform.position, Quaternion.identity);

        // Llamar a la función CambiarPosicion cada 3 segundos
        InvokeRepeating("CambiarPosicionAutomatico", 0f, 3f);
    }

    // Función para cambiar la posición actual entre las tres posiciones
    void CambiarPosicion(GameObject objetoAMover)
    {
        if (objetoAMover.transform.position == posicionInicial.transform.position)
        {
            objetoAMover.transform.position = posicion1.transform.position;
        }
        else if (objetoAMover.transform.position == posicion1.transform.position)
        {
            objetoAMover.transform.position = posicion2.transform.position;
        }
        else if (objetoAMover.transform.position == posicion2.transform.position)
        {
            objetoAMover.transform.position = posicion3.transform.position;
        }
        else
        {
            // Si llega a la última posición, detener el movimiento
            DetenerMovimiento();
        }
    }

    // Función para cambiar la posición automáticamente cada 3 segundos
    void CambiarPosicionAutomatico()
    {
        if (!DetectarColliderConTag("Luz"))
        {
            CambiarPosicion(objetoAMover);
        }
    }

    // Función para detener el movimiento
    void DetenerMovimiento()
    {
        // Puedes realizar acciones adicionales aquí si es necesario
        CancelInvoke("CambiarPosicionAutomatico"); // Detiene la repetición de la función
    }

    // Función para detectar collider con un tag específico
    bool DetectarColliderConTag(string tag)
    {
        Collider[] colliders = Physics.OverlapSphere(objetoAMover.transform.position, 1f); // Puedes ajustar el radio según tus necesidades
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(tag))
            {
                return true;
            }
        }
        return false;
    }
}