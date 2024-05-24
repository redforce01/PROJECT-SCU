using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace SCU
{
    public class PlayerController : MonoBehaviour
    {
        public int AttackComboCount
        {
            set => attackComboCount = value;
        }

        public bool IsEnableMovement
        {
            set => isEnableMovement = value;
        }

        public float moveSpeed = 3.0f;
        public float sprintSpeed = 5.0f;
        public float speedChangeRate = 10.0f;

        public float cameraHorizontalSpeed = 2.0f;
        public float cameraVerticalSpeed = 2.0f;

        [Range(0.0f, 0.3f)] public float rotationSmoothTime = 0.12f;


        public AnimationCurve slidingCurve;


        public float topClamp = 70.0f;
        public float bottomClamp = -30.0f;
        public GameObject cinemachineCameraTarget;
        public float cameraAngleOverride = 0.0f;

        private Animator animator;
        private Camera mainCamera;
        private CharacterController controller;
        private InteractionSensor interactionSensor;

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

        private int attackComboCount = 0;
        private bool isEnableMovement = true;
        private bool isStrafe = false;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            controller = GetComponent<CharacterController>();
            mainCamera = Camera.main;
            interactionSensor = GetComponentInChildren<InteractionSensor>();
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
            isStrafe = Input.GetKey(KeyCode.Mouse1); // Mouse Right Button
            if (isStrafe)
            {
                Vector3 cameraForward = Camera.main.transform.forward.normalized;
                cameraForward.y = 0;
                transform.forward = cameraForward;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                // Attack !
                if (attackComboCount == 0)
                {
                    animator.SetTrigger("Trigger_Attack");
                    attackComboCount++;
                }
                else
                {
                    attackComboCount++;
                    animator.SetInteger("ComboCount", attackComboCount);
                }
            }

            Move();

            animator.SetFloat("Speed", animationBlend);
            animator.SetFloat("Horizontal", move.x);
            animator.SetFloat("Vertical", move.y);
            animator.SetFloat("Strafe", isStrafe ? 1 : 0);
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
    }
}

