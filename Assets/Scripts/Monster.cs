using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class Monster : MonoBehaviour
{
    [SerializeField] private float pauseTimeSeconds = 1.0f;
    [SerializeField] private float delayTimeSeconds = 7.0f;
    [SerializeField] private UnityEvent onActivate;
    [SerializeField] private UnityEvent onEnd;
    
    private Coroutine _coroutine;
    
    public void OnMouseOver()
    {
        if (_coroutine != null) return;
        
        _coroutine = StartCoroutine(JumpScare());
    }

    private IEnumerator JumpScare()
    {
        GetComponent<AudioSource>().Play();
        onActivate?.Invoke();
        yield return new WaitForSeconds(pauseTimeSeconds);
        
        onEnd?.Invoke();
        yield return new WaitForSeconds(delayTimeSeconds);
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
