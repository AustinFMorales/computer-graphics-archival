using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneNodeControl : MonoBehaviour
{
    // Definition of SceneNodeControl is provided by Professor Sung through his CSS451 repo
    // SceneNodeControl is responsible for having the ability to control SceneNodes and access their children

    public Dropdown TheMenu = null;
    
    public SceneNode TheRoot = null;

    public XformControl XformControl = null;

    // used to give an illusion of hierarchy through dropdown
    const string kChildSpace = "  ";
    
    // list of options - scenenodes to choose from
    List<Dropdown.OptionData> mSelectMenuOptions = new List<Dropdown.OptionData>();
    // grab each transform
    List<Transform> mSelectedTransform = new List<Transform>();


    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(TheMenu != null);
        Debug.Assert(TheRoot != null);
        Debug.Assert(XformControl != null);

        mSelectMenuOptions.Add(new Dropdown.OptionData(TheRoot.transform.name));
        mSelectedTransform.Add(TheRoot.transform);
        GetChildrenNames("", TheRoot.transform);
        TheMenu.AddOptions(mSelectMenuOptions);
        TheMenu.onValueChanged.AddListener(SelectionChange);

        XformControl.SetSelectedObject(TheRoot.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // get the children names so they can be displayed in our hierarchy through mSelectMenuOptions
    void GetChildrenNames(string blanks, Transform node)
    {
        string space = blanks + kChildSpace;
        for (int i = node.childCount - 1; i >= 0; i--)
        {
            Transform child = node.GetChild(i);
            SceneNode cn = child.GetComponent<SceneNode>();
            if (cn != null)
            {
                mSelectMenuOptions.Add(new Dropdown.OptionData(space + child.name));
                mSelectedTransform.Add(child);
                GetChildrenNames(blanks + kChildSpace, child);
            }
        }
    }

    // choose the scenenode to adjust transform with.
    void SelectionChange(int index)
    {
        XformControl.SetSelectedObject(mSelectedTransform[index]);
    }

}
