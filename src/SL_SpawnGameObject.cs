using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SideLoader_ExtendedEffects
{

    /// <summary>
    /// TEST CLASS - Going to be the basis for the Mount, this should disable player, spawn mount and attach player to it - will need to add a simple controller just test it all in here
    /// </summary>
    public class SL_SpawnGameObjectNearAffected : SL_Effect, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_SpawnGameObjectNearAffected);
        public Type GameModel => typeof(SLEx_SpawnGameObjectNearAffected);


        public string SLPackName;
        public string AssetBundleName;
        public string PrefabName;
        public Vector3 PositionOffset;


        public override void ApplyToComponent<T>(T component)
        {
          
        }

        public override void SerializeEffect<T>(T effect)
        {
           
        }
    }

    public class SLEx_SpawnGameObjectNearAffected : Effect
    {
        public string SLPackName;
        public string AssetBundleName;
        public string PrefabName;
        public Vector3 PositionOffset;

        private GameObject Instance;

        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            if (Instance != null)
            {
                return;
            }

            GameObject Prefab = OutwardHelpers.GetFromAssetBundle<GameObject>(SLPackName, AssetBundleName, PrefabName);

            if (Prefab != null)
            {
                Instance = GameObject.Instantiate(Prefab, _affectedCharacter.transform.position + PositionOffset, Quaternion.identity);
            }
        }

        public override void StopAffectLocally(Character _affectedCharacter)
        {
            if (Instance != null)
            {
                GameObject.Destroy(Instance);
            }
        }
    }
}
