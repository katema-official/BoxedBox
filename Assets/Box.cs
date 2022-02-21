using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box
{
    //class to represent a box to put in a compartment of the closet.
    //each box has a 3 width, one for each axis, specified in millimeters
    public int xWidth;
    public int yWidth;
    public int zWidth;
    public string name;

    public Box(int x, int y, int z, string name)
    {
        this.xWidth = x;
        this.yWidth = y;
        this.zWidth = z;
        this.name = name;
    }

}
