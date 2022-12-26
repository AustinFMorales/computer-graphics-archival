using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainController : MonoBehaviour
{
    
    // bring XFormControl into MainController
    public XformControl XformControlInstance;
    // bring DropdownBehavior into MainController
    public DropdownBehavior DropdownBehaviorInstance;

    // because mouse select is in the main controller
    // assign the materials for its selection
    public Material selected;
    // but we need to get the material back, so keep previous value
    private Material previousMaterial = null;
    // keep previous reference of the previous game object
    private GameObject previousGameObject = null;

    // MainController is the Controller for MP2
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(selected != null);
        Debug.Assert(XformControlInstance != null);
        Debug.Assert(DropdownBehaviorInstance != null);
    }

    // Update is called once per frame
    void Update()
    {
        MouseSelect();
    }

    // lets do mouse select
    void MouseSelect() {
        // code is reused from MP1, need to redo it to allow for transparency selection

        if (Input.GetMouseButtonDown(0)) {
            // https://docs.unity3d.com/2018.2/Documentation/ScriptReference/EventSystems.EventSystem.IsPointerOverGameObject.html
            // prevent mouse selection affecting UI
            if (EventSystem.current.IsPointerOverGameObject()) {
                Debug.Log("Pointer is on UI");
                return;
            }
            GameObject g = null;
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo); 
            if (hit) {
                Debug.Log("Hit GameObject: " + hitInfo.transform.gameObject.name);
                if (hitInfo.transform.gameObject.name != "StaticPlane") {
                    // add some code here that actually changes transparency of selected object
                    // also add in dropdown menu 
                    // how can we exactly keep track of the previous object's material
                    // do we get the renderer of the gameobject?
                    g = hitInfo.transform.gameObject;
                    // fix small issue with marking objects
                    // where it leaves it stuck when double clicking?
                    if (g != null && previousGameObject != null && g == previousGameObject) {
                        return;
                    }
                    // restore the materials back to previous game object with this if statement
                    if (g != previousGameObject && previousGameObject != null && previousMaterial != null) {
                        previousGameObject.GetComponent<Renderer>().material = previousMaterial;
                    }
                    // assign previous gameObject to current one to be saved for later
                    previousGameObject = g;
                    previousMaterial = g.GetComponent<Renderer>().material;
                    g.GetComponent<Renderer>().material = selected;
                    XformControlInstance.SelectObj(g);
                    XformControlInstance.UpdateSliderValues();
                    DropdownBehaviorInstance.SelectObj(g);
                    
                } else {
                    // restore materials when clicked in staticplane
                    if (previousGameObject != null) {
                        previousGameObject.GetComponent<Renderer>().material = previousMaterial;
                        previousGameObject = null;
                        previousMaterial = null;
                    }
                    // this feels extremely redundant
                    XformControlInstance.SelectObj(g);
                    XformControlInstance.UpdateSliderValues();
                    DropdownBehaviorInstance.SelectObj(g);
                }
            } else {
                // redundant statement here? - required for if we click nowhere
                if (previousGameObject != null) {
                    previousGameObject.GetComponent<Renderer>().material = previousMaterial;
                    previousGameObject = null;
                    previousMaterial = null;
                }
                XformControlInstance.SelectObj(g);
                XformControlInstance.UpdateSliderValues();
                DropdownBehaviorInstance.SelectObj(g);
            }
        }
        
    }
}
