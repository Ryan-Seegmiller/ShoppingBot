using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlasticGui.PlasticTableColumn;

public class ItemRender : MonoBehaviour
{
    [SerializeField] GameObject mesh;

    Transform mainCamTransform; //Stores the main camera's transform
    private bool visible = true; //Sets whether or not the mesh should be rendered
    public float renderDistance = 3; //The distance away from the main camera that the object becomes invisible
    Renderer render; //The object's renderer
    Rigidbody rb;



    void Start()
    {
        mainCamTransform = Camera.main.transform; //Cache the main camera's transform
        Renderer render = mesh.GetComponent<MeshRenderer>(); //Cache the object's mesh renderer
        rb = gameObject.GetComponent<Rigidbody>();

    }

    void Update()
    {
        EnablePhysics();
    }

    public void EnablePhysics()
    {
        rb.isKinematic = false;
    }




    //This tanks the framerate, which is the opposite of what I want to happen. Don't use this.
/*    private void DistanceRender()
    {
        float dist = Vector3.Distance(mainCamTransform.position, transform.position); //Get distance between camera and object

        //Enable object
        if (dist < renderDistance)
        {
            if (!visible)
            {
                render.enabled = true; //Render object
                visible = true;
            }
        }
        //Disable object
        else
        {
            if (visible)
            {
                render.enabled = false; //Stop rendering object
                visible = false; 
            }
        }
    }*/
}
