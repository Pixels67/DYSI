using UnityEngine;
using UnityEngine.InputSystem;

public interface IItem
{
    public void Use(GameObject obj);
}

public class InteractionController : MonoBehaviour
{
    public IItem CurrentItem = null;
    
    [SerializeField] private float maxInteractDistance = 5.0f;
    
    private Camera _camera;

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
        
        if (!Mouse.current.leftButton.wasPressedThisFrame)
        {
            return;
        }
        
        foreach (var component in components)
        {
            component.Interact();
        }
        
        CurrentItem?.Use(hit.collider.gameObject);
    }
}