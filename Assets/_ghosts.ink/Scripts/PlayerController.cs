using DG.Tweening;
using SO.Variables;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speedSmoothing = 100;
    [SerializeField] private float mouseRotateSmoothing = 1000f;
    [SerializeField] private float gamepadRotateSmoothing = 1000f;
    [SerializeField] private float controllerDeadzone = 0.1f;
    [SerializeField] private FloatVariable MaxSpeed;
    [SerializeField] private CharacterController body;
    [SerializeField] private Gun currentGun;

    public static event Action playerDied;

    private Vector3 velocity;
    private Vector2 aim;

    [SerializeField] private bool isGamepad;

    public bool CanMove { get; set; }

    public bool CanRotate { get; set; }

    private void Start()
    {
        CanRotate = true;
    }

    public Transform Body => body.transform;

    private void FixedUpdate()
    {
        if(CanRotate)
            HandleRotation();

        if (CanMove)
            HandleMovement();
    }

    public void OnInputMove(InputAction.CallbackContext context)
    {
        Vector2 inputMove = context.ReadValue<Vector2>();

        velocity = Vector3.right * inputMove.x + Vector3.forward * inputMove.y;

        Debug.Log(velocity);
    }

    public void OnInputAim(InputAction.CallbackContext context)
    {
        aim = context.ReadValue<Vector2>();
    }


    public void OnInputFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            currentGun.HoldFire(true);
            currentGun.Fire();
        }
        
        if(context.canceled)
            currentGun.HoldFire(false);
            
    }


    public void OnInputColor(InputAction.CallbackContext context)
    {
        if(context.performed)
            currentGun.ChangeColor();
    }

    private const string Gamepad = "Controller";

    public void OnDeviceChange(PlayerInput playerInput)
    {
        isGamepad = playerInput.currentControlScheme.Equals(Gamepad);

        Debug.Log("changed");
    }


    private Vector3 targetVelocity = Vector3.zero;

    private void HandleMovement()
    {
        if (isGamepad)
        {
            body.Move(MaxSpeed.Value * Time.fixedDeltaTime * velocity);
        }
        else
        {
            targetVelocity = Vector3.Lerp(targetVelocity, velocity, speedSmoothing * Time.fixedDeltaTime);

            body.Move(MaxSpeed.Value * Time.fixedDeltaTime * targetVelocity);
        }

        body.Move(-9 * Time.fixedDeltaTime * Vector3.up);
    }

    private void HandleRotation()
    {
        if (isGamepad)
        {
            if (Mathf.Abs(aim.x) > controllerDeadzone || Mathf.Abs(aim.y) > controllerDeadzone)
            {
                Vector3 playerDirection = Vector3.right * aim.x + Vector3.forward * aim.y;
                if (playerDirection.sqrMagnitude > 0f)
                {
                    Quaternion newRotation = Quaternion.LookRotation(playerDirection, Vector3.up);
                    body.transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, gamepadRotateSmoothing * Time.fixedDeltaTime);
                }
            }
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(aim);

            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            if (groundPlane.Raycast(ray, out float rayDistance))
            {
                Vector3 point = ray.GetPoint(rayDistance);
                SmoothLookAt(point);
            }
        }
    }

    private void SmoothLookAt(Vector3 lookPoint)
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, body.transform.position.y, lookPoint.z);

        Quaternion targetRotation = Quaternion.LookRotation(heightCorrectedPoint - body.transform.position, Vector3.up);

        body.transform.rotation = Quaternion.Lerp(body.transform.rotation, targetRotation, mouseRotateSmoothing * Time.fixedDeltaTime);
    }

    public void OnDie()
    {
        playerDied?.Invoke();

        CanMove = false;
        CanRotate = false;

        body.transform.DORotate(Vector3.right * -90, 1);

    }

    public void OnStart()
    {
        CanMove = true;
        CanRotate = true;
    }


    
}
