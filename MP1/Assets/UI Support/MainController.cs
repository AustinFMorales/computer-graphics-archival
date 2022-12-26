using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    // To assign options to our dropdown menu, refer to this
    // https://docs.unity3d.com/2018.4/Documentation/ScriptReference/UI.Dropdown.AddOptions.html
    // This only works with Dropdown Legacy
    private List<string> mcDropOptions = new List<string> {"Object to Create", "Cube", 
    "Sphere", "Cylinder"};
    public Dropdown mcDropdown;
    // need to assign this to the MainController in the editor
    public GameObject creationSphere;


    // need this to actually test out cube instantiation in the controller
    // then you bind this to on value changed with this function
    // https://youtu.be/Q4NYCSIOamY
    // editor doesn't do anything, you have to do the binding manually here through script
    public void Dropdown_IndexChanged(int index) {
        if (index == 1) {
            InstantiateCube();
        } else if (index == 2) {
            InstantiateSphere();
        } else if (index == 3) {
            InstantiateCylinder();
        }
        // reset it back to zero
        if (mcDropdown != null) {
            mcDropdown.value = 0;
        }
    }

    // mouse click on object
    // very expensive raycasting on each framerate update
    // but this is all you can do atm
    // https://answers.unity.com/questions/411793/selecting-a-game-object-with-a-mouse-click-on-it.html
    // reference from sung's repo
    void mouseClick() {
        if (Input.GetMouseButtonDown(0)) {
            // Debug.Log("Mouse is down");

            // struct in Unity that contains information of a raycast
            RaycastHit hitInfo = new RaycastHit();
            // shoots a ray from the main camera when executed
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit) {
                Debug.Log("Hit GameObject: " + hitInfo.transform.gameObject.name);
                // NOTE: CreationTarget (in this case creationSphere) ignores raycast hits
                // due to its layer ignoring raycasts
                if (hitInfo.transform.gameObject.name == "CreationPlane") {
                    // move the CreationTarget in the same area 
                    if (creationSphere != null) {
                        Debug.Log("Moving Creation Sphere");
                        // https://answers.unity.com/questions/773911/move-object-to-mouse-click-position.html
                        // referred to this link using RaycastHit.point to get hitInfo's x and z values
                        // for it to move with cursor click
                        Vector3 creationSphereVector = creationSphere.transform.position;
                        creationSphereVector = new Vector3(hitInfo.point.x, 0.250f, hitInfo.point.z);
                        creationSphere.transform.position = creationSphereVector;
                    }
                } else {
                    GameObject temp = hitInfo.transform.gameObject;
                    DestroyObject(temp);
                }  
            } else {
                Debug.Log("No hit");
            }
            // Debug.Log("Mouse is down");
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // grab the Dropdown GameObject which is the component is attached to
        mcDropdown = transform.GetComponent<Dropdown>();
        if (mcDropdown != null) {
            // just in case, clear the old options of the Dropdown menu
            mcDropdown.ClearOptions();
            // add our options to the dropdown menu
            mcDropdown.AddOptions(mcDropOptions);
            // we can do the bindings to the editor later
        }
    }

    // Update is called once per frame
    void Update()
    {
        mouseClick();
    }

    // Instantiation taken from Sung's Repo
    // Last thing to do:
    /*
        In all cases, a newly created shape should be resting on the CreationPlane and attempt to travel towards the position direction. 
        If the creation position is larger than the valid range of 5, 
        the created shape will then travel in the negative direction. 
        After a shape’s first change of traveling direction, it should never cross the 0 to 5 range.
    */
    void InstantiateCube() {
        Debug.Log("Hello1");
        // Instantiation requires all prefabs to be under Resources
        GameObject cube = Instantiate(Resources.Load("Cube")) as GameObject;
        cube.transform.position = new Vector3(creationSphere.transform.position.x, creationSphere.transform.position.y, creationSphere.transform.position.z);
    }

    void InstantiateSphere() {
        Debug.Log("Hello2");
        GameObject sphere = Instantiate(Resources.Load("Sphere")) as GameObject;
        sphere.transform.position = new Vector3(creationSphere.transform.position.x, creationSphere.transform.position.y, creationSphere.transform.position.z);
    }

    void InstantiateCylinder() {
        Debug.Log("Hello3");
        GameObject cylinder = Instantiate(Resources.Load("Cylinder")) as GameObject;
        cylinder.transform.position = new Vector3(creationSphere.transform.position.x, creationSphere.transform.position.y, creationSphere.transform.position.z);
    }

    // https://docs.unity3d.com/ScriptReference/Object.Destroy.html
    // taken from this
    void DestroyObject(GameObject obj) {
        obj.SetActive(false);
        Destroy(obj);
    }
    
}
