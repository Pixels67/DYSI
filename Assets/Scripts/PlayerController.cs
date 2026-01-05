using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")] [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private bool normalizeInput = true;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private Constraint cameraMovement;
    [SerializeField] private Constraint cameraWobble;
    [SerializeField] private float cameraMovementSpeed;

    [Header("Rotation")] [SerializeField] private float sensitivity = 0.2f;
    [SerializeField] private Constraint verticalConstraint = new (-90.0f, 70.0f);

    [Header("Misc.")] [SerializeField] private bool disableCursor = true;

    private CharacterController _characterController;
    private Camera _camera;
    private Vector3 _initialCamPos;
    private Vector2 _prevMousePosition;
    private float _verticalAngle;
    private float _movement = 1.0f;
    private float _wobble = 0.0f;
    private float _wobbleTimer = 0.0f;
    private float _retentionTimer = 0.0f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

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
            Debug.LogError("Player Controller: Camera not found!");
        }
        else
        {
            _initialCamPos = _camera.transform.localPosition;
        }
    }

    private void Start()
    {
        if (disableCursor)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void Update()
    {
        UpdatePosition();
        UpdateRotation();
    }

    private void UpdatePosition()
    {
        _wobbleTimer += Time.deltaTime;
        _retentionTimer += Time.deltaTime;
        
        var moveInput = moveAction.action.ReadValue<Vector2>();
        var moveDir = new Vector3(moveInput.x, 0.0f, moveInput.y);
        if (normalizeInput)
        {
            moveDir.Normalize();
        }

        var moveVec = moveDir * moveSpeed;
        var relativeMoveVec = moveVec.x * transform.right + moveVec.y * transform.up + moveVec.z * transform.forward;
        
        _characterController.Move(relativeMoveVec * Time.deltaTime);

        if (relativeMoveVec.magnitude < 0.1f)
        {
            var y = Mathf.Lerp(_camera.transform.localPosition.y, _initialCamPos.y, _retentionTimer);
            _camera.transform.localPosition = new Vector3(_camera.transform.localPosition.x, y, _camera.transform.localPosition.z);
            return;
        }

        _retentionTimer = 0.0f;

        if (_camera.transform.localPosition.y - _initialCamPos.y < cameraMovement.min)
        {
            _movement = 1.0f;
            _camera.transform.localPosition = new Vector3(_camera.transform.localPosition.x, _initialCamPos.y + cameraMovement.min, _camera.transform.localPosition.z);
            GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
            GetComponent<AudioSource>().Play();
            
        } else if (_camera.transform.localPosition.y - _initialCamPos.y > cameraMovement.max)
        {
            _movement = -1.0f;
        }
        
        _camera.transform.localPosition += _movement * cameraMovementSpeed * Time.deltaTime * Vector3.up;
        _camera.transform.localPosition += _wobble * cameraMovementSpeed * Time.deltaTime * Vector3.right;

        var camPos = _camera.transform.localPosition;
        camPos.x = Mathf.Clamp(camPos.x, cameraWobble.min, cameraWobble.max);
        _camera.transform.localPosition = camPos;

        if (_wobbleTimer >= 0.1f)
        {
            _wobbleTimer = 0.0f;
            _wobble = Random.Range(-0.5f, 0.5f);
        }
    }

    private void UpdateRotation()
    {
        var mouseDelta = Mouse.current.delta.ReadValue();

        var scaledMouseDelta = mouseDelta * sensitivity;
        var clampedMouseDelta = new Vector2(scaledMouseDelta.x, verticalConstraint.Clamp(_verticalAngle + scaledMouseDelta.y) - _verticalAngle);
        _verticalAngle += clampedMouseDelta.y;

        transform.Rotate(Vector3.up, clampedMouseDelta.x);
        _camera.transform.Rotate(Vector3.left, clampedMouseDelta.y);
    }
}