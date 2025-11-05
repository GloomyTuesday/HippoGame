using Scripts.BaseSystems.Core;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.ProjectSrc.Entrys
{
    public interface intEr
    {
        public static int value = 2;
    }

    public class IndexEntry : MonoBehaviour
    {
        [Header("Simulating resource loading time")]
        [SerializeField]
        private float _delayTime = 1; 

        [SerializeField]
        private Object _unityScene;

        [SerializeField]
        [Uneditable]
        private string _nextSceneToLoad; 

        [Space(15)]
        [SerializeField]
        private UnitySceneStateSrc _unitySceneState;

        private void OnValidate()
        {
            if (_delayTime < 0)
                _delayTime = 0; 

            if (_unityScene != null)
                _nextSceneToLoad = _unityScene.name;
        }

        private void Awake()
        {
            Debug.Log("\t IndexEntry \t Awake()");
            _unitySceneState.IsIndexSceneReady = true;
        }

        private async void OnEnable()
        {
            Debug.Log("\t IndexEntry \t OnEnable() \t 1 \t "+Time.realtimeSinceStartup.ToString("F2"));
            //  Launching resource loading process 

            var elapsedTime = 0f;

            while (elapsedTime < _delayTime)
            {
                elapsedTime += Time.deltaTime * Time.timeScale;
                await Task.Yield();
            }

            Debug.Log("\t IndexEntry \t OnEnable() \t 2 \t " + Time.realtimeSinceStartup.ToString("F2")+"\t elapsed: "+ elapsedTime);

            if (!string.IsNullOrEmpty(_nextSceneToLoad))
                await SceneManager.LoadSceneAsync(_nextSceneToLoad);
        }
    }
}
