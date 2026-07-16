using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryInteractionEXAMPLE : MonoBehaviour
{
    [SerializeField] GameObject inventoryScreen;
    [SerializeField] GameObject secondaryInventory;
    [SerializeField] InventoryManager inventoryManager;


    void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            if (secondaryInventory.activeSelf)
            {
                inventoryManager.CloseSecondaryInventory();
                secondaryInventory.SetActive(false);
            }

            inventoryScreen.SetActive(!inventoryScreen.activeSelf);
        }

        if (Mouse.current.leftButton.wasPressedThisFrame && !inventoryScreen.activeSelf)
        {
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.value), out RaycastHit hit)) return;

            if (!hit.collider.gameObject.TryGetComponent(out Inventory chestInventory)) return;

            inventoryManager.OpenSecondaryInventory(chestInventory);
            secondaryInventory.SetActive(true);
            inventoryScreen.SetActive(true);
        }
    }
}
