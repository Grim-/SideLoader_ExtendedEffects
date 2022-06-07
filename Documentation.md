# SideLoader - Extended Effects




### SL_ForceAIState

If the target of this effect is a CharacterAI then they are forced into the state specified by AIState - If you wish to force them into a state for a specified time create a [SL_StatusEffect](https://sinai-dev.github.io/OSLDocs/#/API/SL_StatusEffect) that applies this SL_Effect, then set duration to whatever time you wish to force them into the state for.

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
| PositionOffset  | The Local Position of the VFX - Use this to move the VFX attached to the player. |

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

![An Example of VFX Replacement to a custom prefab](https://user-images.githubusercontent.com/3288858/172501372-1707cbee-13a8-4264-ab18-fb63310dcd40.png)

----------------------------------------------------------------------------------------------------------------------------------------------------------------

### SL_SetWeaponEmission

Sets the EmissionColor value of the currently equipped weapon to the values specified. 
Note : Weapon must have Emission enabled at a minimum.
(Radiant Wolf Sword is a good example of Emission - it has a bright white emission)

| Parameter Name | Description |
| ---| ------------- |
| Color  | x = red value, y = green value, z = blue value - note this is a RGB normalized decimal (https://doc.instantreality.org/tools/color_calculator/)   |
| ColorLerpIntensity  | The intensity (brightness) of the color  |
| ColorLerpTime  | The time to smoothly change from the current Emission color to the new one. |

> The Example produces a very bright red glow

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

----------------------------------------------------------------------------------------------------------------------------------------------------------------

### SL_HidePlayer
This effect simply hides the affected character's model for the time specified in HideTime.

| Parameter Name | Description |
| ---| ------------- |
| HideTime  | The time to hide the affected character's model for.   |

```xml
<?xml version="1.0" encoding="utf-8" ?>
<SL_Effect xsi:type="SL_HidePlayer">
  <Delay>0</Delay>
  <SyncType>OwnerSync</SyncType>
  <OverrideCategory>None</OverrideCategory>
  <HideTime>2</HideTime>
</SL_Effect>
```
----------------------------------------------------------------------------------------------------------------------------------------------------------------
### SL_SummonExtension
This effect should be called after SL_Summon or SL_SummonAI with a small delay to ensure the AI has had the chance to spawn in. It allows you to override the material color for the summon and particle systems.
It also allows you to modify the summons weapon damage and grant it a status effect on spawn.

| Parameter Name | Description |
| ---| ------------- |
| NewBaseColor  | x = red value, y = green value, z = blue value |   
| NewParticlesColor  | x = red value, y = green value, z = blue value |   
| NewWeaponDamage  | The Damage Type and Damage Value to change the summons weapon to. |   
| StatusEffectOnSpawn  | The name of the Status effect to grant the summon on Spawning. |  

```xml
<SL_Effect xsi:type="SL_SummonExtension">
  <Delay>3</Delay>
  <SyncType>OwnerSync</SyncType>
  <OverrideCategory>None</OverrideCategory>
  <NewBaseColor>
    <x>10</x>
    <y>0</y>
    <z>0</z>
  </NewBaseColor>
  <NewParticlesColor>
    <x>0.839</x>
    <y>0.415</y>
    <z>0</z>
  </NewParticlesColor>
  <NewWeaponDamage>
    <Damage>30</Damage>
    <Type>Fire</Type>
  </NewWeaponDamage>
  <StatusEffectOnSpawn>Rage</StatusEffectOnSpawn>
</SL_Effect>
```
----------------------------------------------------------------------------------------------------------------------------------------------------------------

### SL_SuspendStatusEffect
Once this SL_Effect is applied to a target any statues specified in StatusEffectIdentifiers are surpressed until the effect ends upon which time the status will resume.


| Parameter Name | Description |
| ---| ------------- |
| StatusEffectIdentifiers  | A list of StatusIdentifiers to suspend [List](https://docs.google.com/spreadsheets/d/1btxPTmgeRqjhqC5dwpPXWd49-_tX_OVLN1Uvwv525K4/edit#gid=1969601658)  |   


```xml
<?xml version="1.0" encoding="utf-8" ?>
<SL_Effect xsi:type="SL_SuspendStatusEffect">
  <Delay>0</Delay>
  <SyncType>OwnerSync</SyncType>
  <OverrideCategory>None</OverrideCategory>
  <StatusEffectIdentifiers>
    <string>Bleeding</string>
  </StatusEffectIdentifiers>
</SL_Effect>
```

----------------------------------------------------------------------------------------------------------------------------------------------------------------

### SL_SuspendStatusTimer
Once this SL_Effect is applied to a target any statues specified in StatusEffectIdentifiers are have their timers surpressed until the effect ends upon which time the status will resume.

| Parameter Name | Description |
| ---| ------------- |
| StatusEffectIdentifiers  | A list of StatusIdentifiers to suspend [List](https://docs.google.com/spreadsheets/d/1btxPTmgeRqjhqC5dwpPXWd49-_tX_OVLN1Uvwv525K4/edit#gid=1969601658)  |   


```xml
<?xml version="1.0" encoding="utf-8" ?>
<SL_Effect xsi:type="SL_SuspendStatusTimer">
  <Delay>0</Delay>
  <SyncType>OwnerSync</SyncType>
  <OverrideCategory>None</OverrideCategory>
  <StatusEffectIdentifiers>
    <string>Bleeding</string>
  </StatusEffectIdentifiers>
</SL_Effect>

```
