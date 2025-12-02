
using System;

namespace Scripts.ProjectSrc.Hippo
{
    public interface IHippoStateEventsHandler
    {
        public event Action<TriggerIds> OnSetTrigger;
        public event Func<string> OnGetCurrentAnimClipName;
        public event Func<TriggerIds> OnGetCurrentTrigger;
    }
}
