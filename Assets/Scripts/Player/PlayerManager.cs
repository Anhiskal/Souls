using UnityEngine;

public class PlayerManager : CharacterManager
{
    PlayerLocomotionManager PlayerLocomotionManager;
    protected override void Awake()
    {
        base.Awake();
        PlayerLocomotionManager = GetComponent<PlayerLocomotionManager>();
    }

    protected override void Update()
    {
        base.Update();

        //If we do not own this gameobject, we do not control or edit it
        if (!IsOwner) return;

        //Handle movement
        PlayerLocomotionManager.HandleAllMovement();
    }

    protected override void LateUpdate()
    {
        if (!IsOwner) return;
        base.LateUpdate();

        PlayerCamera.Instance.HandleAllCameraActions();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner) 
        {
            PlayerCamera.Instance.playerManager = this;
        }
    }
}
