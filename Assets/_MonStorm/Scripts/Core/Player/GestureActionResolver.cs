

using System;
using System.Numerics;

namespace MonStorm.Core.Player
{
    public class GestureActionResolver : IActionResolver
    {
        ActionResolverConfig config;



        bool armed;

        public GestureActionResolver(ActionResolverConfig config)
        {
            this.config = config;
        }

        public void Reset()
        {
            armed = false;
        }

        public ActionInput Resolve(InputSnapshot snapshot, float deltaTime)
        {
         
                Vector2 input = new Vector2(snapshot.stickX, snapshot.stickY);
                float magnitude = input.Length();
                if (snapshot.specialPressed)
                {
                    return ActionInput.SpecialAction;
                }
                else if (snapshot.stickClicked)
                {
                    return ActionInput.Action5;
                }
                else if(magnitude <= config.rearmThreshold)
                {
                    armed = true;
                    return ActionInput.None;
                }
                else if(magnitude >= config.deadzone && armed)
                {
                    float absX = Math.Abs(input.X);
                    float absY = Math.Abs(input.Y);
                    armed = false;
                    if(absX >= absY)
                    {
                        if(input.X > 0)
                        {
                            return ActionInput.Action2;
                        }
                        else if(input.X < 0)
                        {
                            return ActionInput.Action1;
                        }
                    }
                    else
                    {
                        if(input.Y > 0)
                        {
                            return ActionInput.Action3;
                        }
                        else if(input.Y < 0)
                        {
                            return ActionInput.Action4;
                        }
                    }
                }

            return ActionInput.None;
        }
    }
}
