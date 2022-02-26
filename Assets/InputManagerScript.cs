using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputManagerScript : MonoBehaviour
{
    public InputField xInput;
    public InputField yInput;
    public InputField zInput;

    public Button addBox;
    public Button addComponent;

    public Button showUIButton;

    public GameObject UIToHide;

    public GameObject LineGO;

    public void getInputBox()
    {
        EventSystem.current.SetSelectedGameObject(null);
        bool success = Int32.TryParse(xInput.text, out int x);
        success &= Int32.TryParse(yInput.text, out int y);
        success &= Int32.TryParse(zInput.text, out int z);
        if (success)
        {
            Debug.Log(x);
            Debug.Log(y);
            Debug.Log(z);
        }
        xInput.text = "";
        yInput.text = "";
        zInput.text = "";
    }


    //variable to hold the current "base" on which a new compartment has to be built
    private int yBase = 0;
    public void getInputCompartment()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Debug.Log("HAAAA!");
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
            Debug.Log("HA!");
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
    private void XtoY(string text)
    {
        yInput.Select();
        yInput.ActivateInputField();
    }

    private void YtoZ(string text)
    {
        zInput.Select();
        zInput.ActivateInputField();
    }

    // Start is called before the first frame update
    void Start()
    {
        xInput.onEndEdit.AddListener(XtoY);
        yInput.onEndEdit.AddListener(YtoZ);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
