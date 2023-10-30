### Conditions

----------------------------------------------------------------------------------------------------------------------------------------------------------------

### SL_IsTimeBetween
This condition checks if the current in-game time is between the specified start and end hour. The time is in 24 Hour format.

| Parameter Name | Description |
| ---| ------------- |
| StartHour  | The StartHour you want to check if the current in-game time is at least later than   |
| EndHour  | The EndHour you want to check if the current in-game time is BEFORE  |


The example below checks if the current in-game time is between 2am and 10 am.

```xml
<?xml version="1.0" encoding="utf-8" ?>
<SL_Condition xsi:type="SL_IsTimeBetween">
  <Delay>0</Delay>
  <SyncType>OwnerSync</SyncType>
  <OverrideCategory>None</OverrideCategory>
  <StartHour>2</StartHour>
  <EndHour>10</EndHour>
</SL_Condition>
```

--------

### SL_CanSpendAttributeCondition
Checks whether the owner can pay a given attribute cost. For use with `SL_SpendAttributeEffect`.

| Parameter Name | Description |
| ---| ------------- |
| Cost | The cost to spend |
| Attr | Which attribute to spend |
| Relative | Whether the cost should be relative to the attribute's Max or not |
| BurnedMax | If Relative, whether the Max should account for Burn or not|

```xml
<?xml version="1.0" encoding="utf-8" ?>
<SL_Effect xsi:type="SL_SpendAttributeEffect">
  <Delay>0</Delay>
  <SyncType>OwnerSync</SyncType>
  <OverrideCategory>None</OverrideCategory>
  <Cost>20</Cost>
  <Attr>MANA</Attr>
  <Relative>false</Relative>
  <BurnedMax>false</BurnedMax>
</SL_Effect>
```

--------

### SL_CurrentAttributeCondition
Checks the value of an attribute of the target's against a value and an operator.

| Parameter Name | Description |
| ---| ------------- |
| Attr | Which attribute to check |
| CompareType | Comparator to use between Attr and Value |
| Value | Value to check against |
| Owner | Whether to check on the effect owner or the target |
| Relative | Whether the attribute value should be relative to the attribute's Max or not |
| BurnedMax | If Relative, whether the Max should account for Burn or not|

```xml
<?xml version="1.0" encoding="utf-8" ?>
<SL_EffectCondition xsi:type="SL_CurrentAttributeCondition">
  <Invert>false</Invert>
  <Attr>STAMINA</Attr>
  <Value>10</Value>
  <CompareType>Greater</CompareType>
  <Owner>false</Owner>
  <Relative>true</Relative>
  <BurnedMax>true</BurnedMax>
</SL_EffectCondition>
```

--------

### SL_CurrentAttributeRelativeCondition
Checks the value of an attribute of the target's against a the value of another attribute and an operator.

| Parameter Name | Description |
| ---| ------------- |
| AttrLeft | First attribute to check |
| CompareType | Comparator to use between Attr and Value |
| AttrRight | Second attribute to check |
| Owner | Whether to check on the effect owner or the target |
| Relative | Whether the attribute value should be relative to the attribute's Max or not |
| BurnedMax | If Relative, whether the Max should account for Burn or not|

```xml
<?xml version="1.0" encoding="utf-8" ?>
<SL_EffectCondition xsi:type="SL_CurrentAttributeRelativeCondition">
  <Invert>false</Invert>
  <AttrLeft>STAMINA</AttrLeft>
  <AttrRight>MANA</AttrRight>
  <CompareType>Greater</CompareType>
  <Owner>false</Owner>
  <Relative>true</Relative>
  <BurnedMax>true</BurnedMax>
</SL_EffectCondition>
```

--------

### SL_StatCondition
Checks the value of a stat of the target's against a value and an operator.

| Parameter Name | Description |
| ---| ------------- |
| Stat | Which stat to check |
| CompareType | Comparator to use between Stat and Value |
| Value | Value to check against |
| Owner | Whether to check on the effect owner or the target |

```xml
<?xml version="1.0" encoding="utf-8" ?>
<SL_EffectCondition xsi:type="SL_StatCondition">
  <Invert>false</Invert>
  <Stat>MaxMana</Stat>
  <Value>100</Value>
  <CompareType>Greater</CompareType>
  <Owner>false</Owner>
</SL_EffectCondition>
```

--------

### SL_StatRelativeCondition
Checks the value of a stat of the target's against a the value of another stat and an operator.

| Parameter Name | Description |
| ---| ------------- |
| StatLeft | First attribute to check |
| CompareType | Comparator to use between Attr and Value |
| StatRight | Second attribute to check |
| Owner | Whether to check on the effect owner or the target |

```xml
<?xml version="1.0" encoding="utf-8" ?>
<SL_EffectCondition xsi:type="SL_StatRelativeCondition">
  <Invert>false</Invert>
  <StatLeft>MaxMana</StatLeft>
  <StatRight>MaxStamina</StatRight>
  <CompareType>Greater</CompareType>
  <Owner>false</Owner>
</SL_EffectCondition>
```