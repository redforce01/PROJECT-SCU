using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCU
{
    public class CameraSystem : MonoBehaviour
    {
        public static CameraSystem Instance { get; private set; } = null;

        public float TargetFOV { get; set; } = 60.0f;


        public Cinemachine.CinemachineVirtualCamera playerCamera;

        public float zoomSpeed = 5.0f;

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void LateUpdate()
        {
            playerCamera.m_Lens.FieldOfView = Mathf.Lerp(playerCamera.m_Lens.FieldOfView, TargetFOV, zoomSpeed * Time.deltaTime);
        }

    }
}
