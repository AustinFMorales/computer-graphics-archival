using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderControl : MonoBehaviour
{
    // start small, define the behaviors (model) and view first 
    // before attaching everything into the main controller

    // code is mostly from Sung's 451 repo for defining this SliderControl class
    // text mesh pro text is defined by a different type name
    // https://forum.unity.com/threads/how-to-create-object-reference-to-text-mesh-pro-object.505796/
    public Slider MySlider = null;
    public TextMeshProUGUI Echo = null;
    public TextMeshProUGUI Label = null;

    public delegate void SliderControlDelegate (float v);
    private SliderControlDelegate mCallBack = null;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(MySlider != null);
        Debug.Assert(Echo != null);
        Debug.Assert(Label != null);
        MySlider.onValueChanged.AddListener(SliderValueChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // use this as a reference function pointer
    // to callback
    // this is practically similar to a listener function, but with delegates
    public void SetSliderListener(SliderControlDelegate listener) {
        mCallBack = listener;
    }

    // changes slider value displayed on GUI
    void SliderValueChange(float v) {
        Echo.text = v.ToString("0.0000");
        if (mCallBack != null) {
            mCallBack(v);
        }
    }

    // getters and setters for our SliderControl class
    // actual values
    public float GetSliderValue() {
        return MySlider.value;
    }
    
    public void SetSliderLabel(string l) {
        Label.text = l;
    }
    
    public void SetSliderValue(float v) {
        MySlider.value = v;
        //MySlider.SetValueWithoutNotify(v);
        SliderValueChange(v);
    }

    public void InitSliderRange(float min, float max, float v) {
        MySlider.minValue = min;
        MySlider.maxValue = max;
        SetSliderValue(v);
    }
    
}
