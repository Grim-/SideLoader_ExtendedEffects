# SideLoader - Extended Effects




### SL_ForceAIState

If the target of this effect is a CharacterAI then they are forced into the state specified by AIState

| Parameter Name | Description |
| ---| ------------- |
| AIState  | an integer representing the state you wish to put the target CharacterAI in.   |


__AIState ID Reference__
| ID | Name |
| ---| ------------- |
| 0  | Wander  |
| 1  | Suspicious  |
| 2  | Combat  |
| 3  | CombatFlee  |


XML Example

```xml
<?xml version="1.0" encoding="utf-8" ?>
<SL_Effect xsi:type="SL_ForceAIState">
  <Delay>0</Delay>
  <SyncType>OwnerSync</SyncType>
  <OverrideCategory>None</OverrideCategory>
  <AIState>0</AIState>
</SL_Effect>
```
----------------------------------------------------------------------------------------------------------------------------------------------------------------

### SL_PlayAssetBundleVFX
Spawns a GameObject from the specified SLPack AssetBundle and attach it to the player.

| Parameter Name | Description |
| ---| ------------- |
| SLPackName  | The name of the SL Pack - this is usually the name of the folder in your plugins folder that contains the AssetBundle eg: 'YourMod/SiderLoader/AssetBundles' your SL Pack Name would be "YourMod"   |
| AssetBundleName  | The name of the AssetBundle file  |
| PrefabName  | The name of the GameObject Prefab  |
| PositionOffset  | The Local Position of the VFX - Use this to move the VFX the attached player. |

```xml
<?xml version="1.0" encoding="utf-8" ?>
<SL_Effect xsi:type="SL_PlayAssetBundleVFX">
  <Delay>0</Delay>
  <SyncType>OwnerSync</SyncType>
  <OverrideCategory>None</OverrideCategory>
  <SLPackName>MySLPack</SLPackName>
  <AssetBundleName>MyAssetBundleName</AssetBundleName>
  <PrefabName>NameOfPrefabInAssetBundle</PrefabName>
  <PositionOffset>
      <x>0</x>
      <y>0</y>
      <z>0</z>
  </PositionOffset>
</SL_Effect>
```
----------------------------------------------------------------------------------------------------------------------------------------------------------------

### SL_SetWeaponEmission

Sets the EmissionColor value of the currently equipped weapon to the values specified. (Radiant Wolf Sword is a good example of Emission - it has a bright white emission)

| Parameter Name | Description |
| ---| ------------- |
| Color  | x = red value, y = green value, z = blue value - note this is a RGB normalized decimal (https://doc.instantreality.org/tools/color_calculator/)   |
| ColorLerpIntensity  | The intensity (brightness) of the color  |
| ColorLerpTime  | The time to smoothly change from the current Emission color to the new one. |


```xml
<?xml version="1.0" encoding="utf-8" ?>
<SL_Effect xsi:type="SL_SetWeaponEmission">
  <Delay>0</Delay>
  <SyncType>OwnerSync</SyncType>
  <OverrideCategory>None</OverrideCategory>
  <Color>
    <x>1</x>
    <y>0</y>
    <z>0</z>
  </Color>
  <ColorLerpIntensity>10</ColorLerpIntensity>
  <ColorLerpTime>1</ColorLerpTime>
</SL_Effect>
```
