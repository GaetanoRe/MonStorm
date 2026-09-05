using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonStorm.Core.Player
{
    public struct InputSnapshot
    {
        public float stickX;
        public float stickY;
        public bool stickClicked;
        public bool northPressed;
        public bool eastPressed;
        public bool modifierHeld;
        public bool specialPressed;
    }
}