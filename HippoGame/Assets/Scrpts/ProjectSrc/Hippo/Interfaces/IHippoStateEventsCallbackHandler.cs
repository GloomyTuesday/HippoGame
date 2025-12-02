using System;

namespace Scripts.ProjectSrc.Hippo
{
    public interface IHippoStateEventsCallbackHandler
    {
        public event Action<TriggerIds> OnNewTriggerApplyed;
    }
}
