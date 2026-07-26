
namespace MonStorm.Core.StateMachine
{
    public interface IState<TContext> where TContext : class
    {
        public void Enter(TContext context);

        public void Tick(TContext context, float deltaTime);

        public void Exit(TContext context);
    }
}