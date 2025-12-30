using System;
using UnityEngine;

public enum ButtonType
{
    Yes,
    No
}

[RequireComponent(typeof(Animation))]
public class Button : MonoBehaviour, IInteractable
{
    public static event Action<ButtonType> OnButtonPressed;

    [SerializeField] private ButtonType buttonType;
    
    public void Interact()
    {
        OnButtonPressed?.Invoke(buttonType);
        GetComponent<Animation>().Play();
    }
}
