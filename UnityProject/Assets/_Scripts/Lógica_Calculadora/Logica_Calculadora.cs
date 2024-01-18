using UnityEngine;

public class Logica_Calculadora : MonoBehaviour
{
    private string clavePredeterminada = "123456789";
    private string entradaUsuario = "";

    void Update()
    {
        // Verifica si se hizo clic con el botón izquierdo del mouse
        if (Input.GetMouseButtonDown(0))
        {
            // Dispara un rayo desde la cámara hacia la posición del clic
            Ray rayo = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Verifica si el rayo colisiona con algún objeto
            if (Physics.Raycast(rayo, out hit))
            {
                // Verifica qué objeto fue clicado
                if (hit.collider.CompareTag("Boton"))
                {
                    // Es un botón numérico
                    string digito = hit.collider.gameObject.name.Replace("Button", "");
                    AgregarDigito(digito);
                }
                else if (hit.collider.CompareTag("BotonDelete"))
                {
                    // Es el botón de borrar
                    BorrarDigito();
                    Debug.Log("Boton Borrado");
                }
            }
        }
    }

    void AgregarDigito(string digito)
    {
        if (entradaUsuario.Length < 9)
        {
            entradaUsuario += digito;
            ActualizarPantalla();
        }
    }

    void BorrarDigito()
    {
        if (entradaUsuario.Length > 0)
        {
            entradaUsuario = entradaUsuario.Substring(0, entradaUsuario.Length - 1);
            ActualizarPantalla();
        }
    }

    void ActualizarPantalla()
    {
        // Implementa la lógica para actualizar la pantalla del modelo con la entrada del usuario
        // Puedes usar un TextMesh, cambiar el material, o cualquier otra forma que sea apropiada para tu modelo.
        // En este ejemplo, asumimos que el modelo tiene un componente TextMesh para mostrar la entrada.
        TextMesh pantalla = GetComponentInChildren<TextMesh>();
        if (pantalla != null)
        {
            pantalla.text = entradaUsuario;
        }

        // Luego, puedes llamar a VerificarClave() cuando sea necesario según tus requerimientos.
    }

    void VerificarClave()
    {
        if (entradaUsuario.Length == 9)
        {
            if (entradaUsuario == clavePredeterminada)
            {
                Debug.Log("¡Clave correcta!");
                // Aquí puedes realizar acciones adicionales cuando la clave es correcta.
            }
            else
            {
                Debug.Log("Clave incorrecta. Intenta de nuevo.");
            }
        }
        else
        {
            Debug.Log("La clave debe tener 9 dígitos.");
        }
    }
}
