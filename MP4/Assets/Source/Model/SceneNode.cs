using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneNode : MonoBehaviour
{
    // the definitions of SceneNode and NodePrimitive are from 
    // Professor Sung's repo - Week5 Ex1 - SceneNode
    protected Matrix4x4 mCombinedParentXform;
    
    // origin of this scenenode
    public Vector3 NodeOrigin = Vector3.zero;
    // the list of primitives that will be represented in this scenenode
    // they are the visual component of the scenenode
    public List<NodePrimitive> PrimitiveList;
    
    
    
    // Start is called before the first frame update
    protected void Start()
    {
        InitializeSceneNode();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeSceneNode() {
        mCombinedParentXform = Matrix4x4.identity;
    }

    // Call this before each draw!
    public void CompositeXform(ref Matrix4x4 parentXform) {
        Matrix4x4 orgT = Matrix4x4.Translate(NodeOrigin);
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        mCombinedParentXform = parentXform * orgT * trs;

        // propagate the transform to all children
        foreach (Transform child in transform) {
            SceneNode cn = child.GetComponent<SceneNode>();
            if (cn != null) {
                // have each child move with the parent
                cn.CompositeXform(ref mCombinedParentXform);
            }
        }

        // then disenminate to primitives - have the transform behavior reflected to the primitives through their shaders
        foreach (NodePrimitive p in PrimitiveList) {
            p.LoadShaderMatrix(ref mCombinedParentXform);
        }
    }

}
