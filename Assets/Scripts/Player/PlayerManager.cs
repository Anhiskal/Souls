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
}
