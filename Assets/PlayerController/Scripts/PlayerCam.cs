using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerContoller
{
    public class PlayerCam : MonoBehaviour
    {
        public Vector2 sensitivity = new Vector2(0f, 0f);

        public Transform orientation;
        public Transform camHolder;
        public Transform CameraPos;
        public Transform playerObj;
        Vector2 Rotation = new Vector2(0f, 0f);

        private void Start()
        {
            //Locks the cursor in the center of the window
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }

        public void Update()
        {
            //Gets mouse input
            
            Vector2 mouse = new Vector2(
                Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity.x,
                Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity.y);

            Rotation += new Vector2(-mouse.y, mouse.x);
            //Clamps the roation
            Rotation.x = Mathf.Clamp(Rotation.x, 10f, 30f);
            Rotation.y = Mathf.Clamp(Rotation.y, -20f, 20f);
            /*
            float bound = 5;
            if (Rotation.y < bound && Rotation.y > -bound)
                Rotation.y =0;        
            if (Rotation.x < bound && Rotation.x > -bound)
                Rotation.x =0;
            */

            Vector2 rotationClamp = new Vector2(Mathf.Clamp(Rotation.x, -5f, 15f), Mathf.Clamp(Rotation.y, -12f, 18f));

            if (Rotation != rotationClamp)
            {
                // Rotate the camera
                camHolder.rotation = Quaternion.Euler(Rotation.x - rotationClamp.x , Rotation.y - CameraPos.localEulerAngles.y - rotationClamp.y, 0);
                orientation.rotation = Quaternion.Euler(0, Rotation.y - CameraPos.localEulerAngles.y, 0);
                //playerObj.rotation = Quaternion.Euler(0,Rotation.y,0);
                //orientation.GetComponentInParent<Transform>().Rotate(0, -Rotation.y * 100, 0);
            }
            


        }
    }
}
