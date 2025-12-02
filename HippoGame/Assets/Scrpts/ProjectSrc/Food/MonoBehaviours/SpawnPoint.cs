using Cysharp.Threading.Tasks;
using Scripts.BaseSystems.Core;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Scripts.ProjectSrc.Food
{
    public class SpawnPoint : MonoBehaviour
    {
        [Space(15)]
        [SerializeField]
        private int _maxFoodObj;

        [SerializeField]
        private Vector2 _spawninterval = new Vector2(1, 2);

        [Space(15)]
        [SerializeField]
        private GameObject _foodItemrefab;

        [Space(15)]
        [SerializeField]
        [FilterByType(typeof(IBankTypeId<int, Transform>))]
        private Object _foodTransformHolderBankObj;

        [SerializeField]
        [FilterByType(typeof(IBankTypeId<int, Transform>))]
        private Object _foodActiveObjBankObj;

        private IBankTypeId<int, Transform> _foodTransformHolderBank;
        private IBankTypeId<int, Transform> FoodTransformHolderBank => _foodTransformHolderBank;

        private IBankTypeId<int, Transform> _foodActiveObjBank;
        private IBankTypeId<int, Transform> FoodActiveObjBank => _foodActiveObjBank;

        private CancellationTokenSource _cancellationTokenSourceMove = null;

        private void OnValidate()
        {
            if (_spawninterval.x < 0)
                _spawninterval.x = 0;

            if (_spawninterval.y < 0)
                _spawninterval.y = 0;

            if (_spawninterval.y < _spawninterval.x)
                _spawninterval.y = _spawninterval.x;
        }

        private void Awake()
        {
            _foodTransformHolderBank = _foodTransformHolderBankObj.GetComponent<IBankTypeId<int, Transform>>();
            _foodActiveObjBank = _foodActiveObjBankObj.GetComponent<IBankTypeId<int, Transform>>();
        }

        private void OnEnable()
        {
            Subscribe();

            if (FoodTransformHolderBank.ItemHashSet.Count < 1) return;
            if (FoodActiveObjBank.ItemHashSet.Count >= _maxFoodObj) return;

            
            Spaw();
        }

        private void OnDisable()
        {
            if (_cancellationTokenSourceMove != null)
                _cancellationTokenSourceMove.Cancel(); 

            Unsubscribe();
        }

        private void Subscribe()
        {
            FoodTransformHolderBank.OnItemAdded += FoodTransformHolderAdded;

            FoodActiveObjBank.OnItemAdded += FoodItemAdded;
            FoodActiveObjBank.OnItemRemoved += FoodItemRemoved;
        }

        private void Unsubscribe()
        {
            FoodTransformHolderBank.OnItemAdded -= FoodTransformHolderAdded;

            FoodActiveObjBank.OnItemAdded -= FoodItemAdded;
            FoodActiveObjBank.OnItemRemoved -= FoodItemRemoved;
        }

        private void FoodTransformHolderAdded(int obj)
        {
            if (FoodActiveObjBank.ItemHashSet.Count >= _maxFoodObj) return;

            LaunchSpawnSequence().Forget();
        }

        private void FoodItemAdded(int obj)
        {
            if (FoodActiveObjBank.ItemHashSet.Count >= _maxFoodObj) return;

            LaunchSpawnSequence().Forget();
        }

        private void FoodItemRemoved(int obj)
        {
            if (FoodActiveObjBank.ItemHashSet.Count >= _maxFoodObj) return;

            LaunchSpawnSequence().Forget();
        }

        private async UniTaskVoid LaunchSpawnSequence()
        {
            if (_cancellationTokenSourceMove != null) return;

            _cancellationTokenSourceMove = new CancellationTokenSource();
            var token = _cancellationTokenSourceMove.Token; 

            var spawnTimeSeconds = Random.Range(_spawninterval.x, _spawninterval.y);
            var timeElapsed = 0f;

            while (timeElapsed < spawnTimeSeconds)
            {
                timeElapsed += Time.fixedDeltaTime;
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, token);
            }

            _cancellationTokenSourceMove = null;

            if (FoodActiveObjBank.ItemHashSet.Count < _maxFoodObj)
                Spaw();
        }

        private void Spaw()
        {
            var foodHolderTransformList = new List<Transform>(FoodTransformHolderBank.ItemHashSet);

            if (foodHolderTransformList.Count < 1) return; 

            var foodHolderTransform = foodHolderTransformList[0];

            var obj = Instantiate(_foodItemrefab, transform);
            obj.name = "Food";
            obj.transform.localPosition = _foodItemrefab.transform.localPosition;
            obj.transform.localRotation = _foodItemrefab.transform.localRotation;
            obj.transform.localScale = _foodItemrefab.transform.localScale;

            obj.transform.SetParent(foodHolderTransform);

            LaunchSpawnSequence().Forget();
        }
    }
}
