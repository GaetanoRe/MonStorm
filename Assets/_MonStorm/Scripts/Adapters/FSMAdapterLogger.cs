using UnityEngine;
using MonStorm.Core.StateMachine;

namespace Monstorm.Adapters
{
    public class FSMAdapterLogger : IFSMAdapterLogger
    {
        public void LogMessage(string msg) => Debug.Log(msg);
    }
}
