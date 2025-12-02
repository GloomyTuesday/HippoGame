using Scripts.BaseSystems.Core;
using System;
using UnityEngine;
using Object = UnityEngine.Object; 

namespace Scripts.ProjectSrc
{
    [CreateAssetMenu(fileName = "UnitySceneState", menuName = "Scriptable Obj/Project src/Unity scene state")]
    public class UnitySceneStateSrc : ScriptableObject
    {
        [NonSerialized]
        private bool _isIndexSceneReady ;

        [SerializeField]
        private Object _inexScene;

        [SerializeField]
        [Uneditable]
        private string _indexSceneName;

        public string IndexSceneName => _indexSceneName;

        /// <summary>
        ///     Used for cheking if first unity scene need to be loaded
        /// </summary>
        public bool IsIndexSceneReady
        {
            get => _isIndexSceneReady; 
            set => _isIndexSceneReady = value;
        }

        private void OnValidate()
        {
            if (_inexScene != null)
                _indexSceneName = _inexScene.name;
        }
    }
}


