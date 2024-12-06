using SideLoader;
using System;

public class SL_OverridePlayerTemp : SL_Effect, ICustomModel
{
    public Type SLTemplateModel => typeof(SL_OverridePlayerTemp);
    public Type GameModel => typeof(OverridePlayerTemp);

    public float Amount;

    public override void ApplyToComponent<T>(T component)
    {
        OverridePlayerTemp effect = component as OverridePlayerTemp;
        effect.Amount = this.Amount;
    }

    public override void SerializeEffect<T>(T effect)
    {
        OverridePlayerTemp comp = effect as OverridePlayerTemp;
        this.Amount = comp.Amount;
    }

    public class OverridePlayerTemp : Effect, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_OverridePlayerTemp);
        public Type GameModel => typeof(OverridePlayerTemp);

        public float Amount;


        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            if (_affectedCharacter.TryGetComponent<PlayerCharacterStats>(out PlayerCharacterStats pcs))
            {
                pcs.m_overrideCharTemp = Amount;
            }
        }
    }
}
