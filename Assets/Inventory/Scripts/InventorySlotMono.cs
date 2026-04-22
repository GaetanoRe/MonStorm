using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class InventorySlotMono : MonoBehaviour, IPointerClickHandler
{
    public event Action<InventorySlotMono> OnClicked;
    public event Action<InventorySlotMono> OnSecondaryClicked;
    public InventorySlot Slot { get; private set; }

    [SerializeField] Image icon;
    [SerializeField] Text amount;


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
