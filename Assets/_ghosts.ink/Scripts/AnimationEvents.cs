using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvents : MonoBehaviour
{
    public UnityEvent[] action;

    public void CallEvent(int eventIndex)
    {
        action[eventIndex].Invoke();
    }
}
