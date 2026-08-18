namespace MonStorm.Core.StateMachine
{
    public interface IFSMAdapterLogger
    {
        /// <summary>Changes whether the component logs it's messages to the console.</summary>
        /// <param name="isEnabled">The new state of the component.</param>
        public void ToggleEnabled(bool isEnabled);

        /// <summary>Logs a message to the console using Unity's Debug.Log.</summary>
        /// <param name="msg">The message to log.</param>
        public void LogMessage(string msg);

        /// <summary>Logs a warning message to the console using Unity's Debug.LogWarning.</summary>
        /// <param name="msg">The message to log.</param>
        public void LogWarning(string msg);

        /// <summary>Logs an error message to the console using Unity's Debug.LogError.</summary>
        /// <param name="msg">The message to log.</param>
        public void LogError(string msg);
    }
}
