using System;
using UnityEngine;

public enum ButtonType
{
    Yes,
    No
}

[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(AudioSource))]
public class Button : MonoBehaviour, IInteractable
{
    public static event Action<ButtonType> OnButtonPressed;

    [SerializeField] private ButtonType buttonType;
    
    public void Hover()
    {
    }
    
    public void Interact()
    {
        OnButtonPressed?.Invoke(buttonType);
        GetComponent<Animation>().Play();
        GetComponent<AudioSource>().Play();
    }
}
