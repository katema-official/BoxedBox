using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compartment
{
    //a class to represent a compartment of a closet.
    public int xWidth;
    public int yWidth;
    public int zWidth;
    public int floorHeight;

    public Compartment(int x, int y, int z, int floorHeight)
    {
        this.xWidth = x;
        this.yWidth = y;
        this.zWidth = z;
        this.floorHeight = floorHeight;
    }

}
