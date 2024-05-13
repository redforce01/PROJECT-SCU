using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SCU
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Bullet Setting")]
        public GameObject bulletPrefab;
        public Transform bulletSpawnPoint;
        public float bulletSpeed = 10.0f;
        public float fireRate = 0.1f; // 사격 속도를 의미
        private float fireTime = 0.0f; // 내가 다음 사격을 할 수 있는 시간을 의미


        [Header("Character Setting")]
        public float moveSpeed = 5.0f;

        [Header("Camera Setting")]
        public Transform cameraPivot;
        public Cinemachine.AxisState xAxis;
        public Cinemachine.AxisState yAxis;

        private void LateUpdate()
        {
            // Camera Pivot Transform의 Euler Angle을 AxisState 값을 이용해서 회전을 시켰음
            cameraPivot.eulerAngles = new Vector3(yAxis.Value, cameraPivot.eulerAngles.y, cameraPivot.eulerAngles.z);

            // player controller component 가 붙어있는 transform (player) 객체의 rotation도 동일한 방향을 쳐다보도록 동기화 했음
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis.Value, transform.eulerAngles.z);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Mouse0)) // Mouse Left Button
            {
                // Fire Bullet
                if (Time.time > fireTime) // Time.time => Unity가 Play 되고 나서부터 경과된 시간을 의미
                {
                    var bullet = Instantiate(bulletPrefab);
                    bullet.transform.position = bulletSpawnPoint.position;

                    var bulletRigid = bullet.GetComponent<Rigidbody>();
                    bulletRigid.AddForce(bulletSpawnPoint.forward * bulletSpeed, ForceMode.Impulse);

                    fireTime = Time.time + fireRate;
                }
            }

            xAxis.Update(Time.deltaTime);
            yAxis.Update(Time.deltaTime);

            Vector3 dirForward = Camera.main.transform.forward;
            Vector3 dirRight = Camera.main.transform.right;
            dirForward.y = 0;
            dirRight.y = 0;


            Vector2 input = Vector2.zero;

            if (Input.GetKey(KeyCode.W))
            {
                input.y += 1;
            }

            if (Input.GetKey(KeyCode.S))
            {
                input.y -= 1;
            }

            if (Input.GetKey(KeyCode.A))
            {
                input.x -= 1;
            }

            if (Input.GetKey(KeyCode.D))
            {
                input.x += 1;
            }

            Vector3 vecMove = dirForward * input.y + dirRight * input.x;
            transform.position += vecMove * moveSpeed * Time.deltaTime;
        }
    }
}