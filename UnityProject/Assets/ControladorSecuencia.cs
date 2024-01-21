using System.Collections;
using UnityEngine;

public class ControladorSecuencia : MonoBehaviour
{
    public GameObject[] modelos; // Asigna los modelos en el Inspector
    private int indiceModeloActual = 0;

    void Start()
    {
        // Activa el primer modelo y comienza su animaci�n
        modelos[indiceModeloActual].SetActive(true);
        StartCoroutine(ReproducirSiguienteAnimacion());
    }

    IEnumerator ReproducirSiguienteAnimacion()
    {
        // Obt�n el componente Animator del modelo actual
        Animator animatorActual = modelos[indiceModeloActual].GetComponent<Animator>();

        // Espera hasta que la animaci�n actual haya terminado
        yield return new WaitForSeconds(animatorActual.GetCurrentAnimatorStateInfo(0).length);

        // Desactiva el modelo actual
        modelos[indiceModeloActual].SetActive(false);

        // Incrementa el �ndice del modelo actual
        indiceModeloActual++;

        // Si hemos alcanzado el final de los modelos, reinicia desde el principio
        if (indiceModeloActual >= modelos.Length)
        {
            indiceModeloActual = 0;
        }

        // Activa el nuevo modelo y comienza su animaci�n
        modelos[indiceModeloActual].SetActive(true);

        // Reproduce la siguiente animaci�n en el nuevo modelo
        StartCoroutine(ReproducirSiguienteAnimacion());
    }
}
