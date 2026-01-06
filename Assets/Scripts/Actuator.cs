using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Actuator : MonoBehaviour, IInteractable
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private new Transform transform;
    [SerializeField] private Vector3 translation;
    [SerializeField] private Vector3 rotation;

    private bool _actuated = false;
    private Coroutine _actuateCoroutine;

    private void OnDisable()
    {
        if (_actuated)
        {
            Actuate();
        }
    }

    public void Hover()
    {
    }

    public void Interact()
    {
        _actuateCoroutine ??= StartCoroutine(ActuateCoroutine());
    }

    public void Actuate()
    {
        PlaySound();
        
        if (!_actuated)
        {
            transform.Translate(translation);
            transform.Rotate(rotation);
            _actuated = true;
        }
        else
        {
            transform.Rotate(-rotation);
            transform.Translate(-translation);
            _actuated = false;
        }
    }

    public IEnumerator ActuateCoroutine()
    {
        PlaySound();
        
        if (!_actuated && transform)
        {
            for (float i = 0; i < 1.0f / speed; i += 1)
            {
                transform.Translate(translation * speed);
                transform.Rotate(rotation * speed);
                yield return new WaitForSeconds(speed);
            }

            _actuated = true;
        }
        else
        {
            for (float i = 0; i < 1.0f / speed; i += 1)
            {
                transform.Rotate(-rotation * speed);
                transform.Translate(-translation * speed);
                yield return new WaitForSeconds(speed);
            }

            _actuated = false;
        }

        _actuateCoroutine = null;
    }

    private void PlaySound()
    {
        var source = GetComponent<AudioSource>();
        
        if (source == null) return;
        
        source.Play();
    }
}