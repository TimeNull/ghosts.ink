using SO.Variables;
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

    private Vector3 velocity;
    private Vector2 aim;

    private bool isGamepad;

    public Transform Body => body.transform;

    private void Update()
    {
        HandleRotation();
        HandleMovement();
    }

    public void OnInputMove(InputAction.CallbackContext context)
    {
        Vector2 inputMove = context.ReadValue<Vector2>();

        velocity = Vector3.right * inputMove.x + Vector3.forward * inputMove.y;
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

    private const string Gamepad = "Gamepad";

    public void OnDeviceChange(PlayerInput playerInput)
    {
        isGamepad = playerInput.currentControlScheme.Equals(Gamepad);
    }


    private Vector3 targetVelocity = Vector3.zero;

    private void HandleMovement()
    {
        if (isGamepad)
        {
            body.Move(MaxSpeed.Value * Time.deltaTime * velocity);
        }
        else
        {
            targetVelocity += velocity;

            targetVelocity = Vector3.Lerp(targetVelocity, Vector3.zero, speedSmoothing * Time.deltaTime);

            body.Move(MaxSpeed.Value * Time.deltaTime * targetVelocity);
        }

        body.Move(-9 * Time.deltaTime * Vector3.up);
    }

    private void HandleRotation()
    {
        if (isGamepad)
        {
            if(Mathf.Abs(aim.x) > controllerDeadzone || Mathf.Abs(aim.y) > controllerDeadzone)
            {
                Vector3 playerDirection = Vector3.right * aim.x + Vector3.forward * aim.y;

                if(playerDirection.sqrMagnitude > 0f)
                {
                    Quaternion newRotation = Quaternion.LookRotation(playerDirection, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, gamepadRotateSmoothing * Time.deltaTime);
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

        body.transform.rotation = Quaternion.Lerp(body.transform.rotation, targetRotation, mouseRotateSmoothing * Time.deltaTime);
    }


    
}
