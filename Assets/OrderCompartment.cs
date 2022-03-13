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
        boxList = inputManager.GetComponent<InputManagerScript>().boxList;
        compartmentList = inputManager.GetComponent<InputManagerScript>().compartmentList;

        Compartment c = compartmentList.ElementAt(compartmentNumber);

        LinkedList<Box> resultForCompartment;
        LinkedList<Point> points = new LinkedList<Point>();
        points.AddLast(new Point(0, 0, 0));
        resultForCompartment = putBox(new LinkedList<Box>(), copyBoxList(boxList), c, points);



        //int[][] i = findDisplacement(compartmentNumber, compartment, new int[boxList.Count][], new LinkedList<GameObject>(boxList));
    }


    private LinkedList<Box> putBox(LinkedList<Box> placed, LinkedList<Box> toPlace, Compartment compartment, LinkedList<Point> availablePoints)
    {

        //for all available points (in which i can theoretically place a box)
        foreach(Point p in availablePoints)
        {
            //for each box i have to place
            foreach(Box initialBox in toPlace)
            {
                //for each possible rotation of the box
                foreach(Box b in rotatedBox(initialBox))
                {
                    Debug.Log("Inside");

                    //in this point, this box, rotated like this, goes out of bounds in respect to the compartment?
                    bool outOfCompartment = isBoxOutsideOfCompartment(p, b, compartment);
                    //if so, skip this iteration. You can't put that there.
                    if (outOfCompartment) continue;

                    //arrived here, the box can stay inside the compartment. But, placed in this point, would it
                    //overlap with at least one of the already placed boxes?
                    bool overlapping = doesNewBoxOverlap(placed, b, p);
                    //if so, skit this iteration. Can't put that there.
                    if (overlapping) continue;

                    //if the code arrived at this point, it means that the box can be placed, with the current orientation,
                    //in this point. So, let's:
                    //-place it there;
                    //-notify the fact that that point is no longer available for placing a box
                    //-add new possible points for the displacement
                    //-add this box to the list of the already placed one
                    //-subtract it from the list of the ones to be still placed

                    //1) place the box
                    b.xPoint = p.x;
                    b.yPoint = p.y;
                    b.zPoint = p.z;

                    //2) notify that the point is no longer available
                    LinkedList<Point> newPoints = copyPointList(availablePoints);
                    newPoints.Remove(p);

                    //3) add the new points
                    Point newP1 = new Point(p.x + b.xWidth, p.y, p.z);
                    Point newP2 = new Point(p.x, p.y + b.yWidth, p.z);
                    Point newP3 = new Point(p.x, p.y, p.z + b.zWidth);
                    newPoints.AddLast(newP1);
                    newPoints.AddLast(newP2);
                    newPoints.AddLast(newP3);

                    //4) add the box to the list of placed ones
                    LinkedList<Box> newPlaced = copyBoxList(placed);
                    newPlaced.AddLast(b);

                    //5) subtract the box from the list of the ones to be still placed
                    LinkedList<Box> newToPlace = copyBoxList(toPlace);
                    newToPlace.Remove(b);

                    //before the recursive call, let's call a coroutine that really displaces the boxes in the compartment
                    StartCoroutine(DisplaceBoxesInCompartment(newPlaced, compartment));

                    return null;
                    //now we can call the function recursively
                    putBox(newPlaced, newToPlace, compartment, newPoints);
                }
            }
        }



        //TODO: delete this
        return null;
    }


    private IEnumerator DisplaceBoxesInCompartment(LinkedList<Box> boxesToDisplace, Compartment c)
    {
        //first of all, delete all other boxes inside this compartment
        GameObject[] allBoxes = GameObject.FindGameObjectsWithTag("BoxTag");
        foreach(GameObject g in allBoxes)
        {
            if(g.transform.position.x < c.xWidth || g.transform.position.y < c.floorHeight + c.yWidth || g.transform.position.z < c.zWidth)
            {
                Destroy(g);
            }
        }

        //now we can place our boxes
        foreach(Box b in boxesToDisplace)
        {
            GameObject go = Instantiate(inputManager.GetComponent<InputManagerScript>().createBoxGameObject(b.xWidth, b.yWidth, b.zWidth,
                b.xPoint + b.xWidth/2, b.yPoint + b.yWidth / 2, b.zPoint + b.zWidth / 2, b.name));

        }

        Debug.Log("Corourine almost finished");

        yield return new WaitForSeconds(1f);
        Debug.Log("Corourine finished");

    }



    private Box[] rotatedBox(Box b)
    {
        Box[] result = new Box[6];
        result[0] = b;

        Box bb = new Box(b.xWidth, b.zWidth, b.yWidth, b.name, null);
        result[1] = bb;

        bb = new Box(b.yWidth, b.xWidth, b.zWidth, b.name, null);
        result[2] = bb;

        bb = new Box(b.yWidth, b.zWidth, b.xWidth, b.name, null);
        result[3] = bb;

        bb = new Box(b.zWidth, b.xWidth, b.yWidth, b.name, null);
        result[4] = bb;

        bb = new Box(b.zWidth, b.yWidth, b.xWidth, b.name, null);
        result[5] = bb;

        return result;
    }


    //function used to check if a box, placed in a certain point, would go outside of the compartment.
    //True = it would
    //False = it fits
    private bool isBoxOutsideOfCompartment(Point p, Box b, Compartment compartment)
    {
        if (p.x + b.xWidth > compartment.xWidth || p.y + b.yWidth > compartment.floorHeight + compartment.yWidth || p.z + b.zWidth > compartment.zWidth) return true;
        return false;
    }

    //function used to check if the first box, already placed in the compartment, would overlap
    //with the second if one if it had to be placed in the point possibleNewPoint
    private bool doBoxesOverlap(Box b1, Box b2, Point possibleNewPoint)
    {
        for(int i = possibleNewPoint.x; i < possibleNewPoint.x + b2.xWidth; i++)
        {
            for (int j = possibleNewPoint.y; j < possibleNewPoint.y + b2.yWidth; j++)
            {
                for (int k = possibleNewPoint.z; k < possibleNewPoint.z + b2.zWidth; k++)
                {
                    if((i >= b1.xPoint && i < b1.xPoint + b1.xWidth) && (j >= b1.yPoint && j < b1.yPoint + b1.yWidth) && (k >= b1.zPoint && k < b1.zPoint + b1.zWidth))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    //function used to chech if one of the boxes of the first list (representing the boxes already placed
    //in a compartment) would overlap with the second one if it had to be placed in the point possibleNewPoint
    private bool doesNewBoxOverlap(LinkedList<Box> placed, Box b, Point p)
    {
        foreach(Box placedBox in placed)
        {
            if (doBoxesOverlap(placedBox, b, p)) return true;
        }
        return false;
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
        Debug.Log("Start");
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Ehi");
            findBestOrderingForCompartment(0);
        }
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

    private LinkedList<Point> copyPointList(LinkedList<Point> lp)
    {
        LinkedList<Point> result = new LinkedList<Point>();
        for (int i = 0; i < lp.Count; i++)
        {
            Point current = lp.ElementAt(i);
            Point newPoint = new Point(current.x, current.y, current.z);
            result.AddLast(newPoint);
        }
        return result;
    }
}
