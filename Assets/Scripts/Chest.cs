using UnityEngine;

public class Chest : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public Sprite closedChest;
    public Sprite openedChest;

    private void Awake()
    {
        spriteRenderer.sprite = closedChest;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerInventory inventory = collision.gameObject.GetComponent<PlayerInventory>();
            if (inventory.HasKey())
                {
                    spriteRenderer.sprite = openedChest;
                }
            else
                Debug.Log("NON HAI LA CHIAVE");

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
    }
}
