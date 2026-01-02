using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Item : MonoBehaviour
{
    public GameObject itemObject;
    
    public abstract void Use(GameObject obj);
}

public class InteractionController : MonoBehaviour
{
    public Item currentItem;
    
    [SerializeField] private float maxInteractDistance = 5.0f;
    [SerializeField] private Transform hand;
    
    private Camera _camera;
    private GameObject _itemObject;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        if (_camera == null)
        {
            _camera = GetComponentInChildren<Camera>();
        }

        if (_camera == null)
        {
            _camera = Camera.main;
        }

        if (_camera == null)
        {
            Debug.LogError("Interaction Controller: Camera not found!");
        }
        
        if (currentItem != null)
        {
            _itemObject = Instantiate(currentItem.itemObject, transform);
        }
    }

    private void Update()
    {
        var ray = _camera.ViewportPointToRay(new Vector2(0.5f, 0.5f));

        if (!Physics.Raycast(ray, out var hit, maxInteractDistance))
        {
            return;
        }

        var components = hit.collider.gameObject.GetComponents<IInteractable>();
        foreach (var component in components)
        {
            component.Hover();
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            foreach (var component in components)
            {
                component.Interact();
            }
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (currentItem != null && _itemObject != null)
            {
                var itemComponent = _itemObject.GetComponent<Item>();
                itemComponent?.Use(hit.collider.gameObject);
            }
        }
        
        UpdateItemObject();
    }

    private void UpdateItemObject()
    {
        if (currentItem != null && _itemObject == null)
        {
            _itemObject = Instantiate(currentItem.itemObject, hand);
        }
        
        if (currentItem == null && _itemObject != null)
        {
            Destroy(_itemObject);
        }
    }
}