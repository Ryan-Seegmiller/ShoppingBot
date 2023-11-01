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

        private float desiredMoveSpeed;
        private float lastDesiredMoveSpeed;

        public float speedIncreaseMultiplier;
        public float slopeIncreaseMultiplier;

        public float groundDrag;

        [Header("keybinds")]
        public KeyCode sprintKey = KeyCode.LeftShift;

        [Header("Ground Check")]
        public float playerHeight;
        public LayerMask whatIsGround;
        public bool grounded;

        [Header("Slope Handling")]
        public float maxSlopeAngle;
        private RaycastHit slopeHit;
        private bool exitingSlope = false;

        public Transform orientation;
        public Transform camHolder;

        [Header("Step handiling")]
        public GameObject stepRayLower;
        public GameObject stepRayUpper;

        public float stepHeight = .4f;
        public float stepSmooth = .1f;

        Vector2 playerInput;

        public Transform playerTransform => transform;

        Vector3 moveDirection;

        Rigidbody rb;

        //Sets states for the player to be in
        public MovementState state;
        public enum MovementState
        {
            walking,
            sprintng,
            air
        }

        //public bool isSliding;
        //public bool wallRunning;

        private void Start()
        {
            rb = gameObject.GetComponent<Rigidbody>();
            rb.freezeRotation = true;

            stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHeight, stepRayUpper.transform.position.x);
        }
        private void Update()
        {
            MyInput();
            SpeedControl();
            StateHandler();
        }
        private void FixedUpdate()
        {
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

        }
        //Sets the states of the player
        private void StateHandler()
        {
            //state sprinting
            if (grounded && Input.GetKey(sprintKey))
            {
                state = MovementState.sprintng;
                desiredMoveSpeed = sprintSpeed;
            }

            // State walking
            else if (grounded || OnSlope())
            {
                state = MovementState.walking;
                desiredMoveSpeed = walkSpeed;
            }
            //State air
            else
            {
                state = MovementState.air;
            }

            //Check if desired move speed has changed drastically
            if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0f)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                moveSpeed = desiredMoveSpeed;
            }

            lastDesiredMoveSpeed = desiredMoveSpeed;
        }
        //Movesw the player based on player input
        private void MovePlayer()
        {

            // Calculate movement direction
            moveDirection = playerInput.y * orientation.forward;
            //On slope
            if (OnSlope() && !exitingSlope)
            {
                rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);
                if (rb.velocity.y > 0)
                {
                    //TODO: review
                    rb.AddForce(Vector3.down * 80f, ForceMode.Force);
                }
                transform.Rotate(0, playerInput.normalized.x * playerRotationSpeed, 0);
            }
            else
            {
                //Moves the chartacter
                rb.AddForce(moveSpeed * 10f * moveDirection.normalized, ForceMode.Force);
                transform.Rotate(0, playerInput.normalized.x * playerRotationSpeed, 0);
                camHolder.rotation = Quaternion.Euler(0, transform.rotation.y, 0);

                //Player animations
                playerAnimatorController();

                //Step Controller
                StepClimb();

            }
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
        //Checks the grpound
        void GroundCheck()
        {
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * .5f + .2f, whatIsGround);
        }
        void SpeedControl()
        {
            //Limiting speed on slope
            if (OnSlope() && !exitingSlope)
            {
                if (rb.velocity.magnitude > moveSpeed)
                {
                    rb.velocity = rb.velocity.normalized * moveSpeed;
                }
            }
            //Normal limiting
            else
            {
                Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

                //Limit velocity if needed
                if (flatVel.magnitude > moveSpeed)
                {
                    Vector3 limitedVel = flatVel.normalized * moveSpeed;
                    rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
                }
            }
        }
        public bool OnSlope()
        {
            //Detects if there is a slope below the player
            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + .3f))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                return angle < maxSlopeAngle && angle != 0;
            }
            return false;
        }
        public Vector3 GetSlopeMoveDirection(Vector3 direction)
        {
            //sets a new direction to move based off the slope direction
            return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
        }
        private IEnumerator SmoothlyLerpMoveSpeed()
        {
            // Smoothly lerp movement speed to desired value
            float time = 0;
            float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
            float startValue = moveSpeed;

            while (time < difference)
            {
                moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

                if (OnSlope())
                {
                    //Accelerates more based on the angle of the slope
                    float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                    float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                    time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
                }
                else
                {
                    time += Time.deltaTime * speedIncreaseMultiplier;
                }

                yield return null;
            }
            moveSpeed += desiredMoveSpeed;
        }
        void StepClimb()
        {
            RaycastHit hitLower;
            if(Physics.Raycast(stepRayLower.transform.position, transform.forward, out hitLower, .1f) && !OnSlope())
            {
                RaycastHit hitUpper;
                if (!Physics.Raycast(stepRayUpper.transform.position, transform.forward, out hitUpper, .2f))
                {
                    rb.position -= new Vector3(0f, -stepSmooth, 0f);
                }
            }
        }
    }
}
