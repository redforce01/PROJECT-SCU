using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCU
{
    [CreateAssetMenu(fileName = "ItemBase", menuName = "SCU/ItemBase")]
    public class ItemBase : ScriptableObject
    {
        public int itemId;

        public string itemName;
        public Sprite itemIcon;
    }
}
