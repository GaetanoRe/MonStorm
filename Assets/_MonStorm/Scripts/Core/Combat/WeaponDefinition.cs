using System.Collections.Generic;
using MonStorm.Core.Player;

namespace MonStorm.Core.Combat
{
    public class WeaponDefinition
    {
        public float RawDamage;
        public AttackMove[] moves;
        public Dictionary<ActionInput, string> DirectionalOpenerMap;


        public WeaponDefinition(float RawDamage, AttackMove[] moves, Dictionary<ActionInput, string> DirectionalOpenerMap)
        {
            this.RawDamage = RawDamage;
            this.moves = moves;
            this.DirectionalOpenerMap = DirectionalOpenerMap;
        }

        public AttackMove GetMove(string id)
        {
            foreach(AttackMove attack in moves)
            {
                if(id == attack.Id)
                {
                    return attack;
                }
            }

            return null;
        }
    }
}