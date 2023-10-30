# SideLoader - Extended Effects




### SL_ForceAIState

If the target of this effect is a CharacterAI then they are forced into the state specified by AIState 
> If you wish to force them into a state for a specified time create a [SL_StatusEffect](https://sinai-dev.github.io/OSLDocs/#/API/SL_StatusEffect) that applies this SL_Effect, then set duration to whatever time you wish to force them into the state for.

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
| PositionOffset  | The Local Position(If Attached) of the VFX - Use this to move the VFX attached to the player. |
| RotationOffset  | The Rotation of the VFX |
| ParentToAffected  | Should this GameObject be attached to the AffectedCharacter? |
| LifeTime | Set LifeTime to 0 if you wish the effect to handle destroying the VFX, otherwise set a time in seconds after which the VFX will be automatically destroyed. |
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
  <RotationOffset>
      <x>0</x>
      <y>0</y>
      <z>0</z>
  </RotationOffset>
  <ParentToAffected>true</ParentToAffected>
  <LifeTime>0</LifeTime>
  <RotateToPlayerDirection>false</RotateToPlayerDirection>
</SL_Effect>
```

![An Example of VFX Replacement to a custom prefab](https://user-images.githubusercontent.com/3288858/172501372-1707cbee-13a8-4264-ab18-fb63310dcd40.png)

----------------------------------------------------------------------------------------------------------------------------------------------------------------

### SL_PlayAssetBundleVFX_Bones
Spawns a GameObject from the specified SLPack AssetBundle and attach it to the specified player bone.

| Parameter Name | Description |
| ---| ------------- |
| SLPackName  | The name of the SL Pack - this is usually the name of the folder in your plugins folder that contains the AssetBundle eg: 'YourMod/SiderLoader/AssetBundles' your SL Pack Name would be "YourMod"   |
| AssetBundleName  | The name of the AssetBundle file  |
| PrefabName  | The name of the GameObject Prefab  |
| PositionOffset  | The Local Position(If Attached) of the VFX - Use this to move the VFX attached to the player. |
| RotationOffset  | The Rotation of the VFX |
| ParentToAffected  | Should this GameObject be attached to the AffectedCharacter? |
| RotateToPlayerDirection  | Should this GameObject be rotated so it's forward axis is facing the same direction as the player? Only Applies when ParentToAffected is false |
| LifeTime | Set LifeTime to 0 if you wish the effect to handle destroying the VFX, otherwise set a time in seconds after which the VFX will be automatically destroyed. |
|BoneID| the ID of the bone you want to parent to, you can obtain this from [here](https://github.com/Grim-/SideLoader_ExtendedEffects/blob/main/Resources/HumanBonesReference.md).| 
```xml
<?xml version="1.0" encoding="utf-8" ?>
<SL_Effect xsi:type="SL_PlayAssetBundleVFX_Bones">
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
  <RotationOffset>
      <x>0</x>
      <y>0</y>
      <z>0</z>
  </RotationOffset>
  <ParentToAffected>true</ParentToAffected>
  <LifeTime>0</LifeTime>
  <BoneID>10</BoneID>
</SL_Effect>
```

Example of a Tuansuar model (taken from the assets and chopped up in blender then reimported via assetbundles) on the players head with a flamethrower vfx.

Potentially useful for partial transformation skill trees.

https://imgur.com/nwY9OBY
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
This effect should be called after [SL_SummonAI](https://sinai-dev.github.io/OSLDocs/#/API/SL_Effect?id=sl_summonai-sl_summon) with a small delay to ensure the AI has had the chance to spawn in. It allows you to override the material color for the summon and particle systems.
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
----------------------------------------------------------------------------------------------------------------------------------------------------------------

### SL_CustomImbueVFX
This is used to apply a particle system to a Weapon Mesh, the Particle System must have it's shape mode set to "Mesh". 

| Parameter Name | Description |
| ---| ------------- |
| SLPackName  | The name of the SL Pack - this is usually the name of the folder in your plugins folder that contains the AssetBundle eg: 'YourMod/SiderLoader/AssetBundles' your SL Pack Name would be "YourMod"   |
| AssetBundleName  | The name of the AssetBundle file  |
| PrefabName  | The name of the GameObject Prefab  |
| PositionOffset  | The Local Position(If Attached) of the VFX - Use this to move the VFX attached to the player. |
| RotationOffset  | The Rotation of the VFX |
| IsMainHand | Apply to mainhand or off hand? |
```xml
<SL_Effect xsi:type="SL_CustomImbueVFX">
  <Delay>0</Delay>
  <SyncType>OwnerSync</SyncType>
  <OverrideCategory>None</OverrideCategory>
  <SLPackName>YourSLPackOrModFloder</SLPackName>
  <AssetBundleName>NameOfAssetBundle</AssetBundleName>
  <PrefabName>NameOfPrefabInAssetBundle</PrefabName>
  <PositionOffset>
    <x>0</x>
    <y>0</y>
    <z>0</z>
  </PositionOffset>
  <RotationOffset>
    <x>0</x>
    <y>0</y>
    <z>0</z>
  </RotationOffset>
  <IsMainHand>true</IsMainHand>
</SL_Effect>
```


Example of Custom Imbue VFX
![image](https://user-images.githubusercontent.com/3288858/174453159-74b48c3c-513b-458c-8284-ad950329dece.png)


There is an [example bundle](https://github.com/Grim-/SideLoader_ExtendedEffects/tree/main/Custom%20Imbue%20AssetBundle%20Example) you can drop this into your mod folder and use the following XML to Spawn either 'Life Warden' or 'Black Imbue', generally imbues are just a status effect that also applies a pre-defined particle system. 

You can also view this AssetBundle in Unity in order to see how it is set up.


### Lifewarden
```xml
<SL_Effect xsi:type="SL_CustomImbueVFX">
  <Delay>0</Delay>
  <SyncType>OwnerSync</SyncType>
  <OverrideCategory>None</OverrideCategory>
  <SLPackName>YourSLPackOrModFloder</SLPackName>
  <AssetBundleName>emovfx</AssetBundleName>
  <PrefabName>LifeWarden</PrefabName>
  <PositionOffset>
    <x>0</x>
    <y>0</y>
    <z>0</z>
  </PositionOffset>
  <RotationOffset>
    <x>0</x>
    <y>0</y>
    <z>0</z>
  </RotationOffset>
  <IsMainHand>true</IsMainHand>
</SL_Effect>
```
### Black Imbue
```xml
<SL_Effect xsi:type="SL_CustomImbueVFX">
  <Delay>0</Delay>
  <SyncType>OwnerSync</SyncType>
  <OverrideCategory>None</OverrideCategory>
  <SLPackName>YourSLPackOrModFolder</SLPackName>
  <AssetBundleName>emovfx</AssetBundleName>
  <PrefabName>Black Imbue</PrefabName>
  <PositionOffset>
    <x>0</x>
    <y>0</y>
    <z>0</z>
  </PositionOffset>
  <RotationOffset>
    <x>0</x>
    <y>0</y>
    <z>0</z>
  </RotationOffset>
  <IsMainHand>true</IsMainHand>
</SL_Effect>
```

Here's the full thing - firstly define the new status - this example assumes the 'emovfx' bundle is in a folder called 'vfx' you can change this with the SLPackName variable.

```xml
<?xml version="1.0"?>
<SL_StatusEffect xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <TargetStatusIdentifier>Rage</TargetStatusIdentifier>
  <NewStatusID>-26233</NewStatusID>
  <StatusIdentifier>CustomImbueBlackThunder</StatusIdentifier>
  <Name>CustomImbue Test</Name>
  <Description>CustomImbueBlackThunder</Description>
  <Lifespan>60</Lifespan>
  <RefreshRate>-1</RefreshRate>
  <Purgeable>false</Purgeable>
  <Priority>1</Priority>
  <IgnoreBarrier>false</IgnoreBarrier>
  <BuildupRecoverySpeed>3</BuildupRecoverySpeed>
  <IgnoreBuildupIfApplied>false</IgnoreBuildupIfApplied>
  <AmplifiedStatusIdentifier />
  <RemoveRequiredStatus>false</RemoveRequiredStatus>
  <NormalizeDamageDisplay>false</NormalizeDamageDisplay>
  <DisplayedInHUD>true</DisplayedInHUD>
  <IsHidden>false</IsHidden>
  <IsMalusEffect>false</IsMalusEffect>
  <DelayedDestroyTime>0</DelayedDestroyTime>
  <ActionOnHit>None</ActionOnHit>
  <FamilyMode>Bind</FamilyMode>
  <BindFamily>
    <UID>uZIZmVwT0kKtz9jfHkIszA</UID>
    <Name>CustomImbueBlackThunder_FAMILY</Name>
    <StackBehaviour>IndependantUnique</StackBehaviour>
    <MaxStackCount>1</MaxStackCount>
    <LengthType>Short</LengthType>
  </BindFamily>
  <PlayFXOnActivation>true</PlayFXOnActivation>
  <FXOffset>
    <x>0</x>
    <y>0</y>
    <z>0</z>
  </FXOffset>
  <VFXInstantiationType>Normal</VFXInstantiationType>
  <VFXPrefab xsi:nil="true" />
  <SpecialSFX>NONE</SpecialSFX>
  <PlaySpecialFXOnStop>false</PlaySpecialFXOnStop>
  <EffectBehaviour>Destroy</EffectBehaviour>
  <Effects>
    <SL_EffectTransform>
      <TransformName>Activation</TransformName>
      <Position xsi:nil="true" />
      <Rotation xsi:nil="true" />
      <Scale xsi:nil="true" />
      <Effects>
<SL_Effect xsi:type="SL_CustomImbueVFX">
  <Delay>0</Delay>
  <SyncType>OwnerSync</SyncType>
  <OverrideCategory>None</OverrideCategory>
  <SLPackName>vfx</SLPackName>
  <AssetBundleName>emovfx</AssetBundleName>
  <PrefabName>Black Imbue</PrefabName>
  <PositionOffset>
    <x>0</x>
    <y>0</y>
    <z>0</z>
  </PositionOffset>
  <RotationOffset>
    <x>0</x>
    <y>0</y>
    <z>0</z>
  </RotationOffset>
  <IsMainHand>true</IsMainHand>
</SL_Effect>
        <SL_Effect xsi:type="SL_WeaponDamage">
          <Delay>0</Delay>
          <SyncType>OwnerSync</SyncType>
          <OverrideCategory>None</OverrideCategory>
          <Damage>
            <SL_Damage>
              <Damage>20</Damage>
              <Type>Ethereal</Type>
            </SL_Damage>
          </Damage>
          <Damages_AI />
          <Knockback>0</Knockback>
          <HitInventory>false</HitInventory>
          <IgnoreHalfResistances>false</IgnoreHalfResistances>
          <OverrideType>Physical</OverrideType>
          <ForceOnlyLeftHand>false</ForceOnlyLeftHand>
          <Damage_Multiplier>0</Damage_Multiplier>
          <Damage_Multiplier_Kback>0</Damage_Multiplier_Kback>
          <Damage_Multiplier_Kdown>0</Damage_Multiplier_Kdown>
          <Impact_Multiplier>0</Impact_Multiplier>
          <Impact_Multiplier_Kback>0</Impact_Multiplier_Kback>
          <Impact_Multiplier_Kdown>0</Impact_Multiplier_Kdown>
        </SL_Effect>
      </Effects>
      <EffectConditions />
      <ChildEffects />
    </SL_EffectTransform>
  </Effects>
</SL_StatusEffect>
```


And an example potion that applies this status to the player granting the imbue 

```xml
<?xml version="1.0"?>
<SL_Item xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Target_ItemID>4300130</Target_ItemID>
  <New_ItemID>-26700</New_ItemID>
  <Name>Imbue Black Thunder Potion</Name>
  <Description>Test Imbue Black Thunder Effect Potion</Description>
  <LegacyItemID>-1</LegacyItemID>
  <IsPickable>true</IsPickable>
  <IsUsable>true</IsUsable>
  <QtyRemovedOnUse>0</QtyRemovedOnUse>
  <GroupItemInDisplay>false</GroupItemInDisplay>
  <HasPhysicsWhenWorld>false</HasPhysicsWhenWorld>
  <RepairedInRest>true</RepairedInRest>
  <BehaviorOnNoDurability>NotSet</BehaviorOnNoDurability>
  <CastType>Potion</CastType>
  <CastModifier>Immobilized</CastModifier>
  <CastLocomotionEnabled>false</CastLocomotionEnabled>
  <MobileCastMovementMult>-1</MobileCastMovementMult>
  <CastSheatheRequired>1</CastSheatheRequired>
  <OverrideSellModifier>-1</OverrideSellModifier>
  <Tags>
    <string>Drink</string>
    <string>Consummable</string>
    <string>Item</string>
  </Tags>
  <StatsHolder>
    <BaseValue>15</BaseValue>
    <RawWeight>0.5</RawWeight>
    <MaxDurability>-1</MaxDurability>
  </StatsHolder>
  <ExtensionsEditBehaviour>NONE</ExtensionsEditBehaviour>
  <ItemExtensions>
    <SL_ItemExtension xsi:type="SL_MultipleUsage">
      <Savable>true</Savable>
      <AppliedOnPrice>true</AppliedOnPrice>
      <AppliedOnWeight>true</AppliedOnWeight>
      <AutoStack>true</AutoStack>
      <MaxStackAmount>999</MaxStackAmount>
    </SL_ItemExtension>
  </ItemExtensions>
  <EffectBehaviour>Destroy</EffectBehaviour>
  <EffectTransforms>
    <SL_EffectTransform>
      <TransformName>Effects</TransformName>
      <Position xsi:nil="true" />
      <Rotation xsi:nil="true" />
      <Scale xsi:nil="true" />
      <Effects>
        <SL_Effect xsi:type="SL_AddStatusEffect">
          <Delay>0</Delay>
          <SyncType>OwnerSync</SyncType>
          <OverrideCategory>None</OverrideCategory>
          <StatusEffect>CustomImbueBlackThunder</StatusEffect>
          <ChanceToContract>100</ChanceToContract>
          <AffectController>false</AffectController>
          <AdditionalLevel>0</AdditionalLevel>
          <NoDealer>false</NoDealer>
        </SL_Effect>
      </Effects>
      <EffectConditions />
      <ChildEffects />
    </SL_EffectTransform>
  </EffectTransforms>
  <ItemVisuals>
    <ResourcesPrefabPath>Assets/_Prefabs/_ItemsAssets/4000000_Consumables/4300000_Potions/4300902_GenericGrayPotion_v</ResourcesPrefabPath>
    <Prefab_SLPack />
    <Prefab_AssetBundle />
    <Prefab_Name />
    <Position>
      <x>0</x>
      <y>0</y>
      <z>0</z>
    </Position>
    <Rotation>
      <x>-0</x>
      <y>0</y>
      <z>0</z>
    </Rotation>
    <Scale>
      <x>1.1</x>
      <y>1.1</y>
      <z>1.1</z>
    </Scale>
    <PositionOffset xsi:nil="true" />
    <RotationOffset xsi:nil="true" />
  </ItemVisuals>
</SL_Item>
```
----------------------------------------------------------------------------------------------------------------------------------------------------------------

### SL_ApplyEnchantment
This SL_Effect applies an Enchantment to the specified EquipmentSlot.


| Parameter Name | Description |
| ---| ------------- |
| EquipmentSlot  | The EquipmentSlotID |   
| EnchantmentID  | The Enchantment ID to Apply |   
| ApplyPermanently  | Apply this Enchantment permanently? Otherwise its removed when the weapon is unequipped. |   

```xml
<SL_Effect xsi:type="SL_ApplyEnchantment">
  <Delay>0</Delay>
  <SyncType>OwnerSync</SyncType>
  <OverrideCategory>None</OverrideCategory>
  <EquipmentSlot>5</EquipmentSlot>
  <EnchantmentID>2</EnchantmentID>
  <ApplyPermanently>false</ApplyPermanently>
</SL_Effect>
```
----------------------------------------------------------------------------------------------------------------------------------------------------------------

### SL_OnHitEffect
This SL_Effect applies it's sub-effects each time it detects a matching hit. Hits by the effect owner apply `Hit` effects on the target and `Normal` effects on the owner.
Hits on the owner apply `Block` effects on the attacker, and `Referenced` effects on the owner. Only up to `ActivationLimit` applications are allowed on a character in the same frame.

| Parameter Name | Description |
| ---| ------------- | 
| ChildEffects  | A list of EffectTransforms containing the effects that should be applied. |   
| ActivationLimit  | How many times should effects be allowed to apply to each character. |   
| RequiredSourceType  | Whether hits need to come from weapons or not. |   
| DamageTypes  | Which damage types should trigger effects. |   
| RequireAllTypes  | If true, each damage type is required rather than just one. |   
| MinDamage  | How much damage must be done to trigger the effects. |   
| OnlyCountRequiredTypes  | If true, only damage from `Damage Types` counts towards `Min Damage` |   
| UseHighestType  | If true, only the highest type of damage counts for any conditions. |   
| IgnoreDamageReduction  | If true, calculations are done based on damage before reductions from target's defenses. |   

```xml
<SL_Effect xsi:type="SL_OnHitEffect">
    <Delay>0</Delay>
    <SyncType>OwnerSync</SyncType>
    <OverrideCategory>None</OverrideCategory>
    <EffectBehavior>Destroy</EffectBehavior>
    <ChildEffects>
    </ChildEffects>
    <ActivationLimit>3</ActivationLimit>
    <RequiredSourceType>WEAPON</RequiredSourceType>
    <RequireAllTypes>false</RequireAllTypes>
    <MinDamage>0</MinDamage>
    <OnlyCountRequiredTypes>false</OnlyCountRequiredTypes>
    <UseHighestType>false</UseHighestType>
    <IgnoreDamageReduction>false</IgnoreDamageReduction>
</SL_Effect>
```
----------------------------------------------------------------------------------------------------------------------------------------------------------------

### SL_OnEquipEffect
This SL_Effect applies it's sub-effects each time it detects a change in equipped items. Equipping an item applies `Activation` and `Normal` effects. Unequipping an item applies `Normal` and `Referenced` effects. These take places *after* their action, so items will already be in/out of their equip slots when effects are resolved.

| Parameter Name | Description |
| ---| ------------- | 
| ChildEffects  | A list of EffectTransforms containing the effects that should be applied. |
| ActivationLimit  | How many times should effects be allowed to apply to each character. |
| AllowedSlots  | Which equipment slots should be tracked. |

```xml
<SL_Effect xsi:type="SL_OnEquipEffect">
    <Delay>0</Delay>
    <SyncType>OwnerSync</SyncType>
    <OverrideCategory>None</OverrideCategory>
    <EffectBehavior>Destroy</EffectBehavior>
    <ChildEffects>
    </ChildEffects>
    <ActivationLimit>2</ActivationLimit>
    <AllowedSlots />
</SL_Effect>
```
----------------------------------------------------------------------------------------------------------------------------------------------------------------

### SL_EffectLifecycleEffect
This SL_Effect applies it's sub-effects each time it is activated. The first activation applies `Activation` and `Normal` effects. Subsequent activations apply just `Normal` effects.
Finally, the effect stopping applies `Referenced` effects.

| Parameter Name | Description |
| ---| ------------- | 
| ChildEffects  | A list of EffectTransforms containing the effects that should be applied. |
| ActivationLimit  | How many times should effects be allowed to apply to each character. Not used, since the effect can't activate more than once. |

```xml
<SL_Effect xsi:type="SL_EffectLifecycleEffect">
    <Delay>0</Delay>
    <SyncType>OwnerSync</SyncType>
    <OverrideCategory>None</OverrideCategory>
    <EffectBehavior>Destroy</EffectBehavior>
    <ChildEffects>
    </ChildEffects>
    <ActivationLimit>0</ActivationLimit>
</SL_Effect>
```

Example uses:

Apply a status effect at the end of another status effect
```xml
<SL_Effect xsi:type="SL_EffectLifecycleEffect">
    <Delay>0</Delay>
    <SyncType>OwnerSync</SyncType>
    <OverrideCategory>None</OverrideCategory>
    <EffectBehavior>Destroy</EffectBehavior>
    <ChildEffects>
        <SL_EffectTransform>
            <TransformName>Referenced (Run at the end of the effect)</TransformName>
            <Position xsi:nil="true" />
            <Rotation xsi:nil="true" />
            <Scale xsi:nil="true" />
            <Effects>
                <SL_Effect xsi:type="SL_AddStatusEffect">
                    <Delay>0</Delay>
                    <SyncType>OwnerSync</SyncType>
                    <OverrideCategory>None</OverrideCategory>
                    <StatusEffect>Discipline</StatusEffect>
                    <ChanceToContract>100</ChanceToContract>
                    <AffectController>false</AffectController>
                    <AdditionalLevel>0</AdditionalLevel>
                    <NoDealer>false</NoDealer>
                </SL_Effect>
            </Effects>
        </SL_EffectTransform>
    </ChildEffects>
    <ActivationLimit>0</ActivationLimit>
</SL_Effect>
```

To evaluate a condition after a delay, rather than immediately
```xml
<SL_Effect xsi:type="SL_EffectLifecycleEffect">
    <Delay>3</Delay>
    <SyncType>OwnerSync</SyncType>
    <OverrideCategory>None</OverrideCategory>
    <EffectBehavior>Destroy</EffectBehavior>
    <ChildEffects>
        <SL_EffectTransform>
            <TransformName>Normal (Run on each activation)</TransformName>
            <Position xsi:nil="true" />
            <Rotation xsi:nil="true" />
            <Scale xsi:nil="true" />
            <Effects>
              ...
            </Effects>
            <EffectConditions>
              ...
            </EffectConditions>
        </SL_EffectTransform>
    </ChildEffects>
    <ActivationLimit>0</ActivationLimit>
</SL_Effect>
```

Other possibilities include running effects only the first time a skill is used, nested conditions, and other structural shenanigans.
----------------------------------------------------------------------------------------------------------------------------------------------------------------

### SL_AffectCooldown
Affect active cooldowns for the character's skills. By defauly, subtracts `Amount` seconds from the remaining cooldown, but can be changed to subtract a `Amount` percent of the effective cooldown instead. May also be filtered to apply on to a fixed list of skills.

| Parameter Name | Description |
| ---| ------------- | 
| Amount | How much to reduce the cooldown by, in seconds. |
| IsModifier | Whether `Amount` should be treated as a flat value or a percentage. |
| OnOwner | Whether cooldowns should be reduced on the affected target or the owner. |
| AllowedSkills | Which skills should have their cooldown affected' |

```xml
<SL_Effect xsi:type="SL_AffectCooldown">
    <Delay>0</Delay>
    <SyncType>OwnerSync</SyncType>
    <OverrideCategory>None</OverrideCategory>
    <Amount>100</Amount>
    <IsModifier>false</IsModifier>
    <OnOwner>false</OnOwner>
    <AllowedSkills>
      <int>8100320</int>
    </AllowedSkills>
</SL_Effect>
```

----------------------------------------------------------------------------------------------------------------------------------------------------------------

### SL_ChangeMaterialTimed
This effect changes all the materials on a Character to another material, then changes them back after the set time. 

| Parameter Name | Description |
| ---| ------------- |
| SLPackName  | The name of the SL Pack - this is usually the name of the folder in your plugins folder that contains the AssetBundle eg: 'YourMod/SiderLoader/AssetBundles' your SL Pack Name would be "YourMod"   |
| AssetBundleName  | The name of the AssetBundle file  |
| PrefabName  | The name of the GameObject Prefab  |
| ChangeTime  | How long to change the materials for, set it to 0 for indefinite.  |

```xml
<?xml version="1.0" encoding="utf-8" ?>
<SL_Effect xsi:type="SL_ChangeMaterialTimed">
  <Delay>0</Delay>
  <SyncType>OwnerSync</SyncType>
  <OverrideCategory>None</OverrideCategory>
  <ChangeTime>2</ChangeTime>
   <SLPackName>YourSLPackOrModFloder</SLPackName>
  <AssetBundleName>emovfx</AssetBundleName>
  <PrefabName>LifeWarden</PrefabName>
</SL_Effect>
```

![image](https://user-images.githubusercontent.com/3288858/179397512-f8b2d4d4-ff6a-48b5-9def-601883b8a5ae.png)

----------------------------------------------------------------------------------------------------------------------------------------------------------------

### SL_ScalingWeaponDamage
Deals damage to the AffectedCharacter based on the currently equipped weapon.

| Parameter Name | Description |
| ---| ------------- | 
| Amount | How much to reduce the cooldown by, in seconds. |
| HitInventory | Whether the damage should apply to the target's inventory items. |
| KnockbackAmount | How much knockback you to deal |

```xml
<?xml version="1.0" encoding="utf-8" ?>
<SL_Effect xsi:type="SL_ScalingWeaponDamage">
  <Delay>0</Delay>
  <SyncType>OwnerSync</SyncType>
  <OverrideCategory>None</OverrideCategory>
  <KnockbackAmount>0</KnockbackAmount>
  <HitInventory>false</HitInventory>
</SL_Effect>
```

--------------

### SL_SpendAttributeEffect

Spends Health, Mana, Stamina, or one of their Burns as though a cost for a skill. Only affects the effect owner. Mana and Stamina costs respect their cost reductions. Optionally, costs can be a percentage of the base or effective maximum for the attribute.

| Parameter Name | Description |
| --- | ------------- | 
| Value | The cost to spend |
| Attr | Which attribute to spend |
| Relative | Whether the cost should be relative to the attribute's Max or not |
| BurnedMax | If Relative, whether the Max should account for Burn or not|

```xml
<?xml version="1.0" encoding="utf-8" ?>
<SL_Effect xsi:type="SL_SpendAttributeEffect">
  <Delay>0</Delay>
  <SyncType>OwnerSync</SyncType>
  <OverrideCategory>None</OverrideCategory>
  <Value>0.5</Value>
  <Attr>STAMINA</Attr>
  <Relative>false</Relative>
  <BurnedMax>false</BurnedMax>
</SL_Effect>
```

--------------

### SL_StatScalingEffect

Applies subeffects scaled to the value of a stat of the target. Like leveled passives, only works for the following effects:

    AffectStat
    AffectNeed and similar
    AffectStatusEffectBuildUpResistance
    AffectCooldown
    SpendAttributeEffect


| Parameter Name | Description |
| --- | ------------- | 
| Stat | The Stat Tag to base effect potency on |
| BaselineValue | The Stat value at which effects should be applied at 100% potency |
| Round | Whether scaling should only increase at integer multiples of BaselineValue |
| Owner | Whether scaling should be based on the effect owner's stats instead |

```xml
<?xml version="1.0" encoding="utf-8" ?>
<SL_Effect xsi:type="SL_StatScalingEffect">
  <Delay>0</Delay>
  <SyncType>OwnerSync</SyncType>
  <OverrideCategory>None</OverrideCategory>
  <EffectBehavior>Destroy</EffectBehavior>
  <ChildEffects>
    <SL_EffectTransform>
      <TransformName>Normal</TransformName>
      <Position xsi:nil="true" />
      <Rotation xsi:nil="true" />
      <Scale xsi:nil="true" />
      <Effects>
        <SL_Effect xsi:type="SL_AffectStat">
          <Delay>0</Delay>
          <SyncType>OwnerSync</SyncType>
          <OverrideCategory>None</OverrideCategory>
          <Stat_Tag>StaminaRegen</Stat_Tag>
          <AffectQuantity>10</AffectQuantity>
          <IsModifier>true</IsModifier>
          <Duration>-1</Duration>
        </SL_Effect>
      </Effects>
    </SL_EffectTransform>
  </ChildEffects>
  <ActivationLimit>0</ActivationLimit>
  <Stat>MaxMana</Stat>
  <BaselineValue>20</BaselineValue>
  <Round>false</Round>
  <Owner>false</Owner>
</SL_Effect>
```

--------------

### SL_CurrentAttributeScalingEffect

Similar to `SL_StatScalingEffect` above, but scaling off of an attribute instead. 

| Parameter Name | Description |
| --- | ------------- | 
| BaselineValue | The attribute value at which effects should be applied at 100% potency |
| Attr | Which attribute to base scaling off of |
| Relative | Whether the scaling should be relative to the attribute's Max or not |
| BurnedMax | If Relative, whether the Max should account for Burn or not|
| Owner | Whether scaling should be based on the effect owner's attribute instead |
| Round | Whether scaling should only increase at integer multiples of BaselineValue |

```xml
<?xml version="1.0" encoding="utf-8" ?>
<SL_Effect xsi:type="SL_CurrentAttributeScalingEffect">
  <Delay>0</Delay>
  <SyncType>OwnerSync</SyncType>
  <OverrideCategory>None</OverrideCategory>
  <CastPosition>Local</CastPosition>
  <LocalPositionAdd>
    <x>0</x>
    <y>0</y>
    <z>0</z>
  </LocalPositionAdd>
  <NoAim>false</NoAim>
  <TargetType>Enemies</TargetType>
  <EffectBehavior>Destroy</EffectBehavior>
  <ChildEffects>
    <SL_EffectTransform>
      <TransformName>Normal Value</TransformName>
      <Position xsi:nil="true" />
      <Rotation xsi:nil="true" />
      <Scale xsi:nil="true" />
      <Effects>
        <SL_Effect xsi:type="SL_SpendAttributeEffect">
          <Delay>0</Delay>
          <SyncType>OwnerSync</SyncType>
          <OverrideCategory>None</OverrideCategory>
          <Value>20</Value>
          <Attr>MANA</Attr>
          <Relative>false</Relative>
          <BurnedMax>false</BurnedMax>
        </SL_Effect>
      </Effects>
    </SL_EffectTransform>
  </ChildEffects>
  <ActivationLimit>0</ActivationLimit>
  <BaselineValue>20</BaselineValue>
  <Round>false</Round>
  <Attr>MANA</Attr>
  <Owner>true</Owner>
  <Relative>false</Relative>
  <BurnedMax>false</BurnedMax>
</SL_Effect>
```
