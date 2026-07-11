Demo scene for the Item Dropper system.

The ItemDropper script should be added to the object that drops any kind of loot whether on being hit, on death,
or however else. Currently there is a temporary script "ItemDropActivatorEXAMPLE" that activates the ItemDropper to
drop it's loot, however any script can call the Activate() method to trigger it's effect.

The DroppedItemPicker is an implementation of picking up dropped items. Currently just left-clicking picks up
the closest dropped item within it's pickup range. Maybe in the future we might want to pick up the item that the
player is looking at or some other way, but for the time being i made it proximity based.


How to use demo:

R key - activate item dropper to drop it's loot table
Mouse left-click - pick up the closest item within range