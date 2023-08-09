using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    public event Action<EventType> OnEventTriggered;

    private void Awake()
    {
        #region ΩÃ±€≈Ê
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        #endregion
    }

    public void TriggerEvent(EventType eventType)
    {
        OnEventTriggered?.Invoke(eventType);
    }
}

public enum EventType
{
    UIJoystickUp,
    UIJoystickDown,
    UIJoystickLeft,
    UIJoystickRight
}
