using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCU
{
    public class SplashLogic : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            for (int i = 0; i < detectedObjects.Count; i++)
            {
                Gizmos.DrawLine(transform.position, detectedObjects[i].transform.position);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        public List<GameObject> detectedObjects = new List<GameObject>();
        public float radius = 3f;


        [ContextMenu("Detect")]
        public void DetectObjectsBySphere()
        {
            detectedObjects.Clear();
            Collider[] overlappedObjects = Physics.OverlapSphere(transform.position, radius);
            for (int i = 0; i < overlappedObjects.Length; i++)
            {
                if (overlappedObjects[i].attachedRigidbody != null)
                {
                    overlappedObjects[i].attachedRigidbody.AddExplosionForce(1000f, transform.position, radius);
                }

                detectedObjects.Add(overlappedObjects[i].gameObject);
            }
        }
    }
}