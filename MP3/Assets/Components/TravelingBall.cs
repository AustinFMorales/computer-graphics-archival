using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelingBall : MonoBehaviour
{
    /*
        the traveling balls have three characteristics
        all of which are defined by slider ui elements
        interval - how many traveling balls are generated? - probably should be handled by TheWorld or Line
        speed - how fast the ball is going - defined in this class
        alive - gameobject's lifetime - defined in this class

    */
    private float speed; // represents how fast the ball is going
    private float lifespan; // represents how long the ball will last
    private Vector3 velocity; // velocity vector, always normalized.
    // public Transform Traveler = null; don't need to do this component is assigned to the ball
    private float timeAlive;
    // set a reference line in case we need the direction of where the ball moves next
    // once it hits the barrier
    private Line ReferenceToLine;
    
    // the travelling ball needs to know if it's going towards the direction of the barrier
    public GameObject Barrier;
    public TheBarrier BarrierComp;
    private Line ShadowLine;
    // prefab assignments
    public GameObject ShadowLineObject;
    // the shadow projected on the plane
    public GameObject Pon;
    // the shadow instances that are generated by the travelling ball
    // during its lifespan
    private GameObject GeneratedLine;
    private GameObject PonShadow;


    // Start is called before the first frame update
    void Start()
    {
        // timeAlive = 0f;
        // speed = 0f;
        // lifespan = 500f;
        // velocity = Vector3.zero;
        // isFacing = false;
        Barrier = GameObject.Find("TheBarrier");
        BarrierComp = Barrier.GetComponent<TheBarrier>();
        ShadowLine = null;
        // PonShadow = null;
        // GeneratedLine = null;
        // ShadowLineObject = GameObject.Find("ShadowLine");
        // Pon = GameObject.Find("ShadowSphere");
        // Pon.transform.localPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive > lifespan) {
            // make sure they are destroyed
            if (GeneratedLine != null && PonShadow != null) {
                Destroy(GeneratedLine);
                Destroy(PonShadow);
            }
            Destroy(gameObject);
            
        }
        CalculateClosestPointOnPlane();
        UpdatePosition();

        if (PonShadow != null) {
            BounceOff();
        }
        
    }

    // return true if we are infront of the barrier
    // other wise false
    // this will be useful for casting shadows and reflecting off the barrier
    // but we need to reflect and bounce off the sphere child, so we are only accounting for the plane parent
    public bool InfrontOfBarrier() {
        // the distance between the barrier
        // taken from quiz 4
        // from plane towards travelling ball
        // Vector3 V = Barrier.transform.position - transform.position;
        Vector3 V = Barrier.transform.GetChild(0).position - transform.position;
        // float result = Vector3.Dot(-Barrier.transform.position.normalized, V);
        // the normal vector needs to be negative because we used a quad as a plane 
        // to determine our calculations.
        float d = Vector3.Dot(-BarrierComp.GetNormal().up, V);
        // we should also check if its travelling closer and closer to the plane
        // so if our direction (velocity) is travelling towards the plane's normal.
        // d2 represents if the relationship between our direction (velocity vector) and the normal plane (plane's direction)
        float d2 = Vector3.Dot(velocity, -BarrierComp.GetNormal().up);
        // Debug.Log("d2 product val: " + d2);
        // if we are travelling towards the barrier and we are getting closer towards it and
        // we are facing towards it
        // we're definitely infront of it
        // we can use d2 later to truly check if its facing away the plane.
        if (d > 0) {  // (d2 > 0 && d > 0) // proper projection? just with d it projects in weird places.
            //Debug.Log("success d product val: " + d);
            // Debug.Log("success d2 product val: " + d2);
            return true;
        }
        // otherwise we are not
        // Debug.Log("failed d product val: " + d);
        // Debug.Log("failed d2 product val: " + d2);
        return false;
    }

    // calculate closest point on plane? - intersection point
    // we're trying to get those shadow spheres
    void CalculateClosestPointOnPlane() {
        // now that we are infront of the barrier, how can we derive Pon (the shadow point)
        // take the math from week 3 example 7
        // let the traveling balls project the 
        // get the direction
        Vector3 normal = -BarrierComp.GetNormal().up;
        // get the center of our barrier
        Vector3 center = Barrier.transform.GetChild(0).position;
        // plane equation
        float d = Vector3.Dot(normal, center);
        // the perpendicular distance between the traveling ball and the plane
        float h = Vector3.Dot(transform.localPosition, normal) - d;
        
        
        // calculate the shadow that is going to be casted
        Vector3 NewShadowPosition = transform.localPosition - (normal * h);
        // distance check
        Vector3 NewShadowFromOrigin = NewShadowPosition - center;
    
        if (InfrontOfBarrier() && ShadowLine == null) {        
            // this mean the shadow is not within the sphere, disregard
            if (NewShadowFromOrigin.magnitude >= 6f) {
                // Debug.Log("bye bye" + " NewShadowFromOrigin: " + NewShadowFromOrigin.magnitude);
                return;
            }
            PonShadow = Instantiate(Pon);
            GeneratedLine = Instantiate(ShadowLineObject);
            // Pon.transform.localPosition = transform.localPosition - (normal * h);
            // PonShadow.transform.localPosition = transform.localPosition - (normal * h);
            PonShadow.transform.localPosition = NewShadowPosition - 1f * normal;
            PonShadow.transform.right = -BarrierComp.GetNormal().up;
            // distance check from origin
            // ShadowLineObject.gameObject.AddComponent<Line>();
            GeneratedLine.gameObject.AddComponent<Line>();
            ShadowLine = GeneratedLine.gameObject.GetComponent<Line>();
            ShadowLine.SetLine(this.transform, PonShadow.transform);
            ShadowLine.SetLineScale(0.01f);
        } else if (InfrontOfBarrier() && ShadowLine != null) { // if its already instantiated, just keep updating the line and the shadow casted.
            /*
            Vector3 normal = -BarrierComp.GetNormal().up;
            Vector3 center = Barrier.transform.position;
            float d = Vector3.Dot(normal, center);
            float h = Vector3.Dot(transform.localPosition, normal) - d;
            */
            // PonShadow.transform.localPosition = transform.localPosition - (normal * h);
            // redundant if statement, but we need this if the shadowline is already created.
            if (NewShadowFromOrigin.magnitude >= 6f) {
                // Debug.Log("bye bye" + " NewShadowFromOrigin: " + NewShadowFromOrigin.magnitude);
                Destroy(PonShadow);
                Destroy(GeneratedLine);
                PonShadow = null;
                GeneratedLine = null;
                return;
            }
            // do we want to calculate the position to scale properly?
            PonShadow.transform.localPosition = NewShadowPosition; 
            // make it where the shadow is like the normalized vector cylinder so it can stick regardless of rotation
            PonShadow.transform.right = -BarrierComp.GetNormal().up;
            
        } else if (!InfrontOfBarrier() && (ShadowLine != null || ShadowLine == null)) { // no longer pointing the proper direction!
            // destroy the gameobjects
            ShadowLine = null;
            Destroy(PonShadow);
            Destroy(GeneratedLine);
            PonShadow = null;
            GeneratedLine = null;
        }
    }

    // check to see if our travelling ball has collided with Pon
    // true if its touching or within bounds of the radius
    // Pon's radius is 1 since it's a flattened sphere.
    public bool CollisionDetection(Transform p1, Transform p2 , float r) {
        if ((p1 == null || p2 == null) || r < 0) {
            return false;
        }
        float distance = (p1.transform.localPosition - p2.transform.localPosition).magnitude;
        distance = Mathf.Abs(distance);
        if (distance > r) {
            return false;
        }
        Debug.Log("collided!");
        return true;
    }

    // we need to make the ball bounce off once it collides with the wall
    // so we need to update the velocity vector (direction)
    private void BounceOff() {
        // if it fails, return early
        if (!CollisionDetection(this.transform, PonShadow.transform, float.Epsilon) && InfrontOfBarrier()) {
            return;
        } else {
            if (ReferenceToLine.GetReflectNormal().Equals(Vector3.zero)) {
                this.SetVelocity(-BarrierComp.GetNormal().up);
                return;
            } 
            
            this.SetVelocity(ReferenceToLine.GetReflectNormal());
        }
    }

    // create a function that checks if it is inside the shadow sphere
    // once it is, bounce off.
    
    public void SetSpeed(float s) {
        speed = s;
    }

    public void SetLifespan(float l) {
        lifespan = l;
    }

    public void SetVelocity(Vector3 v) {
        velocity = v;
    }
    
    public void SetReferenceToLine(Line l) {
        ReferenceToLine = l;
    }
    
    public Line GetReferenceToLine() {
        return ReferenceToLine;
    }
    

    // calculate the velocity and direction
    void UpdatePosition() {
        // Traveler.position += (direction) * velocity * Time.deltaTime;
        transform.localPosition += velocity * speed * Time.deltaTime;
    }

}
