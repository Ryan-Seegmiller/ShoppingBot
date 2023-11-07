using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace PlayerContoller
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        private float moveSpeed;
        public float walkSpeed;
        public float sprintSpeed;
        public float playerRotationSpeed;

        public Animator playerAnimator;

        public GameObject backupCameraCanvas; 

        public float speedIncreaseMultiplier;
        public float slopeIncreaseMultiplier;

        public float groundDrag;

        [Header("keybinds")]
        public KeyCode sprintKey = KeyCode.LeftShift;

        [Header("Ground Check")]
        public float playerHeight;
        public LayerMask whatIsGround;
        public bool grounded;

        public Transform orientation;
        public Transform camHolder;

        Vector2 playerInput;
        private bool isSprinting;

        public Transform playerTransform => transform;

        Vector3 moveDirection;

        Rigidbody rb;

        Transform playerCollider;

        //Reference varieble
        public static Vector3 MouseOffset = new Vector3(15, -17, 0);

        bool OnSlope;
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
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.freezeRotation = true;

            playerCollider = GetComponentInChildren<SphereCollider>().gameObject.transform;

        }
        private void Update()
        {
            MyInput();

        }
        private void FixedUpdate()
        {
            onSlope = OnSlopeCheck(out RaycastHit slopeHit);
            GroundCheck();
            MovePlayer();


            // Adds drag to the player if on the ground
            rb.drag = grounded ? groundDrag : 0;
        }
        //Collects the player input
        private void MyInput()
        {
            //Input
            playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            isSprinting = Input.GetKey(sprintKey) && grounded;

        }
        //Movesw the player based on player input
        private void MovePlayer()
        {
            if (!grounded && !onSlope) { return; }
            // Calculate movement direction
            moveDirection = GetMoveDirection();

            moveSpeed = (isSprinting) ? sprintSpeed: walkSpeed;
            
            //Moves the chartacter
            rb.AddForce(moveSpeed * 10f * moveDirection.normalized, ForceMode.Force);
            transform.Rotate(0, playerInput.normalized.x * playerRotationSpeed, 0);
            camHolder.rotation = Quaternion.Euler(0, transform.rotation.y, 0);


            //Player animations
            playerAnimatorController();
        }
        
        //Checks the grpound
        void GroundCheck()
        {
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * .5f + .2f, whatIsGround);
        }
        
        public bool OnSlopeCheck(out RaycastHit slopeHit)
        {
            //Detects if there is a slope below the player
            return (Physics.Raycast(playerCollider.position, Vector3.down, out slopeHit, playerHeight * 0.5f + .3f) && slopeHit.normal != Vector3.up);
            
        }
        public Vector3 GetMoveDirection()
        {
            return OnSlopeCheck(out RaycastHit slopeHit) ? Vector3.ProjectOnPlane(orientation.forward * playerInput.y, slopeHit.normal) : orientation.forward * playerInput.y;
        }
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
    }
}
