using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SideLoader_ExtendedEffects.Released
{
   /// <summary>
   /// Allows you to deal damage based on the equipped weapon
   /// </summary>
    public class SL_ScalingWeaponDamage : SL_Effect, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_ScalingWeaponDamage);
        public Type GameModel => typeof(SLEx_ScalingWeaponDamage);

        public SL_Damage[] DamageModifiers;
        public float KnockbackAmount;
        public bool HitInventory;

        public override void ApplyToComponent<T>(T component)
        {
            SLEx_ScalingWeaponDamage comp = component as SLEx_ScalingWeaponDamage;
            comp.KnockbackAmount = KnockbackAmount;
            comp.HitInventory = HitInventory;
            comp.DamageModifiers = DamageModifiers.ToList();
        }

        public override void SerializeEffect<T>(T effect)
        {
            SLEx_ScalingWeaponDamage comp = effect as SLEx_ScalingWeaponDamage;
            this.KnockbackAmount = comp.KnockbackAmount;
            this.HitInventory = comp.HitInventory;
            this.DamageModifiers = comp.DamageModifiers.ToArray();
        }
    }

    public class SLEx_ScalingWeaponDamage : Effect, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_ScalingWeaponDamage);
        public Type GameModel => typeof(SLEx_ScalingWeaponDamage);

        public List<SL_Damage> DamageModifiers;
        public float KnockbackAmount;
        public bool HitInventory;

        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            Character Owner = this.m_parentSynchronizer.OwnerCharacter;
            Weapon Weapon = Owner.CurrentWeapon;
            if (Weapon != null)
            {
                Vector3 Direction = Owner.transform.position - _affectedCharacter.transform.position;
                _affectedCharacter.ReceiveHit(OwnerCharacter, CalculateWeaponDamage(Weapon), Direction, _affectedCharacter.transform.position, 0, 0, Owner, KnockbackAmount, HitInventory);
            }
            else
            {
                ExtendedEffects.Instance.Log("SLEx_ScalingWeapon Owner has no weapon equipped!");
            }
        }

        private DamageList CalculateWeaponDamage(Weapon weapon)
        {
            //Clone Weapon Damage
            DamageList NewList =  weapon.Damage.Clone();

            if (DamageModifiers != null && DamageModifiers.Count > 0)
            {
                //Modify
                for (int i = 0; i < NewList.Count; i++)
                {
                    SL_Damage scalingModifier = DamageModifiers.Find(x => x.Type == NewList[i].Type);
                    if (scalingModifier != null)
                    {
                        NewList[i].Damage *= scalingModifier.Damage;
                    }
                }
            }

            return NewList;
        }
    }
}
