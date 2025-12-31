using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{
    [SerializeField] private PromptManager promptManager;

    public void Hover()
    {
    }

    public void Interact()
    {
        promptManager.AdvanceShift();
    }
}
