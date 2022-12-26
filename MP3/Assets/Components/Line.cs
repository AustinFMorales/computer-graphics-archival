using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    /*
        Line Object is responsible for connecting a line between two transform
        the game object that represents a line here is a cylinder
        code taken from Sung's demonstration
    */
    public Transform p1;
    public Transform p2;
    public Transform line;
    // private Vector3 intersection;
    
    
    // extra variables we need for calculating reflection
    // Pon - intersection position
    // Pr - the endpoint of the direction 
    // Pl - the project of P1 on Vn
    /*
    public GameObject IntersectionLine;
    private GameObject Pl;
    private GameObject Pon;
    private GameObject Pr;
    private GameObject IntersectionInstance;
    private Line CompInstance;

    public float D = 0.1f;
    */ 

    private Line ReflectLine;
    private GameObject BarrierInstance = null;
    private TheBarrier BarrierComp = null;
    private bool isIntersectingWithBarrier;
    
    // prefab assignment - so we can instantiate once
    public GameObject PrefabReflectSegment;
    public GameObject PrefabPr;
    public GameObject PrefabPon;
    
    
    private GameObject ReflectSegment; // line segment we generate
    private GameObject Pr; // Pr (the end position of reflection)
    private GameObject Pon; // the intersection we cast on 
    // private Vector3 IntersectionNormal; // the direction of where the reflection vector goes.
    // default line scale
    private float scale = 0.2f;
    
    // Start is called before the first frame update
    void Start()
    {
        // Debug.Assert(ball != null);
        // currentTime = 0f;
        // SetGenerationInterval(1f);
        // IntersectionLine = GameObject.Find("IntersectionLine");
        // ReflectLine = null;
        // isIntersectingWithBarrier = false;
        // intersection = Vector3.zero;
        // Debug.Assert(PrefabReflectSegment != null);
        // Debug.Assert(PrefabPon != null);
        // Debug.Assert(PrefabPr != null);
        BarrierInstance = GameObject.Find("TheBarrier");
        BarrierComp = BarrierInstance.GetComponent<TheBarrier>();
    }

    // Update is called once per frame
    void Update()
    {
        if (InfrontOfBarrier() && !isReflectLine()) {
            CalculateReflection();
            // Debug.Log("infront of the barrier!");
        } else if (!InfrontOfBarrier() && !isReflectLine()){
            // Debug.Log("not infront of the barrier.");
        }
        
        CalculateLine();
    }

    // make this into a function for the meantime because we are going to be instantiating new lines later
    void CalculateLine() {
        if ((p1 == null && p2 == null) || (p1 == null || p2 == null)) {
            return;
        }
        // compute the line segment between p1 and p2
        // unity does the vector calculation automatically
        Vector3 line = p2.localPosition - p1.localPosition;
        // Mathf.Sqrt(line.x * line.x + line.y * line.y + line.z * line.z)
        float lineSize = line.magnitude;
        // divide each vector value by the magnitude, in this case unity does it for us
        Vector3 lineDirection = line.normalized;
        // intersection = Pon.transform;
        // based on the scale of the cylinder (0.2, lineSize / 2, 0.2);
        transform.localScale = new Vector3(scale, lineSize / 2f, scale);
        transform.up = lineDirection;
        // p1.transform.right = lineDirection;
        // calculate on the midpoint - where the cylinder is going to be
        transform.localPosition = (p1.localPosition + p2.localPosition) * 0.5f;
        // Pon.transform.position = intersection.position;
    }
    
    // set the line up by assigning its endpoints
    // the prefab linesegment will always be guaranteed to set its line on itself as the gameobject
    // so only focus on setting up the endpoints
    public void SetLine(Transform newP1, Transform newP2) {
        // line = newLine;
        p1 = newP1;
        p2 = newP2;
    }

    // Destroy the Line
    // useful for the world 
    public void DestroyLine() {
        Destroy(p1.gameObject);
        Destroy(p2.gameObject);
        Destroy(line.gameObject);
        Destroy(Pon);
        Destroy(Pr);
        Destroy(ReflectSegment);
    }

    // I want to know if this line segment is front of this barrier
    // lets make this a private function returns bool, so 
    // i can constantly use this in my calculatereflection function.

    private bool InfrontOfBarrier() {
        // null check
        if (isReflectLine() ||(p1 == null && p2 == null) || (p1 == null || p2 == null)) {
            return false;
        }
        
        Vector3 Vn = -BarrierComp.GetNormal().up;
        // in sung's example or the textbooks, D is always 2F, so pay attention to the math and derive
        // D properly
        float D = Vector3.Dot(Vn, BarrierInstance.transform.localPosition);
        Vector3 Pn = D * Vn;
        Vector3 Pt = BarrierInstance.transform.localPosition - p1.transform.localPosition;
        // Vector3 Pt = p1.transform.localPosition - BarrierInstance.transform.localPosition;
        bool Infront = (Vector3.Dot(Pn, Pt) > D);
        return Infront;
    }

    private void CalculateReflection() {
        // we can't calculate the reflection if this is a reflect line or
        // if we're not infront of the barrier
        if (isReflectLine() || (!InfrontOfBarrier() && !isReflectLine())) {
            ReflectSegment.SetActive(false); 
            Pr.SetActive(false); 
            Pon.SetActive(false); 
            return;
        }

        // instantiate if we haven't done it yet
        if (ReflectSegment == null && Pr == null && Pon == null) {
            ReflectSegment = Instantiate(PrefabReflectSegment);
            Pr = Instantiate(PrefabPr);
            Pon = Instantiate(PrefabPon);
            Pr.GetComponent<Renderer>().enabled = false;
            Pon.GetComponent<Renderer>().enabled = false;
            ReflectSegment.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            ReflectLine = ReflectSegment.GetComponent<Line>();
        }
        ReflectSegment.SetActive(true); 
        Pr.SetActive(true); 
        Pon.SetActive(true); 
        
        // now, we start with the calculation
        // we derived it originally from the world, but it failed
        // because of poor design decisions
        Vector3 Vn = -BarrierInstance.transform.forward;
        Vector3 v1 = p2.transform.localPosition - p1.transform.localPosition;
        float D = Vector3.Dot(Vn, BarrierInstance.transform.localPosition);
        if (v1.magnitude < float.Epsilon) {
            ReflectSegment.SetActive(false); 
            Pr.SetActive(false); 
            Pon.SetActive(false); 
            return;
        }
        float denom = Vector3.Dot(Vn, v1);
        bool lineNotParallelPlane = (Mathf.Abs(denom) > float.Epsilon); // prevent dividing by zero
        if (lineNotParallelPlane) {
            // calculation begins
            float d = (D - Vector3.Dot(p1.transform.localPosition, Vn)) / denom;
            Vector3 V3Pon = p1.transform.position + d * v1;
            // D = Vector3.Dot(V3Pon, Barrier.transform.localPosition);
            
            float h = 0;
            Vector3 Von, m;
            Von = p1.transform.position - V3Pon;
            // Von = V3Pon - p1.transform.position;
            h = Vector3.Dot(Von, Vn);
            Vector3 Pl = V3Pon + h * Vn;
            m = p1.transform.localPosition - Pl;
            Vector3 V3Pr = Pl - m;
            // do we really need Vr
            // maybe
            // Vector3 Vr = 2 * (Vector3.Dot(Von, Vn) * Vn) - Von;
            // check if we are within inbounds of the sphere
            // remember, the sphere in thebarrier is radius 6f
            Vector3 PonDistanceFromOrigin = V3Pon - BarrierInstance.transform.localPosition;
            if (PonDistanceFromOrigin.magnitude <= 6f) {
                Pon.transform.localPosition = V3Pon;
                Pr.transform.localPosition = V3Pr;
                ReflectSegment.GetComponent<Line>().SetLine(Pon.transform, Pr.transform);
                ReflectLine = ReflectSegment.GetComponent<Line>();
            } else {
                ReflectSegment.SetActive(false); 
                Pr.SetActive(false); 
                Pon.SetActive(false); 
            }

        } else {
            ReflectSegment.SetActive(false); 
            Pr.SetActive(false); 
            Pon.SetActive(false); 
        }
    }


    public Transform GetP1() {
        return p1;
    }

    public Transform GetP2() {
        return p2;
    }

    public Vector3 GetDirection() {
        return transform.up;
    }
    
    public void SetLineScale(float f) {
        scale = f;
    }
    
    public Vector3 GetReflectNormal() {
        // we should only return this reflection direction
        // if the reflectline's gameobjects are active.
        if (ReflectSegment == null && Pon == null && Pr == null && ReflectLine == null) {
            return Vector3.zero;
        }
        return ReflectLine.GetDirection();
        
    }


    // check if this is a reflect line
    public bool isReflectLine() {
        
        // if this is true, then we are working with a reflect line
        // otherwise, we are working with a regular line segment.
        return (PrefabReflectSegment == null && PrefabPr == null && PrefabPon == null);
    }
}
