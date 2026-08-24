
namespace MonStorm.Core.Combat
{
    public class AttackMove
    {
        public string Id; // The identifiable hash-string. How everything can reference this move.
        public float MotionValue; // Damage multiplier. How we will know how hard someone swings, etc.

        public string AnimationKey;

        public int AnimHash;
        public float Duration;
        public float Cooldown;

        public string[] ChainLinks;

        public AttackMove(string Id, float MotionValue, string AnimationKey,int AnimHash, float Duration, float Cooldown, string[] ChainLinks)
        {
            this.Id = Id;
            this.MotionValue = MotionValue;
            this.AnimationKey = AnimationKey;
            this.AnimHash = AnimHash;
            this.Duration = Duration;
            this.Cooldown = Cooldown;
            this.ChainLinks = ChainLinks;
        }
    }
}