using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCU
{
    public class InteractionUI_ListItem : MonoBehaviour
    {
        public string DataKey
        {
            set => key = value;
            get => key;
        }

        public string Message
        {
            set => text.text = value;
        }

        public bool IsSelected
        {
            set => selection.SetActive(value);
        }

        public GameObject selection;
        public TMPro.TextMeshProUGUI text;
        
        private string key;
    }
}
