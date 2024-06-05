using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SCU
{
    public class TitleScene : SceneBase
    {
        public override IEnumerator OnStartScene()
        {
            var asyncSceneLoad = SceneManager.LoadSceneAsync(SceneType.Title.ToString(), LoadSceneMode.Single);
            yield return new WaitUntil(() => asyncSceneLoad.isDone);

            UIManager.Show<TitleUI>(UIList.TitleUI);
        }

        public override IEnumerator OnEndScene()
        {
            yield return null;

            UIManager.Hide<TitleUI>(UIList.TitleUI);
        }
    }
}
