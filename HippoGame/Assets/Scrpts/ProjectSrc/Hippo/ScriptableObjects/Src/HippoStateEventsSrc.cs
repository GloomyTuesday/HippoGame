using System;
using UnityEngine;

namespace Scripts.ProjectSrc.Hippo
{
    [CreateAssetMenu(fileName = "HippoStateEvents", menuName = "Scriptable Obj/Project src/Hippo state events")]
    public class HippoStateEventsSrc : 
        ScriptableObject,
        IHippoStateEventsCallbackInvoker,
        IHippoStateEventsCallbackHandler,
        IHippoStateEventsInvoker,
        IHippoStateEventsHandler
    {
        private event Action<TriggerIds> _onSetTrigger;
        event Action<TriggerIds> IHippoStateEventsHandler.OnSetTrigger
        {
            add => _onSetTrigger += value;
            remove => _onSetTrigger -= value;
        }
        void IHippoStateEventsInvoker.SetTrigger(TriggerIds stateId) => _onSetTrigger?.Invoke(stateId);


        private event Func<string> _onGetCurrentAnimClipName;
        event Func<string> IHippoStateEventsHandler.OnGetCurrentAnimClipName
        {
            add => _onGetCurrentAnimClipName += value; 
            remove => _onGetCurrentAnimClipName -= value;
        }
        string IHippoStateEventsInvoker.GetCurrentAnimClipName() => _onGetCurrentAnimClipName?.Invoke();


        private event Func<TriggerIds> _onGetCurrentState;
        event Func<TriggerIds> IHippoStateEventsHandler.OnGetCurrentTrigger
        {
            add => _onGetCurrentState += value;
            remove => _onGetCurrentState -= value;
        }
        TriggerIds IHippoStateEventsInvoker.GetCurrentTrigger()
        {
            TriggerIds? request = _onGetCurrentState?.Invoke();

            if (request == null) return TriggerIds.Non;

            return request.Value;
        }


        private Action<TriggerIds> _onNewTriggerApplyed;
        event Action<TriggerIds> IHippoStateEventsCallbackHandler.OnNewTriggerApplyed
        {
            add => _onNewTriggerApplyed += value;
            remove => _onNewTriggerApplyed -= value;
        }
        void IHippoStateEventsCallbackInvoker.NewTriggerApplied(TriggerIds triggerId) => _onNewTriggerApplyed?.Invoke(triggerId);
    }
}

