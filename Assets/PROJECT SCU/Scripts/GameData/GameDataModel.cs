using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCU
{
    public class GameDataModel : SingletonBase<GameDataModel>
    {
        public GameDataBase myDummyData;

        public List<TurretData> turretDatas = new List<TurretData>();
    }
}
