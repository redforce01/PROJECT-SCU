using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCU
{
    public class CharacterUI : MonoBehaviour
    {
        public Image hpBar;

        public CharacterBase linkedCharacter;

        private void Start()
        {
            linkedCharacter.onDamageCallback += RefreshHpBar; // Delegate에 Chain(체인)을 건다.
            linkedCharacter.onDamagedAction += RefreshHpBar; // Action에 Chain(체인)을 건다.
        }

        public void RefreshHpBar(float currentHp, float maxHp)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }
}
