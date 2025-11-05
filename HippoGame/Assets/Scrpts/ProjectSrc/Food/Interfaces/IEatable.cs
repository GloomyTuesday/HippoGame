using UnityEngine;

namespace Scripts.ProjectSrc.Food
{
    public interface IEatable
    {
        public Rigidbody Rigidbody { get; }
        public void Destroy();
    }
}
