﻿using SideLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using static AreaManager;

namespace SideLoader_ExtendedEffects.Released
{
    public class PortalManager
    {
        private Dictionary<string, ActivePortalData> portals = new Dictionary<string, ActivePortalData>();
        private Dictionary<string, PortalInstance> firstPortals = new Dictionary<string, PortalInstance>();
        private Dictionary<string, PortalInstance> secondPortals = new Dictionary<string, PortalInstance>();

        private string Default_AssetBundleName = "magiccircles";
        private string Default_PrefabName = "MagicCircle";
        private string Default_SLPackName = "sideloaderextended-magiccircles";

        private int CurrentAreaID => AreaManager.Instance == null || AreaManager.Instance.CurrentArea == null ? -1 : AreaManager.Instance.CurrentArea.ID;

        private bool AreaSwitchInProgress = false;
        private Vector3 targetPosition;
        private Vector3 targetRotation;
        private AreaManager.AreaEnum targetArea = AreaEnum.Berg;
        private Character targetCharacter;

        public PortalManager()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }



        public void StartAreaSwitchAndSetPosition(Character Character, AreaManager.AreaEnum areaEnum, Vector3 position, Vector3 rotation)
        {
            if (!CanStartAreaSwitch(areaEnum))
            {
                return;
            }

            targetPosition = position;
            targetRotation = rotation;
            targetArea = areaEnum;
            targetCharacter = Character;
            StartAreaSwitch(Character, areaEnum, 0);
        }

        public void StartAreaSwitch(Character Character, AreaManager.AreaEnum areaEnum, int spawnPointIndex = 0, bool moveBag = true)
        {
            if (!CanStartAreaSwitch(areaEnum))
            {
                return;
            }

            Debug.Log("Starting area switch teleport");

            AreaSwitchInProgress = true;

            Area ChosenArea = AreaManager.Instance.GetArea(areaEnum);

            if (ChosenArea != null)
            {
                NetworkLevelLoader.Instance.RequestSwitchArea(ChosenArea.SceneName, spawnPointIndex, 1.5f, moveBag);
            }
            else
            {
                Debug.LogError($"Failed to start Teleport to {areaEnum} Area could not be found");
                ResetAreaSwitch();
            }  
        }

        private bool CanStartAreaSwitch(AreaEnum targetArea)
        {
            return !AreaSwitchInProgress && targetArea != AreaEnum.Nath_Test;
        }

        private void OnSceneLoaded(Scene Scene, LoadSceneMode mode)
        {
            if (Scene.name == "MainMenu_Empty" || Scene.name == "LowMemory_TransitionScene")
            {
                return;
            }

            if (AreaSwitchInProgress && targetCharacter != null)
            {
                if (targetPosition != Vector3.zero)
                {
                    if (Scene.name == AreaManager.Instance.GetArea(targetArea).SceneName)
                    {
                        ExtendedEffects.Instance.StartCoroutine(DelayedTeleport());
                    }
                }
            }


            RecreatePortalsForScene();
        }

        private IEnumerator DelayedTeleport()
        {
            yield return new WaitForSeconds(5f);
            //yield return new WaitUntil(() => CharacterManager.Instance.IsAllCharactersInitialized);

            if (targetCharacter != null)
            {
               NavMeshHit hit;
                if (NavMesh.SamplePosition(targetPosition, out hit, 5f, NavMesh.AllAreas))
                {
                    targetCharacter.Teleport(hit.position, targetRotation);
                }
                else
                {
                    Debug.LogWarning("Could not find valid NavMesh position for teleport, using original position");
                    targetCharacter.Teleport(targetPosition, targetRotation);
                }
            }

            Debug.Log("Finished area switch teleport");
            ResetAreaSwitch();
        }


        private void ResetAreaSwitch()
        {
            AreaSwitchInProgress = false;
            targetPosition = Vector3.zero;
            targetRotation = Vector3.zero;
            targetCharacter = null;
            targetArea = AreaEnum.Nath_Test;

        }


        private void RecreatePortalsForScene()
        {
            foreach (var entry in portals)
            {
                if (entry.Value.AreaID == CurrentAreaID)
                {
                    RecreateExistingPortal(entry.Key);
                }
            }
        }

        private void RecreateExistingPortal(string characterUID)
        {
            if (!portals.ContainsKey(characterUID)) return;

            ActivePortalData portalData = portals[characterUID];
            Character character = CharacterManager.Instance.GetCharacter(characterUID);
            if (character == null) return;

            GameObject prefab = GetPrefabFromAssetsOrDefault(portalData.SLPackName, portalData.AssetBundleName, portalData.PrefabName);
            if (prefab == null) return;

  
            if (portalData.FirstPortal != null)
            {
                GameObject firstObj = GameObject.Instantiate(
                    prefab,
                    portalData.FirstPortal.Position + portalData.FirstPortal.PositionOffset,
                    Quaternion.Euler(portalData.FirstPortal.RotationOffset));

                var firstInstance = firstObj.AddComponent<PortalInstance>();
                firstInstance.Initialize(characterUID, true,
                    portalData.FirstPortal.TeleportCooldown,
                    portalData.FirstPortal.CanTeleportAI,
                    portalData.FirstPortal.CanTeleportPlayers,
                    portalData.FirstPortal.CanTeleportProjectiles);
                firstPortals[characterUID] = firstInstance;

                if (portalData.FirstPortal.RemainingLifetime > 0)
                {
                    firstInstance.SetLifetime(portalData.FirstPortal.RemainingLifetime);
                }
            }

 
            if (portalData.SecondPortal != null)
            {
                GameObject secondObj = GameObject.Instantiate(
                    prefab,
                    portalData.SecondPortal.Position + portalData.SecondPortal.PositionOffset,
                    Quaternion.Euler(portalData.SecondPortal.RotationOffset));

                var secondInstance = secondObj.AddComponent<PortalInstance>();
                secondInstance.Initialize(characterUID, false,
                    portalData.SecondPortal.TeleportCooldown,
                    portalData.SecondPortal.CanTeleportAI,
                    portalData.SecondPortal.CanTeleportPlayers,
                    portalData.SecondPortal.CanTeleportProjectiles);
                secondPortals[characterUID] = secondInstance;

                if (portalData.SecondPortal.RemainingLifetime > 0)
                {
                    secondInstance.SetLifetime(portalData.SecondPortal.RemainingLifetime);
                }
            }
        }

 
        public void PlaceFirstPortal(Character owner, Vector3 position, string slPackName, string assetBundleName,
                    string prefabName, Vector3 posOffset, Vector3 rotOffset, float lifetime, float portalTeleportCooldown,
                    bool canTeleportAI, bool canTeleportPlayers, bool canTeleportProjectiles)
        {
            var portalData = new PortalData
            {
                Position = position,
                PositionOffset = posOffset,
                RotationOffset = rotOffset,
                RemainingLifetime = lifetime,
                TeleportCooldown = portalTeleportCooldown,
                CanTeleportAI = canTeleportAI,
                CanTeleportPlayers = canTeleportPlayers,
                CanTeleportProjectiles = canTeleportProjectiles
            };

            portals[owner.UID] = new ActivePortalData
            {
                FirstPortal = portalData,
                AreaID = CurrentAreaID,
                SLPackName = slPackName,
                AssetBundleName = assetBundleName,
                PrefabName = prefabName
            };

            GameObject firstPortal = SpawnPortalInstance(position, posOffset, rotOffset, slPackName, assetBundleName, prefabName);
            if (firstPortal == null) return;

            var firstInstance = firstPortal.AddComponent<PortalInstance>();
            firstInstance.Initialize(owner.UID, true, portalTeleportCooldown, canTeleportAI, canTeleportPlayers, canTeleportProjectiles);
            firstPortals[owner.UID] = firstInstance;

            if (lifetime > 0)
            {
                firstInstance.SetLifetime(lifetime);
            }
        }

        public void PlaceSecondPortal(Character owner, Vector3 position, string slPackName, string assetBundleName,
            string prefabName, Vector3 posOffset, Vector3 rotOffset, float lifetime, float portalTeleportCooldown,
            bool canTeleportAI, bool canTeleportPlayers, bool canTeleportProjectiles)
        {
            if (!portals.ContainsKey(owner.UID) || portals[owner.UID].AreaID != CurrentAreaID)
                return;

            var portalData = new PortalData
            {
                Position = position,
                PositionOffset = posOffset,
                RotationOffset = rotOffset,
                RemainingLifetime = lifetime,
                TeleportCooldown = portalTeleportCooldown,
                CanTeleportAI = canTeleportAI,
                CanTeleportPlayers = canTeleportPlayers,
                CanTeleportProjectiles = canTeleportProjectiles
            };

            portals[owner.UID].SecondPortal = portalData;

            GameObject secondPortal = SpawnPortalInstance(position, posOffset, rotOffset, slPackName, assetBundleName, prefabName);
            if (secondPortal == null) return;

            var secondInstance = secondPortal.AddComponent<PortalInstance>();
            secondInstance.Initialize(owner.UID, false, portalTeleportCooldown, canTeleportAI, canTeleportPlayers, canTeleportProjectiles);
            secondPortals[owner.UID] = secondInstance;

            if (lifetime > 0)
            {
                secondInstance.SetLifetime(lifetime);
            }
        }

        public AreaTeleporter SpawnAreaPortalInstance(AreaManager.AreaEnum area, Vector3 SpawnPosition, Vector3 TeleportPosition, Vector3 Rotation, string slPackName, string assetBundleName, string prefabName)
        {
            GameObject prefab = GetPrefabFromAssetsOrDefault(slPackName, assetBundleName, prefabName);

            if (prefab == null)
                return null;

            GameObject areaTeleporter = GameObject.Instantiate(
                prefab,
                SpawnPosition,
                Quaternion.Euler(Rotation));
            
            var areaTeleport = areaTeleporter.AddComponent<AreaTeleporter>();

            areaTeleport.area = area;
            areaTeleport.Position = TeleportPosition;
            return areaTeleport;
        }


        private GameObject SpawnPortalInstance(Vector3 Position, Vector3 PostiionOffset, Vector3 Rotation, string slPackName, string assetBundleName, string prefabName)
        {
            GameObject prefab = GetPrefabFromAssetsOrDefault(slPackName, assetBundleName, prefabName);

            if (prefab == null) 
                return null;

            return GameObject.Instantiate(
                prefab,
                Position + PostiionOffset,
                Quaternion.Euler(Rotation));
        }

        private GameObject GetPrefabFromAssetsOrDefault(string SLPackName, string AssetBundleName, string PrefabName)
        {
            GameObject prefab = OutwardHelpers.GetFromAssetBundle<GameObject>(
                SLPackName,
                AssetBundleName,
                PrefabName);

            if (prefab != null)
            {
                return prefab;
            }

            prefab = OutwardHelpers.GetFromAssetBundle<GameObject>(
                Default_SLPackName,
                Default_AssetBundleName,
                Default_PrefabName);

            if (prefab != null)
            {
                return prefab;
            }

            ExtendedEffects._Log.LogMessage($"Could not find a valid prefab for Portal Visual using SLPack : {SLPackName} Asset Bundle name : {AssetBundleName}  Prefab name : {PrefabName}");
            return null;
        }
        public Vector3? GetNearestPortalPosition(Character portalOwner, Vector3 fromPosition, float maxDistance, bool getFarthest = false)
        {
            if (!portals.ContainsKey(portalOwner.UID) ||
                portals[portalOwner.UID].AreaID != CurrentAreaID)
                return null;

            Vector3? bestPosition = null;
            float bestDistance = getFarthest ? -1f : float.MaxValue;

            // Check first portal
            if (HasFirstPortal(portalOwner))
            {
                Vector3 firstPos = portals[portalOwner.UID].FirstPortal.Position;
                float distance = Vector3.Distance(fromPosition, firstPos);

                if (distance <= maxDistance)
                {
                    if (getFarthest)
                    {
                        if (distance > bestDistance)
                        {
                            bestDistance = distance;
                            bestPosition = firstPos;
                        }
                    }
                    else if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        bestPosition = firstPos;
                    }
                }
            }

            // Check second portal
            if (HasSecondPortal(portalOwner))
            {
                Vector3 secondPos = portals[portalOwner.UID].SecondPortal.Position;
                float distance = Vector3.Distance(fromPosition, secondPos);

                if (distance <= maxDistance)
                {
                    if (getFarthest)
                    {
                        if (distance > bestDistance)
                        {
                            bestDistance = distance;
                            bestPosition = secondPos;
                        }
                    }
                    else if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        bestPosition = secondPos;
                    }
                }
            }

            return bestPosition;
        }

        public void RegisterPortalInstance(PortalInstance instance, string characterUID, bool isFirst)
        {
            if (isFirst)
                firstPortals[characterUID] = instance;
            else
                secondPortals[characterUID] = instance;
        }

        public void UnregisterPortalInstance(PortalInstance instance, string characterUID, bool isFirst)
        {
            if (isFirst)
                firstPortals.Remove(characterUID);
            else
                secondPortals.Remove(characterUID);
        }

        public bool HasFirstPortal(Character owner) =>
            portals.ContainsKey(owner.UID) &&
            portals[owner.UID].AreaID == CurrentAreaID;

        public bool HasSecondPortal(Character owner) =>
            portals.ContainsKey(owner.UID) &&
            portals[owner.UID].AreaID == CurrentAreaID &&
            portals[owner.UID].SecondPortal != null;

        public void ClearPortals(Character owner)
        {
            string uid = owner.UID;
            if (firstPortals.ContainsKey(uid) && firstPortals[uid] != null)
                firstPortals[uid].Cleanup();
            if (secondPortals.ContainsKey(uid) && secondPortals[uid] != null)
                secondPortals[uid].Cleanup();

            portals.Remove(uid);
            firstPortals.Remove(uid);
            secondPortals.Remove(uid);
        }

        public PortalInstance GetFirstPortal(string characterUID)
        {
            return firstPortals.ContainsKey(characterUID) ? firstPortals[characterUID] : null;
        }

        public PortalInstance GetSecondPortal(string characterUID)
        {
            return secondPortals.ContainsKey(characterUID) ? secondPortals[characterUID] : null;
        }

        public Dictionary<string, ActivePortalData> GetSaveData()
        {
            return new Dictionary<string, ActivePortalData>(portals);
        }

        public void ApplySaveData(Dictionary<string, ActivePortalData> savedData)
        {
            portals = new Dictionary<string, ActivePortalData>(savedData);

           
            foreach (var entry in portals)
            {
                if (entry.Value.AreaID == CurrentAreaID)
                {
                    RecreateExistingPortal(entry.Key);
                }
            }
        }
    }
}

