using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InputManagerScript : MonoBehaviour
{
    public InputField nameInputField;
    public InputField xInput;
    public InputField yInput;
    public InputField zInput;

    public Button addBox;
    public Button addComponent;

    public Button showUIButton;

    public GameObject boxGO;

    public GameObject UIToHide;

    public GameObject LineGO;

    public GameObject textGO;


    //lists to conserve the boxes and the text on top of them
    public LinkedList<Box> boxList = new LinkedList<Box>();

    //list to conserve the x,y and z values of all compartments
    public LinkedList<Compartment> compartmentList = new LinkedList<Compartment>();

    private int zSpawn = 0;
    public void getInputBox()
    {
        EventSystem.current.SetSelectedGameObject(null);
        bool success = Int32.TryParse(xInput.text, out int x);
        success &= Int32.TryParse(yInput.text, out int y);
        success &= Int32.TryParse(zInput.text, out int z);
        if (success)
        {
            zSpawn += z / 2;
            GameObject go = createBoxGameObject(x, y, z, 300, -150, zSpawn, nameInputField.text);
            zSpawn += z / 2;

            //now add the data into the lists
            Box b = new Box(x, y, z, nameInputField.text, go);
            boxList.AddLast(b);

        }
        nameInputField.text = "";
        xInput.text = "";
        yInput.text = "";
        zInput.text = "";
    }

    public GameObject createBoxGameObject(int xWidth, int yWidth, int zWidth, int xSpawn, int ySpawn, int zSpawn, string text)
    {
        //creation of the gameobject
        GameObject go = Instantiate(boxGO);

        go.transform.localScale = new Vector3(xWidth, yWidth, zWidth);
        go.transform.localPosition = new Vector3(xSpawn, ySpawn, zSpawn);
        
        //creation of the 3D text with its name
        createTextOnAllFaces(go, text, xWidth, yWidth, zWidth);

        return go;
    }

    private void createTextOnAllFaces(GameObject cube, string text, int x, int y, int z)
    {
        int textLen = text.Length;

        GameObject go;
        TextMeshPro t;
        for (int i = 0; i < 6; i++)
        {
            go = cube.transform.GetChild(i).gameObject;
            t = go.GetComponent<TextMeshPro>();
            t.text = text;
        }

        /*
        //on top
        GameObject go1 = cube.transform.GetChild(4).gameObject;
        TextMeshPro t = go1.GetComponent<TextMeshPro>();
        t.text = text;
        //t.fontSize = (13 * cube.transform.localScale.x) / textLen;
        RectTransform rt = go1.GetComponent<RectTransform>();
        //rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x);
        //rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, z);
        
        go1.transform.localPosition = new Vector3(cube.transform.position.x,
            cube.transform.position.y + cube.transform.localScale.y / 2 + 0.1f,
            cube.transform.position.z);
        go1.transform.Rotate(90f, 0, 0);
        

        //on the bottom
        go1 = cube.transform.GetChild(5).gameObject;
        t = go1.GetComponent<TextMeshPro>();
        t.text = text;
        //t.fontSize = (13 * cube.transform.localScale.x) / textLen;
        //rt = go1.GetComponent<RectTransform>();
        //rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x);
        //rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, z);
        
        go1.transform.localPosition = new Vector3(cube.transform.position.x,
            cube.transform.position.y - cube.transform.localScale.y / 2 - 0.1f,
            cube.transform.position.z);
        go1.transform.Rotate(-90f, 0, 0);
        


        //on the left
        go1 = cube.transform.GetChild(2).gameObject;
        t = go1.GetComponent<TextMeshPro>();
        t.text = text;

        //on the right
        go1 = cube.transform.GetChild(3).gameObject;
        t = go1.GetComponent<TextMeshPro>();
        t.text = text;

        //on the front
        go1 = cube.transform.GetChild(0).gameObject;
        t = go1.GetComponent<TextMeshPro>();
        t.text = text;

        //behind
        go1 = cube.transform.GetChild(1).gameObject;
        t = go1.GetComponent<TextMeshPro>();
        t.text = text;
        */

        go = cube.transform.GetChild(6).gameObject;
        t = go.GetComponent<TextMeshPro>();
        t.text = x.ToString();

        go = cube.transform.GetChild(7).gameObject;
        t = go.GetComponent<TextMeshPro>();
        t.text = y.ToString();

        go = cube.transform.GetChild(8).gameObject;
        t = go.GetComponent<TextMeshPro>();
        t.text = z.ToString();
    }


    //variable to hold the current "base" on which a new compartment has to be built
    private int yBase = 0;
    public void getInputCompartment()
    {
        EventSystem.current.SetSelectedGameObject(null);
        bool success = Int32.TryParse(xInput.text, out int x);
        success &= Int32.TryParse(yInput.text, out int y);
        success &= Int32.TryParse(zInput.text, out int z);


        if (success)
        {
            GameObject[] lines = new GameObject[12];
            GameObject[] tmp = generatePlaneOnHeight(yBase + 0, x, z);
            for(int i = 0; i < 4; i++)
            {
                lines[i] = tmp[i];
            }
            tmp = generatePlaneOnHeight(yBase + y, x, z);
            for (int i = 0; i < 4; i++)
            {
                lines[i+4] = tmp[i];
            }

            lines[8] = Instantiate(LineGO);
            LineRenderer lr = lines[8].GetComponent<LineRenderer>();
            lr.SetPosition(0, new Vector3(0, yBase + 0, 0));
            lr.SetPosition(1, new Vector3(0, yBase + y, 0));

            lines[9] = Instantiate(LineGO);
            lr = lines[9].GetComponent<LineRenderer>();
            lr.SetPosition(0, new Vector3(x, yBase + 0, 0));
            lr.SetPosition(1, new Vector3(x, yBase + y, 0));

            lines[10] = Instantiate(LineGO);
            lr = lines[10].GetComponent<LineRenderer>();
            lr.SetPosition(0, new Vector3(0, yBase + 0, z));
            lr.SetPosition(1, new Vector3(0, yBase + y, z));

            lines[11] = Instantiate(LineGO);
            lr = lines[11].GetComponent<LineRenderer>();
            lr.SetPosition(0, new Vector3(x, yBase + 0, z));
            lr.SetPosition(1, new Vector3(x, yBase + y, z));

            Compartment c = new Compartment(x, y, z, yBase);
            compartmentList.AddLast(c);

            yBase += y;
        }
        xInput.text = "";
        yInput.text = "";
        zInput.text = "";
    }

    private GameObject[] generatePlaneOnHeight(int y, int x, int z)
    {
        GameObject[] lines = new GameObject[4];
        lines[0] = Instantiate(LineGO);
        LineRenderer lr = lines[0].GetComponent<LineRenderer>();
        lr.SetPosition(0, new Vector3(0, y, 0));
        lr.SetPosition(1, new Vector3(x, y, 0));

        lines[1] = Instantiate(LineGO);
        lr = lines[1].GetComponent<LineRenderer>();
        lr.SetPosition(0, new Vector3(0, y, 0));
        lr.SetPosition(1, new Vector3(0, y, z));

        lines[2] = Instantiate(LineGO);
        lr = lines[2].GetComponent<LineRenderer>();
        lr.SetPosition(0, new Vector3(x, y, z));
        lr.SetPosition(1, new Vector3(x, y, 0));

        lines[3] = Instantiate(LineGO);
        lr = lines[3].GetComponent<LineRenderer>();
        lr.SetPosition(0, new Vector3(x, y, z));
        lr.SetPosition(1, new Vector3(0, y, z));

        return lines;
    }

    public void setUI()
    {
        Animator anim = UIToHide.GetComponent<Animator>();
        if (anim != null)
        {
            bool hide = anim.GetBool("Hide");
            anim.SetBool("Hide", !hide);
        }
        if (showUIButton.GetComponentInChildren<Text>().text == "Nascondi interfaccia")
        {
            showUIButton.GetComponentInChildren<Text>().text = "Mostra interfaccia";
        }
        else
        {
            showUIButton.GetComponentInChildren<Text>().text = "Nascondi interfaccia";
        }
    }




    //functions to go from xInputField to yInputField and from y to z
    private void NametoX(string text)
    {
        xInput.Select();
    }

    private void XtoY(string text)
    {
        yInput.Select();      
    }

    private void YtoZ(string text)
    {
        zInput.Select();        
    }

    // Start is called before the first frame update
    void Start()
    {
        nameInputField.onEndEdit.AddListener(NametoX);
        xInput.onEndEdit.AddListener(XtoY);
        yInput.onEndEdit.AddListener(YtoZ);
    }


    public bool isUIFocused = false;
    // Update is called once per frame
    void Update()
    {
        //when the camera is moving no UI and viceversa


        if(!nameInputField.isFocused && !xInput.isFocused && !yInput.isFocused && !zInput.isFocused)
        {
            isUIFocused = false;
        }
        else
        {
            isUIFocused = true;
        }
    }
}
