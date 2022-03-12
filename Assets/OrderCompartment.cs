using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OrderCompartment : MonoBehaviour
{
    //a class to order some compartments

    //JUST A COPY OF THE DATA PRESENT IN INPUTMANAGERSCRIPT
    //lists to conserve the boxes and the text on top of them
    private LinkedList<Box> boxList = new LinkedList<Box>();

    //list to conserve the x,y and z values of all compartments
    private LinkedList<Compartment> compartmentList = new LinkedList<Compartment>();
    private int yBase;

    private GameObject inputManager;
    private bool animate = true;
    
    //function used to find the best placement of boxes for a given compartment
    //NOTE: for our calculations, we will consider the lower-left-behind corner of
    //the cube as "center". This means that we will start from that point and check if
    //the currently considered box can fit in an area which has as lower-left-behind
    //point this one.
    public void findBestOrderingForCompartment(int compartmentNumber)
    {
        Compartment c = compartmentList.ElementAt(compartmentNumber);

        LinkedList<Box> resultForCompartment;
        LinkedList<Point> points = new LinkedList<Point>();
        points.AddLast(new Point(0, 0, 0));
        resultForCompartment = putBox(new LinkedList<Box>(), copyBoxList(boxList), c, points);



        //int[][] i = findDisplacement(compartmentNumber, compartment, new int[boxList.Count][], new LinkedList<GameObject>(boxList));
    }


    private LinkedList<Box> putBox(LinkedList<Box> placed, LinkedList<Box> toPlace, Compartment compartment, LinkedList<Point> availablePoints)
    {


        foreach(Point p in availablePoints)
        {
            foreach(Box initialBox in toPlace)
            {
                foreach(Box b in rotatedBox(initialBox))
                {

                }
            }
        }






        //TODO: delete this
        return null;
    }




    private Box[] rotatedBox(Box b)
    {
        Box[] result = new Box[3];
        result[0] = b;

        GameObject bbGO = Instantiate(b.go);
        Box bb = new Box(b.xWidth, b.zWidth, b.yWidth, b.name, null);
        result[1] = bb;

        bbGO = Instantiate(b.go);
        bb = new Box(b.yWidth, b.xWidth, b.zWidth, b.name, null);
        result[2] = bb;

        bbGO = Instantiate(b.go);
        bb = new Box(b.yWidth, b.zWidth, b.xWidth, b.name, null);
        result[3] = bb;

        bbGO = Instantiate(b.go);
        bb = new Box(b.zWidth, b.xWidth, b.yWidth, b.name, null);
        result[4] = bb;

        bbGO = Instantiate(b.go);
        bb = new Box(b.zWidth, b.yWidth, b.xWidth, b.name, null);
        result[5] = bb;

        return null;
    }










    //function that, given a compartment, a list of already placed boxes inside of it and a list
    //of boxes that still need to be placed, places another box recursively, until all the unplaced boxes
    //have been placed.
    //for the 3D matrix, 0 = empty, 1 = occupied.
    private int[][] findDisplacement(int numberOfCompartment, int[,,] compartment, int[][] boxesPlaced, LinkedList<GameObject> boxesToPlace)
    {
        //base case: there are no more boxes to order
        if(boxesToPlace.Count == 0)
        {
            return boxesPlaced;
        }

        //if there are boxes to order, then check (this is a custom condition I'm using to, I hope, simplify
        //the calculations): the compartment is empty? Then place the first box on the lower-left-behind corner.
        if(boxesPlaced.Length == 0)
        {
            //Since I'm putting the very first box, I don't know which box would be better to place first.
            //So i search among all of them.
            for(int a = 0; a < boxesToPlace.Count; a++)
            {
                int[][] boxesPlacedCopy = boxesPlaced;
                int[,,] compartmentCopy = compartment;
                boxesPlacedCopy[a] = new int[]{0,0,0};
                GameObject currentBox = boxesToPlace.ElementAt(a);
                for(int i = 0; i < currentBox.transform.localScale.x; i++)
                {
                    for(int j = 0; j < currentBox.transform.localScale.y; j++)
                    {
                        for(int k = 0; k < currentBox.transform.localScale.z; k++)
                        {
                            compartmentCopy[i, j, k] = 1;
                        }
                    }
                }
                LinkedList<GameObject> boxesToPlaceCopy = boxesToPlace;
                boxesToPlaceCopy.Remove(currentBox);
                int[][] currentSolution = findDisplacement(numberOfCompartment, compartmentCopy, boxesPlacedCopy, boxesToPlaceCopy);
                //now I have to check, among all solutions, the best one, that is, the one that contains the highest
                //number of boxes.
            }
        }





        //TODO: remove this
        return null;

    }

















    void Start()
    {
        inputManager = GameObject.Find("InputManager");
        boxList = inputManager.GetComponent<InputManagerScript>().boxList;
        compartmentList = inputManager.GetComponent<InputManagerScript>().compartmentList;

    }


    void Update()
    {
        
    }

    




    private LinkedList<Box> copyBoxList(LinkedList<Box> lb)
    {
        LinkedList<Box> result = new LinkedList<Box>();
        for(int i = 0; i < lb.Count; i++)
        {
            Box current = lb.ElementAt(i);
            Box newBox = new Box(current.xWidth, current.yWidth, current.zWidth, current.name, current.go);
            newBox.xPoint = current.xPoint;
            newBox.yPoint = current.yPoint;
            newBox.zPoint = current.zPoint;
            result.AddLast(newBox);
        }
        return result;
    }
}
