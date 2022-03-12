using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OrderCompartment : MonoBehaviour
{
    //a class to order some compartments

    //JUST A COPY OF THE DATA PRESENT IN INPUTMANAGERSCRIPT
    //lists to conserve the boxes and the text on top of them
    private LinkedList<GameObject> boxList = new LinkedList<GameObject>();
    private LinkedList<GameObject[]> textList = new LinkedList<GameObject[]>();

    //list to conserve the x,y and z values of all compartments
    private LinkedList<int[]> compartmentList = new LinkedList<int[]>();
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
        int[] compartmentSizes = compartmentList.ElementAt(compartmentNumber);
        int[,,] compartment = new int[compartmentSizes[0], compartmentSizes[1], compartmentSizes[2]];
        //it returns an array of integers array, where each array contains the coordinates
        //for the lower-left-behind point of the corresponding box.
        //Note that the 2D matrix returned will be long boxList.Count iif all the boxes fit
        //in the compartment, otherwise the number of boxes will be < boxList.Count.
        int[][] boxesCoordinates = new int[boxList.Count][];
        for(int j = 0; j < boxesCoordinates.Length; j++)
        {
            boxesCoordinates[j] = null;
        }
        int[][] i = findDisplacement(compartmentNumber, compartment, new int[boxList.Count][], new LinkedList<GameObject>(boxList));
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
    }

    
    void Update()
    {
        
    }

    

}
