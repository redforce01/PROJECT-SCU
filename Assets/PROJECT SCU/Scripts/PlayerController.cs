using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace SCU
{
    public class PlayerController : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(aimTarget.position, 0.1f);
        }


        public bool IsEnableMovement
        {
            set => isEnableMovement = value;
        }

        [Header("Character Setting")]
        public float moveSpeed = 3.0f;
        public float sprintSpeed = 5.0f;
        public float speedChangeRate = 10.0f;

        [Header("Camera Setting")]
        public float cameraHorizontalSpeed = 2.0f;
        public float cameraVerticalSpeed = 2.0f;

        [Range(0.0f, 0.3f)] public float rotationSmoothTime = 0.12f;

        [Header("Sliding Setting")]
        public AnimationCurve slidingCurve;

        [Header("Weapon Holder")]
        public GameObject weaponHolder;

        [Header("Weapon FOV")]
        public float defaultFOV;
        public float aimFOV;

        [Header("Camera Clamping")]
        public float topClamp = 70.0f;
        public float bottomClamp = -30.0f;
        public GameObject cinemachineCameraTarget;
        public float cameraAngleOverride = 0.0f;


        [Header("Animation Rigging")]
        public Transform aimTarget;
        public LayerMask aimingLayer;
        public UnityEngine.Animations.Rigging.Rig aimingRig;

        public float aimingIKBlendCurrent;
        public float aimingIKBlendTarget;

        private Animator animator;
        private Camera mainCamera;
        private CharacterController controller;
        private InteractionSensor interactionSensor;
        private Weapon currentWeapon;
        private AnimationEventListener animationEventListener;

        private bool isSprint = false;
        private Vector2 move;
        private float speed;
        private float animationBlend;
        private float targetRotation = 0.0f;
        private float rotationVelocity;
        private float verticalVelocity;

        private Vector2 look;
        private const float _threshold = 0.01f;
        private float cinemachineTargetYaw;
        private float cinemachineTargetPitch;

        private bool isEnableMovement = true;
        private bool isStrafe = false;
        private bool isReloading = false; // Reload 중 인지 여부

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            controller = GetComponent<CharacterController>();
            mainCamera = Camera.main;
            interactionSensor = GetComponentInChildren<InteractionSensor>();
            animationEventListener = GetComponentInChildren<AnimationEventListener>();
            animationEventListener.OnTakenAnimationEvent += OnTakenAnimationEvent;

            var weaponGameObject = TransformUtility.FindGameObjectWithTag(weaponHolder, "Weapon");
            currentWeapon = weaponGameObject.GetComponent<Weapon>();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnEnable()
        {
            interactionSensor.OnDetected += OnDetectedInteraction;
            interactionSensor.OnLost += OnLostInteraction;
        }

        private void OnDetectedInteraction(IInteractable interactable)
        {
            InteractionUI.Instance.AddInteractionData(interactable);
        }

        private void OnLostInteraction(IInteractable interactable)
        {
            InteractionUI.Instance.RemoveInteractionData(interactable);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                InteractionUI.Instance.DoInteract();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                InteractionUI.Instance.SelectNext();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                InteractionUI.Instance.SelectPrev();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("Trigger_Slide");
            }

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            move = new Vector2(horizontal, vertical);

            //if (interactionSensor.HasInteractable)
            //{
            //    move = Vector2.zero;
            //}

            float hMouse = Input.GetAxis("Mouse X");
            float vMouse = Input.GetAxis("Mouse Y") * -1;
            look = new Vector2(hMouse, vMouse);

            isSprint = Input.GetKey(KeyCode.LeftShift);
            if (isSprint)
            {
                animator.SetFloat("MotionSpeed", 1.25f);
            }
            else
            {
                animator.SetFloat("MotionSpeed", 1f);
            }

            isStrafe = Input.GetKey(KeyCode.Mouse1); // Mouse Right Button
            if (isStrafe)
            {
                Vector3 cameraForward = Camera.main.transform.forward.normalized;
                cameraForward.y = 0;
                transform.forward = cameraForward;
            }

            if (Input.GetKey(KeyCode.Mouse0)) // Mouse Left Button
            {
                currentWeapon?.Shoot();

                var cameraForward = Camera.main.transform.forward.normalized;
                cameraForward.y = 0;
                transform.forward = cameraForward;
            }

            if (Input.GetKeyDown(KeyCode.Mouse1)) // Mouse Right Button Down
            {
                // Zoom In
                CameraSystem.Instance.TargetFOV = aimFOV;
                aimingIKBlendTarget = isReloading ? 0f : 1f;
            }

            if (Input.GetKeyUp(KeyCode.Mouse1)) // Mouse Right Button Up
            {
                // Zoom Out
                CameraSystem.Instance.TargetFOV = defaultFOV;
                aimingIKBlendTarget = 0f;
            }
                        
            if (Input.GetKeyDown(KeyCode.R))
            {
                // Reload
                animator.SetLayerWeight(1, 1f);
                animator.SetTrigger("Trigger_Reload");
                isReloading = true;
            }

            aimingIKBlendCurrent = Mathf.Lerp(aimingIKBlendCurrent, aimingIKBlendTarget, Time.deltaTime * 10f);
            aimingRig.weight = aimingIKBlendCurrent;

            Move();

            animator.SetFloat("Speed", animationBlend);
            animator.SetFloat("Horizontal", move.x);
            animator.SetFloat("Vertical", move.y);
            animator.SetFloat("Strafe", isStrafe ? 1 : 0);

            // 카메라의 ViewportPointToRay를 이용하여 화면 중앙을 바라보는 Ray를 생성
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            // Raycast가 실패하면 카메라의 정면으로 1000m 떨어진 지점을 설정
            // Raycast를 이용하여 화면 중앙을 바라보는 Ray와 충돌된 대상의 정보를 저장
            Vector3 aimingTargetPosition = Camera.main.transform.position + Camera.main.transform.forward * 1000f;

            // Raycast가 성공하면 aimingTargetPosition을 Raycast가 성공한 지점으로 설정
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000f, aimingLayer))
            {
                if (hitInfo.transform.root != transform)
                {
                    aimingTargetPosition = hitInfo.point;
                }
            }

            // aimingTargetPosition을 aimTarget의 위치로 설정
            aimTarget.position = aimingTargetPosition;
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (look.sqrMagnitude >= _threshold)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = 1.0f;

                cinemachineTargetYaw += look.x * deltaTimeMultiplier * cameraHorizontalSpeed;
                cinemachineTargetPitch += look.y * deltaTimeMultiplier * cameraVerticalSpeed;
            }

            // clamp our rotations so our values are limited 360 degrees
            cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
            cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

            // Cinemachine will follow this target
            cinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + cameraAngleOverride,
                cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            if (!isEnableMovement)
                return;

            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = isSprint ? sprintSpeed : moveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = move.magnitude;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * speedChangeRate);

                // round speed to 3 decimal places
                speed = Mathf.Round(speed * 1000f) / 1000f;
            }
            else
            {
                speed = targetSpeed;
            }

            animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);
            if (animationBlend < 0.01f) animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(move.x, 0.0f, move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (move != Vector2.zero)
            {
                targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                    mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
                    rotationSmoothTime);

                if (!isStrafe)
                {
                    // rotate to face input direction relative to camera position
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                }
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

            // move the player
            controller.Move(targetDirection.normalized * (speed * Time.deltaTime) +
                             new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        public void OnTakenAnimationEvent(string eventName)
        {
            if (eventName.Equals("Reload"))
            {
                isReloading = false;
                animator.SetLayerWeight(1, 0f);
            }
        }
    }
}

