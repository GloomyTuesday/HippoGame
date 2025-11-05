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

        public Transform TargetTransform { get; set; }
        public bool IsFoodGrabbed { get; set; }

    }
}

