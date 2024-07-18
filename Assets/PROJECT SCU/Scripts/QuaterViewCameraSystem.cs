using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SCU
{
    public class QuaterViewCameraSystem : MonoBehaviour
    {
        public float cameraRotationSpeed = 3f;
        public float cameraMoveSpeed = 3f;

        public Camera mainCamera;
        public Transform cameraPivot;
        public Cinemachine.CinemachineVirtualCamera quaterViewCamera;


        public float cameraBorderThickness = 0.2f;

        private void Update()
        {
            float rotateDirection = 0f;
            if (Input.GetKey(KeyCode.Q))
            {
                rotateDirection = -1;
                Vector3 originalRot = quaterViewCamera.transform.localRotation.eulerAngles;
                quaterViewCamera.transform.localRotation = Quaternion.Euler(originalRot +
                    (Vector3.up * rotateDirection * cameraRotationSpeed * Time.deltaTime));
            }

            if (Input.GetKey(KeyCode.E))
            {
                rotateDirection = 1;
                Vector3 originalRot = quaterViewCamera.transform.localRotation.eulerAngles;
                quaterViewCamera.transform.localRotation = Quaternion.Euler(originalRot +
                    (Vector3.up * rotateDirection * cameraRotationSpeed * Time.deltaTime));
            }


            Vector3 cameraForward = mainCamera.transform.forward;
            Vector3 cameraRight = mainCamera.transform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;

            if (Input.GetKey(KeyCode.W))
            {
                cameraPivot.Translate(cameraForward * cameraMoveSpeed * Time.deltaTime, Space.World);
            }

            if (Input.GetKey(KeyCode.S))
            {
                cameraPivot.Translate(cameraForward * cameraMoveSpeed * (-1) * Time.deltaTime, Space.World);
            }

            if (Input.GetKey(KeyCode.A))
            {
                cameraPivot.Translate(cameraRight * cameraMoveSpeed * (-1) * Time.deltaTime, Space.World);
            }

            if (Input.GetKey(KeyCode.D))
            {
                cameraPivot.Translate(cameraRight * cameraMoveSpeed * Time.deltaTime, Space.World);
            }



            // Mouse Screen Border Check
            Vector3 mousePosition = Input.mousePosition;
            Vector3 viewportMousePosition = mainCamera.ScreenToViewportPoint(mousePosition);

            // Left Check
            if (viewportMousePosition.x < cameraBorderThickness)
            {
                // To do : Camera Move To Left
                cameraPivot.Translate(cameraRight * cameraMoveSpeed * (-1) * Time.deltaTime, Space.World);

                // Top
                if (viewportMousePosition.y > 1 - cameraBorderThickness)
                {
                    // Top Left
                    cameraPivot.Translate(cameraForward * cameraMoveSpeed * Time.deltaTime, Space.World);
                }
                // Bottom
                else if (viewportMousePosition.y < cameraBorderThickness)
                {
                    // Bottom Left
                    cameraPivot.Translate(cameraForward * cameraMoveSpeed * (-1) * Time.deltaTime, Space.World);
                }
            }
            // Right Check
            else if (viewportMousePosition.x > 1 - cameraBorderThickness)
            {
                // To do : Camera Move To Right
                cameraPivot.Translate(cameraRight * cameraMoveSpeed * Time.deltaTime, Space.World);
                // Top
                if (viewportMousePosition.y > 1 - cameraBorderThickness)
                {
                    // Top Left
                    cameraPivot.Translate(cameraForward * cameraMoveSpeed * Time.deltaTime, Space.World);
                }
                // Bottom
                else if (viewportMousePosition.y < cameraBorderThickness)
                {
                    // Bottom Left
                    cameraPivot.Translate(cameraForward * cameraMoveSpeed * (-1) * Time.deltaTime, Space.World);
                }
            }
            // Top Check
            else if (viewportMousePosition.y > 1 - cameraBorderThickness)
            {
                // To do : Camera Move To Top
                cameraPivot.Translate(cameraForward * cameraMoveSpeed * Time.deltaTime, Space.World);
                // Left Check
                if (viewportMousePosition.x < cameraBorderThickness)
                {
                    cameraPivot.Translate(cameraRight * cameraMoveSpeed * (-1) * Time.deltaTime, Space.World);
                }
                // Right Check
                else if (viewportMousePosition.x > 1 - cameraBorderThickness)
                {
                    cameraPivot.Translate(cameraRight * cameraMoveSpeed * Time.deltaTime, Space.World);
                }
            }
            // Bottom Check
            else if (viewportMousePosition.y < cameraBorderThickness)
            {
                // To do : Camera Move To Bottom
                cameraPivot.Translate(cameraForward * cameraMoveSpeed * (-1) * Time.deltaTime, Space.World);
                // Left Check
                if (viewportMousePosition.x < cameraBorderThickness)
                {
                    cameraPivot.Translate(cameraRight * cameraMoveSpeed * (-1) * Time.deltaTime, Space.World);
                }
                // Right Check
                else if (viewportMousePosition.x > 1 - cameraBorderThickness)
                {
                    cameraPivot.Translate(cameraRight * cameraMoveSpeed * Time.deltaTime, Space.World);
                }
            }
        }
    }
}
