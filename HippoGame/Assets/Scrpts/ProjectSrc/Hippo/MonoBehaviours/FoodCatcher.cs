using Cysharp.Threading.Tasks;
using Scripts.BaseSystems.Core;
using Scripts.ProjectSrc.Food;
using Scripts.ProjectSrc.MainCharacter;
using System.Threading;
using UnityEngine;

namespace Scripts.ProjectSrc.Hippo
{
    public class FoodCatcher : MonoBehaviour
    {
        [SerializeField]
        private Transform _mouthPoint;

        [SerializeField]
        private float _catchRadius; 

        [Space(15)]
        [SerializeField]
        [FilterByType(typeof(IHippoStateEventsInvoker))]
        private Object _hippoStateEventsInvokerObj;

        [SerializeField]
        [FilterByType(typeof(AwarenessDistanceSrc))]
        private AwarenessDistanceSrc _awarenessDistanceSrc;

        [SerializeField]
        [FilterByType(typeof(TargetBankSrc))]
        private TargetBankSrc _targetBankSrc;

        private IHippoStateEventsInvoker _iHippoStateEventsInvoker;
        private IHippoStateEventsInvoker IHippoStateEventsInvoker => _iHippoStateEventsInvoker;

        private IEatable _thrownEatable;

        private CancellationTokenSource _cancellationTokenSource;

        private void Awake()
        {
            _iHippoStateEventsInvoker = _hippoStateEventsInvokerObj.GetComponent<IHippoStateEventsInvoker>();
        }

        private void OnEnable()
        {
            Subscribe(); 
        }

        private void OnDisable()
        {
            Unsubscribe(); 
        }

        private void Subscribe()
        {
            _targetBankSrc.OnThrowFood += ThrowFood;
        }

        private void Unsubscribe()
        {
            _targetBankSrc.OnThrowFood -= ThrowFood;
        }

        private void ThrowFood(IEatable eatable)
        {
            _thrownEatable = eatable;

            if (_thrownEatable == null) return;

            var distance = Vector3.Distance(transform.position, _thrownEatable.Rigidbody.transform.position);

            if (distance < _awarenessDistanceSrc.AwarnessDistance)
            {
                IHippoStateEventsInvoker.SetTrigger(TriggerIds.Catching);

                if (_cancellationTokenSource != null)
                    _cancellationTokenSource.Cancel();

                _cancellationTokenSource = new CancellationTokenSource();
                var token = _cancellationTokenSource.Token;
                ChatchingProcess(token).Forget();
            }
        }
           
        private async UniTaskVoid ChatchingProcess(CancellationToken token)
        {
            float distance;

            while (!token.IsCancellationRequested)
            {
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, token);

                if (_thrownEatable == null || _thrownEatable.Rigidbody == null)
                {
                    IHippoStateEventsInvoker.SetTrigger(TriggerIds.Idle);
                    _cancellationTokenSource.Cancel();
                    return;
                }

                distance = Vector3.Distance(_mouthPoint.position, _thrownEatable.Rigidbody.position);

                if (distance < _catchRadius)
                {
                    _thrownEatable.Destroy(); 
                    _thrownEatable = null;

                    IHippoStateEventsInvoker.SetTrigger(TriggerIds.Chew);
                    _cancellationTokenSource.Cancel();
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (_mouthPoint == null) return; 

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_mouthPoint.position, _catchRadius);
        }
    }
}
