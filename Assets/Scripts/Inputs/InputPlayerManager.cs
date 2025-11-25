using UnityEngine;
using UnityEngine.SceneManagement;

public class InputPlayerManager : MonoBehaviour
{
    public static InputPlayerManager instance;
    PlayerActions playerActions;

    [Header("Player Movement input")]
    [SerializeField] Vector2 movementInput;    
    public float horizontalInput;
    public float verticalInput;
    public float moveAmount;

    [Header("Camera movement input")]
    [SerializeField] Vector2 cameraInput;
    public float cameraHorizontalInput;
    public float cameraVerticalInput;

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }        
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        //When the scena changes, this script run
        SceneManager.activeSceneChanged += OnSceneChange;
        instance.enabled = false;        
    }

    /// <summary>
    /// If we are loading into our world scene, enable our players controls. Otherwise we must be at the main menu, disable our players controls
    /// </summary>
    /// <param name="oldScene"></param>
    /// <param name="newScene"></param>
    private void OnSceneChange(Scene oldScene, Scene newScene) 
    {
        if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorlfSceneIndex()) 
        {
            instance.enabled = true;
        }
        //This is our player cant move around if we enter thigs like a charecter creartion menu
        else 
        {
            instance.enabled = false;
        }
    }
    void OnEnable()
    {
        if(playerActions == null) 
        {
            playerActions = new PlayerActions();

            playerActions.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerActions.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
        }
        playerActions.Enable();
    }

    private void OnDestroy()
    {
        //When we destroy this object, unsubcribe from this event
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    //If we minimize or lower the window, stop adjusting inputs
    private void OnApplicationFocus(bool focus)
    {
        if (enabled) 
        {
            if (focus) 
            {
                playerActions.Enable();
            }
            else 
            {
                playerActions.Disable();
            }
        }
    }

    private void Update()
    {
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
    }

    private void HandlePlayerMovementInput() 
    {
        verticalInput = movementInput.y; 
        horizontalInput = movementInput.x;

        //Returns the absolute number
        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

        //we clamp the values, so they are 0, 0.5 or 1
        if (moveAmount <= 0.5 && moveAmount > 0) 
        {
            moveAmount = 0.5f;
        }
        else if(moveAmount > 0.5 && moveAmount <= 1)
        {
            moveAmount = 1;
        }
    }

    private void HandleCameraMovementInput() 
    {
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x; 
    }
}
