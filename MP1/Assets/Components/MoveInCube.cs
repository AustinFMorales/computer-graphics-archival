using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInCube : MonoBehaviour
{
    // Cube Behavior
    /*
        Speed = 1 unit per second
        Rotation speed = rotate on y-axis, 90 degrees per second
        Moving direction y-axis
        Range 0 < y < 5
        Color:
        positive-Y color = (1, 1, 1)
        negative-Y color = (1, 0, 1)
    */
    private float mDelta = 1f;
    private float mDir = 1f;
    public float yRange = 5f;
    private float count = 0f;
    private float rotationSpeed = 90f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       // something to note
       Vector3 current = transform.position;
       current.y += (mDir * mDelta) * Time.smoothDeltaTime;

       // 90 degrees per second rotation 
       float nextAngle = rotationSpeed * Time.smoothDeltaTime;

       count += (1f * Time.smoothDeltaTime);
       
       if (count > yRange) {
            mDir *= -1f;
            count = 0f;
            
            Renderer cubeRenderer = GetComponent<Renderer>();
            // positive-Y color = (1, 1, 1)
            if (mDir == 1f && cubeRenderer != null) {
                Material cubeMaterial = cubeRenderer.material;
                Color cubeColor = cubeMaterial.color;
                cubeColor.r = 1f;
                cubeColor.g = 1f;
                cubeColor.b = 1f;
                cubeMaterial.color = cubeColor;
            // negative-Y color = (1, 0, 1)
            } else if (mDir == -1f && cubeRenderer != null) {
                Material cubeMaterial = cubeRenderer.material;
                Color cubeColor = cubeMaterial.color;
                cubeColor.r = 1f;
                cubeColor.g = 0f;
                cubeColor.b = 1f;
                cubeMaterial.color = cubeColor;
            }
       }
       transform.position = current;
       // Use quartenions in unity to do rotational transform without using Rotate function
       // https://docs.unity3d.com/ScriptReference/Quaternion-operator_multiply.html
       transform.rotation *= Quaternion.AngleAxis(nextAngle, Vector3.up);
    }
}
