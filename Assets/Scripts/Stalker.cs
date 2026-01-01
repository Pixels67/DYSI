using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Stalker : MonoBehaviour, IInteractable
{
    [SerializeField] private Vector3 retreatPos;
    [SerializeField] private float retreatSpeed = 0.1f;

    private Vector3 _initialPos;
    private bool _active;
    private Coroutine _retreatCoroutine;

    private void Awake()
    {
        _initialPos = transform.position;
        
        Deactivate();
    }
    
    public void Hover()
    {
        if (!_active)
        {
            return;
        }

        _retreatCoroutine ??= StartCoroutine(Retreat());
    }

    public void Interact()
    {
    }

    public void Activate()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        _active = true;
    }

    private void Deactivate()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        _active = false;
        transform.position = _initialPos;
        _retreatCoroutine = null;
    }

    private IEnumerator Retreat()
    {
        const float step = 0.01f;
        
        while (Vector3.Distance(transform.position, retreatPos) > step)
        {
            transform.Translate(retreatSpeed * step * (retreatPos - transform.position).normalized);
            yield return new WaitForSeconds(step);
        }
        
        Deactivate();
    }
}
