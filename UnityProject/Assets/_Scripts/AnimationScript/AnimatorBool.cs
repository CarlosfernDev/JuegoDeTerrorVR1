using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorBool : MonoBehaviour
{
    public Animator myanimator;

    public void SwitchBool(string value)
    {
        myanimator.SetBool(value, !myanimator.GetBool(value));
    }

}
