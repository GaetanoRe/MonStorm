Demo scene for the Hit Detection system.

Each "hittable object" (animals, player, whatever) can have multiple parts: head, body, arms, etc,
and each part can have a different damage multiplier, for example the head takes 1.5x damage and the legs take
0.5x damage.


How to use demo:

A key - start attack
S key - stop attack

When the scene is started and attack is started move the "Hit Applier (Weapon)" object in the scene view and make contact
with the Hittable Object, and you will see a message in the console depending on which part was hit.
To attack again you need to stop the attack first, then start it again.


How to setup:

Hit Applier object (the weapon that will do the damage)
 - Add HitApplier script and trigger box collider component, and set the Damage variable in the inspector.
 - To use from a script, simply call HitApplier.SetActive(true) when an attack starts, and HitApplier.SetActive(false) when it ends.

Hittable Object (the object that will get hit and take damage)
 - Hittable Object (parent), add a script that inherits from IHitDetectionManager interface and Rigidbody
 - Add child objects for each separate body component that needs to take different amount of damage (head, arms, etc.),
   add Hit Detector script to each and set the Damage Multiplier variable, also add a trigger box collider component
   which will be the hitbox of that particular part.
 - Everything is setup to work but since we use a rigidbody we need a physics collider that is not a trigger and will not
   be hit from damage sources, so add that.