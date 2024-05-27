using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCU
{
    public class Projectile : MonoBehaviour
    {
        public float speed = 30f;
        public float lifeTime = 10f;


        public GameObject metalImpactPrefab;
        public GameObject woodImpactPrefab;

        private void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        private void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            string physicMaterialName = collision.collider.material.name;
            string[] splitNames = physicMaterialName.Split(" ");
            string originalName = splitNames[0];

            if (originalName.Equals("PhysicMaterial_Metal")) // Impact Metal
            {
                // To do : Create Metal Impact 
                var newImpact = Instantiate(metalImpactPrefab);
                newImpact.transform.SetPositionAndRotation(collision.contacts[0].point, Quaternion.Euler(-collision.contacts[0].normal));
            }
            else if (originalName.Equals("PhysicMaterial_Wood")) // Impact Wood
            {
                // To do : Create Wood Impact
                var newImpact = Instantiate(woodImpactPrefab);
                newImpact.transform.SetPositionAndRotation(collision.contacts[0].point, Quaternion.Euler(-collision.contacts[0].normal));
            }

            Destroy(gameObject);
        }
    }
}
