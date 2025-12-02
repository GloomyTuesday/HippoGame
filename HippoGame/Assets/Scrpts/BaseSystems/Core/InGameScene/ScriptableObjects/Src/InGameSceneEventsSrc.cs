using UnityEngine;
using System;

namespace Scripts.BaseSystems.Core
{
    [CreateAssetMenu(fileName = "InGameSceneEvents", menuName = "Scriptable Obj/Base systems/Core/In game scene/In game scene events")]
    public class InGameSceneEventsSrc : ScriptableObject, IInGameSceneEvents, IInGameSceneEventsHandler
    {
        private Action<GameObject> onLoadInGameScene; 
        event Action<GameObject> IInGameSceneEventsHandler.OnLoadInGameScene
        {
            add => onLoadInGameScene = value; 
            remove => onLoadInGameScene = value;
        }
        public void LoadInGameScene(GameObject prefab)
        {
            onLoadInGameScene?.Invoke(prefab);
        }


        private Action onLoadPreviousInGameScene;
        event Action IInGameSceneEventsHandler.OnLoadPreviousInGameScene
        {
            add => onLoadPreviousInGameScene = value;
            remove => onLoadPreviousInGameScene = value; 
        }
        public void LoadPreviousInGameScene() => onLoadPreviousInGameScene?.Invoke(); 


    }
}