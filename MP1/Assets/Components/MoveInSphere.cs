using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInSphere : MonoBehaviour
{   
    // Sphere Behavior
    // Speed = 1 unit per second
    // No rotation
    // Moves in the x-axis
    // Range 0 < x < 5
    // Color When traveling in the Positive-X (1, 1, 1)
    // Color When traveling in the Negative-X (0, 1, 1)
    private float mDir = 1f; // direction
    private float mDelta = 1f; // 1 unit per second
    public float xRange = 5f; // x-axis range
    private float count = 0f; // calculate range at any coordinate with count?
    // check if the creation position is valid,
    // if the creation position is larger than the range, then first move it to
    // the CreationPlane's position then start its behavior
    private bool creationPosCheck;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 current = transform.position;
        if (current.x > xRange) {
            Debug.Log("RESET THIS SPHERE POSITION");
            creationPosCheck = false;
        } else {
            creationPosCheck = true;
        }
    }

    // Update is called once per frame
    void Update()
    {   
        if (creationPosCheck) {
            // this isn't a good idea because what if you want to instantiate it at
            // any coordinate and move within a range?
            Vector3 current = transform.position;
            // move 1 unit per second without being defined by framerate speed
            current.x += (mDelta * mDir) * Time.smoothDeltaTime;
        
            // this works, but can we do better?
            count += (1f * Time.smoothDeltaTime);
            // something to note
            // a newly created shape position will travel towards positive direction first if current.x < 5
            // otherwise (current.x > 5), it will travel negative first 
            // KEEP IN MIND - after this it will never cross the 0 to 5 range, so revise later
            if (count > xRange) {
                // switch the sign to negative
                mDir *= -1f;
                count = 0f;
                Renderer sphereRenderer = GetComponent<Renderer>();
                // travelling positive X, the color should be (1, 1, 1)
                if (mDir == 1f && sphereRenderer != null) {
                    Material sphereMaterial = sphereRenderer.material;
                    Color sphereColor = sphereMaterial.color;
                    sphereColor.r = 1f;
                    sphereColor.g = 1f;
                    sphereColor.b = 1f;
                    sphereMaterial.color = sphereColor;
                // travelling negative X, the color should be (0, 1, 1)
                } else if (mDir == -1f && sphereRenderer != null) {
                    Material sphereMaterial = sphereRenderer.material;
                    Color sphereColor = sphereMaterial.color;
                    sphereColor.r = 0f;
                    sphereColor.g = 1f;
                    sphereColor.b = 1f;
                    sphereMaterial.color = sphereColor;
                }
            }
            transform.position = current;
        } else {
            // reset the creation position 
            Debug.Log("Resetting SPHERE POSITION");
            mDir *= -1f;
            creationPosCheck = true;
            
        }
        
        
    }
}
