using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonStorm.Core.Player
{
    public class ButtonActionResolver : IActionResolver
    {
        bool armed;
        float comboBufferWindow;

        public ButtonActionResolver(ActionResolverConfig config)
        {
            comboBufferWindow = config.comboBufferWindow;
        }
        public void Reset()
        {
            armed = false;
        }

        public ActionInput Resolve(InputSnapshot snapshot, float deltaTime)
        {
            if (snapshot.specialPressed)
            {
                return ActionInput.SpecialAction;
            }
            else if (snapshot.modifierHeld)
                {
                    if (snapshot.northPressed)
                    {
                        return ActionInput.Action4;
                    }
                    if (snapshot.eastPressed)
                    {
                        return ActionInput.Action5;
                    }
                }
                else if(snapshot.northPressed && snapshot.eastPressed)
                {
                    return ActionInput.Action3;
                }
                else if (snapshot.northPressed && !snapshot.eastPressed)
                {
                    return ActionInput.Action1;
                }
                else if(!snapshot.northPressed && snapshot.eastPressed)
                {
                    return ActionInput.Action2;
                }

            return ActionInput.None;
        }
    }
}