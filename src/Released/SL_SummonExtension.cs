using SideLoader;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SideLoader_ExtendedEffects
{
    public class SL_SummonExtension : SL_Effect, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_SummonExtension);
        public Type GameModel => typeof(SummonExtension);

        public Vector3 NewBaseColor;
        public Vector3 NewParticlesColor;
        public SL_Damage NewWeaponDamage;

        public string StatusEffectOnSpawn;

        public override void ApplyToComponent<T>(T component)
        {
            SummonExtension comp = component as SummonExtension;
            comp.NewBaseColor = NewBaseColor;
            comp.NewParticlesColor = NewParticlesColor;
            comp.NewWeaponDamage = new DamageType(NewWeaponDamage.Type, NewWeaponDamage.Damage);
            comp.StatusEffectOnSpawn = StatusEffectOnSpawn;
        }

        public override void SerializeEffect<T>(T effect)
        {
            SummonExtension comp = effect as SummonExtension;
            this.NewBaseColor = comp.NewBaseColor;
            this.NewParticlesColor = comp.NewParticlesColor;
            this.StatusEffectOnSpawn = comp.StatusEffectOnSpawn;
            this.NewWeaponDamage = new SL_Damage()
            {
                Damage = comp.NewWeaponDamage.Damage,
                Type = comp.NewWeaponDamage.Type
            };

        }
    }
    public class SummonExtension : Effect
    {
        #region Visual
        public Vector3 NewBaseColor;
        public Vector3 NewParticlesColor;
        #endregion

        #region Stat

        public DamageType NewWeaponDamage;
        public string StatusEffectOnSpawn;

        #endregion

        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            if (_affectedCharacter.CurrentSummon != null)
            {
                UpdateSummonBaseColor(_affectedCharacter);
                UpdateSummonParticles(_affectedCharacter);


                if (!string.IsNullOrEmpty(StatusEffectOnSpawn))
                {
                    AddSummonStatusEffects(_affectedCharacter.CurrentSummon);
                }

                if (NewWeaponDamage != null)
                {
                    OverrideSummonWeaponDamage(_affectedCharacter.CurrentSummon);
                }

            }
        }



        private void UpdateSummonBaseColor(Character _affectedCharacter)
        {
            foreach (var item in this.OwnerCharacter.CurrentSummon.GetComponentsInChildren<Renderer>())
            {
                item.material.SetColor("_Color", new Color(NewBaseColor.x, NewBaseColor.y, NewBaseColor.z));
            }
        }

        private void UpdateSummonParticles(Character _affectedCharacter)
        {
            foreach (var ps in this.OwnerCharacter.CurrentSummon.GetComponentsInChildren<ParticleSystem>())
            {
                if (ps)
                {
                    ParticleSystem.MainModule main = ps.main;
                    main.startColor = new Color(NewParticlesColor.x, NewParticlesColor.y, NewParticlesColor.z);
                }
            }
        }

        private void AddSummonStatusEffects(Character _summonCharacter)
        {
            StatusEffect statusEffect = ResourcesPrefabManager.Instance.GetStatusEffectPrefab(StatusEffectOnSpawn);

            if (statusEffect != null)
            {
                _summonCharacter.StatusEffectMngr.AddStatusEffect(statusEffect.StatusName);
            }
            else
            {
                SideLoader_ExtendedEffects.ExtendedEffects._Log.LogMessage($"SL_SummonExtension : Could not find Status by name {StatusEffectOnSpawn}");
            }
        }

        private void OverrideSummonWeaponDamage(Character _summonCharacter)
        {
            //SideLoaderCustomEffects.SL_ExtendedEffects.Log.LogMessage($"Override Summon Weapon Damage");
            if (_summonCharacter.CurrentWeapon != null && NewWeaponDamage != null)
            {
                OutwardHelpers.UpdateWeaponDamage(_summonCharacter.CurrentWeapon, new DamageList(NewWeaponDamage));
            }
        }
    }
    //public class SL_BothWeaponDamage : SL_PunctualDamage, ICustomModel
    //{
      
    //    public override void ApplyToComponent<T>(T component)
    //    {
    //        base.ApplyToComponent<T>(component);
    //        WeaponDamage weaponDamage = component as WeaponDamage;
    //        weaponDamage.ForceOnlyLeftHand = this.ForceOnlyLeftHand;
    //        weaponDamage.OverrideDType = this.OverrideType;
    //        weaponDamage.WeaponDamageMult = this.Damage_Multiplier;
    //        weaponDamage.WeaponDamageMultKBack = this.Damage_Multiplier_Kback;
    //        weaponDamage.WeaponDamageMultKDown = this.Damage_Multiplier_Kdown;
    //        weaponDamage.WeaponKnockbackMult = this.Impact_Multiplier;
    //        weaponDamage.WeaponKnockbackMultKBack = this.Impact_Multiplier_Kback;
    //        weaponDamage.WeaponKnockbackMultKDown = this.Impact_Multiplier_Kdown;
    //    }

       
    //    public override void SerializeEffect<T>(T effect)
    //    {
    //        base.SerializeEffect<T>(effect);
    //        WeaponDamage weaponDamage = effect as WeaponDamage;
    //        this.ForceOnlyLeftHand = weaponDamage.ForceOnlyLeftHand;
    //        this.OverrideType = weaponDamage.OverrideDType;
    //        this.Damage_Multiplier = weaponDamage.WeaponDamageMult;
    //        this.Damage_Multiplier_Kback = weaponDamage.WeaponDamageMultKBack;
    //        this.Damage_Multiplier_Kdown = weaponDamage.WeaponDamageMultKDown;
    //        this.Impact_Multiplier = weaponDamage.WeaponKnockbackMult;
    //        this.Impact_Multiplier_Kback = weaponDamage.WeaponKnockbackMultKBack;
    //        this.Impact_Multiplier_Kdown = weaponDamage.m_knockback;
    //    }

    //    public DamageType.Types OverrideType;

    //    public bool ForceOnlyLeftHand;
    //    public float Damage_Multiplier;

    //    public float Damage_Multiplier_Kback;
    //    public float Damage_Multiplier_Kdown;
    //    public float Impact_Multiplier;
    //    public float Impact_Multiplier_Kback;
    //    public float Impact_Multiplier_Kdown;

    //    public Type SLTemplateModel => typeof(SL_BothWeaponDamage);

    //    public Type GameModel => typeof(BothWeaponsDamage);
    //}

    //public class BothWeaponsDamage : PunctualDamage
    //{
    //    public override void ActivateLocally(Character _targetCharacter, object[] _infos)
    //    {
    //        List<Weapon> Weapons = new List<Weapon>();

    //        if (this.m_parentItem is Weapon SkillWeapon)
    //        {
    //            Weapons.Add(SkillWeapon);
    //        }

    //        if (this.OwnerCharacter && this.OwnerCharacter.LeftHandWeapon is Weapon LeftHandWeapon)
    //        {
    //            Weapons.Add(LeftHandWeapon);
    //        }

    //        if (Weapons.Count == 1)
    //        {
    //            base.ActivateLocally(_targetCharacter, _infos);
    //        }
    //        else
    //        {
    //            this.m_startPos = (Vector3)_infos[0];
    //            this.m_dir = (Vector3)_infos[1];
    //            if (_targetCharacter != null && !_targetCharacter.IsDead && !_targetCharacter.IsUndyingDead)
    //            {
    //                foreach (var weapon in Weapons)
    //                {
    //                    this.tmpTags.Clear();
    //                    this.m_tempList.Clear();
    //                    this.m_knockback = 0f;
    //                    this.BuildDamage(_targetCharacter, ref this.m_tempList, ref this.m_knockback);

    //                    if (weapon)
    //                    {
    //                        this.tmpTags.AddRange(weapon.Tags);
    //                    }


    //                    if (this.DamageAmplifiedByOwner && base.SourceCharacter && base.SourceCharacter.Stats)
    //                    {
    //                        if (this.m_parentItem)
    //                        {
    //                            this.tmpTags.AddRange(this.m_parentItem.Tags);
    //                        }
    //                        if (this.m_parentItem != base.SourceSynchronizer)
    //                        {
    //                            Item item = base.SourceSynchronizer as Item;
    //                            if (item != null)
    //                            {
    //                                this.tmpTags.AddRange(item.Tags);
    //                            }
    //                        }
    //                        StatusEffect statusEffect = base.ParentSynchronizer as StatusEffect;
    //                        if (statusEffect != null)
    //                        {
    //                            this.tmpTags.AddRange(statusEffect.InheritedTags);
    //                        }
    //                        base.SourceCharacter.Stats.GetAmplifiedDamage(this.tmpTags, ref this.m_tempList);
    //                        this.m_knockback = base.SourceCharacter.Stats.GetAmplifiedImpact(this.tmpTags, this.m_knockback);
    //                    }
    //                    bool flag = false;
    //                    MeleeSkill meleeSkill = base.ParentSynchronizer as MeleeSkill;
    //                    if (meleeSkill)
    //                    {
    //                        flag = meleeSkill.WasBlocked;
    //                    }
    //                    if (!flag)
    //                    {
    //                        DamageList dealtDamage = this.DealHit(_targetCharacter);
    //                        if (weapon)
    //                        {
    //                            weapon.ProcessDamageDealt(dealtDamage);
    //                            return;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        this.DealBlock(_targetCharacter);
    //                    }
    //                }

    //            }
    //        }


    //    }
    //}
}