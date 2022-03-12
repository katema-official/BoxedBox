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

    //associated gameobject
    public GameObject go;

    //effective 3D point in which the lower-left-behind point of the box is located
    //(when the box is placed inside a compartment)
    public int xPoint;
    public int yPoint;
    public int zPoint;

    public Box(int x, int y, int z, string name, GameObject go)
    {
        this.xWidth = x;
        this.yWidth = y;
        this.zWidth = z;
        this.name = name;
        this.go = go;

        //initially, the box isn't positioned
        xPoint = -1;
        yPoint = -1;
        zPoint = -1;
    }

}
