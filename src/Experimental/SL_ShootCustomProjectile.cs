using HarmonyLib;
using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SideLoader_ExtendedEffects.Experimental
{
    //public class SL_CustomProjectile : Projectile
    //{
    //    public override void RetrieveFXs()
    //    {
    //        base.RetrieveFXs();

    //        GameObject newVFX = OutwardHelpers.GetFromAssetBundle<GameObject>("one", "two", "three");

    //        if (newVFX != null)
    //        {
    //            GameObject VFXInstance = GameObject.Instantiate(newVFX, base.transform);
    //            this.m_projectileFX = VFXInstance.GetComponent<ParticleSystem>();
    //        }
    //    }
    //}


    //public class SL_ShootCustomProjectile : ShootProjectile, ICustomModel
    //{
    //    private Projectile ProjectileInstance = null;

    //    public Type SLTemplateModel => typeof(SL_ShootProjectile);

    //    public Type GameModel => typeof(SL_ShootCustomProjectile);

    //    public override void AwakeInit()
    //    {
    //        base.AwakeInit();

    //        if (ProjectileInstance == null)
    //        {
    //            ProjectileInstance = UnityEngine.Object.Instantiate<SL_CustomProjectile>((SL_CustomProjectile)this.BaseProjectile);
    //            //add an instance of the SL_CustomProjectile to the array for our ShootCustomProjectile and set the ID
    //            this.m_projectiles.AddToArray(ProjectileInstance);
    //            this.m_currentProjectileID = this.m_projectiles.Length;
    //        }

    //    }
    //}
}
