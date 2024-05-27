using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCU
{
    public class Weapon : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(firePosition.position, firePosition.position + firePosition.forward * range);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.forward * range);
        }

        public GameObject muzzlePrefab;
        public GameObject bulletPrefab;
        public Transform firePosition;

        public float fireRate = 0.1f;
        public float range = 100f;

        private float lastShootTime = 0f;

        public void Shoot()
        {
            if (Time.time > lastShootTime + fireRate)
            {
                // Possible Shoot 

                lastShootTime = Time.time;

                // Create Muzzle
                var newMuzzle = Instantiate(muzzlePrefab);
                newMuzzle.transform.SetPositionAndRotation(firePosition.position, firePosition.rotation);
                newMuzzle.gameObject.SetActive(true);
                Destroy(newMuzzle, 1f);

                // Create Bullet
                var newBullet = Instantiate(bulletPrefab);
                var cameraTransform = Camera.main.transform;
                newBullet.transform.SetPositionAndRotation(cameraTransform.position, cameraTransform.rotation);
                newBullet.gameObject.SetActive(true);

                // 두 오브젝트가 서로 충돌처리가 되지 않게 Unity Physics Engine에게 무시하도록 지시
                Physics.IgnoreCollision(newBullet.GetComponent<Collider>(), transform.root.GetComponent<Collider>());
            }
        }
    }
}
