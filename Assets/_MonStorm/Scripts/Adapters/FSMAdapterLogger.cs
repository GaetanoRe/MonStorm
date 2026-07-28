using UnityEngine;
using MonStorm.Core.StateMachine;

namespace MonStorm.Adapters
{
    public class FSMAdapterLogger : IFSMAdapterLogger
    {
        bool isEnabled;


        public FSMAdapterLogger(bool isEnabled) => ToggleEnabled(isEnabled);

        public void LogMessage(string msg)
        {
            if (!isEnabled) return;

            Debug.Log(msg);
        }

        public void ToggleEnabled(bool isEnabled) => this.isEnabled = isEnabled;
    }
}
