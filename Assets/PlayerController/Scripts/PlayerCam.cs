using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public Vector2 sensitivity = new Vector2(0f,0f);

    public Transform orientation;
    public Transform camHolder;
    public Transform CameraPos;

    Vector2 Rotation = new Vector2 (0f,0f);

    private void Start()
    {
        //Locks the cursor in the center of the window
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Update()
    {
        //Gets mouse input
        Vector2 mouse = new Vector2(
            Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity.x, 
            Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity.y);

        Rotation += new Vector2(-mouse.y, mouse.x);

        //Clamps the roation of the up and down 
        Rotation.x = Mathf.Clamp(Rotation.x, -10f, 30f);
        Rotation.y = Mathf.Clamp(Rotation.y, -50f, 50f);

        // Rotate the camera
        camHolder.rotation = Quaternion.Euler(Rotation.x, Rotation.y - CameraPos.localEulerAngles.y, 0);
        orientation.rotation = Quaternion.Euler(0,Rotation.y - CameraPos.localEulerAngles.y, 0);


    }
}
