using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SCU
{
    public class GameScene : SceneBase
    {
        public override IEnumerator OnStartScene()
        {
            var asyncSceneLoad = SceneManager.LoadSceneAsync(SceneType.Game.ToString(), LoadSceneMode.Single);
            yield return new WaitUntil(() => asyncSceneLoad.isDone);
        }

        public override IEnumerator OnEndScene()
        {
            yield return null;
        }
    }
}
