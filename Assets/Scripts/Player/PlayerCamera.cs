using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance;
    public PlayerManager playerManager;
    public Camera Camera;    
    [SerializeField] Transform cameraPivotTransform;

    [Header("Camera Settings")]    
    private float cameraSmoothSpeed = 1;      //The bigger this number, the longer for the camera to reach its position during movement
    [SerializeField] float leftAndRightRotationsSpeed = 220f;
    [SerializeField] float upAndDownRotationSpeed = 220f;
    [SerializeField] float minimunPivot = -30f;         //The lowest point you are able to look down
    [SerializeField] float maximinPivot = 60f;          //The highest point you are able to look up
    [SerializeField] float cameraCollisionRadius = 0.2f;
    [SerializeField] LayerMask collideWithLayers;

    [Header("Camera values")]
    private Vector3 cameraVelocity;
    private Vector3 cameraObjectPosition;               //Used for camera collisions (moves the camera object to this position upon colliding)
    [SerializeField] float leftAndRightLookAngle;
    [SerializeField] float upAndDownLookAngle;
    private float cameraZPosition;                      //Values used for camera collisions
    private float targetCameraZPosition;                //Values used for camera coliisions

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        cameraZPosition = Camera.transform.localPosition.z;
    }

    public void HandleAllCameraActions() 
    {
        if(playerManager != null) 
        {
            HandleFollowTarget();
            HandleRotations();
            HandleCollisions();
        }
    }

    private void HandleFollowTarget() 
    {
        Vector3 targerCameraPosition = Vector3.SmoothDamp(  transform.position, 
                                                            playerManager.transform.position, 
                                                            ref cameraVelocity, 
                                                            cameraSmoothSpeed * Time.deltaTime);
        transform.position = targerCameraPosition;
    }

    private void HandleRotations() 
    {
        //If locked on, force rotation towards target
        //Else rotate regularly
        
        //Rotate left and right based on horizontal movement
        leftAndRightLookAngle += (InputPlayerManager.instance.cameraHorizontalInput * leftAndRightRotationsSpeed) * Time.deltaTime;

        //Rotate up and down based on vertical movement
        upAndDownLookAngle -= (InputPlayerManager.instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;

        //Camp the up and down look angle between a min and max value
        upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimunPivot, maximinPivot);

        Vector3 cameraRotation;
        Quaternion targetRotation;
        //Rotate this gameobject left and right
        cameraRotation = Vector3.zero;
        cameraRotation.y = leftAndRightLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;

        //Rotate the pivot gameobject up and down
        cameraRotation = Vector3.zero;
        cameraRotation.x = upAndDownLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    private void HandleCollisions() 
    {
        targetCameraZPosition = cameraZPosition;
        RaycastHit hit;
        //Direction for cllision check
        Vector3 direction = Camera.transform.position - cameraPivotTransform.position;
        direction.Normalize();

        //We check if there is an object in front of our desired direction
        if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers)) 
        {
            //If there is, we get our distance from it
            float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
            //we then equate our target z position to the following
            targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
        }

        //If our target position is less than our collision radius, we subtract our collision radius (Making it snap back)
        if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius) 
        {
            targetCameraZPosition = -cameraCollisionRadius;
        }

        //We then apply pur final position using a lerp over a time of 0.2f
        cameraObjectPosition.z = Mathf.Lerp(Camera.transform.localPosition.z, targetCameraZPosition, 0.2f);
        Camera.transform.localPosition = cameraObjectPosition;
    }
}
