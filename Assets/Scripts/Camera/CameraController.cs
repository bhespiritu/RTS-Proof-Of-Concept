using System;
using UnityEngine;



public class CameraController : MonoBehaviour
{


    private bool firstMiddleClick;
    private Vector3 middleVector;

    public GameObject terrarin;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        firstMiddleClick = false;
        middleVector = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(Input.mouseScrollDelta);
        Vector3 tempVec = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        float tempVecY = Input.mouseScrollDelta.y;

        //TODO: make a maximum ammount that yu can zoom out or in
        transform.position += new Vector3(0, -10 * tempVecY, 0);

        if (Input.GetMouseButton(2))
        {
            if (firstMiddleClick == false)
            {
                middleVector = tempVec;
                firstMiddleClick = true;
            }

            transform.position += new Vector3(10 * (middleVector.x - tempVec.x), 0, 10 * (middleVector.y - tempVec.y));
        }
        else
        {
            float tempVecX = tempVec.x - .5f;
            float tempVecZ = tempVec.y - .5f;

            //TODO: add shit so taht the user cannot span outside of the map
            if (Math.Abs(tempVecX) > .49f)//left right mouse handler
            {
                transform.position += new Vector3( .02f *(transform.position.y - terrarin.transform.position.y ) * tempVecX, 0, 0);
            }
            if (Math.Abs(tempVecZ) > .49f)//up down mouse handler
            {
                transform.position += new Vector3(0, 0, .02f * (transform.position.y - terrarin.transform.position.y ) * tempVecZ);
            }

            if (Input.GetKey("up") || Input.GetKey("w"))
            {
                transform.position += new Vector3(0, 0, 10);
            }
            else if (Input.GetKey("down") || Input.GetKey("s"))
            {
                transform.position += new Vector3(0, 0, -10);
            }

            if (Input.GetKey("right") || Input.GetKey("d"))
            {
                transform.position += new Vector3(10, 0, 0);
            }
            else if (Input.GetKey("left") || Input.GetKey("a"))
            {
                transform.position += new Vector3(-10, 0, 0);
            }
            firstMiddleClick = false;
        }

    }
}
