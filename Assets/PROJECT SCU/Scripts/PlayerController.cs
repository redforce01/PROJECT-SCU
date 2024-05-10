using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SCU
{
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float rotateSpeed = 5f;
        public float lastMouseX = 0;

        private void Update()
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += transform.forward * moveSpeed * Time.deltaTime; // x = 0 y = 0 z = 1
            }

            if (Input.GetKey(KeyCode.S))
            {
                transform.position += transform.forward * (-1) * moveSpeed * Time.deltaTime; // x = 0 y = 0 z = -1
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.position += transform.right * (-1) * moveSpeed * Time.deltaTime; // x = -1 y = 0 z = 0
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.position += transform.right * moveSpeed * Time.deltaTime; // x = 1 y = 0 z = 0
            }

            bool isMouseLeftMoved = lastMouseX - Input.mousePosition.x < 0;
            bool isMouseMoved = Input.mousePosition.x != lastMouseX;
            if (isMouseMoved)
            {
                transform.Rotate(Vector3.up * rotateSpeed * (isMouseLeftMoved ? -1 : 1) * Time.deltaTime);
            }
        }

        private void LateUpdate()
        {
            lastMouseX = Input.mousePosition.x;
        }
    }
}