using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCU
{
    public class Bullet : MonoBehaviour
    {
        public float lifeTime = 3.0f;

        private void Start()
        {
            Destroy(gameObject, lifeTime);
        }
    }
}