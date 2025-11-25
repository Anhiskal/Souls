using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;

public class CharacterManager : NetworkBehaviour
{
    public CharacterController characterController;
    CharacterNetWorkManager CharacterNetWorkManager;
    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);
        characterController = GetComponent<CharacterController>();
        CharacterNetWorkManager = GetComponent<CharacterNetWorkManager>();
    }

    protected virtual void Update() 
    {
        // If this character is being controlled from our side, then assign its network oisition to the position of our transform
        if (IsOwner)
        {
            CharacterNetWorkManager.networkPosition.Value = transform.position;
            CharacterNetWorkManager.networkRotation.Value = transform.rotation;
        }
        // If this character is being controlled from else where, then assign its position here locally by the position of its network transform
        else 
        {
            //Position
            transform.position = Vector3.SmoothDamp(transform.position,
                                                    CharacterNetWorkManager.networkPosition.Value,
                                                    ref CharacterNetWorkManager.networkPositionVelocity,
                                                    CharacterNetWorkManager.networkPositionSmoothTime);
            //Rotation
            transform.rotation = Quaternion.Slerp(  transform.rotation, 
                                                    CharacterNetWorkManager.networkRotation.Value, 
                                                    CharacterNetWorkManager.networkRotationSmoothTime);
        }
    }

    protected virtual void LateUpdate() 
    {
    
    }

    
}
