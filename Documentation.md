# SideLoader - Extended Effects




### SL_ForceAIState

If the target of this effect is a CharacterAI then they are forced into the state specified by AIState

AIState ID's
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
