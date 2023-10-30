using System;
using HarmonyLib;
using UnityEngine;

namespace SideLoader_ExtendedEffects.Events {

    public struct BlockEvent {
        public Character dealer;
        public Character target;
        public float damage;
        public Vector3 hitDirection;
        public float angle;
        public float angleDirection;
        public EffectSynchronizer source;
        public float knockback;
    }

    [HarmonyPatch(typeof(Character), nameof(Character.ReceiveBlock), argumentTypes:new Type[]{
        typeof(UnityEngine.MonoBehaviour),
        typeof(float),
        typeof(Vector3),
        typeof(float),
        typeof(float),
        typeof(Character),
        typeof(float)
    })]
    public class Character_ReceiveBlock_Postfix
    {
        static void Postfix(
            Character __instance,
            UnityEngine.MonoBehaviour hitBehaviour,
            float _damage,
            Vector3 _hitDir,
            float _angle,
            float _angleDir,
            Character _dealerChar,
            float _knockBack
        )
        {
            var e =  new BlockEvent {
                dealer   = _dealerChar,
                target  = __instance,
                damage = _damage,
                hitDirection = _hitDir,
                angle = _angle,
                angleDirection = _angleDir,
                source = hitBehaviour as EffectSynchronizer,
                knockback = _knockBack
            };
            Publisher<BlockEvent>.RaiseEvent(e);
        }
    }

}