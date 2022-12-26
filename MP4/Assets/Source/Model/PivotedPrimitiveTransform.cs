using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotedPrimitiveTransform : NodePrimitive
{
    
    // this component we are trying to reimplement transform.rotatearound
    // let us inherit from nodeprimitive, because we want the nodeprimitive's MyTRSMatrix to do calculations
    // while we are rotating at the sametime.
    // rotate to respective node origin
    public Vector3 NodeOrigin = Vector3.zero;
    // public Vector3 Pivot = Vector3.zero;
    // choose what axis we want to rotate from
    public enum AxisChoice {
        XAxis = 0, // transform.right
        YAxis = 1, // transform.up
        ZAxis = 2 // transform.forward

    }
    // what direction does it want to move from?
    public enum DirChoice {
        XAxis = 0,
        YAxis = 1,
        ZAxis = 2
    }
    // how many degrees we're rotating
    // in this instance, this is going to be our rotational speed as well
    public float RotationSpeed = 0f;
    // how far it should go given an time range interval
    public float Range = 0f;
    // how fast it should go
    public float Velocity = 0f;
    private float Direction = 1f;
    // counts how many units have we made.
    private float CurrentTime = 0f;

    // choose where to rotate in respect with axis
    public AxisChoice AxisChoiceMode = AxisChoice.XAxis;
    // choose what direction to go towards in respect with axis.
    public DirChoice DirChoiceMode = DirChoice.XAxis;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // updates the position according to the selected axis chosen 
    void UpdatePosition() {
        if (CurrentTime > Range) {
            Direction *= -1f;
            CurrentTime = 0f;
        }
        switch (DirChoiceMode) {
            case DirChoice.XAxis:
                transform.position += (Vector3.right * (Velocity * Time.deltaTime)) * Direction; // we are going x-axis ignoring rotation!
                break;
            case DirChoice.YAxis:
                transform.position += (Vector3.up * (Velocity * Time.deltaTime)) * Direction; // we are going y-axis ignoring rotation!
                break;
            case DirChoice.ZAxis:
                transform.position += (Vector3.forward * (Velocity * Time.deltaTime)) * Direction; // we are going z-axis ignoring rotation!
                break;
        }
    }

    // updates the rotation according to the node origin and the selected axis chosen
    // rotates within respective axis
    // we are trying to emulate Transform.RotateAround()
    // https://answers.unity.com/questions/489350/rotatearound-without-transform.html
    void UpdateRotation() {
        if (CurrentTime > Range) {
            Direction *= -1f;
            CurrentTime = 0f;
        }
        // Vector3 position = transform.position;
        Quaternion rotation = Quaternion.identity;
        /*
        switch (AxisChoiceMode) {
            case AxisChoice.XAxis:
            Quaternion rotation = Quaternion.AngleAxis(RotationSpeed, transform.right * Direction); // get our desired rotation, multiply the axis by direction to go back and forth.
            Vector3 currentDirection = position - NodeOrigin; // find current direction relative to our nodeorigin
            currentDirection = rotation * currentDirection; // rotate the currentDirection
            transform.position = NodeOrigin + currentDirection; // define the new position
            // rotate the object to keep looking at the center of our nodeorigin
            Quaternion myRotation = transform.rotation;
            transform.rotation *= (Quaternion.Inverse(myRotation) * rotation * myRotation);
            break;
        }
        */

        switch(AxisChoiceMode) {
            case AxisChoice.XAxis:
                rotation = Quaternion.AngleAxis(RotationSpeed * Time.deltaTime * Direction, transform.right);
                break;
            case AxisChoice.YAxis:
                rotation = Quaternion.AngleAxis(RotationSpeed * Time.deltaTime * Direction, transform.up);
                break;
            case AxisChoice.ZAxis:
                rotation = Quaternion.AngleAxis(RotationSpeed * Time.deltaTime * Direction, transform.forward);
                break;
        }

        // don't do this, because the scenenode system and nodeprimitives somewhat override this.
        // hierarchy system also somewhat interferes with this implementation
        // Vector3 currentDirection = position - NodeOrigin; // find current direction relative to our nodeorigin
        // currentDirection = rotation * currentDirection; // rotate the currentDirection
        // transform.position = NodeOrigin + currentDirection; // define the new position
        // rotate the object to keep looking at the center of our nodeorigin - KEEP THIS instead
        Quaternion myRotation = transform.rotation;
        transform.rotation *= (Quaternion.Inverse(myRotation) * rotation * myRotation);
    }


    // Update is called once per frame
    void Update()
    {
        CurrentTime += Time.deltaTime;
        UpdatePosition();
        UpdateRotation();
        
    }
}
