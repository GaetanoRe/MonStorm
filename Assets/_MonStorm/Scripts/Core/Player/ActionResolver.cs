

using System;
using System.Numerics;

namespace MonStorm.Core.Player
{
    public class ActionResolver
    {
        float deadzone; 
        float rearmThreshold;

        bool armed;

        public ActionResolver(float rearmThreshold, float deadzone)
        {
            this.rearmThreshold = rearmThreshold;
            this.deadzone = deadzone;
        }

        public ActionInput actionResolve(float x, float y, bool stickClicked, bool specialPressed)
        {
            Vector2 input = new Vector2(x, y);
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
                armed = false;
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

            return ActionInput.None;
        }

        


    }

    public class ActionResolverConfig
    {
        
    }
}
