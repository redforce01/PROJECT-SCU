using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCU
{
    public class GameManager : MonoBehaviour
    {
        public bool isGameOver;

        public CharacterBase playerCharacter;

        private void Start()
        {
            playerCharacter.onCharacterDead += GameOverCheck;
        }

        public void GameOverCheck()
        {
            // To do : Game Over
            Debug.Log("Game Over");
        }
    }
}
