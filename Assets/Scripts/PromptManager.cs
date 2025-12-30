using System.Collections;
using TMPro;
using UnityEngine;

public class PromptManager : MonoBehaviour
{
    [SerializeField] private Prompt[] prompts;
    [SerializeField] private TMP_Text uiText;
    [SerializeField] private float charDelaySeconds = 0.05f;

    private uint _promptCounter;

    private void OnEnable()
    {
        Button.OnButtonPressed += ButtonCallback;
    }

    private void OnDisable()
    {
        Button.OnButtonPressed -= ButtonCallback;
    }

    private void ButtonCallback(ButtonType type)
    {
        DisplayNextPrompt();
    }

    private void DisplayNextPrompt()
    {
        if (prompts.Length <= _promptCounter)
        {
            Debug.LogWarning("Prompt Manager: No prompts left!");
            return;
        }

        StartCoroutine(DisplayText(prompts[_promptCounter].text));
        _promptCounter++;
    }

    private IEnumerator DisplayText(string text)
    {
        uiText.text = string.Empty;
        
        string buffer = text;
        while (buffer.Length != 0)
        {
            uiText.text += buffer[0];
            buffer = buffer[1..];
            yield return new WaitForSeconds(charDelaySeconds);
        }
    }
}

[System.Serializable]
internal struct Prompt
{
    public string text;
}