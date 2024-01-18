using UnityEngine;

public class Movimiento_Enemigo : MonoBehaviour
{
    // Variable p�blica para el prefab del objeto a mover
    public GameObject prefabObjetoAMover;

    // Variables p�blicas para las posiciones
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

        // Llamar a la funci�n CambiarPosicion cada 3 segundos
        InvokeRepeating("CambiarPosicionAutomatico", 0f, 3f);
    }

    // Funci�n para cambiar la posici�n actual entre las tres posiciones
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
            // Si llega a la �ltima posici�n, detener el movimiento
            DetenerMovimiento();
        }
    }

    // Funci�n para cambiar la posici�n autom�ticamente cada 3 segundos
    void CambiarPosicionAutomatico()
    {
        if (!DetectarColliderConTag("Luz"))
        {
            CambiarPosicion(objetoAMover);
        }
    }

    // Funci�n para detener el movimiento
    void DetenerMovimiento()
    {
        // Puedes realizar acciones adicionales aqu� si es necesario
        CancelInvoke("CambiarPosicionAutomatico"); // Detiene la repetici�n de la funci�n
    }

    // Funci�n para detectar collider con un tag espec�fico
    bool DetectarColliderConTag(string tag)
    {
        Collider[] colliders = Physics.OverlapSphere(objetoAMover.transform.position, 1f); // Puedes ajustar el radio seg�n tus necesidades
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