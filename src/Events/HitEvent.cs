using System;
using HarmonyLib;
using UnityEngine;

namespace SideLoader_ExtendedEffects.Events {

    public struct HitEvent {
        public Character dealer;
        public Character target;
        public DamageList damage;
        public Vector3 hitDirection;
        public Vector3 hitLocation;
        public float angle;
        public float angleDirection;
        public EffectSynchronizer source;
        public float knockback;
    }

    [HarmonyPatch(typeof(Character), nameof(Character.ReceiveHit), argumentTypes:new Type[]{
        typeof(UnityEngine.Object),
        typeof(DamageList),
        typeof(Vector3),
        typeof(Vector3),
        typeof(float),
        typeof(float),
        typeof(Character),
        typeof(float),
        typeof(bool)
    })]
    public class Character_RecieveHit_Postfix
    {
        static void Postfix(
            Character __instance,
            UnityEngine.Object _damageSource,
            DamageList _damage,
            Vector3 _hitDir,
            Vector3 _hitPoint,
            float _angle,
            float _angleDir,
            Character _dealerChar,
            float _knockBack,
            bool _hitInventory
        )
        {
            var e =  new HitEvent {
                dealer   = _dealerChar,
                target  = __instance,
                damage = _damage,
                hitDirection = _hitDir,
                hitLocation = _hitPoint,
                angle = _angle,
                angleDirection = _angleDir,
                source = _damageSource as EffectSynchronizer,
                knockback = _knockBack
            };
            Publisher<HitEvent>.RaiseEvent(e);
        }
    }

}