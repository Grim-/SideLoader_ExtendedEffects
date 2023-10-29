using System;
using System.Collections.Generic;
using SideLoader;
using UnityEngine;

namespace SideLoader_ExtendedEffects.Containers
{

    public abstract class SL_ParentEffect : SL_Shooter
    {
        public EditBehaviours EffectBehavior = EditBehaviours.Destroy;
        public SL_EffectTransform[] ChildEffects;
        public int ActivationLimit;

        public override void ApplyToComponent<T>(T component)
        {
            base.ApplyToComponent(component);
            try {
                var comp = component as ParentEffect;

                var obj = new GameObject();
                GameObject.DontDestroyOnLoad(obj);
                obj.SetActive(false);
                comp.ActivationLimit = this.ActivationLimit;
                comp.ChildEffects = obj.gameObject.AddComponent<SubEffect>();
                SL_EffectTransform.ApplyTransformList(comp.ChildEffects.transform, ChildEffects, EffectBehavior);
            } catch (Exception e) {
                SL.Log(e);
            }
        }

        public override void SerializeEffect<T>(T effect)
        {
            base.SerializeEffect(effect);
            try {
                var comp = effect as ParentEffect;
                if (comp.ChildEffects.transform.childCount > 0) {
                    List<SL_EffectTransform> list = new List<SL_EffectTransform>();
                    foreach (Transform child in comp.ChildEffects.transform)
                    {
                        SL_EffectTransform effectsChild = SL_EffectTransform.ParseTransform(child);

                        if (effectsChild.HasContent)
                        {
                            list.Add(effectsChild);
                        }
                    }
                    ChildEffects = list.ToArray();
                }
                this.ActivationLimit = comp.ActivationLimit;
            } catch (Exception e) {
                SL.Log(e);
            }
        }
    }

    public abstract class ParentEffect: Shooter {

        public SubEffect ChildEffects;
        public int ActivationLimit;

        private float lastApplyTime = 0;
        private Dictionary<Character, int> affectedThisInterval;

        public override void Setup(Character.Factions[] _targetFactions, Transform _parent)
        {
            try {
                base.Setup(_targetFactions, _parent);
                
                if (this.m_subEffects == null)
                {
                    this.m_subEffects = new SubEffect[] {
                        UnityEngine.Object.Instantiate<SubEffect>(this.ChildEffects)
                    };
                }
                this.m_subEffects[0].gameObject.SetActive(true);
                this.m_subEffects[0].Setup(this, 0, _targetFactions, base.transform); // Not an independent object, so it should attach to the skill transform tree
                foreach (Shooter component in this.m_subEffects[0].GetComponentsInChildren<Shooter>(true)) {
                    component.Setup(_targetFactions, _parent); // Raw SubEffect doesn't set up it's children, so we need to do this here
                }
            } catch (Exception e) {
                SL.Log("=========Setup Error!===========");
                SL.Log(e);
            }
        }

        // Always gotta clean up, so that might as well be on the parent class.
        public override void StopAffectLocally(Character _affectedCharacter)
        {
            foreach (EffectSynchronizer.EffectCategories category in Enum.GetValues(typeof(EffectSynchronizer.EffectCategories))) {
                this.m_subEffects[0].StopAllEffects(category, _affectedCharacter);
            }
            base.StopAffectLocally(_affectedCharacter);
        }

                
        public virtual void StartApply(EffectSynchronizer.EffectCategories[] categories, Character affectedCharacter, Vector3 pos, Vector3 dir) {
            if (Time.time - this.lastApplyTime > 0)
            {
                this.lastApplyTime = Time.time;
                if (this.affectedThisInterval == null)
                {
                    this.affectedThisInterval = new Dictionary<Character, int>();
                }
                else
                {
                    this.affectedThisInterval.Clear();
                }
            }
            int activations = 0;
            if (!this.affectedThisInterval.TryGetValue(affectedCharacter, out activations))
            {
                this.affectedThisInterval.Add(affectedCharacter, 0);
            }
            if (this.ActivationLimit == 0 || activations < this.ActivationLimit)
            {
                this.affectedThisInterval[affectedCharacter] += 1;
                foreach (var category in categories)
                {
                    this.m_subEffects[0].SynchronizeEffects(category, affectedCharacter, pos, dir);
                }
            }
        }
        public virtual void StopApply(EffectSynchronizer.EffectCategories[] categories, Character affectedCharacter)
        {
            foreach (var category in categories)
            {
                this.m_subEffects[0].StopAllEffects(category, affectedCharacter);
            }
        }

        public virtual void StartApply(EffectSynchronizer.EffectCategories[] categories, Character affectedCharacter)
        {
            Vector3 pos = Vector3.zero;
            Vector3 dir = Vector3.zero;
            GetActivationInfos(affectedCharacter, ref pos, ref dir);
            StartApply(categories, affectedCharacter, pos, dir);
        }

        public virtual void Apply(EffectSynchronizer.EffectCategories[] categories, Character affectedCharacter, Vector3 pos, Vector3 dir) {
            StartApply(categories, affectedCharacter, pos, dir);
            StopApply(categories, affectedCharacter);
        }

        public virtual void Apply(EffectSynchronizer.EffectCategories[] categories, Character affectedCharacter)
        {
            StartApply(categories, affectedCharacter); 
            StopApply(categories, affectedCharacter);
        }

    }

}