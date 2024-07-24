using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCU
{
    public class GameDataModel : SingletonBase<GameDataModel>
    {
        public List<ItemBase> ItemDatas = new List<ItemBase>();

        public WeaponItemData GetWeaponItemData(int itemId)
        {
            ItemBase targetItemData = ItemDatas.Find(x => x.itemId == itemId);
            if (targetItemData is WeaponItemData)
            {
                return targetItemData as WeaponItemData;
            }

            return null;
        }
    }
}
