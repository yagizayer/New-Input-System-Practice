/// Author: Yağız A. AYER
/// Github: github.com/yagizayer
/// Date: 30 May 2021
/// Used Style guide: Google C# StyleGuide (https://google.github.io/styleguide/csharp-style.html)


using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ComplexMove : MonoBehaviour
{

    [SerializeField] private float _moveSpeed = 12f, _rotationSpeed = 720;

    private CharacterController controller;
    private Transform currentCamera;
    private Animator animator;
    private bool _isGrounded, _moving = false, _isSprinting = false;
    private float _horizontal = 0, _vertical = 0;
    private Vector3 _gravityForce;

    private void Start()
    {
        if (controller == null) controller = GetComponent<CharacterController>();
        if (currentCamera == null) currentCamera = Camera.main.transform;
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        MoveCharacter();
        // as we dont have any rigidbody component(character controller doesnt allow) we need to make our own gravity force
        MakeGravity();
    }

    void MoveCharacter()
    {
        Vector3 rawMoveDir = Vector3.forward * _vertical + Vector3.right * _horizontal;

        Vector3 cameraForwardNormalized = Vector3.ProjectOnPlane(currentCamera.forward, Vector3.up);
        Quaternion rotationToCamNormal = Quaternion.LookRotation(cameraForwardNormalized, Vector3.up);

        Vector3 finalMoveDir = rotationToCamNormal * rawMoveDir;

        _moving = false;
        if (!Vector3.Equals(finalMoveDir, Vector3.zero))
        {
            Quaternion rotationToMoveDir = Quaternion.LookRotation(finalMoveDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationToMoveDir, _rotationSpeed * Time.deltaTime);
            _moving = true;
        }

        animator.SetBool("Running", _moving);
        animator.SetBool("Sprinting", _isSprinting);


        controller.Move(finalMoveDir * (_isSprinting ? _moveSpeed * 2 : _moveSpeed) * Time.deltaTime);
    }
    void MakeGravity()
    {
        _isGrounded = Physics.CheckSphere(transform.position, .5f);
        if (_isGrounded && _gravityForce.y < 0)
        {
            _gravityForce.y = -2f;
        }
        _gravityForce.y += -9.8f * Time.deltaTime * Time.deltaTime;
        controller.Move(_gravityForce);
    }

    public void OnMoveInput(float horizontal, float vertical)
    {
        this._horizontal = horizontal;
        this._vertical = vertical;
    }
    public void OnSprintInput(bool sprintInput)
    {
        this._isSprinting = sprintInput;
    }
}
