# SideLoader - Extended Effects v1.2.0

A small addition to SideLoader allowing mod developers more flexibility when creating mods using SideLoader.

You would include this mod as dependcy of your own mod, as is often done with SideLoader already. 

Dependency string can be found on [ThunderStore page](https://outward.thunderstore.io/package/SLExtendedEffects/SideLoader_ExtendedEffects/)

[Documentation](https://github.com/Grim-/SideLoader_ExtendedEffects/blob/main/Documentation.md)


## Change Log

1.2.0 - 
Added a simple API for adding custom context menu options for Items (Docs to follow) 
- Added [SL_IsTimeBetweenCondition](https://github.com/Grim-/SideLoader_ExtendedEffects/blob/main/Conditions.md#sl_istimebetween) a Effect condition that checks if the current time is between StartHour and EndHour
- Bug fixes for CustomImbue not correctly applying the MeshRenderer if the MeshRenderer shape module option is selected, before this only the Mesh shape module option would work correctly.

1.1.5 - Added [SL_OnHitEffect](https://github.com/Grim-/SideLoader_ExtendedEffects/blob/main/XML/SL_OnHitEffect.xml), [SL_OnEquipEffect](https://github.com/Grim-/SideLoader_ExtendedEffects/blob/main/XML/SL_OnEquipEffect.xml), [SL_EffectLifecycleEffect](https://github.com/Grim-/SideLoader_ExtendedEffects/blob/main/XML/SL_EffectLifecycleEffect.xml), and [SL_AffectCooldown](https://github.com/Grim-/SideLoader_ExtendedEffects/blob/main/XML/SL_AffectCooldown.xml). Also added generalized event publishers for C# mods to hook in to, as well as implementations for hit and equip events.

1.1.4 - Added [SL_ApplyEnchantment](https://github.com/Grim-/SideLoader_ExtendedEffects/blob/main/XML/SL_ApplyEnchantment.xml) Thanks to [Dan](https://github.com/dansze)

1.1.3 - Added [SL_PlayAssetBundleVFX_Bones](https://github.com/Grim-/SideLoader_ExtendedEffects/blob/main/Documentation.md#sl_playassetbundlevfx_bones)

1.1.2 - Added [SL_CustomImbueVFX](https://github.com/Grim-/SideLoader_ExtendedEffects/blob/main/Documentation.md#sl_customimbuevfx)

1.1.1 - Changed SL_PlayAssetBundleVFX adding LifeTime parameter.

1.1.0 - Changed SL_PlayAssetBundleVFX adding RotationOffset and ParentToAffected parameters.

1.0.0 > 1.0.3 - ReadMe changes only
