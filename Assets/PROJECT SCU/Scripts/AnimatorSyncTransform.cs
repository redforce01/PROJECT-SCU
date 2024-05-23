using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCU
{
    public class AnimatorSyncTransform : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void OnAnimatorMove()
        {
            transform.root.position = animator.rootPosition;
            transform.root.rotation = animator.rootRotation;
        }
    }
}