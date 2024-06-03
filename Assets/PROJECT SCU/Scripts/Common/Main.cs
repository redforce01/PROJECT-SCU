using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SCU
{
    public enum SceneType
    {
        None = 0,
        Empty = 1,
        Title = 2,
        Game = 3,
    }

    public class Main : MonoBehaviour
    {
        public static Main Instance { get; private set; } = null;
        public SceneBase CurrentSceneController => currentScene;


        private SceneBase currentScene = null;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void Initialize()
        {
            ChangeScene(SceneType.Title);
        }

        public void ChangeScene(SceneType sceneType)
        {
            IEnumerator sceneCoroutine = null;
            switch (sceneType)
            {
                case SceneType.Title:
                    sceneCoroutine = ChangeSceneAsync<TitleScene>(SceneType.Title);
                    break;
                case SceneType.Game:
                    sceneCoroutine = ChangeSceneAsync<GameScene>(SceneType.Game);
                    break;
            }
            StartCoroutine(sceneCoroutine);
        }

        private IEnumerator ChangeSceneAsync<T>(SceneType sceneType) where T : SceneBase
        {
            // To do : Show Loading UI

            // if Current Scene is not null, call OnEndScene and destroy it
            if (currentScene != null)
            {
                yield return currentScene.OnEndScene();
                Destroy(currentScene.gameObject);
            }

            // Load Empty Scene 
            var async = SceneManager.LoadSceneAsync(SceneType.Empty.ToString(), LoadSceneMode.Single);
            yield return new WaitUntil(() => async.isDone);

            // Create Scene GameObject and add Scene Component
            GameObject sceneGo = new GameObject(typeof(T).Name);
            sceneGo.transform.parent = transform;
            currentScene = sceneGo.AddComponent<T>();

            // Load Scene
            yield return StartCoroutine(currentScene.OnStartScene());

            // To do : Hide Loading UI
        }
    }
}
