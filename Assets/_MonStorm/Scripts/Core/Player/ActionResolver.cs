

using System;
using System.Numerics;

namespace MonStorm.Core.Player
{
    public class ActionResolver
    {
        float deadzone; 
        float rearmThreshold;

        Vector2 input;

        bool armed;

        public ActionResolver(float rearmThreshold, float deadzone)
        {
            this.rearmThreshold = rearmThreshold;
            this.deadzone = deadzone;
        }

        public ActionInput actionResolve(Vector2 input, bool stickClicked, bool specialPressed)
        {
            float magnitude = input.Length();
            if (specialPressed)
            {
                return ActionInput.SpecialAction;
            }
            else if (stickClicked)
            {
                return ActionInput.Action5;
            }
            else if(magnitude <= rearmThreshold)
            {
                armed = true;
                return ActionInput.None;
            }
            else if(magnitude >= deadzone && armed)
            {
                if(input.X > 0)
                {
                    return ActionInput.Action3;
                }
                else if(input.X < 0)
                {
                    return ActionInput.Action4;
                }
                else if(input.Y > 0)
                {
                    return ActionInput.Action1;
                }
                else if(input.Y < 0)
                {
                    return ActionInput.Action2;
                }
            }
        }


    }

    public class ActionResolverConfig
    {
        
    }
}
