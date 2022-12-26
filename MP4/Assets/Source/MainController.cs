using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    // code is provided by sung's repo week 5 example 2
    // reference to all UI elements in the Canvas
    public Camera MainCamera = null;
    public TheWorld TheWorld = null;
    public SceneNodeControl NodeControl = null;
    
    public Button ResetButton = null;
    void Awake()
    {
        Debug.Assert(NodeControl != null);
        NodeControl.TheRoot = TheWorld.TheRoot;
    }


    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(MainCamera != null);
        Debug.Assert(TheWorld != null);
        Debug.Assert(ResetButton != null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // reset the scene by following this
    // https://answers.unity.com/questions/1261937/creating-a-restart-button.html
    // bind to resetbutton on value changed.
    public void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads our current scene, reloads it.
    }
}
