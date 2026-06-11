using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    /// <summary>The item currently held on the cursor.</summary>
    public InventoryItem HeldItem
    {
        get => heldItem;
        private set
        {
            heldItem = value;

            if (value == null)
            {
                heldItemGO.SetActive(false);
                return;
            }

            amount.text = value.Data.IsStackable ? value.Amount.ToString() : string.Empty;
            icon.sprite = value.Data.Icon;
            heldItemGO.SetActive(true);
        }
    }

    [Header("Inventory")]
    [SerializeField] Inventory mainInventory;
    [SerializeField] InventorySlotMono slotPrefab;
    [SerializeField] Transform mainSlotsParent;
    [SerializeField] Transform secondarySlotsParent;

    [Header("Held Item")]
    [SerializeField] GameObject heldItemGO;
    [SerializeField] Image icon;
    [SerializeField] Text amount;

    InventoryItem heldItem;
    Inventory secondaryInventory;

    readonly List<InventorySlotMono> mainSlotsCollection = new();
    readonly List<InventorySlotMono> secondarySlotsCollection = new();


    public void OpenSecondaryInventory(Inventory secondaryInventory)
    {
        this.secondaryInventory = secondaryInventory;
        InitializeInventory(secondaryInventory, secondarySlotsParent, secondarySlotsCollection);
    }

    public void CloseSecondaryInventory()
    {
        foreach (InventorySlotMono slotMono in secondarySlotsCollection)
        {
            slotMono.OnClicked -= OnSlotClicked;
            slotMono.OnSecondaryClicked -= OnSlotSecondaryClicked;
            Destroy(slotMono.gameObject);
        }
        secondarySlotsCollection.Clear();
        secondaryInventory = null;
    }

    // Called from Unity buttons
    public void TransferAllItems(bool toMainInventory)
    {
        if (secondaryInventory == null) return;

        Inventory inventoryFrom = toMainInventory ? secondaryInventory : mainInventory;
        Inventory inventoryTo = toMainInventory ? mainInventory : secondaryInventory;

        foreach (InventorySlot slot in inventoryFrom.ItemSlotArray)
        {
            if (slot.IsEmpty) continue;

            TransferItem(slot, inventoryTo);
        }
    }

    void Start()
    {
        InitializeInventory(mainInventory, mainSlotsParent, mainSlotsCollection);
    }

    void OnDestroy()
    {
        mainInventory.OnSizeChanged -= OnInventoryResized;

        foreach (InventorySlotMono slotMono in mainSlotsCollection)
        {
            slotMono.OnClicked -= OnSlotClicked;
            slotMono.OnSecondaryClicked -= OnSlotSecondaryClicked;
        }
    }

    void Update()
    {
        if (HeldItem != null)
        {
            heldItemGO.transform.position = Mouse.current.position.value;
        }

        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            mainInventory.ChangeInventorySize(5);
        }
        if (Keyboard.current.yKey.wasPressedThisFrame)
        {
            mainInventory.ChangeInventorySize(25);
        }
    }

    void InitializeInventory(Inventory inventory, Transform parent, List<InventorySlotMono> slotCollection)
    {
        inventory.OnSizeChanged += OnInventoryResized;

        foreach (InventorySlot slot in inventory.ItemSlotArray)
        {
            InventorySlotMono slotMono = Instantiate(slotPrefab, parent);
            slotMono.Initialize(slot);
            slotMono.OnClicked += OnSlotClicked;
            slotMono.OnSecondaryClicked += OnSlotSecondaryClicked;
            slotCollection.Add(slotMono);
        }
    }

    void TransferItem(InventorySlot slotFrom, Inventory inventoryTo)
    {
        slotFrom.Replace(inventoryTo.Add(slotFrom.Item));
    }

    void OnSlotClicked(InventorySlotMono slotMono)
    {
        if (Keyboard.current.leftShiftKey.isPressed || Keyboard.current.leftCtrlKey.isPressed)
        {
            if (slotMono.Slot.IsEmpty) return;

            if (secondaryInventory == null) return;

            Inventory inventoryTo = mainSlotsCollection.Contains(slotMono) ? secondaryInventory : mainInventory;
            TransferItem(slotMono.Slot, inventoryTo);
            return;
        }

        if (HeldItem != null)
        {
            HeldItem = slotMono.Slot.Interact(HeldItem);
        }
        else if (!slotMono.Slot.IsEmpty)
        {
            HeldItem = slotMono.Slot.Remove();
        }
    }

    void OnSlotSecondaryClicked(InventorySlotMono slotMono)
    {
        if (HeldItem != null)
        {
            HeldItem = slotMono.Slot.Interact(HeldItem);
        }
        else if (!slotMono.Slot.IsEmpty)
        {
            HeldItem = slotMono.Slot.RemoveHalf();
        }
    }

    void OnInventoryResized()
    {
        foreach (InventorySlotMono slotMono in mainSlotsCollection)
        {
            slotMono.OnClicked -= OnSlotClicked;
            slotMono.OnSecondaryClicked -= OnSlotSecondaryClicked;
            Destroy(slotMono.gameObject);
        }
        mainSlotsCollection.Clear();
        mainInventory.OnSizeChanged -= OnInventoryResized;
        InitializeInventory(mainInventory, mainSlotsParent, mainSlotsCollection);
    }
}
