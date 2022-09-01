using SideLoader_ExtendedEffects.Events;
using SideLoader;
using System;
using System.Collections.Generic;

namespace SideLoader_ExtendedEffects.Containers.Triggers
{

    public class SL_OnHitEffect: SL_ParentEffect {
        public OnHitEffect.DamageSourceType RequiredSourceType;
        public DamageType.Types[] DamageTypes;
        public bool RequireAllTypes;
        public int MinDamage;
        public bool OnlyCountRequiredTypes;
        public bool UseHighestType;
        public bool IgnoreDamageReduction;

        public override void ApplyToComponent<T>(T component)
        {
            base.ApplyToComponent<T>(component);
            var comp = component as OnHitEffect;
            comp.RequiredSourceType = this.RequiredSourceType;
            comp.DamageTypes = this.DamageTypes;
            comp.RequireAllTypes = this.RequireAllTypes;
            comp.MinDamage = this.MinDamage;
            comp.OnlyCountRequiredTypes = this.OnlyCountRequiredTypes;
            comp.UseHighestType = this.UseHighestType;
            comp.IgnoreDamageReduction = this.IgnoreDamageReduction;
        }
        public override void SerializeEffect<T>(T effect)
        {
            base.SerializeEffect<T>(effect);
            var comp = effect as OnHitEffect;
            this.RequiredSourceType = comp.RequiredSourceType;
            this.DamageTypes = comp.DamageTypes;
            this.RequireAllTypes = comp.RequireAllTypes;
            this.MinDamage = comp.MinDamage;
            this.OnlyCountRequiredTypes = comp.OnlyCountRequiredTypes;
            this.UseHighestType = comp.UseHighestType;
            this.IgnoreDamageReduction = comp.IgnoreDamageReduction;
        }

    }

    public class OnHitEffect : TriggeredEffect<HitEvent>, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_OnHitEffect);

        public Type GameModel => typeof(OnHitEffect);

        public enum DamageSourceType {
            ANY,
            WEAPON,
            NON_WEAPON
        }

        public DamageSourceType RequiredSourceType;
        public DamageType.Types[] DamageTypes;
        public bool RequireAllTypes;
        public int MinDamage;
        public bool OnlyCountRequiredTypes;
        public bool UseHighestType;
        public bool IgnoreDamageReduction;

        public override void OnEvent(object sender, HitEvent args)
        {
            try{
                //Check if the hit was made with a weapon
                if (
                    (RequiredSourceType == DamageSourceType.NON_WEAPON && args.source is Weapon) ||
                    (RequiredSourceType == DamageSourceType.WEAPON && !(args.source is Weapon))
                )
                {
                    ExtendedEffects.Instance.DebugLogMessage("Wrong Hit Type");
                    return; // Wrong type of hit
                }
                if (!IgnoreDamageReduction)
                {

                    // Sine the hook is pre-damage reduction, we need to process that ourselves
                    if (args.source is Weapon)
                    {
                        args.target.ProcessDamageReduction(args.source as Weapon, args.damage, false);
                    }
                    else if (args.source is StatusEffect)
                    {
                        args.target.ProcessDamageReduction(null, args.damage, (args.source as StatusEffect).IgnoreBarrier);
                    }
                    else
                    {
                        args.target.ProcessDamageReduction(null, args.damage, false);
                    }
                }
                // Highest Type variants of checks
                if (UseHighestType)
                {
                    int highestType = args.damage.GetDTypeOfHighestDamage((List<int>)(DamageType.TypeList));
                    if (DamageTypes != null && DamageTypes.Length != 0)
                    {
                        if (
                            (DamageTypes != null && DamageTypes.Length > 1 && RequireAllTypes) || // Requiers multiple types, but can only match one
                            (!DamageTypes.Contains(args.damage[highestType].Type)) // Highest types isn't among required
                        )
                        {
                            ExtendedEffects.Instance.DebugLogMessage("Highest Damage Type Wrong");
                            return; 
                        }
                    }

                    if (args.damage[highestType].Damage < MinDamage)
                    {
                        ExtendedEffects.Instance.DebugLogMessage("Highest Damage Too Low");
                        return; // Highest damage type doesn't pass minimum
                    }
                }
                else
                {
                    if (RequireAllTypes)
                    {
                        foreach (var type in DamageTypes)
                        {
                            if (!args.damage.Contains(type)) 
                            {
                                ExtendedEffects.Instance.DebugLogMessage("Not All Types Present");
                                return; // If the type doesn't appear in the hit, not all required types are there.
                            }
                        }
                    }
                    else if (DamageTypes != null && DamageTypes.Length != 0)
                    {
                        foreach (var type in DamageTypes)
                        {
                            bool found = false;
                            if (args.damage.Contains(type)) 
                            {
                                found = true;
                                break;
                            }
                            if (!found)
                            {
                                ExtendedEffects.Instance.DebugLogMessage("None Of Types Present");
                                return; // Not one of the types was represented
                            }
                        }
                    }
                }

                List<EffectSynchronizer.EffectCategories> dealerList = new List<EffectSynchronizer.EffectCategories>();
                List<EffectSynchronizer.EffectCategories> targetList = new List<EffectSynchronizer.EffectCategories>();
                if (args.target == this.OwnerCharacter) // Effect owner is the target
                {
                    try {
                        // apply block effects to the source of the hit (usually an enemy hitting the buff owner)
                        dealerList.Add(EffectSynchronizer.EffectCategories.Block);
                        // apply reference effects to the target of the hit (the owner of the effect)
                        targetList.Add(EffectSynchronizer.EffectCategories.Reference);
                    } catch (Exception e) {
                        ExtendedEffects.Instance.DebugLogMessage(e);
                    }
                }
                if (args.dealer == this.OwnerCharacter) // Effect owner dealt the hit; 
                {
                    // Apply Hit effects to target
                    targetList.Add(EffectSynchronizer.EffectCategories.Hit);
                    // Apply Normal effects to source
                    dealerList.Add(EffectSynchronizer.EffectCategories.Normal);
                }
                // Actually apply all of the required effects.
                // Uses apply helper to make sure only one application per update happens
                if (args.dealer == args.target)
                {
                    dealerList.AddRange(targetList);
                    Apply(dealerList.ToArray(), args.dealer, args.hitLocation, args.hitDirection);
                }
                else
                {
                    Apply(dealerList.ToArray(), args.dealer, args.hitLocation, args.hitDirection);
                    Apply(targetList.ToArray(), args.target, args.hitLocation, args.hitDirection);
                }
            } catch (Exception e) {
                ExtendedEffects.Instance.DebugLogMessage("=============Hit Event Error============");
                ExtendedEffects.Instance.DebugLogMessage(e);
            }
            
        }

    }

}
