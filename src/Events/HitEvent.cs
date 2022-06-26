using HarmonyLib;
using UnityEngine;
using SideLoader;

namespace SideLoader_ExtendedEffects.Events {

    public struct HitEvent {
        public Character source;
        public Character target;
        public float totalDamage;
        public DamageList damage;
        public Vector3 hitDirection;
        public Vector3 hitLocation;
        public float angle;
        public float angleDirection;
        public Weapon weapon;
        public float knockback;
    }

    [HarmonyPatch(typeof(Character), nameof(Character.OnReceiveHit))]
    public class Character_RecieveHit_Postfix
    {
        static void Postfix(
            Character __instance,
            Weapon _weapon,
            float _damage,
            DamageList _damageList,
            Vector3 _hitDir,
            Vector3 _hitPoint,
            float _angle,
            float _angleDir,
            Character _dealerChar,
            float _knockBack)
        {
            var e =  new HitEvent {
                source   = _dealerChar,
                target  = __instance,
                totalDamage = _damage,
                damage = _damageList,
                hitDirection = _hitDir,
                hitLocation = _hitPoint,
                angle = _angle,
                angleDirection = _angleDir,
                weapon = _weapon,
                knockback = _knockBack
            };
            Publisher<HitEvent>.RaiseEvent(e);
        }
    }

}