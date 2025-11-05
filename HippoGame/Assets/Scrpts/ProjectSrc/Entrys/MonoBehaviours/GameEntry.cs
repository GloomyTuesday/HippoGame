using Scripts.BaseSystems.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.ProjectSrc.Entrys
{
    [DefaultExecutionOrder(-1000)]
    public class GameEntry: MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _objectsToActivateWhenReady;

        [Space(15)]
        [SerializeField]
        private float _timeToWaitForObjToGetReady;

        [Space(15)]
        [SerializeField]
        private UnitySceneStateSrc _unitySceneState;

        private void OnValidate()
        {
            if (_timeToWaitForObjToGetReady < 0)
                _timeToWaitForObjToGetReady = 0; 
        }

        private void Awake()
        {            
            if (!_unitySceneState.IsIndexSceneReady)
            {
                SceneManager.LoadSceneAsync(_unitySceneState.IndexSceneName);
                Destroy(gameObject);
            }

            DisableHierarchyAboveExceptParent();
            SetCollectionActiveState(_objectsToActivateWhenReady, false);
        }

        private void OnEnable()
        {
            SetCollectionActiveState(_objectsToActivateWhenReady, true); 
        }

        private void OnDisable()
        {
            SetCollectionActiveState(_objectsToActivateWhenReady, false);
        }

        private async void SetCollectionActiveState(GameObject[] collection, bool activeState)
        {
            foreach (var item in collection)
            {
                if (!item) continue;

                item.SetActive(activeState);

                if (!activeState)
                    continue;
                
                var iReady = item.GetComponent<IReady>();

                if (iReady == null )
                    continue; 

                var endTime = Time.realtimeSinceStartup + _timeToWaitForObjToGetReady; 

                while (endTime > Time.realtimeSinceStartup)
                {
                    if(iReady.Ready)
                        break; 

                    await Task.Yield(); 
                }
            }
        }

        private void DisableHierarchyAboveExceptParent()
        {
            Transform current = transform;
            Transform stopAtParent = current == null ? current : current.parent;
            Transform buff;

            HashSet<Transform> relativeCollection = new HashSet<Transform>();
            relativeCollection.Add(transform);

            while (current != null)
            {
                buff = current.parent;

                if (buff == null) break;

                foreach (Transform sibling in buff)
                {
                    if (!relativeCollection.Contains(sibling))
                        sibling.gameObject.SetActive(false);
                }

                relativeCollection.Add(buff);
                current = buff;
            }

            foreach (GameObject root in gameObject.scene.GetRootGameObjects())
            {
                if (!relativeCollection.Contains(root.transform))
                {
                    root.SetActive(false);
                }
            }
        }
    }
}
