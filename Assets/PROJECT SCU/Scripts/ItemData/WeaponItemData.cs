using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCU
{
    [CreateAssetMenu(fileName = "WeaponItemData", menuName = "SCU/WeaponItemData")]
    public class WeaponItemData : ItemBase
    {
        public float damage;
        public float range;
    }
}
