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
