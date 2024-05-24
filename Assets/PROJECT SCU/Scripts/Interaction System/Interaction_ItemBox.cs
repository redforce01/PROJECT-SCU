using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCU
{
    public class Interaction_ItemBox : MonoBehaviour, IInteractable
    {
        public string Key => "ItemBox." + gameObject.GetHashCode();
        public string Message => "Pick Up";

        public void Interact()
        {
            Destroy(gameObject);

            InteractionUI.Instance.RemoveInteractionData(this);

            // To do : Add item to inventory
        }
    }
}
