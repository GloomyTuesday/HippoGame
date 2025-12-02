namespace Scripts.ProjectSrc.Hippo
{
    public interface IHippoStateEventsInvoker
    {
        public void SetTrigger(TriggerIds triggerId);
        public string GetCurrentAnimClipName();
        public TriggerIds GetCurrentTrigger();
    }
}
