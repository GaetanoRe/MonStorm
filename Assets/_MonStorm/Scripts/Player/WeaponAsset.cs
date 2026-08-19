using System;
using System.Collections.Generic;
using UnityEngine;
using MonStorm.Core.Combat;
using MonStorm.Core.Player;


[CreateAssetMenu(menuName = "MonStorm/Weapon Asset", fileName = "New Weapon Asset")]
public class WeaponAsset : ScriptableObject
{
    [SerializeField] public string weaponName;
    [SerializeField] public float rawDamage;
    [SerializeField] public AttackMoveData[] moves;
    [SerializeField] public List<DirectionalEntry> directionalOpenerMap;
    [SerializeField] public HitApplier hitApplier;

    public WeaponDefinition Build()
    {
        AttackMove [] attackMoves = new AttackMove[moves.Length];
        for(int i = 0; i < attackMoves.Length; i++)
        {
            attackMoves[i] = new AttackMove(moves[i].id, moves[i].motionValue, moves[i].animationKey, moves[i].chainLinks);
        }

        Dictionary<ActionInput, string> actionDictionary = new Dictionary<ActionInput, string>();
        foreach(DirectionalEntry entry in directionalOpenerMap)
        {
            actionDictionary.Add(entry.input, entry.moveId);
        }

        return new WeaponDefinition(rawDamage, attackMoves, actionDictionary);
    }
}

[Serializable]
public class AttackMoveData
{
    public string id;
    public float motionValue;
    public string animationKey;
    public string [] chainLinks;
    public AnimationClip clip;

}

[Serializable]
public class DirectionalEntry
{
    public ActionInput input;
    public string moveId;
}
