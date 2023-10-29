using SideLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SideLoader_ExtendedEffects
{
    /// <summary>
    /// Intended to allow modders to hide the player model for a period of time to emulate invisibilty - poorly - I might make a dissolve shader for this.
    /// </summary>
    public class SL_HidePlayer : SL_Effect, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_HidePlayer);
        public Type GameModel => typeof(SLEx_HidePlayer);

        public float HideTime;
        public override void ApplyToComponent<T>(T component)
        {
            SLEx_HidePlayer sLEx_HidePlayer = component as SLEx_HidePlayer;
            sLEx_HidePlayer.HideTime = HideTime;
        }

        public override void SerializeEffect<T>(T effect)
        {
            SLEx_HidePlayer sLEx_HidePlayer = effect as SLEx_HidePlayer;
            this.HideTime = sLEx_HidePlayer.HideTime;
        }
    }

    ///TODO : Custom base class for Effects that make use of coroutines, otherwise depending on the context the effect is called in, it could start multiple a frame

    public class SLEx_HidePlayer : Effect, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_HidePlayer);
        public Type GameModel => typeof(SLEx_HidePlayer);

        public float HideTime;

        private bool IsRunning = false;

        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            SL.Log("Activating Locally");
            if (!IsRunning)
            {
                StartCoroutine(ToggleCharacterVisual(this.OwnerCharacter));
            }
        }

        private IEnumerator ToggleCharacterVisual(Character _affectedCharacter)
        {
            IsRunning = true;
            _affectedCharacter.VisualHolderTrans.gameObject.SetActive(false);

            yield return new WaitForSeconds(HideTime);

            _affectedCharacter.VisualHolderTrans.gameObject.SetActive(true);

            IsRunning = false;
            yield break;
        }
    }


    //public class SL_WeaponDamageTag : PunctualDamage
    //{

    //    public List<Tag> AcceptableTags;
    //    public bool UseMainHand = true;
    //    public bool UseOffHand = false;

    //    public override Weapon BuildDamage(Character _targetCharacter, ref DamageList _list, ref float _knockback)
    //    {
    //        Weapon result =  base.BuildDamage(_targetCharacter, ref _list, ref _knockback);


    //        if (this.ParentItem && this.ParentItem is AttackSkill)
    //        {
    //            foreach (var item in AcceptableTags)
    //            {
    //                if (this.ParentItem.OwnerCharacter.CurrentWeapon != null && this.ParentItem.OwnerCharacter.CurrentWeapon.HasTag(item))
    //                {
                        
    //                }
    //            }
    //        }
    //        else
    //        {

    //        }
    //    }

    //    protected virtual void BuildDamage(Weapon _weapon, Character _targetCharacter, bool _isSkillOrShield, ref DamageList _list, ref float _knockback)
    //    {;
    //        if (_targetCharacter)
    //        {
    //            _weapon.ReduceDurability(this.WeaponDurabilityLoss);
    //            _weapon.ReduceDurability(this.WeaponDurabilityLossPercent / 100f * (float)_weapon.MaxDurability);
    //            if (this.m_attackSkill && this.SyncWeaponHitEffects)
    //            {
    //                _weapon.SynchronizeHitEffects(_targetCharacter, this.m_startPos, this.m_dir);
    //            }
    //        }
    //        DamageList damageList;
    //        if (_isSkillOrShield)
    //        {
    //            damageList = _weapon.Damage.Clone();
    //            ProjectileWeapon projectileWeapon = _weapon as ProjectileWeapon;
    //            if (projectileWeapon != null)
    //            {
    //                damageList += projectileWeapon.Loadout.GetAmmunitionDamage(false);
    //            }
    //        }
    //        else
    //        {
    //            damageList = _weapon.GetLastAttackDamage(false).Clone();
    //        }
    //        for (int i = 0; i < damageList.Count; i++)
    //        {
    //            DamageType damageType;
    //            if (this.OverrideDType == DamageType.Types.Count)
    //            {
    //                damageType = new DamageType(damageList[i]);
    //            }
    //            else
    //            {
    //                damageType = new DamageType(this.OverrideDType, damageList[i].Damage);
    //            }
    //            if (_weapon.Stats)
    //            {
    //                damageType.Damage *= _weapon.Stats.Effectiveness;
    //            }
    //            damageType.Damage *= this.DamageMult(_targetCharacter, _isSkillOrShield);
    //            _list.Add(damageType);
    //        }
    //        float num;
    //        if (_isSkillOrShield)
    //        {
    //            num = _weapon.Impact;
    //            ProjectileWeapon projectileWeapon2 = _weapon as ProjectileWeapon;
    //            if (projectileWeapon2 != null)
    //            {
    //                num += projectileWeapon2.Loadout.GetAmmunitionImpact(false);
    //            }
    //        }
    //        else
    //        {
    //            num = _weapon.GetLastAttackKnockback(false);
    //        }
    //        _knockback += num * this.KnockbackMult(_targetCharacter, _isSkillOrShield);
    //    }
    //}
}
