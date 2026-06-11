using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class InventorySlotMono : MonoBehaviour, IPointerClickHandler
{
    /// <summary>Invoked when the left mouse button clicks this object.</summary>
    public event Action<InventorySlotMono> OnClicked;

    /// <summary>Invoked when the right mouse button clicks this object.</summary>
    public event Action<InventorySlotMono> OnSecondaryClicked;

    /// <summary>The associated InventorySlot with this object.</summary>
    public InventorySlot Slot { get; private set; }

    [SerializeField] Image icon;
    [SerializeField] Text amount;


    // Called from Unity
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Slot == null)
        {
            Debug.LogWarning("InventorySlotMono is not initialized!");
            return;
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnClicked?.Invoke(this);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnSecondaryClicked?.Invoke(this);
        }
    }

    /// <summary>Initializes the object with the provided InventorySlot.</summary>
    public void Initialize(InventorySlot slot)
    {
        Slot = slot;
        slot.OnItemUpdated += UpdateGFX;
        UpdateGFX();
    }

    void OnDestroy()
    {
        Slot.OnItemUpdated -= UpdateGFX;
    }

    void UpdateGFX()
    {
        if (Slot == null)
        {
            Debug.LogWarning("Trying to update inventory slot GFX when slot reference is null!");
            return;
        }

        if (Slot.IsEmpty)
        {
            icon.gameObject.SetActive(false);
            amount.text = string.Empty;
            return;
        }

        amount.text = Slot.Item.Data.IsStackable ? Slot.Item.Amount.ToString() : string.Empty;
        icon.sprite = Slot.Item.Data.Icon;
        icon.gameObject.SetActive(true);
    }
}
