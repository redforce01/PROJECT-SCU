using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCU
{
    public class InteractionUI : MonoBehaviour
    {
        public Transform root;
        public InteractionUI_ListItem itemPrefab;

        private List<InteractionUI_ListItem> createdItems = new List<InteractionUI_ListItem>();

        public void AddInteractionData(string dataKey)
        {
            var newListItem = Instantiate(itemPrefab);
            newListItem.gameObject.SetActive(true);

            createdItems.Add(newListItem);
        }

        public void RemoveInteractionData(string dataKey)
        {
            var targetItem = createdItems.Find(x => x.DataKey.Equals(dataKey));
            if(targetItem != null)
            {
                createdItems.Remove(targetItem);
                Destroy(targetItem.gameObject);
            }
        }
    }
}
