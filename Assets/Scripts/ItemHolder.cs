using UnityEngine;

public class ItemHolder : MonoBehaviour, IInteractable
{
    [SerializeField] private Item item;
    [SerializeField] private InteractionController interactController;

    private GameObject _itemObject;

    private void Awake()
    {
        if (item != null)
        {
            _itemObject = Instantiate(item.itemObject, transform);
        }
    }

    public void Hover()
    {
    }

    public void Interact()
    {
        if (interactController.currentItem != null)
        {
            item = interactController.currentItem;
            interactController.currentItem = null;
            
            _itemObject = Instantiate(item.itemObject, transform);
        }
        else
        {
            interactController.currentItem = item;
            item = null;
            
            Destroy(_itemObject);
        }
    }
}
