Demo scene for the Inventory system.


How to use demo:

E key - open/close inventory screen
Mouse left-click on the brown cubes (containers) - open the container

While in the inventory screen:
- Mouse left-click to pick up and place item
- Mouse right-click to pick up half of the stack
- Shift-mouse left-click (if a container is open) to transfer selected item between containers
- Use arrow buttons to transfer all items between containers


How to setup:
- For the UI setup see how it is setup in the demo scene if you need to replicate it in any other scene.
- Add an InventoryManager and Inventory script to any object in the scene.
(doesn't have to be the same object but keeps things more clear)
- Reference the needed fields in the inspector.
- And add a way to open/close the inventory, there is a script for that in the demo scene called InventoryInteractionEXAMPLE
  for testing purposes

The main inventory screen should work now, but if you want to add secondary inventories (containers/chests) then you should
add the Inventory script to the desired object.
After that you just need to add a way to open the secondary inventories, which the example script also can do.