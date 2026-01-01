using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PromptManager : MonoBehaviour
{
    public bool FinishedShift { get; private set; }
    
    [SerializeField] private PromptList[] promptLists;
    [SerializeField] private TMP_Text uiText;
    [SerializeField] private float charDelaySeconds = 0.05f;

    private int _shiftCounter;
    private int _promptCounter;

    private Coroutine _displayPromptCoroutine;
    private Coroutine _displayTextCoroutine;

    public void AdvanceShift()
    {
        if (_shiftCounter + 1 < promptLists.Length)
        {
            _shiftCounter++;
            _promptCounter = 0;
            FinishedShift = false;
        }
    }

    private void OnEnable()
    {
        Button.OnButtonPressed += ButtonCallback;
    }

    private void OnDisable()
    {
        Button.OnButtonPressed -= ButtonCallback;
    }

    private void Awake()
    {
        _displayPromptCoroutine = StartCoroutine(DisplayNextPrompt());
    }

    private void ButtonCallback(ButtonType type)
    {
        if (_displayPromptCoroutine != null)
        {
            return;
        }

        _displayPromptCoroutine = StartCoroutine(DisplayNextPrompt());
    }

    private IEnumerator DisplayNextPrompt()
    {
        var prompts = promptLists[_shiftCounter].prompts;

        if (_promptCounter >= prompts.Count)
        {
            yield break;
        }

        var prompt = prompts[_promptCounter];

        if (_displayTextCoroutine != null)
        {
            StopCoroutine(_displayTextCoroutine);
        }

        yield return new WaitForSeconds(prompt.delaySeconds);

        prompt.promptEvent.Invoke();
        uiText.text = string.Empty;
        _displayTextCoroutine = StartCoroutine(DisplayPromptText(prompt, _promptCounter != prompts.Count - 1));
        _promptCounter++;

        yield return new WaitForSeconds(prompt.pauseSeconds);

        if (_promptCounter >= prompts.Count)
        {
            FinishedShift = true;
        }
        
        _displayPromptCoroutine = null;
    }

    private IEnumerator DisplayPromptText(Prompt prompt, bool hint = true)
    {
        string buffer = prompt.text;

        if (hint)
        {
            buffer += prompt.type switch
            {
                PromptType.Continue => "\n \n[Press any button to continue]",
                PromptType.YesNo => "\n \n[Y/N]",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        while (buffer.Length != 0)
        {
            if (buffer[0] != ' ' && buffer[0] != '\\')
            {
                yield return new WaitForSeconds(charDelaySeconds);
            }

            if (buffer[0] == '\\' && buffer[1] == 'd')
            {
                buffer = buffer[2..];
                yield return new WaitForSeconds(1);
                continue;
            }

            uiText.text += buffer[0];
            buffer = buffer[1..];
        }

        _displayTextCoroutine = null;
    }
}

public enum PromptType
{
    YesNo,
    Continue
}

[System.Serializable]
internal struct PromptList
{
    public List<Prompt> prompts;
}

[System.Serializable]
internal struct Prompt
{
    public PromptType type;
    [TextArea(3, 20)] public string text;
    public UnityEvent promptEvent;
    public float delaySeconds;
    public float pauseSeconds;
}