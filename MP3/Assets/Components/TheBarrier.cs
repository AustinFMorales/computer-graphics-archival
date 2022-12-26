using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBarrier : MonoBehaviour
{
    // visually define the normal vector of TheBarrier
    // taken by sung's examples in class
    // in the next coming days, the barrier needs to be robust enough to have functions
    // drawing lines  - week3 example 8
    // drawing shadows - look into week3 example 7
    // reflection - look into week3 example 9
    public Transform Normal = null;
    private Vector3 Vn;
    private float D;
    // private radius;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(Normal != null);
    }

    // Update is called once per frame
    void Update()
    {
        ComputeNormal();
    }

    // compute the normal vector (direction) of this plane
    void ComputeNormal() {
        // derive from plane equation
        // the plane is QUAD in this instance
        // so the default direction is -transform.forward
        Vector3 Vn = -transform.forward;
        D = Vector3.Dot(Vn, transform.localPosition);

        // set up the normal
        Normal.up = Vn;
        Normal.localScale = new Vector3(0.1f, 5f, 0.1f);
        Normal.transform.localPosition = transform.localPosition + 5f * Vn;
        // Debug.Log("Normal position x: " + Normal.transform.localPosition.x);
        // Debug.Log("Normal position y: " + Normal.transform.localPosition.y);
        // Debug.Log("Normal position z: " + Normal.transform.localPosition.z)
    }

    // sung's implementation doesn't account for the scale  of the shadow, leading to incorrect shadow casts
    /*
    public float GetRadius() {
        if (transform.localScale.x == transform.localScale )
    }
    */
    
    public Transform GetNormal() {
        return Normal;
    }
}
