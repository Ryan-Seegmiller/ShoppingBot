using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerContoller
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        public float walkSpeed;
        [Range(1, 2)]public float sprintMoveMultiplier;
        [Range(1, 2)] public float sprintTurnMultiplier;
        public float playerRotationSpeed;
        private float moveSpeed;
        private float turnSpeed;
        

        [Header("Animator")]
        public Animator playerAnimator;

        [Header("BackupCamera")]
        public GameObject backupCameraCanvas;

        [Header("Drag")]
        public float groundDrag;

        [Header("keybinds")]
        public KeyCode sprintKey = KeyCode.LeftShift;
        public KeyCode boostKey = KeyCode.B;

        [Header("Ground Check")]
        public LayerMask groundLayer;

        [Header("References")]
        public Transform orientation;

        //Input and direction holders
        Vector2 playerInput;
        Vector3 moveDirection;

        //Rigidbody
        Rigidbody rb;

        //Transform for the collider
        Transform playerColliderTR;

        //Reference variables
        public static Vector3 mousePos;
        public static bool isPaused;
        public Transform playerTransform => transform;


        #region IsSprintingBool
        private bool IsSprinting;
        private bool isSprinting
        {
            get { return IsSprinting; }
            set { 
                    if(IsSprinting == value) { return; }
                    IsSprinting = value; 
                }
        }
        #endregion

        #region OnSlopeBool
        private bool OnSlope;
        private bool onSlope
        {
            get { return OnSlope; }
            set
            {
                if (OnSlope != value)
                {
                    
                    OnSlope = value;
                }
            }
        }
        #endregion

        #region GroundedBool
        private bool Grounded;
        private bool grounded
        {
            get { return Grounded; }
            set { 
                if(Grounded == value) {return; }
                rb.drag = (value) ? groundDrag : 0;
                Grounded = value; 
            }
        }
        #endregion

       
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;

            playerColliderTR = GetComponentInChildren<SphereCollider>().gameObject.transform;
        }
        private void Update()
        {   //Checks input and colliders
            InputsAndChecks();
        }
        private void FixedUpdate()
        {
            MovePlayer();
            RotatePlayer();
            StuckCheck();
        }
        //Collects the player input
        private void InputsAndChecks()
        {
            //Sets the mouse position
            SetMousePos();

            //Input
            playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            isSprinting = Input.GetKey(sprintKey) && grounded;

            //Checks if on ground
            grounded = GroundCheck();
            
            onSlope = OnSlopeCheck(out RaycastHit slopeHit);


        }
        //Moves the player based on player input
        private void MovePlayer()
        {
            if ((!grounded && !onSlope) || isPaused) { return; }
            // Calculate movement direction
            moveDirection = GetMoveDirection();

            //Speed calculators
            moveSpeed = (isSprinting) ? walkSpeed * sprintMoveMultiplier : walkSpeed;

            //Moves the chartacter
            rb.AddForce(moveSpeed * 10f * moveDirection.normalized, ForceMode.Force);

            //Player animations
            playerAnimatorController();
        }
        private void RotatePlayer()
        {
            if (isPaused) { return; }
            turnSpeed = (isSprinting) ? playerRotationSpeed * sprintTurnMultiplier : playerRotationSpeed;
            //Rotates the player
            transform.Rotate(0, playerInput.normalized.x * turnSpeed, 0);
        }
        private void SetMousePos()
        {
            mousePos = Input.mousePosition;
        }
        #region Checks
        //Checks if the player is on the ground ground
        bool GroundCheck()
        {
            return Physics.Raycast(transform.position, Vector3.down, 1.5f, groundLayer);
        }
        public Vector3 GetMoveDirection()
        {
            return OnSlopeCheck(out RaycastHit slopeHit) ? Vector3.ProjectOnPlane(orientation.forward * playerInput.y, slopeHit.normal) : orientation.forward * playerInput.y;
        }
        private void StuckCheck()
        {
            if((!grounded && !onSlope) && rb.velocity.magnitude == 0)
            {
                rb.AddForce(orientation.forward * 10f, ForceMode.Impulse);
            }
        }
        public bool OnSlopeCheck(out RaycastHit slopeHit)
        {
            //Detects if there is a slope below the player
            return (Physics.Raycast(playerColliderTR.position, Vector3.down, out slopeHit, 1.5f) && slopeHit.normal != Vector3.up);
        }
        #endregion

        #region Player Animation
        void playerAnimatorController()
        {
            if (playerInput.y * -1f == -playerInput.y && playerInput.y != 0)
            {
                playerAnimator.SetBool("RightTread", true);
                playerAnimator.SetBool("LeftTread", true);
            }
            else if (playerInput.y * -1f == playerInput.y && playerInput.y != 0)
            {
                playerAnimator.SetBool("RightTread", true);
                playerAnimator.SetBool("LeftTread", true);
                playerAnimator.SetFloat("RightTreadBoost", -1);
                playerAnimator.SetFloat("LeftTreadBoost", -1);
            }
            else
            {
                playerAnimator.SetBool("RightTread", false);
                playerAnimator.SetBool("LeftTread", false);

            }
            if (playerInput.x * -1f == -1 && playerInput.x != 0)
            {
                playerAnimator.SetBool("RightTread", true);
                playerAnimator.SetFloat("RightTreadBoost", -20);
            }
            else if (playerInput.x * -1f == 1 && playerInput.x != 0)
            {
                playerAnimator.SetBool("LeftTread", true);
                playerAnimator.SetFloat("LeftTreadBoost", -20);
            }
        }
        #endregion
    

       
    }
}
