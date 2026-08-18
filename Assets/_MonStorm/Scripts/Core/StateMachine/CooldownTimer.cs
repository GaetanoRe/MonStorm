using System;

namespace MonStorm.Core.StateMachine
{
    public class CooldownTimer
    {
        readonly float cooldownDuration;
        readonly Func<float> getTime;

        float nextReadyTime;


        public CooldownTimer(float cooldownDuration, Func<float> getTime)
        {
            this.cooldownDuration = cooldownDuration;
            this.getTime = getTime;
        }

        public bool IsReady => getTime() >= nextReadyTime;

        public void StartCooldown() => nextReadyTime = getTime() + cooldownDuration;
    }
}
