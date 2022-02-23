using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

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

    public void getInputCompartment()
    {
        bool success = Int32.TryParse(xInput.text, out int x);
        success &= Int32.TryParse(yInput.text, out int y);
        success &= Int32.TryParse(zInput.text, out int z);

        if (success)
        {
            GameObject[] lines = new GameObject[12];
            GameObject[] tmp = generatePlaneOnHeight(0, x, z);
            for(int i = 0; i < 4; i++)
            {
                lines[i] = tmp[i];
            }
            tmp = generatePlaneOnHeight(y, x, z);
            for (int i = 0; i < 4; i++)
            {
                lines[i+4] = tmp[i];
            }

            lines[8] = Instantiate(LineGO);
            LineRenderer lr = lines[8].GetComponent<LineRenderer>();
            lr.SetPosition(0, new Vector3(0, 0, 0));
            lr.SetPosition(1, new Vector3(0, y, 0));

            lines[9] = Instantiate(LineGO);
            lr = lines[9].GetComponent<LineRenderer>();
            lr.SetPosition(0, new Vector3(x, 0, 0));
            lr.SetPosition(1, new Vector3(x, y, 0));

            lines[10] = Instantiate(LineGO);
            lr = lines[10].GetComponent<LineRenderer>();
            lr.SetPosition(0, new Vector3(0, 0, z));
            lr.SetPosition(1, new Vector3(0, y, z));

            lines[11] = Instantiate(LineGO);
            lr = lines[11].GetComponent<LineRenderer>();
            lr.SetPosition(0, new Vector3(x, 0, z));
            lr.SetPosition(1, new Vector3(x, y, z));

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
