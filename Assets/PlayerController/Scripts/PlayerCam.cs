using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerContoller
{
    public class PlayerCam : MonoBehaviour
    {
        public Vector2 sensitivity;

        public Transform orientation;
        public Transform camHolder;
        public Transform CameraPos;
        public Transform playerObj;

        Vector2 Rotation;
        Vector2 mouse;
        Vector2 rotationClamp;

        private void Start()
        {
            //Locks the cursor in the center of the window
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }

        public void Update()
        {
            MouseInput();
            CameraClapms();
            CameraMovement();
        }
        public void MouseInput()
        {
            //Gets mouse input
            mouse = new Vector2(
                Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity.x,
                Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity.y);
        }
        public void CameraClapms()
        {
            //Gets mouse input
            Rotation += new Vector2(-mouse.y, mouse.x);
            //Clamps the roation
            Rotation.x = Mathf.Clamp(Rotation.x, 10f, 30f);
            Rotation.y = Mathf.Clamp(Rotation.y, -20f, 20f);

            //Deadzone
            rotationClamp = new Vector2(Mathf.Clamp(Rotation.x, -5f, 15f), Mathf.Clamp(Rotation.y, -12f, 18f));
        }
        public void CameraMovement()
        {   //Checks deadzone
            if (Rotation != rotationClamp)
            {   //Moves the cxamera with the mouse
                camHolder.rotation = Quaternion.Euler(Rotation.x - rotationClamp.x, Rotation.y - CameraPos.localEulerAngles.y - rotationClamp.y, 0);
            }
        }
    }
}
