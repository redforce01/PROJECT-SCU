using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCU
{
    public class AnimationEventListener : MonoBehaviour
    {
        public System.Action<string> OnTakenAnimationEvent;

        public void OnAnimationEvent(string eventName)
        {
            OnTakenAnimationEvent?.Invoke(eventName);
        }
    }
}
