using System;

namespace Scripts.BaseSystems.Dependent
{
    public interface IEventDataUpdated
    {
        public event Action OnDataUpdated;
    }
}
