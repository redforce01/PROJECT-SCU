using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCU
{
    [CreateAssetMenu(fileName = "TurretData", menuName = "SCU/TurretData", order = 1)]
    public class TurretData : ScriptableObject
    {
        public int turretId;
        public float range;
        public float damage;
        public float fireRate;
    }
}
