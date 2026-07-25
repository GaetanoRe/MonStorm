using System;

namespace MonStorm.Core.StateMachine
{
    public class FuncPredicate
    {
        readonly Func<bool> func;


        public FuncPredicate(Func<bool> func) => this.func = func;

        public bool Evaluate() => func.Invoke();
    }
}
