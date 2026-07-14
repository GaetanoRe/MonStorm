using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public InventoryItem Item { get; private set; }

    [SerializeField] Rigidbody rb;

    readonly float maxHorizontalForce = 1.5f;
    readonly float maxVerticalForce = 7f;
    readonly float maxRotationSpeed = 8f;


    /// <summary>Initializes the InventoryItem data and adds a starting force to the object.</summary>
    /// <param name="item">The InventoryItem which will be associated with this DroppedItem.</param>
    public void Initialize(InventoryItem item)
    {
        Item = item;

        rb.AddForce(new Vector3(
            Random.Range(-maxHorizontalForce, maxHorizontalForce),
            Random.Range(maxVerticalForce / 2f, maxVerticalForce),
            Random.Range(-maxHorizontalForce, maxHorizontalForce)), ForceMode.Impulse);

        rb.angularVelocity = new Vector3(
            Random.Range(-maxRotationSpeed, maxRotationSpeed),
            Random.Range(-maxRotationSpeed, maxRotationSpeed),
            Random.Range(-maxRotationSpeed, maxRotationSpeed));
    }

    /// <summary>Destroys the DroppedItem.</summary>
    public void DestroyItem() => Destroy(gameObject, 0.05f);
}
