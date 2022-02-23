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
