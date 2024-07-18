using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SCU.CharacterBase;

namespace SCU
{
    public class CharacterBase : MonoBehaviour
    {
        public float currentHP;
        public float maxHP;

        public delegate void OnDamage(float currentHP, float maxHP);
        public delegate void OnCharacterDead();

        public OnDamage onDamageCallbackEvent;
        public OnDamage onDamageCallback;
        public event OnCharacterDead onCharacterDead;

        public System.Action<float, float> onDamagedAction;
        public Action OnCharacterDeadAction;


        public void Damage(float damage)
        {
            currentHP -= damage;

            onDamageCallback(currentHP, maxHP);
            onDamagedAction(currentHP, maxHP);

            if (currentHP <= 0)
            {
                onCharacterDead();
                Destroy(gameObject);
            }
        }

        [ContextMenu("Damage Debug")]
        public void DamageDebugButton()
        {
            Damage(20);
        }
    }
}
