// Decompiled with JetBrains decompiler
// Type: FPSWalkerEnhanced
// Assembly: Assembly-UnityScript, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB4F58D3-CB97-41DB-A469-1243E4919DBE
// Assembly location: D:\Steam Games\steamapps\common\Project Warlock\pw_x64_Data\Managed\Assembly-UnityScript.dll

using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[Serializable]
public class FPSWalkerEnhanced : MonoBehaviour
{
    public float walkSpeed;
    public float runSpeed;
    public float crouchSpeed;
    public bool limitDiagonalSpeed;
    public bool enableRun;
    public bool enableCrouch;
    public float jumpSpeed;
    public float gravity;
    public bool enableFallingDamage;
    public float fallingDamageThreshold;
    public int fallingDamageMultiplier;
    public bool slideWhenOverSlopeLimit;
    public bool slideOnTaggedObjects;
    public float slideSpeed;
    public bool airControl;
    public float antiBumpFactor;
    public int antiBunnyHopFactor;
    private Vector3 moveDirection;
    private bool grounded;
    private CharacterController controller;
    private Transform myTransform;
    private float speed;
    private RaycastHit hit;
    private float fallStartLevel;
    private bool falling;
    private float slideLimit;
    private float rayDistance;
    private Vector3 contactPoint;
    private bool playerControl;
    private int jumpTimer;
    public GameObject mainCamera;
    private float charHeight;

    public FPSWalkerEnhanced()
    {
        walkSpeed = 6f;
        runSpeed = 11f;
        crouchSpeed = 3f;
        limitDiagonalSpeed = true;
        enableRun = true;
        enableCrouch = true;
        jumpSpeed = 8f;
        gravity = 20f;
        enableFallingDamage = true;
        fallingDamageThreshold = 10f;
        fallingDamageMultiplier = 2;
        slideSpeed = 12f;
        antiBumpFactor = 0.75f;
        antiBunnyHopFactor = 1;
        moveDirection = Vector3.zero;
    }

    public virtual void Start()
    {
        controller = (CharacterController)GetComponent(typeof(CharacterController));
        myTransform = transform;
        speed = walkSpeed;
        rayDistance = controller.height * 0.5f + controller.radius;
        slideLimit = controller.slopeLimit - 0.1f;
        jumpTimer = antiBunnyHopFactor;
    }

    public virtual void FixedUpdate()
    {
        float axis1 = Input.GetAxis("Horizontal");
        float axis2 = Input.GetAxis("Vertical");
        float num = (double)axis1 == 0.0 || (double)axis2 == 0.0 || !limitDiagonalSpeed ? 1f : 0.7071f;
        if (grounded)
        {
            bool flag = false;
            if (Physics.Raycast(myTransform.position, -Vector3.up, out hit, rayDistance))
            {
                if ((double)Vector3.Angle(hit.normal, Vector3.up) > (double)slideLimit)
                    flag = true;
            }
            else
            {
                Physics.Raycast(contactPoint + Vector3.up, -Vector3.up, out hit);
                if ((double)Vector3.Angle(hit.normal, Vector3.up) > (double)slideLimit)
                    flag = true;
            }
            if (falling)
            {
                falling = false;
                if ((double)myTransform.position.y < (double)fallStartLevel - (double)fallingDamageThreshold && enableFallingDamage)
                    ApplyFallingDamage(fallStartLevel - myTransform.position.y);
            }
            if (Input.GetKey(KeyCode.LeftShift) && enableRun)
                speed = runSpeed;
            else if (Input.GetKey("c") && enableCrouch)
            {
                enableRun = false;
                speed = crouchSpeed;
            }
            else
            {
                enableRun = true;
                speed = walkSpeed;
            }
            if (flag && slideWhenOverSlopeLimit || slideOnTaggedObjects && hit.collider.tag == "Slide")
            {
                Vector3 normal = hit.normal;
                moveDirection = new Vector3(normal.x, -normal.y, normal.z);
                Vector3.OrthoNormalize(ref normal, ref moveDirection);
                moveDirection *= slideSpeed;
                playerControl = false;
            }
            else
            {
                moveDirection = new Vector3(axis1 * num, -antiBumpFactor, axis2 * num);
                moveDirection = myTransform.TransformDirection(moveDirection) * speed;
                playerControl = true;
            }
            if (!Input.GetButton("Jump"))
                ++jumpTimer;
            else if (jumpTimer >= antiBunnyHopFactor)
            {
                moveDirection.y = jumpSpeed;
                jumpTimer = 0;
            }
        }
        else
        {
            if (!falling)
            {
                falling = true;
                fallStartLevel = myTransform.position.y;
            }
            if (airControl && playerControl)
            {
                moveDirection.x = axis1 * speed * num;
                moveDirection.z = axis2 * speed * num;
                moveDirection = myTransform.TransformDirection(moveDirection);
            }
        }
        moveDirection.y -= gravity * Time.deltaTime;
        grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != CollisionFlags.None;
    }

    public virtual void Update()
    {
    }

    public virtual void OnControllerColliderHit(ControllerColliderHit hit) => contactPoint = hit.point;

    public virtual void ApplyFallingDamage(float fallDistance)
    {
        gameObject.SendMessage("ApplyDammage", (object)(float)((double)fallDistance * (double)fallingDamageMultiplier));
        // Debug.Log((object) RuntimeServices.op_Addition(RuntimeServices.op_Addition("Ouch! Fell ", (object) fallDistance), " units!"));
    }

    public virtual void Main()
    {
    }
}
