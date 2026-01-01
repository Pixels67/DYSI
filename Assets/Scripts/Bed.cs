using System.Collections;
using UnityEngine;

public class Bed : MonoBehaviour, IInteractable
{
    [SerializeField] private PromptManager promptManager;
    [SerializeField] private Cover cover;

    private Coroutine _sleepCoroutine;

    public void Hover()
    {
    }

    public void Interact()
    {
        if (promptManager.FinishedShift && _sleepCoroutine == null)
        {
            _sleepCoroutine = StartCoroutine(Sleep());
        }
    }

    private IEnumerator Sleep()
    {
        promptManager.AdvanceShift();
        cover.FadeIn();
        yield return new WaitForSeconds(2.0F);
        cover.FadeOut();
        yield return new WaitForSeconds(1.0F);
        
        _sleepCoroutine = null;
    }
}
