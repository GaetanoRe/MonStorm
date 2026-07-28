namespace MonStorm.Core.StateMachine
{
    public interface IFSMAdapterLogger
    {
        public void LogMessage(string msg);
        public void ToggleEnabled(bool isEnabled);
    }
}
