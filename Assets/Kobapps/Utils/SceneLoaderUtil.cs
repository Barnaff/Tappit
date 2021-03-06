using UnityEngine;
using System.Collections;

namespace Kobapps
{
    public enum eSceneTransition
    {
        None,
        FadeOut,
        FadeOutFadeIn,
    }

    public class SceneLoaderutil 
    {

        #region Public

        public static void LoadSceneAsync(string sceneName, System.Action sceneLoadedAction, eSceneTransition sceneTransition = eSceneTransition.None)
        {
            CreateSceneLoaderController().LoadSceneAsync(sceneName, sceneLoadedAction, sceneTransition);
        }

        public static void LoadSceneAsync(int sceneBuildIndex, System.Action sceneLoadedAction, eSceneTransition sceneTransition = eSceneTransition.None)
        {
            CreateSceneLoaderController().LoadSceneAsync(sceneBuildIndex, sceneLoadedAction, sceneTransition);
        }

        public static void LoadSceneAddativeAsync(string sceneName, System.Action sceneLoadedAction, eSceneTransition sceneTransition = eSceneTransition.None)
        {
            CreateSceneLoaderController().LoadSceneAddativeAsync(sceneName, sceneLoadedAction, sceneTransition);
        }

        public static void LoadSceneAddativeAsync(int sceneBuildIndex, System.Action sceneLoadedAction, eSceneTransition sceneTransition = eSceneTransition.None)
        {
            CreateSceneLoaderController().LoadSceneAddativeAsync(sceneBuildIndex, sceneLoadedAction, sceneTransition);
        }

        public static void ReloadCurrentScene(System.Action sceneLoadedAction, eSceneTransition sceneTransition = eSceneTransition.None)
        {
            CreateSceneLoaderController().LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex, sceneLoadedAction, sceneTransition);
        }

        public static void UnloadSceneAsync(int sceneBuildIndex)
        {
            Debug.Log("unload scene : " + sceneBuildIndex);
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneBuildIndex);
        }

        #endregion


        #region Private

        private static SceneLoaderController CreateSceneLoaderController()
        {
            GameObject sceneLoaderContainer = new GameObject();
            sceneLoaderContainer.name = "Scene Loader";
            SceneLoaderController sceneLoaderController = sceneLoaderContainer.AddComponent<SceneLoaderController>();
            GameObject.DontDestroyOnLoad(sceneLoaderContainer);
            return sceneLoaderController;
        }

        #endregion
    }


    internal class SceneLoaderController : MonoBehaviour
    {
        private float _fadeValue = 0;
        private Texture2D _overlayTexture = null;
        private const float _fadeSpeed = 1f;
        private bool _displayOverlay = false;
        private float _fadeDirection = 1;

        public void LoadSceneAsync(string sceneName, System.Action sceneLoadedAction, eSceneTransition sceneTransition)
        {
            int sceneBuildIndex = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName).buildIndex;
            StartCoroutine(LoadSceenCorutine(sceneBuildIndex, false, sceneTransition, sceneLoadedAction));
        }

        public void LoadSceneAsync(int sceneBuildIndex, System.Action sceneLoadedAction, eSceneTransition sceneTransition)
        {
            StartCoroutine(LoadSceenCorutine(sceneBuildIndex, false, sceneTransition, sceneLoadedAction));
        }

        public void LoadSceneAddativeAsync(string sceneName, System.Action sceneLoadedAction, eSceneTransition sceneTransition)
        {
            int sceneBuildIndex = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName).buildIndex;
            StartCoroutine(LoadSceenCorutine(sceneBuildIndex, true, sceneTransition, sceneLoadedAction));
        }

        public void LoadSceneAddativeAsync(int sceneBuildIndex, System.Action sceneLoadedAction, eSceneTransition sceneTransition)
        {
            StartCoroutine(LoadSceenCorutine(sceneBuildIndex, true, sceneTransition, sceneLoadedAction));
        }

        private IEnumerator LoadSceenCorutine(int sceneBuildIndex, bool isAddative, eSceneTransition sceneTransition, System.Action completionAction)
        {
            switch (sceneTransition)
            {
                case eSceneTransition.FadeOut:
                case eSceneTransition.FadeOutFadeIn:
                    {
                        CreateOverlayTexture();
                        _fadeValue = 0;
                        _displayOverlay = true;
                        _fadeDirection = 1;
                        while (_fadeValue <= 1f)
                        {
                            yield return 0f;
                        }
                        break;
                    }
            }

            UnityEngine.SceneManagement.LoadSceneMode loadSceneMode = UnityEngine.SceneManagement.LoadSceneMode.Single;
            if (isAddative)
            {
                loadSceneMode = UnityEngine.SceneManagement.LoadSceneMode.Additive;
            }

            AsyncOperation sceneLoadOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneBuildIndex, loadSceneMode);

            while (sceneLoadOperation != null && !sceneLoadOperation.isDone)
            {
                yield return 0f;
            }

            if (completionAction != null)
            {
                completionAction();
            }

            switch (sceneTransition)
            {
                case eSceneTransition.FadeOutFadeIn:
                    {
                        CreateOverlayTexture();
                        _fadeValue = 1;
                        _displayOverlay = true;
                        _fadeDirection = -1;
                        while (_fadeValue >= 0f)
                        {
                            yield return 0f;
                        }

                        break;
                    }
            }
            Destroy(this.gameObject);
        }

        private void CreateOverlayTexture()
        {
            if (_overlayTexture == null)
            {
                Color pixelColor = new Color(0,0,0,1);
                _overlayTexture = new Texture2D(1, 1);
                _overlayTexture.SetPixel(0, 0, pixelColor);
                _overlayTexture.Apply();
            }
        }

        void OnGUI()
        {
            if (_displayOverlay)
            {
                _fadeValue += _fadeSpeed * _fadeDirection * Time.deltaTime;
                GUI.color = new Color(0, 0, 0, _fadeValue);
                GUI.depth = -1000 ;
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _overlayTexture);
            }
        }
    }
}
