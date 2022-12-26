using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtControl : MonoBehaviour
{
    // we need to control the lookatposition with three slider bars
    // so we have the ability to either control the camera purely on sliders
    // or with mouse control

    // let's take some elements of XformControl, and assign it here.

    public SliderWithEcho X, Y, Z;

    public Transform mSelected;

    private Vector3 mPreviousSliderValues = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(mSelected != null);
        X.SetSliderListener(XValueChanged);
        Y.SetSliderListener(YValueChanged);
        Z.SetSliderListener(ZValueChanged);
        X.InitSliderRange(-20, 20, mSelected.localPosition.x);
        Y.InitSliderRange(-20, 20, mSelected.localPosition.y);
        Z.InitSliderRange(-20, 20, mSelected.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // strip down most of the elements of XformControl, and prioritize on the sliders for LookAtControl.
    // no rotation will happen to LookAtTarget, so don't worry about quaternions.
    void XValueChanged(float v)
    {
        Vector3 p = ReadObjectXfrom();
        p.x = v;
        UISetObjectXform(ref p);
    }
    
    void YValueChanged(float v)
    {
        Vector3 p = ReadObjectXfrom();
        p.y = v;        
        UISetObjectXform(ref p);
    }

    void ZValueChanged(float v)
    {
        Vector3 p = ReadObjectXfrom();
        p.z = v;
        UISetObjectXform(ref p);
    }
    
    // let the UI set the object's transform.
    private void UISetObjectXform(ref Vector3 p) {
        if (mSelected == null) {
            return;
        }
        mSelected.transform.localPosition = p;
    }
    
    
    // have a convenient function read the object's transform and return it.
    private Vector3 ReadObjectXfrom() {
        Vector3 p;

        if (mSelected != null) {
            p = mSelected.localPosition;
        } else {
            p = Vector3.zero;
        }
        return p;
    }
}
