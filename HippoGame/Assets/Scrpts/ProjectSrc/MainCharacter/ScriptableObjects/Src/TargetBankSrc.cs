using Scripts.ProjectSrc.Food;
using System;
using UnityEngine;

namespace Scripts.ProjectSrc.MainCharacter
{
    [CreateAssetMenu(fileName = "TargetBank", menuName = "Scriptable Obj/Project src/Target bank")]
    public class TargetBankSrc : ScriptableObject
    {
        public event Action<IEatable> OnThrowFood;
        public void ThrowFood(IEatable eatable) => OnThrowFood?.Invoke(eatable);

        [SerializeField]
        private string _targetObjname;

        private Transform _targetTransform;
        public Transform TargetTransform
        {
            get => _targetTransform;
            set
            {
                _targetTransform = value; 

                if (value!=null)
                    _targetObjname = value.name;
            }
        }

        [SerializeField]
        private bool _foodGrabbed;
        public bool IsFoodGrabbed
        {
            get => _foodGrabbed;
            set
            {
                _foodGrabbed = value;
            }
        }

    }
}

