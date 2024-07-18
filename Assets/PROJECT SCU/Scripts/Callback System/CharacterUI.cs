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
            linkedCharacter.onDamageCallback += RefreshHpBar; // Delegate�� Chain(ü��)�� �Ǵ�.
            linkedCharacter.onDamagedAction += RefreshHpBar; // Action�� Chain(ü��)�� �Ǵ�.
        }

        public void RefreshHpBar(float currentHp, float maxHp)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }
}
