using HarmonyLib;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SideLoader_ExtendedEffects
{
    //[HarmonyPatch(typeof(Character), nameof(Character.OnReceiveHit))]
    //public class SLEx_Character_RecieveHit
    //{
    //    static void Postfix(Character __instance, Weapon _weapon, float _damage, DamageList _damageList, Vector3 _hitDir, Vector3 _hitPoint, float _angle, float _angleDir, Character _dealerChar, float _knockBack)
    //    {
    //        OutwardEvents.OnCharacterRecievedHit.InvokeForInstance(__instance, new CharacterDamageEvent
    //        {
    //            DamageTarget = __instance,
    //            DamageSource = _weapon,
    //            DamageDone = _damageList,
    //            TotalDamage = _damage,
    //            DamageDirection = _hitDir,
    //            DamagePosition = _hitPoint,
    //            HitAngle = _angle,
    //            HitAngleDirection = _angleDir,
    //            DamageSourceCharacter = _dealerChar,
    //            KnocksBack = _knockBack,
    //        });
    //    }
    //}

    public struct CharacterDamageEvent
    {
        public Character DamageTarget;
        public object DamageSource;
        public float TotalDamage;
        public DamageList DamageDone;
        public Vector3 DamageDirection;
        public Vector3 DamagePosition;
        public float HitAngle;
        public float HitAngleDirection;
        public Character DamageSourceCharacter;
        public float KnocksBack;
        public bool DamageInventory;
    }

    public struct BagEventData
    {
        public Bag Bag;
        public Character CharacterOwner;
    }
}
