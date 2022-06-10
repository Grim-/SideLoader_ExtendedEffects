//using SideLoader;
//using System;
//using System.Linq;
////Applies the SubEffect In Radius around the target of the current Effect..hopefully
//public class SL_ApplyChildEffectInRadius : SL_Effect, ICustomModel
//{
//    public Type SLTemplateModel => typeof(SL_ApplyChildEffectInRadius);
//    public Type GameModel => typeof(SLEx_ApplyChildEffectInRadius);


//    public override void ApplyToComponent<T>(T component)
//    {
    
//    }

//    public override void SerializeEffect<T>(T effect)
//    {
       
//    }
//}

//public class SLEx_ApplyChildEffectInRadius : Effect, ICustomModel
//{
//    public Type SLTemplateModel => typeof(SL_ApplyChildEffectInRadius);
//    public Type GameModel => typeof(SLEx_ApplyChildEffectInRadius);

//    public Effect Effect;

//    public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
//    {
//        if (HasSubEffects())
//        {
//            foreach (var _subEffect in m_subEffects.ToArray())
//            {
//                if (_subEffect == this)
//                {
//                    SideLoader_ExtendedEffects.ExtendedEffects.Log.LogMessage($"SL_ApplyChildEffectInRadius SL_Effect contains itself as a SubEffect this isn't allowed.");
//                    continue;
//                }
//                else
//                {
//                    ActivateSubEffect(_subEffect, _affectedCharacter, _infos);
//                }            
//            }
//        }
//    }

//    private bool HasSubEffects()
//    {
//        return this.SubEffects.Length > 0;
//    }

//    private void ActivateSubEffect(SubEffect subEffect, Character _affectedCharacter, object[] _infos)
//    {
//        foreach (var _Effect in subEffect.m_effects)
//        {
//            _Effect.Value.Effect.ActivateLocally(_affectedCharacter, _infos);
//        }

//    }
//}

