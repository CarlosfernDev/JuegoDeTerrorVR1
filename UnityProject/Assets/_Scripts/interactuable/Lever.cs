using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    public Rigidbody _rb;

    private bool Energized = false;

    public UnityEvent ActionOnLever;

    private void FixedUpdate()
    {
        if (!Energized)
            return;

        if(ActionOnLever != null)
        {
            ActionOnLever.Invoke();
        }
    }

    public void RecargarEnergia()
    {
        Energized = true;

    }

    public void NoRecargar()
    {


        Energized = false;


    }

}
