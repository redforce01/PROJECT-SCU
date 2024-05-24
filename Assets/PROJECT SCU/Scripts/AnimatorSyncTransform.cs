using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCU
{
    public class AnimatorSyncTransform : MonoBehaviour
    {
        private Animator animator;
        private PlayerController playerController;
        private float slidingTime = 0f;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            playerController = GetComponentInParent<PlayerController>();
        }

        private void OnAnimatorMove()
        {
            bool isSliding = animator.GetCurrentAnimatorStateInfo(0).IsName("Sliding");
            if (isSliding)
            {
                slidingTime += Time.deltaTime;
                float curveValue = playerController.slidingCurve.Evaluate(slidingTime / 2f);
                transform.root.position = animator.rootPosition + animator.deltaPosition * curveValue;
            }
            else
            {
                slidingTime = 0f;
                transform.root.position = animator.rootPosition;
                transform.root.rotation = animator.rootRotation;
            }
        }
    }
}