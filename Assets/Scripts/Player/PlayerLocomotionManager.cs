using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager playerManager;

    public float verticalMovement;
    public float horizontalMovement;
    public float moveAmount;

    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;
    [SerializeField] float speedRotation = 15f;
    [SerializeField] float walkingSpeed = 2f;
    [SerializeField] float runningSpeed = 5f;


    protected override void Awake()
    {
        base.Awake();
        playerManager = GetComponent<PlayerManager>();
    }
    public void HandleAllMovement() 
    {
        HandleGroundMovement();
        HandleRotation();
    }

    private void GetVerticalAndHorizontalInputs() 
    {
        verticalMovement = InputPlayerManager.instance.verticalInput;
        horizontalMovement = InputPlayerManager.instance.horizontalInput;
    }

    //Our move direction is base on our cameras facing perspective and our movement inputs
    private void HandleGroundMovement() 
    {
        GetVerticalAndHorizontalInputs();

        moveDirection = PlayerCamera.Instance.transform.forward * verticalMovement;
        moveDirection = moveDirection + PlayerCamera.Instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (InputPlayerManager.instance.moveAmount > 0.5f) 
        {
            //Move at runing speed
            playerManager.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
        }
        else if(InputPlayerManager.instance.moveAmount <= 0.5f) 
        {
            //move at a walking speed
            playerManager.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
        }
    }

    private void HandleRotation() 
    {
        targetRotationDirection = Vector3.zero;
        targetRotationDirection = PlayerCamera.Instance.Camera.transform.forward * verticalMovement;
        targetRotationDirection = targetRotationDirection + PlayerCamera.Instance.Camera.transform.right * horizontalMovement;
        targetRotationDirection.Normalize();
        targetRotationDirection.y = 0;

        if (targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = transform.forward;
        }

        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, speedRotation * Time.deltaTime);
        transform.rotation = targetRotation;
    }
}
