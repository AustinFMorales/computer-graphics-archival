using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropdownBehavior : MonoBehaviour
{
    // current version of unity uses TMP now for most UI elements
    // each 
    // https://answers.unity.com/questions/1556733/unable-to-assign-a-dropdown-variableunable-to-assi.html
    public TMPro.TMP_Dropdown DropdownInstance;
    public Material RootMaterial;
    public Material LeafMaterial;
    // selected game object 
    private GameObject Parent;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(DropdownInstance != null);
    }

    // reuse the code from MP1 for dropdown menu behavior
    // instantiate new primitives in the dropdown menu
    // HOWEVER - you must assign the correct parent-child relationship
    // refer to this for more
    // https://docs.unity3d.com/ScriptReference/Transform.SetParent.html - setting your parent
    // https://docs.unity3d.com/ScriptReference/Transform.GetSiblingIndex.html - grab sibling
    // https://docs.unity3d.com/ScriptReference/Transform-childCount.html - check children
    public void Dropdown_IndexChanged(int index) {
        // when instantiating a new object
        // the material chosen is dependent on factors
        /*
            If mSelected is null
                Newly created GameObject will be in black
            Else
            Newly created GameObject will:
            have mSelected as parent
            if mSelected has other children
                Have the same materials as the siblings sharing the same mSelected parent
            Else
                Be in White.
        Note: the above means, new root object will always be in black, and new leaf objects will always be in white.
        */
        GameObject g = null;
        
        // https://docs.unity3d.com/ScriptReference/GameObject.CreatePrimitive.html
        // primer on creating primitives
        if (index == 1) {
            // instantiate cube game object
            // then assign instantiated game object to parent
            Debug.Log("creating cube");
            g = GameObject.CreatePrimitive(PrimitiveType.Cube) as GameObject;
        } else if (index == 2) {
            // instantiate sphere game object
            // then assign instantiated game object to parent
            Debug.Log("creating sphere");
            g = GameObject.CreatePrimitive(PrimitiveType.Sphere) as GameObject;
        } else if (index == 3) {
            // instantiate cylinder game object
            // then assign instantiated game object to parent
            Debug.Log("creating cylinder");
            g = GameObject.CreatePrimitive(PrimitiveType.Cylinder) as GameObject;
        }

        if (Parent != null && g != null) {
            g.transform.SetParent(Parent.transform);
            if (Parent.transform.childCount > 0) {
                // if the parent has siblings, copy the sibling's material
                GameObject sibling = Parent.transform.GetChild(0).gameObject;
                Material siblingMaterial = sibling.GetComponent<Renderer>().material;
                g.GetComponent<Renderer>().material = siblingMaterial;
            } else {
                // otherwise, copy the leaf material
                g.GetComponent<Renderer>().material = LeafMaterial;
            }
            // prevent overlap
            g.transform.localPosition = new Vector3 (Parent.transform.localPosition.x + 0.1f, Parent.transform.localPosition.y + 0.1f, Parent.transform.localPosition.z + 0.1f);
        } else if (g != null) {
            // no parent - new primitive instance becomes root material
            g.GetComponent<Renderer>().material = RootMaterial;
            g.transform.localPosition = new Vector3 (1f, 1f, 1f);
        }

        // then reset the dropdown behavior value
        if (DropdownInstance != null) {
            DropdownInstance.value = 0;
        }   
    }

    // Gets reference to gameobject passed in 
    public void SelectObj(GameObject g) {
        Parent = g;
    }
    // getter for game object
    public GameObject GetObj() {
        return Parent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
