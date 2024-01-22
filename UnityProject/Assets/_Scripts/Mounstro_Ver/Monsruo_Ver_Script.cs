using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monsruo_Ver_Script : MonoBehaviour
{
    private int posicionActualIndex;
    private float tiempoDetectado;

    private bool enemiesWaiting;

    public Animator myAnim;

    public GameObject objectoAMover;

    public Coroutine coreCorutine;

    public float tiempoEspera;

    public float tiempoMinimo;

    private float tiempoDetectadoReferencia;

    public BoxCollider boxCollider;




    // Start is called before the first frame update
    void Start()
    {
        TriggearMosntruo();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void RestartValues()
    {
        tiempoDetectado = 0f;
        enemiesWaiting = false;
        posicionActualIndex = 0;
        boxCollider.enabled = false;
        tiempoDetectadoReferencia = 0f;
    }

    private void TriggearMosntruo()
    {
        RestartValues();
        objectoAMover.SetActive(true);

        if(coreCorutine != null)
        {
            StopCoroutine(coreCorutine);
        }
        
        coreCorutine = StartCoroutine(RutinaMonstruo());
    }

    void OnTriggerEnter(Collider other)
    {
        if (!enemiesWaiting)
        {
            return;
        }

        if (other.CompareTag("Player_Mirando"))
        {
            tiempoDetectadoReferencia = Time.time;
            tiempoDetectado = 0;
            
        }
    }


    private void OnTriggerStay(Collider other)
    {
        Debug.Log("TriggeStay");
        if (other.CompareTag("Player_Mirando"))
        {
            Debug.Log("TriggeStay");
            tiempoDetectado = Time.time - tiempoDetectadoReferencia;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player_Mirando"))
        {   
            tiempoDetectado = 0f;
        }
    }

    


    IEnumerator RutinaMonstruo()
    {
        myAnim.SetTrigger("Invoke_Animation");
        while (true)
        {
            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("In_Room"))
            {
                break;
            }
            yield return null;

        }

        enemiesWaiting = true;

        float timeReferencie = Time.time;
        boxCollider.enabled = true;
        while (true)
        {
            if (tiempoEspera <= Time.time - timeReferencie)
            {
                HasPerdido();
                Debug.Log("Te han matado");
                StopCoroutine(coreCorutine);  
            }

            if (tiempoMinimo < tiempoDetectado)
            {
                Debug.Log("asdhniasdfikln");
                break;
            }

            yield return null;
        }
        
        boxCollider.enabled = false;
        enemiesWaiting = false;
        myAnim.SetTrigger("Leaving");

        while (true)
        {
            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Joining"))
            {
                break;
            }
            yield return null;
        }

        DesactivarMonstruo();
    }



    public void HasPerdido()
    {
        boxCollider.enabled = false;
        DesactivarMonstruo();
    }

    public void DesactivarMonstruo()
    {
        objectoAMover.SetActive(false);
        TriggearMosntruo();
    }
}
