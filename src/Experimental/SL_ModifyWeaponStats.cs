using SideLoader;
using SideLoader_ExtendedEffects.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SideLoader_ExtendedEffects.Released
{
    //public class SL_ModifyWeaponStats : SL_Effect, ICustomModel
    //{
    //    public Type SLTemplateModel => typeof(SL_ModifyWeaponStats);
    //    public Type GameModel => typeof(SLEx_ModifyWeaponStats);

    //    public override void ApplyToComponent<T>(T component)
    //    {
    //        SLEx_ModifyWeaponStats comp = component as SLEx_ModifyWeaponStats;
    //    }

    //    public override void SerializeEffect<T>(T effect)
    //    {

    //    }
    //}

    //public class SLEx_ModifyWeaponStats : Effect, ICustomModel
    //{
    //    public Type SLTemplateModel => typeof(SL_ModifyWeaponStats);
    //    public Type GameModel => typeof(SLEx_ModifyWeaponStats);

    //    private Weapon WeaponInstance = null;

    //    public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
    //    {
    //        Character Owner = this.m_parentSynchronizer.OwnerCharacter;
    //        WeaponInstance = Owner.CurrentWeapon;

    //        if (WeaponInstance != null)
    //        {
    //            ApplyStatsToWeapon(WeaponInstance);
    //        }
    //        else
    //        {
    //            ExtendedEffects.Log("SLEx_ScalingWeapon Owner has no weapon equipped!");
    //        }
    //    }

    //    private void ApplyStatsToWeapon(Weapon Weapon)
    //    {
    //        Publisher<EquipEvent>.Handler += (Sender, EquipEvent) =>
    //        {
    //            if (EquipEvent.item == Weapon)
    //            {
    //                UndoStatsOnWeapon(Weapon);
    //            }
    //        };
    //    }



    //    private void UndoStatsOnWeapon(Weapon Weapon)
    //    {

    //    }


    //    private DamageList GetWeaponDamageList(Weapon weapon)
    //    {
    //        return weapon.Damage;
    //    }
    //}
}
