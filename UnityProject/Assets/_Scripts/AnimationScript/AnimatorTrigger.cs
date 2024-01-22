using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTrigger : MonoBehaviour
{
    public Animator myanimator;

    public void SetTrigger(string value)
    {
        myanimator.SetTrigger(value);
    }

}
