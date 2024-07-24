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

        public List<Vector3> recoilShakePattern = new List<Vector3>();
        private int currentRecoilIndex = 0;

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
                newBullet.transform.SetPositionAndRotation(firePosition.position, firePosition.rotation);
                newBullet.gameObject.SetActive(true);

                // 두 오브젝트가 서로 충돌처리가 되지 않게 Unity Physics Engine에게 무시하도록 지시
                Physics.IgnoreCollision(newBullet.GetComponent<Collider>(), transform.root.GetComponent<Collider>());

                Vector3 velocity = recoilShakePattern[currentRecoilIndex];
                currentRecoilIndex++;
                if (currentRecoilIndex >= recoilShakePattern.Count)
                {
                    currentRecoilIndex = currentRecoilIndex = 0;
                }

                CameraSystem.Instance.ShakeCamera(velocity, 0.2f, 1);
            }
        }
    }
}
