using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ToggleBehavior : MonoBehaviour
{
    /*
    public Toggle TransformToggle = null;
    public Toggle ScaleToggle = null;
    public Toggle RotationToggle = null;
    */
    public ToggleGroup ToggleGroupInstance;
    
    // Start is called before the first frame update
    void Start()
    {
        /*
        Debug.Assert(TransformToggle != null);
        Debug.Assert(ScaleToggle != null);
        Debug.Assert(RotationToggle != null);
        */
        Debug.Assert(ToggleGroupInstance != null);

    }
    
    /*
    void Update() {
        Debug.Log("Toggle Selected: " + GetCurrent().name);
    }
    */
    
    // Use a ToggleGroup to make the process of having the toggle buttons
    // be enabled one at a time
    // https://www.youtube.com/watch?v=6QJ789LOcu8
    // Then we can use it as a script
    // https://youtu.be/0b6KmdPcDQU
    public void SelectToggle(int id) {
        var Toggles = ToggleGroupInstance.GetComponentsInChildren<Toggle>();
        Toggles[id].isOn = true;
        Debug.Log("Toggle Selected: " + Toggles[id].name);
    }
    
    // Get our current selection
    // https://stackoverflow.com/questions/52739763/how-to-get-selected-toggle-from-toggle-group
    public Toggle GetCurrent() {
        var Toggles = ToggleGroupInstance.GetComponentsInChildren<Toggle>();
        foreach (var t in Toggles) {
            if (t.isOn) {
                return t;
            }
        }
        return null;
    }
}
