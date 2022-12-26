using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWorld : MonoBehaviour
{
    // Start is called before the first frame update
    // to save ourselves time, we can run this game and edit at the same time
    // [ExecuteInEditMode]

    // note: all the code provided for the hierarchy is derived from sung's examples.
    public SceneNode TheRoot;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // make a matrix 4x4
        Matrix4x4 i = Matrix4x4.identity;
        TheRoot.CompositeXform(ref i);
    }
}
