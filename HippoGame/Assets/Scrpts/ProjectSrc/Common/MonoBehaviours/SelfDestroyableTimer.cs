using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Scripts.ProjectSrc.Common
{
    public class SelfDestroyableTimer : MonoBehaviour
    {
        [SerializeField]
        private float _lifetime = 1;

        private CancellationTokenSource _cancellationTokenSource = null;

        private void OnValidate()
        {
            if (_lifetime < 0)
                _lifetime = 0; 
        }

        private void OnEnable()
        {
            _cancellationTokenSource = new CancellationTokenSource();   
            var token = _cancellationTokenSource.Token;
            LaunchSpawnSequence(token, _lifetime).Forget(); 
        }

        private void OnDisable()
        {
            _cancellationTokenSource.Cancel(); 
        }

        private async UniTaskVoid LaunchSpawnSequence(CancellationToken token, float delaySeconds)
        {
            await UniTask.Delay((int)(delaySeconds * 1000), cancellationToken: token);
            Destroy(gameObject);
        }
    }
}
