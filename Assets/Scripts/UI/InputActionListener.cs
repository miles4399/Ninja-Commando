using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class InputActionListener : MonoBehaviour
{
    // input to listen to
    [SerializeField] private InputActionReference _action;

    // custom Unity Event
    public UnityEvent OnPerformed;

    // we need to subscribe/unsubscribe to the action's performed event
    // we use OnEnable and OnDisable to subscribe/unsubscribe

    private void OnEnable()
    {
        // subscribe to action's "performed" event when InputActionListener is Enabled
        _action.action.performed += Performed;
    }

    private void OnDisable()
    {
        // unsubscribe to action's "performed" event when InputActionListener is Disabled
        _action.action.performed -= Performed;
    }

    private void Performed(InputAction.CallbackContext obj)
    {
        // execute OnPerformed event
        OnPerformed.Invoke();
    }
}