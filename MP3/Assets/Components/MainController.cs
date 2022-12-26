using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    public Camera MainCamera = null;
    public TheWorld BoxWorld = null;
    public XformControl TransformController = null;
    public Material SelectedColor;
    public Material PreviousColor;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(MainCamera != null);
        Debug.Assert(BoxWorld != null);
        Debug.Assert(TransformController != null);
        Debug.Assert(SelectedColor != null);
        TransformController.SetSelectedObject(GameObject.Find("TheBarrier").transform);
    }

    // Update is called once per frame
    void Update()
    {
        ProcessMouseEvents();
    }

    // define mouse behavior here
    // RAYCASTING can only be used here, not anywhere else 
    // Most of the code for mouse control is taken by Sung
    void ProcessMouseEvents() {
        // NOTE: here are the defined layers
        /*
            WallLayer = 8
            EndPtLayer = 9
            LineLayer = 10
        */
        GameObject selected = null;
        Vector3 hitPoint;
        // if mouse click is on the endpoints
        if (Input.GetMouseButtonDown(0)) { // getmousebutton down only gets mouse click on first frame
            if (SelectObject(out selected, out hitPoint, LayerMask.GetMask("EndPtLayer"))) {
                // selected is assigned to the gameobject we just selected
                Debug.Log("EndPoint Selected");
                BoxWorld.SetSelected(ref selected);
            } else if (SelectObject(out selected, out hitPoint, LayerMask.GetMask("LineLayer"))) {
                // raycast check on line first because it seems to focus on the wall layer first
                // delete this gameobject's line, so delete the endpoint and line
                Debug.Log("Destroying Line");
                // BoxWorld.SetSelected(ref selected);
                BoxWorld.DeleteLine(ref selected);
            } else if (SelectObject(out selected, out hitPoint, LayerMask.GetMask("WallLayer"))) {
                // otherwise, this is on the walls and floor layer
                // check first if its left wall to generate endpoints
                if (selected.name.Equals("LeftWall")) {
                    Debug.Log("Generating Endpoint");
                    BoxWorld.InstantiateNewLine(ref hitPoint);
                } else {
                    Debug.Log("Wall Hit");
                }
                BoxWorld.ReleaseSelected();
            }  else {
                Debug.Log("Unknown Raycast Hit");
                BoxWorld.ReleaseSelected();
            }
        }
        if (Input.GetMouseButton(0)) { // getmousebutton returns true when ever its pressed down per each frame
            if (BoxWorld.GetSelected() != null && BoxWorld.GetSelected().layer == 9) {
                BoxWorld.GetSelected().GetComponent<Renderer>().material = SelectedColor;
                GameObject wall;
                // raycast to the wall/floor layer?
                SelectObject(out wall, out hitPoint, LayerMask.GetMask("WallLayer"));
                // Debug.Log("raycasting to the wall/floor layer");
                BoxWorld.SetSelectedPosition(ref hitPoint);
            } 
        } else {
            if (BoxWorld.GetSelected() != null) {
                BoxWorld.GetSelected().GetComponent<Renderer>().material = PreviousColor;
            }
        }
    }

    // pass the parameters by reference using the out keyword
    // out keyword actually directly modifies the parameter passed in
    // should we use layermasks?
    bool SelectObject(out GameObject self, out Vector3 p, int layerMask) {
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(MainCamera.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, layerMask);
        if (hit) {
            self = hitInfo.transform.gameObject;
            p = hitInfo.point;
        } else {
            self = null;
            p = Vector3.zero;
        }
        return hit;
    }
}
