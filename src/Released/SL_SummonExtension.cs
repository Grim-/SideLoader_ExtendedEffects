using SideLoader;
using System;
using UnityEngine;

namespace SideLoader_ExtendedEffects
{
    public class SL_SummonExtension : SL_Effect, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_SummonExtension);
        public Type GameModel => typeof(SLEx_SummonExtension);

        public Vector3 NewBaseColor;
        public Vector3 NewParticlesColor;
        public SL_Damage NewWeaponDamage;

        public string StatusEffectOnSpawn;

        public override void ApplyToComponent<T>(T component)
        {
            SLEx_SummonExtension comp = component as SLEx_SummonExtension;
            comp.NewBaseColor = NewBaseColor;
            comp.NewParticlesColor = NewParticlesColor;
            comp.NewWeaponDamage = new DamageType(NewWeaponDamage.Type, NewWeaponDamage.Damage);
            comp.StatusEffectOnSpawn = StatusEffectOnSpawn;
        }

        public override void SerializeEffect<T>(T effect)
        {
            SLEx_SummonExtension comp = effect as SLEx_SummonExtension;
            this.NewBaseColor = comp.NewBaseColor;
            this.NewParticlesColor = comp.NewParticlesColor;
            this.StatusEffectOnSpawn = comp.StatusEffectOnSpawn;
        }
    }
    public class SLEx_SummonExtension : Effect
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
}