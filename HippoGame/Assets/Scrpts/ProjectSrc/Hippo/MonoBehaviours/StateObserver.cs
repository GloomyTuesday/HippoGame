using Cysharp.Threading.Tasks;
using Scripts.BaseSystems.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Object = UnityEngine.Object;  

namespace Scripts.ProjectSrc.Hippo
{
    public class StateObserver : MonoBehaviour
    {
        [SerializeField]
        private TriggerIds _defaultAnimTrigger;

        [Space(15)]
        [SerializeField]
        private Animator _animator; 

        [Space(15)]
        [SerializeField]
        [FilterByType(typeof(IHippoStateEventsHandler))]
        private Object _hippoStateEventsHandlerObj;

        [SerializeField]
        [FilterByType(typeof(IHippoStateEventsCallbackInvoker))]
        private Object _hippoStateEventsCallbackInvokerObj;

        [SerializeField]
        private AnimationClipMapItem[] _animationClipTriggermap;

        private Dictionary<string, TriggerIds> _animationClipTriggerMapDictionary;

        private IHippoStateEventsHandler _iHippoStateEventsHandler;
        private IHippoStateEventsHandler IHippoStateEventsHandler => _iHippoStateEventsHandler;

        private IHippoStateEventsCallbackInvoker _iHippoStateEventsCallbackInvoker;
        private IHippoStateEventsCallbackInvoker IHippoStateEventsCallbackInvoker => _iHippoStateEventsCallbackInvoker;

        private TriggerIds _currentTrigger;
        private CancellationTokenSource _cancellationTokenSource;

        private void OnValidate()
        {
            if(_animationClipTriggermap!=null)
            {
                for(int i = 0; i < _animationClipTriggermap.Length; i ++)
                {
                    _animationClipTriggermap[i].Name = _animationClipTriggermap[i].AnimationClip == null ?
                        "" : _animationClipTriggermap[i].AnimationClip.name; 
                }
            }
        }

        private void Awake()
        {
            _iHippoStateEventsHandler = _hippoStateEventsHandlerObj.GetComponent<IHippoStateEventsHandler>();
            _iHippoStateEventsCallbackInvoker = _hippoStateEventsCallbackInvokerObj.GetComponent<IHippoStateEventsCallbackInvoker>();

            _animationClipTriggerMapDictionary = new Dictionary<string, TriggerIds>();

            if (_animationClipTriggermap == null) return;

            foreach (var item in _animationClipTriggermap)
                _animationClipTriggerMapDictionary[item.AnimationClip.name] = item.AssociatedTrigger;
        }

        private void OnEnable()
        {
            _currentTrigger = _defaultAnimTrigger; 
            Subscribe();
            SetTrigger(_currentTrigger); 
        }

        private void OnDisable()
        {
            Unsubscribe();

            if (_cancellationTokenSource != null)
                _cancellationTokenSource.Cancel();
        }

        private void Subscribe()
        {
            IHippoStateEventsHandler.OnSetTrigger += SetTrigger;
            IHippoStateEventsHandler.OnGetCurrentAnimClipName += GetCurrentAnimationName;
            IHippoStateEventsHandler.OnGetCurrentTrigger += GetCurrentTrigger;
        }

        private void Unsubscribe()
        {
            IHippoStateEventsHandler.OnSetTrigger -= SetTrigger;
            IHippoStateEventsHandler.OnGetCurrentAnimClipName -= GetCurrentAnimationName;
            IHippoStateEventsHandler.OnGetCurrentTrigger -= GetCurrentTrigger;
        }

        private TriggerIds GetCurrentTrigger() => _currentTrigger; 

        private void SetTrigger(TriggerIds triggerId)
        {
            _currentTrigger = triggerId;
            _animator.SetTrigger(triggerId.ToString());
            IHippoStateEventsCallbackInvoker.NewTriggerApplied(triggerId);

            if (_cancellationTokenSource != null)
                _cancellationTokenSource.Cancel();

            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            UpdateCurrentTrigger(token).Forget(); 
        }

        private void SetTriggerIgnoringAnimator(TriggerIds triggerId)
        {
            _currentTrigger = triggerId;
            IHippoStateEventsCallbackInvoker.NewTriggerApplied(triggerId);
        }

        private string GetCurrentAnimationName()
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            foreach (var clip in _animator.runtimeAnimatorController.animationClips)
            {
                if (Animator.StringToHash(clip.name) == stateInfo.shortNameHash)
                    return clip.name;
            }

            return default;
        }

        /// <summary>
        /// Same animation clips but with different triggers won't by took in a count
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async UniTaskVoid UpdateCurrentTrigger(CancellationToken token)
        {
            string animationClipName;
            TriggerIds trigger = _currentTrigger; 

            while (!token.IsCancellationRequested)
            {
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, token);

                animationClipName = GetCurrentAnimationName();

                if(!string.IsNullOrEmpty(animationClipName))
                    if (_animationClipTriggerMapDictionary.ContainsKey(animationClipName))
                        trigger = _animationClipTriggerMapDictionary[animationClipName];

                if(_currentTrigger != trigger)
                    SetTriggerIgnoringAnimator(trigger);
            }
        }

        [Serializable]
        private struct AnimationClipMapItem
        {
            [HideInInspector]
            public string Name;

            public AnimationClip AnimationClip;
            public TriggerIds AssociatedTrigger; 
        }
    }

    
}
