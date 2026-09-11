using UnityEngine;
using System.Collections;

public class ItemDropperComponentConfigured : MonoBehaviour
{
    public ItemDropper Data { get; private set; }

    // The game object that spawns, which holds the item data
    // In the future it can also hold the mesh associated with the item and whatever else is necessary
    DroppedItem prefab;

    readonly float yOffset = 1f;


    public void Initialize(ItemDropper itemDropper)
    {
        Data = itemDropper;

        string path = "Prefabs/Dropped Item Default Prefab";
        prefab = Resources.Load<DroppedItem>(path);
    }

    /// <summary>Activates the ItemDropper to drop it's loot table, with an optional delay.</summary>
    /// <param name="delay">The delay after which the effect will activate.</param>
    public void Activate(float delay = 0f)
    {
        StartCoroutine(ActivateDelayed(delay));
    }

    IEnumerator ActivateDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (InventoryItem item in Data.GetDrops())
        {
            DroppedItem droppedItem = Instantiate(prefab, transform.position + new Vector3(0f, yOffset, 0f), Quaternion.identity);
            droppedItem.Initialize(item);
        }
    }
}
