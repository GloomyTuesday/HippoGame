using UnityEngine;

namespace Scripts.ProjectSrc.Food
{
    public interface IEatable
    {
        public void Charge();
        public Rigidbody Rigidbody { get; }
        public void Destroy();
    }
}
