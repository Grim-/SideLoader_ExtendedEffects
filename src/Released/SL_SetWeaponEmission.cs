using SideLoader;
using System;
using System.Collections;
using UnityEngine;
namespace SideLoader_ExtendedEffects
{
    public class SL_SetWeaponEmission : SL_Effect, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_SetWeaponEmission);
        public Type GameModel => typeof(SLEx_SetWeaponEmission);

        /// <summary>
        /// The Color to Change to
        /// </summary>
        public Vector3 Color;
        public float ColorIntensity = 4f;
        public float ColorLerpTime = 0.5f;
        public override void ApplyToComponent<T>(T component)
        {
            SLEx_SetWeaponEmission SLEx_SetWeaponEmission = component as SLEx_SetWeaponEmission;
            // apply values from this template to game component
            SLEx_SetWeaponEmission.ColorIntensity = ColorIntensity;
            SLEx_SetWeaponEmission.NewColor = Color;
            SLEx_SetWeaponEmission.ColorLerpTime = ColorLerpTime;
        }

        public override void SerializeEffect<T>(T component)
        {
            // write values from component to this template
            SLEx_SetWeaponEmission effect = component as SLEx_SetWeaponEmission;
            this.Color = effect.NewColor;
            this.ColorIntensity = effect.ColorIntensity;
            this.ColorLerpTime = effect.ColorLerpTime;
        }
    }

    public class SLEx_SetWeaponEmission : Effect, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_SetWeaponEmission);
        public Type GameModel => typeof(SLEx_SetWeaponEmission);

        public Vector3 NewColor;
        public float ColorIntensity;
        public float ColorLerpTime;

        private bool IsRunning = false;

        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            Weapon EquippedWeapon = this.OwnerCharacter.CurrentWeapon;

            if (EquippedWeapon != null)
            {
                ///Loaded Visual > Weapon Renderer usually where BoxCollider is > Renderer > Material
                Renderer WeaponRendererInstance = EquippedWeapon.LoadedVisual.GetComponentInChildren<BoxCollider>().GetComponent<Renderer>();
                if (WeaponRendererInstance != null)
                {
                    //Don't start multiple coroutines, wait until the current one is complete
                    if (!IsRunning)
                    {
                        StartCoroutine(LerpMaterialEmissionColor(WeaponRendererInstance, ColorLerpTime, new Color(NewColor.x, NewColor.y, NewColor.z, 1f), ColorIntensity));
                    }

                }
            }
        }

        /// _EmissionColor for Emission or _Color for diffuse color
        private IEnumerator LerpMaterialEmissionColor(Renderer Renderer, float lerpTime, Color newColor, float Intensity)
        {
            IsRunning = true;
            Color OriginalColor = Renderer.material.GetColor("_EmissionColor");

            float timer = 0;
            while (timer < lerpTime)
            {

                float currentPercent = timer / lerpTime;
                Renderer.material.SetColor("_EmissionColor", Color.Lerp(OriginalColor, newColor * Intensity, currentPercent));
                timer += Time.deltaTime;
                yield return null;
            }

            IsRunning = false;
            yield break;
        }

    }
}
