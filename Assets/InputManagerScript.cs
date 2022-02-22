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

    public void getInput()
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
