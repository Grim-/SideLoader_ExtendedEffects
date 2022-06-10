//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SideLoader_ExtendedEffects.Released
//{
//    using SideLoader;
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Text;
//    using System.Threading.Tasks;
//    using UnityEngine;

//    namespace SideLoader_ExtendedEffects
//    {
//        /// <summary>
//        /// Spawn a VFX GameObject/Prefab and Attach the Player to it, disable player control and enable simple controller
//        /// </summary>
//        public class SL_SpawnMount : SL_Effect, ICustomModel
//        {
//            public Type SLTemplateModel => typeof(SL_SpawnMount);
//            public Type GameModel => typeof(SLEx_SpawnMount);

//            public string SLPackName;
//            public string AssetBundleName;
//            public string PrefabName;

//            public Vector3 PositionOffset;
//            public Vector3 RotationOffset;

//            public override void ApplyToComponent<T>(T component)
//            {
//                SLEx_SpawnMount comp = component as SLEx_SpawnMount;
//                comp.SLPackName = SLPackName;
//                comp.AssetBundleName = AssetBundleName;
//                comp.PrefabName = PrefabName;

//                comp.PositionOffset = PositionOffset;
//                comp.RotationOffset = RotationOffset;
//            }

//            public override void SerializeEffect<T>(T effect)
//            {

//            }
//        }

//        public class SLEx_SpawnMount : Effect, ICustomModel
//        {
//            public Type SLTemplateModel => typeof(SL_SpawnMount);
//            public Type GameModel => typeof(SLEx_SpawnMount);

//            public string SLPackName;
//            public string AssetBundleName;
//            public string PrefabName;

//            public Vector3 PositionOffset;
//            public Vector3 RotationOffset;
//            public GameObject Prefab;

//            private GameObject MountInstance;

//            public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
//            {
//                Prefab = OutwardHelpers.GetFromAssetBundle<GameObject>(SLPackName, AssetBundleName, PrefabName);

//                if (Prefab == null)
//                {
//                    ExtendedEffects.Log.LogMessage($"SLEx_CUSTOM_MOUNT Prefab from AssetBundle was null.");
//                    return;
//                }

//                //Its just easier to destroy it and recreate it. Or I can figure out the bug where summoning it a second time will teleport player and mount to origin, likely because the player position when its parented being 0,0,0, cba to type more ask me lol.
//                if (MountInstance != null)
//                {
//                    Destroy(MountInstance);
//                }

//                if (MountInstance == null)
//                {
//                    MountInstance = GameObject.Instantiate(Prefab, _affectedCharacter.transform.position + PositionOffset, Quaternion.Euler(RotationOffset));
//                    Emo_BasicMountController basicMountController = MountInstance.AddComponent<Emo_BasicMountController>();
//                    basicMountController.SetMountTarget(_affectedCharacter);
//                }
//            }


//            public override void StopAffectLocally(Character _affectedCharacter)
//            {
//                if (MountInstance)
//                {
//                    GameObject.Destroy(MountInstance);
//                }
//            }

//            public override void CleanUpOnDestroy()
//            {
//                base.CleanUpOnDestroy();

//                if (MountInstance)
//                {
//                    GameObject.Destroy(MountInstance);
//                }
//            }
//        }


//        public class MountInteraction : InteractionBase
//        {
//            private Emo_BasicMountController MOUNT_CONTROLLER => GetComponent<Emo_BasicMountController>();

//            public override bool CanBeActivated => true;
//            public override void Activate(Character _character)
//            {

//                ExtendedEffects.Log.LogMessage("ON ACtivate basic interaction");
//                if (MOUNT_CONTROLLER != null)
//                {
//                    MOUNT_CONTROLLER.SetMountTarget(_character);
//                }
//            }

//        }
//    }

//}
