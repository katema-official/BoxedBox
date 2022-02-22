using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Closet
{
    //a closet is just a stack of compartments
    public Compartment[] compartments;

    public Closet(Compartment[] c)
    {
        compartments = c;
    }
}
