using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInCylinder : MonoBehaviour
{
    
    // Cylinder Behavior
    /*
    Speed = 1 unit per second
    No rotation
    Moving direction z-axis
    Range: 0 < z < 5
    Color
    Positive-Z - color is (1, 1, 1)
    Negative-Z - color is (1, 1, 0)
    */

    private float mDir = 1f; 
    private float mDelta = 1f;
    public float zRange = 5f;
    private float count = 0f;
    // check if the creation position is valid,
    // if the creation position is larger than the range, then first move it to
    // the CreationPlane's position then start its behavior
    private bool creationPosCheck;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Vector3 current = transform.position;
        if (current.z > zRange) {
            Debug.Log("RESET THIS CYLINDER POSITION");
            creationPosCheck = false;
        } else {
            creationPosCheck = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (creationPosCheck) {
            // something to note
            // a newly created shape position will travel towards positive direction first if current.z < 5
            // otherwise (current.z > 5), it will travel negative first 
            // KEEP IN MIND - after this it will never cross the 0 to 5 range, so revise later
            Vector3 current = transform.position;
            current.z += (mDir * mDelta) * Time.smoothDeltaTime;
            // count should be incremented using smoothDeltaTime
            // otherwise you would be making a weird calculation based on unlocked framerate
            count += (1f * Time.smoothDeltaTime);
            if (count > zRange) {
                mDir *= -1f;
                // reset count
                count = 0f;
                Renderer cylinderRenderer = GetComponent<Renderer>();
                // Positive-Z - color is (1, 1, 1)
                if (mDir == 1f && cylinderRenderer != null) {
                    Material cylinderMaterial = cylinderRenderer.material;
                    Color cylinderColor = cylinderMaterial.color;
                    cylinderColor.r = 1f;
                    cylinderColor.g = 1f;
                    cylinderColor.b = 1f;
                    cylinderMaterial.color = cylinderColor;
                // Negative-Z - color is (1, 1, 0)
                } else if (mDir == -1f && cylinderRenderer != null) {
                    Material cylinderMaterial = cylinderRenderer.material;
                    Color cylinderColor = cylinderMaterial.color;
                    cylinderColor.r = 1f;
                    cylinderColor.g = 1f;
                    cylinderColor.b = 0f;
                    cylinderMaterial.color = cylinderColor;
                }
            }
            transform.position = current;
        } else {
            Debug.Log("RESETTING CYLINDER POSITION");
            mDir *= -1f;
            creationPosCheck = true;
        }
        
        

    }
}
