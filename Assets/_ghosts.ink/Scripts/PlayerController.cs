using SO.Variables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float mouseRotateSmoothing = 1000f;
    [SerializeField] private float gamepadRotateSmoothing = 1000f;
    [SerializeField] private float controllerDeadzone = 0.1f;
    [SerializeField] private FloatVariable Speed;
    [SerializeField] private CharacterController body;

    private Vector2 moveDirection;
    private Vector2 aim;
    [SerializeField] private bool isGamepad;


    private void Update()
    {
        HandleRotation();
        HandleMovement();
    }

    public void OnInputMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public void OnInputFire(InputAction.CallbackContext context)
    {

    }

    public void OnInputAim(InputAction.CallbackContext context)
    {
        aim = context.ReadValue<Vector2>();
    }

    public void OnInputColor(InputAction.CallbackContext context)
    {

    }

    public void OnDeviceChange(PlayerInput playerInput)
    {
        isGamepad = playerInput.currentControlScheme.Equals("Gamepad");
    }

    private void HandleMovement()
    {
        Vector3 velocity = Vector3.right * moveDirection.x + Vector3.forward * moveDirection.y;

        body.Move(Speed.Value * Time.deltaTime * velocity);
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

            Debug.Log(ray);

            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            float rayDistance;

            if (groundPlane.Raycast(ray, out rayDistance))
            {
                Vector3 point = ray.GetPoint(rayDistance);
                LookAt(point);
            }
        }
    }

    private void LookAt(Vector3 lookPoint)
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, body.transform.position.y, lookPoint.z);

        Quaternion targetRotation = Quaternion.LookRotation(heightCorrectedPoint - body.transform.position, Vector3.up);


        transform.rotation = Quaternion.RotateTowards(body.transform.rotation, targetRotation, mouseRotateSmoothing * Time.deltaTime);
    }

    
}
