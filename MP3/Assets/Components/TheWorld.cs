using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWorld : MonoBehaviour
{
    // need to keep track of the amount of lines
    // to keep spitting out travelling balls
    private List <Line> WorldLines;
    // private List <Line> WorldReflectionLines;
    // assign these as prefabs so we can easily instantiate using TheWorld
    // most of this is taken from sung's office hours when discussing design
    public GameObject EndPoint;
    public GameObject LineSegment;
    public GameObject TravelBall;
    private GameObject current;
    // bad idea because you're breaking MVC rules
    // but because we're generating balls here and affecting their values
    // it is somewhat justified.
    public SliderWithEcho Interval;
    public SliderWithEcho Speed;
    public SliderWithEcho AliveSec;
    
    public GameObject BarrierInstance;

    // now, we set the values of 
    // traveling ball spawn interval
    // traveling ball speed
    // traveling ball lifespan
    public float WorldBallSpawnInterval;
    public float WorldBallSpeed;
    public float WorldBallLifespan;
    private float WorldTime;
    
    // Start is called before the first frame updat
    void Start()
    {
        Debug.Assert(EndPoint != null);
        Debug.Assert(LineSegment != null);
        Debug.Assert(TravelBall != null);
        Debug.Assert(Interval != null);
        Debug.Assert(Speed != null);
        Debug.Assert(AliveSec != null);
        Debug.Assert(BarrierInstance != null);
        WorldTime = 0f;
        WorldBallSpawnInterval = 1f;
        WorldBallSpeed = 5f;
        WorldBallLifespan = 10f;
        // set the listeners to the sliders
        Interval.SetSliderListener(SetWorldBallSpawnInterval);
        Speed.SetSliderListener(SetWorldBallSpeed);
        AliveSec.SetSliderListener(SetWorldBallLifespan);
        // initialize the sliders
        Interval.InitSliderRange(0.5f, 4f, WorldBallSpawnInterval);
        Speed.InitSliderRange(0.5f, 15f, WorldBallSpeed);
        AliveSec.InitSliderRange(1f, 15f, WorldBallLifespan);
        

        WorldLines = new List<Line>();
        // WorldReflectionLines = new List<Line>();
        // append the first line segment generated in the scene
        GameObject first_line = GameObject.Find("LineSegment");
        Line start = first_line.GetComponent<Line>();
        WorldLines.Add(start);
    }

    // Update is called once per frame
    void Update()
    {
        WorldTime += Time.deltaTime;
        if (WorldTime > WorldBallSpawnInterval) {
            GenerateBallOnEachLine();
            WorldTime = 0f;
        }
    }
    
    // basic getters and setters
    public void SetWorldBallSpawnInterval(float ix) {
        WorldBallSpawnInterval = ix;
    }
    
    public void SetWorldBallSpeed(float bx) {
        WorldBallSpeed = bx;
    }

    public void SetWorldBallLifespan(float lx) {
        WorldBallLifespan = lx;
    }
    
    public GameObject GetSelected() {
        return current;
    }
    
    public void SetSelected(ref GameObject g) {
        ReleaseSelected();
        current = g;
        // Debug.Assert(current != null);
        Debug.Log("selected: " + g.name);
        Debug.Log("current layermask: " + g.layer);
    }

    public void ReleaseSelected() {
        if (current != null) {
            current = null;
        }
    }

    public void SetSelectedPosition(ref Vector3 position) {
        if (current != null) {
            current.transform.localPosition = position;
        }
    }



    // instantiate new line with the world on location
    public void InstantiateNewLine(ref Vector3 location) {
        // instantiate game object two endpoints
        // instantiate the line segment
        // add to the list
        // first we instantiate a new line
        GameObject newLine = Instantiate(LineSegment);
        newLine.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        newLine.layer = 10;
        // instantiate the endpoints
        GameObject newP1 = Instantiate(EndPoint);
        newP1.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        newP1.layer = 9;
        GameObject newP2 = Instantiate(EndPoint);
        newP2.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        newP2.layer = 9;
        // now because we're always generating a new line at the left wall
        // x will always be the same but y and z will not
        newP1.transform.position = new Vector3(-17f, location.y, location.z);
        // generate the other endpoint on the other side
        newP2.transform.position = new Vector3(17f, location.y, location.z);
        newLine.GetComponent<Line>().SetLine(newP1.transform, newP2.transform);
        // add the newLine into our WorldLines
        WorldLines.Add(newLine.GetComponent<Line>());

    }
    
    // delete a line segment from our WorldLines
    // then destroy the gameobject
    // https://answers.unity.com/questions/752382/how-to-compare-if-two-gameobjects-are-the-same-1.html
    // use references to directly check if they are the same
    public bool DeleteLine(ref GameObject l) {
        Line temp = l.GetComponent<Line>();
        if (temp != null && WorldLines.Remove(temp)) { // if the line is found in the WorldLines List
            // destroy the line 
            Debug.Log("Line name: " +  l.name + " removed");
            temp.DestroyLine();
            return true;
        }
        Debug.Log("Invalid line removal");
        return false;
    }

    // function responsible for going to each line in the world and spawning traveling balls
    // per each update
    void GenerateBallOnEachLine() {
        foreach (Line l in WorldLines) {
            GameObject b = Instantiate(TravelBall);
            // instantiate the travel ball on l's p2
            b.transform.position = l.p1.transform.position;
            // we need to get the unit vector (direction)
            // so we need to calculate the line from p1 to p2
            Vector3 line = l.p2.localPosition - l.p1.localPosition;
            // normalize to get unit vector
            Vector3 direction = line.normalized;
            b.GetComponent<TravelingBall>().SetVelocity(direction);
            // set its values
            b.GetComponent<TravelingBall>().SetSpeed(WorldBallSpeed);
            b.GetComponent<TravelingBall>().SetLifespan(WorldBallLifespan);
            b.GetComponent<TravelingBall>().SetReferenceToLine(l);
            // set a reference to this line because we need it for calculation for Pon
            // remove shadows
            b.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
    }    
    
}
