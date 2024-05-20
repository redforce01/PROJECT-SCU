using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCU
{
    public class LocomotionBehaviour : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var controller = animator.transform.root.GetComponent<PlayerController>();
            controller.AttackComboCount = 0;
            animator.SetInteger("ComboCount", 0);
        }
    }
}

