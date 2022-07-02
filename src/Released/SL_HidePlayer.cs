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
}
