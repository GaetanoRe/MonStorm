namespace MonStorm.Core.Combat
{
    public static class DamageCalculator
    {
        public static float Resolve(float weaponDamage, float zoneMultiplier)
        {
            return weaponDamage * zoneMultiplier;
        }
    }
}