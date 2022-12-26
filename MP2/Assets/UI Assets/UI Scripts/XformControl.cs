using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


// controls the panel 
public class XformControl : MonoBehaviour
{
    // One thing to notice is when you assign this to the editor
    // the GameObjects with the component is available in the hierarchy
    public ToggleBehavior ToggleBehaviorInstance;
    public SliderControl XControl;
    public SliderControl YControl;
    public SliderControl ZControl;
    public TextMeshProUGUI SelectedLabel;

    // game object we are currently selecting
    private GameObject Selected = null;
    // keeps track of the UI's state
    private char stateVal;
    
    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(ToggleBehaviorInstance != null);
        // XControl.SetSliderListener(NewXValue);
        stateVal = GetUIState();
        XControl.SetSliderListener(NewXValue);
        YControl.SetSliderListener(NewYValue);
        ZControl.SetSliderListener(NewZValue);
        // xformcontrol always starts at transform
        // XControl.InitSliderRange(-10f, 10f, Selected.transform.localPosition.x);
        // YControl.InitSliderRange(-10f, 10f, Selected.transform.localPosition.y);
        // ZControl.InitSliderRange(-10f, 10f, Selected.transform.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {   
        UpdateUIState();
        if (Selected != null) {  
            // UpdateSliderValues();
            SelectedLabel.text = Selected.name;
            // XControl, YControl, and ZControl already have listeners
            // NewXValue(XControl.GetSliderValue());
            // NewYValue(YControl.GetSliderValue());
            // NewZValue(ZControl.GetSliderValue());
            
        } else {
            SelectedLabel.text = "Null";
        }
        
        
    }

    public void SelectObj(GameObject g) {
        Selected = g;
        // UpdateUI();
    }

    // Get's the current state of the UI
    // so we can initialize the values properly
    // translation between -10 to + 10
    // rotation between -180 to 180
    // scale between 1 to 5
    public char GetUIState() {
        // something to keep in note:
        // keep proper update binding
        // have this placeholder for the mean time before we implement mouse select
        if (Selected != null) {
            if (ToggleBehaviorInstance.GetCurrent().name.Equals("TransformToggle")) {
                return 'T';
            } else if (ToggleBehaviorInstance.GetCurrent().name.Equals("ScaleToggle")) {
                return 'S';
            } else if (ToggleBehaviorInstance.GetCurrent().name.Equals("RotationToggle")) {
                return 'R';
            }
        }
        // terminal value for failed
        return '#';
    }
    // now this is where sung's delegates come in handy
    // the GUI now sets the object transform properties
    public void NewXValue(float nx) {
        if (Selected != null) {
            if (stateVal.Equals('T')) {
                Vector3 current = Selected.transform.localPosition;
                current.x = nx;
                Selected.transform.localPosition = current;
            } else if (stateVal.Equals('S')) {
                Vector3 current = Selected.transform.localScale;
                current.x = nx;
                Selected.transform.localScale = current;
            } else if (stateVal.Equals('R')) {
                Vector3 current = Selected.transform.localRotation.eulerAngles;
                current.x = AngleFix(nx);
                // set a rotation through mkaking a new quaternion first
                // then assign the new quarternion to current
                Quaternion q = new Quaternion();
                q.eulerAngles = current;
                // reassign the new rotation vector to Selected
                Selected.transform.localRotation = q;
            }
        }
    }

     public void NewYValue(float ny) {
        if (Selected != null) {
            if (stateVal.Equals('T')) {
                Vector3 current = Selected.transform.localPosition;
                current.y = ny;
                Selected.transform.localPosition = current;
            } else if (stateVal.Equals('S')) {
                Vector3 current = Selected.transform.localScale;
                current.y = ny;
                Selected.transform.localScale = current;
            } else if (stateVal.Equals('R')) {
                Vector3 current = Selected.transform.localRotation.eulerAngles;
                current.y = AngleFix(ny);
                // set a rotation through mkaking a new quaternion first
                // then assign the new quarternion to current
                Quaternion q = new Quaternion();
                q.eulerAngles = current;
                // reassign the new rotation vector to Selected
                Selected.transform.localRotation = q;
            }
        }
    }

    public void NewZValue(float nz) {
        if (Selected != null) {
            if (stateVal.Equals('T')) {
                Vector3 current = Selected.transform.localPosition;
                current.z = nz;
                Selected.transform.localPosition = current;
            } else if (stateVal.Equals('S')) {
                Vector3 current = Selected.transform.localScale;
                current.z = nz;
                Selected.transform.localScale = current;
            } else if (stateVal.Equals('R')) {
                Vector3 current = Selected.transform.localRotation.eulerAngles;
                current.z = AngleFix(nz);
                // set a rotation through mkaking a new quaternion first
                // then assign the new quarternion to current
                Quaternion q = new Quaternion();
                q.eulerAngles = current;
                // reassign the new rotation vector to Selected
                Selected.transform.localRotation = q;
            }
        }
    }

    
    // we need the slider values to update to accurately reflect the gameobject's 
    // current coordinates
    // when we're swapping around gameobjects but the state stays the same
    // only use this for maincontroller on click
    public void UpdateSliderValues() {
        // redundant code, but this is the best we can do
        // don't forget this as well
        if (Selected == null) {
            XControl.SetSliderValue(0f);
            YControl.SetSliderValue(0f);
            ZControl.SetSliderValue(0f);
            return;
        }
        if (stateVal.Equals('T')) {
            Vector3 SelectedTransform = Selected.transform.localPosition;
            XControl.SetSliderValue(SelectedTransform.x);
            YControl.SetSliderValue(SelectedTransform.y);
            ZControl.SetSliderValue(SelectedTransform.z);
        } else if (stateVal.Equals('S')) {
            Vector3 SelectedScale = Selected.transform.localScale;
            XControl.SetSliderValue(SelectedScale.x); 
            YControl.SetSliderValue(SelectedScale.y);
            ZControl.SetSliderValue(SelectedScale.z);
        } else if (stateVal.Equals('R')) {
            // rotation bugs out here - the slider fights with me here
            // doesn't like accepting negative values
            // use method AngleFix?
            Vector3 SelectedRotation = Selected.transform.localRotation.eulerAngles;
            XControl.SetSliderValue(AngleFix(SelectedRotation.x)); 
            YControl.SetSliderValue(AngleFix(SelectedRotation.y));
            ZControl.SetSliderValue(AngleFix(SelectedRotation.z));
        }
    }
    
    // when slider goes into negative, it keeps clamping towards positive
    // https://answers.unity.com/questions/554743/how-to-calculate-transformlocaleuleranglesx-as-neg.html
    float AngleFix(float v) {
        float angle = v;
        angle = (angle > 180f) ? angle - 360f : angle;
        return angle;
    }


    public void UpdateUIState() {
        // something to keep in note:
        // keep proper update binding
        // have this placeholder for the mean time before we implement mouse select
        char prevStateVal = stateVal;
        stateVal = GetUIState();
        if (prevStateVal.Equals(stateVal)) {
            // will this work?
            // UpdateSliderValues();
            return;
        } else {
            Debug.Log("State changed");
            if (Selected != null) {
                // get the vectors
                if (stateVal.Equals('T')) {
                    // XControl.GetSliderValue is a placeholder, use the GameObject's current Position
                    // XControl.SetSliderValue(SelectedTransform.x);
                    
                    Vector3 SelectedTransform = Selected.transform.localPosition;
                    XControl.InitSliderRange(-10f, 10f, SelectedTransform.x); 
                    YControl.InitSliderRange(-10f, 10f, SelectedTransform.y);
                    ZControl.InitSliderRange(-10f, 10f, SelectedTransform.z);
                    // UpdateSliderValues();
                    
                } else if (stateVal.Equals('S')) {
                    // XControl.GetSliderValue is a placeholder, use the GameObject's current Position
                    // XControl.SetSliderValue(SelectedScale.x);
                    
                    Vector3 SelectedScale = Selected.transform.localScale;
                    XControl.InitSliderRange(1f, 5f, SelectedScale.x); 
                    YControl.InitSliderRange(1f, 5f, SelectedScale.y);
                    ZControl.InitSliderRange(1f, 5f, SelectedScale.z);
                    // UpdateSliderValues();
                } else if (stateVal.Equals('R')) {
                    // XControl.GetSliderValue is a placeholder, use the GameObject's current Position
                    // XControl.SetSliderValue(SelectedRotation.x);
                    
                    Vector3 SelectedRotation = Selected.transform.localRotation.eulerAngles;
                    XControl.InitSliderRange(-180f, 180f, AngleFix(SelectedRotation.x)); 
                    YControl.InitSliderRange(-180f, 180f, AngleFix(SelectedRotation.y));
                    ZControl.InitSliderRange(-180f, 180f, AngleFix(SelectedRotation.z));
                    // UpdateSliderValues();
                }
            }
        }
        
    }


    
    
    
    


}
