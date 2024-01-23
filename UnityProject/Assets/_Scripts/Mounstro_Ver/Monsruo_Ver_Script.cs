using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monsruo_Ver_Script : MonoBehaviour
{
    public static Monsruo_Ver_Script Instance;

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

    public AudioSource SpawnEnemySound;


    private void Awake()
    {
        Instance = this;
    }

    private void RestartValues()
    {
        tiempoDetectado = 0f;
        enemiesWaiting = false;
        posicionActualIndex = 0;
        boxCollider.enabled = false;
        tiempoDetectadoReferencia = 0f;
    }

    public void TriggearMosntruo()
    {
        RestartValues();
        SpawnEnemySound.Play();
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

        if (other.CompareTag("MainCamera"))
        {
            tiempoDetectadoReferencia = Time.time;
            tiempoDetectado = 0;
            
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (!enemiesWaiting)
        {
            return;
        }

        if (other.CompareTag("MainCamera"))
        {
            tiempoDetectado = Time.time - tiempoDetectadoReferencia;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
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
            if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
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
        StopCoroutine(coreCorutine);
    }

    public void DesactivarMonstruo()
    {
        StopCoroutine(coreCorutine);
        objectoAMover.SetActive(false);
    }
}
