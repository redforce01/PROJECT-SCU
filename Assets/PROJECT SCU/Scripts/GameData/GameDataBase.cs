using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCU
{
    [CreateAssetMenu(fileName = "GameDataBase", menuName = "SCU/GameDataBase", order = 1)]
    public class GameDataBase : ScriptableObject
    {
        public float characterMoveSpeed;
        public int dummyData_2;
        public string dummyDataValue;
    }
}
