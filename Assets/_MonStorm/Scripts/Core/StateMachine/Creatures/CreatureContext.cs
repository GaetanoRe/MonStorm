using System.Numerics;
using System;

namespace MonStorm.Core.StateMachine
{
    public class CreatureContext
    {
        public StateMachine<CreatureContext> StateMachine { get; set; }
        public IFSMAdapterAnimator AdapterAnimator { get; }
        public IFSMAdapterNavMeshAgent AdapterNavMeshAgent { get; }
        public IFSMAdapterTransform AdapterTransform { get; }
        public IFSMAdapterLogger AdapterLogger { get; }
        public IFSMAdapterTransform TargetAdapterTransform { get; }
        public IFSMAdapterSensorVision AdapterSensorVision { get; }

        public Action OnAttackAnimationEnd;

        public bool GotHitThisFrame { get; private set; }
        public bool IsDead { get; private set; }
        public bool IsAnimationFinished { get; private set; }
        public bool HasReachedDestination { get; private set; }
        public bool IsFacingTarget { get; private set; }
        public bool IsTargetInVision { get; private set; }
        public float DistanceToTarget { get; private set; }


        public CreatureContext(StateMachine<CreatureContext> stateMachine, IFSMAdapterAnimator adapterAnimator, IFSMAdapterNavMeshAgent adapterNavMeshAgent,
            IFSMAdapterTransform adapterTransform, IFSMAdapterLogger adapterLogger, IFSMAdapterTransform targetAdapterTransform, IFSMAdapterSensorVision adapterSensorVision)
        {
            StateMachine = stateMachine;
            AdapterAnimator = adapterAnimator;
            AdapterNavMeshAgent = adapterNavMeshAgent;
            AdapterTransform = adapterTransform;
            AdapterLogger = adapterLogger;
            TargetAdapterTransform = targetAdapterTransform;
            AdapterSensorVision = adapterSensorVision;
        }

        public void UpdateContextValues(bool gotHitThisFrame, bool isDead)
        {
            GotHitThisFrame = gotHitThisFrame;
            IsDead = isDead;
            IsAnimationFinished = AdapterAnimator.IsAnimationFinished;
            HasReachedDestination = !AdapterNavMeshAgent.HasActivePath;
            IsFacingTarget = AdapterSensorVision.IsFacingTarget;
            IsTargetInVision = AdapterSensorVision.IsTargetInVision;
            DistanceToTarget = Vector2.Distance(AdapterTransform.PositionV2, TargetAdapterTransform.PositionV2);
        }
    }
}
