using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class ButtonFunction : MonoBehaviour
{
    public UnityEvent OnClick;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<XRSimpleInteractable>().selectEntered.AddListener(x => OnClickButton());
    }

    public void OnClickButton()
    {
        Debug.Log("Se Clickeo el boton");
        if (OnClick == null)
            return;

        OnClick.Invoke();
    }
}
