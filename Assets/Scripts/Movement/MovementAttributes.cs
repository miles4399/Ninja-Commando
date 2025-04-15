using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MovementAttributes : ScriptableObject
{
    [Header("Movement")]
    public float Speed = 5f;
    public float Acceleration = 10f;

    [Header("Airborne")] 
    public float Gravity = -20f;
    public float JumpHeight = 2f;
    public float WallJumpDistance = 2f;
    public float WallJumpHeight = 5f;
    public float WallFallHeight = 0.625f;
    public float AirControl = 0.1f;
    public int NumberOfJumps = 2;
    public float MovementDisableTime = 0.15f;       //Change this along with Wall jump Height
    
    [Header("Grounding")]
    public float GroundCheckRadius = 0.25f;
    public Vector3 GroundCheckStart = new Vector3(0f, 0.4f);
    public Vector3 GroundCheckEnd = new Vector3(0f, -0.1f, 0f);
    public float MaxSlopeAngle = 50f;
    public float GroundedFudgeTime = 0.1f;
    public LayerMask GroundMask = 1 << 0;
    public LayerMask WallMask;
}
