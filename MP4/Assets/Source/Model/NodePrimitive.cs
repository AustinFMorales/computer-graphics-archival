using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePrimitive : MonoBehaviour
{
    // the definitions of SceneNode and NodePrimitive are from 
    // Professor Sung's repo - Week5 Ex1 - SceneNode
    
    // will this force a color or this is just a placeholder
    // for color to be changed by our NodeShader.shader?
    public Color MyColor = new Color(0.1f, 0.1f, 0.2f, 1.0f);
    public Vector3 Pivot;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // responsible for loading the respective TRS matrix and rotation matrix
    // based on our NodeShader implementation
    public void LoadShaderMatrix(ref Matrix4x4 nodeMatrix) {
        Matrix4x4 p = Matrix4x4.TRS(Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 invp = Matrix4x4.TRS(-Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        Matrix4x4 m = nodeMatrix * p * trs * invp; // multiply our very own matrix altogether
        // in previous examples, the shader we want to use has its matrix typically being set to MyXformMat
        // but because we are working with NodeShader.shader, it is now MyTRSMatrix
        GetComponent<Renderer>().material.SetMatrix("MyTRSMatrix", m);
        GetComponent<Renderer>().material.SetColor("MyColor", MyColor);
    }
}
