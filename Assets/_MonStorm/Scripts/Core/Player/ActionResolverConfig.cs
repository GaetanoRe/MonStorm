
namespace MonStorm.Core.Player
{
    public class ActionResolverConfig
    {
        public float rearmThreshold = 0.2f;
        public float deadzone = 0.5f;
        public float comboBufferWindow = 0.08f; 

        public ActionResolverConfig(float rearmThreshold, float deadzone, float comboBufferWindow)
        {
            this.rearmThreshold = rearmThreshold;
            this.deadzone = deadzone;
            this.comboBufferWindow = comboBufferWindow;
        }
    }
}