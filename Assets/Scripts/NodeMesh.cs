using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeMesh : MonoBehaviour
{


    //state variables: these
    private Vector2[,] parkMesh = new Vector2[5,5];
    private Vector2[] exitMesh = new Vector2[8];

    // Start is called before the first frame update
    void Start()
    {
        //get park nodes in a 2d array of vector2

        //get all the transforms in a 1d array
        Transform[] parkNodes = new Transform[25];
        for (int i = 0; i < 25; i++)
        {
            parkNodes[i] = transform.GetChild(0).GetChild(i);
        }

        //order two for-loops so that the positions of the nodes are put in correct order in the 2d array
        int index = 0;
        for(int y = 0; y < 5; y++)
        {
            for(int x = 0; x < 5; x++)
            {
                parkMesh[x, y] = parkNodes[index].transform.position; //take all the positions of the child objects and put them in order into the 2d array parkMesh
                index++;
            }
        }


        //get exit nodes in a normal array of vector2

        for(int i = 0; i < 8; i++)
        {
            exitMesh[i] = transform.GetChild(1).GetChild(i).transform.position;
        }


        //Debug.Log("Park Mesh");
        //index = 0;
        //for(int y = 0; y < 5; y++)
        //{
        //    for(int x = 0; x < 5; x++)
        //    {
        //        Debug.Log(parkMesh[x, y]);
        //        index++;
        //    }
        //}

        //Debug.Log("Exit Mesh");
        //foreach(Vector2 vec in exitMesh)
        //{
        //    Debug.Log(vec);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2[,] ParkMesh
    {
        get
        {
            return parkMesh;
        }
    }

    public Vector2[] ExitMesh
    {
        get
        {
            return exitMesh;
        }
    }
}
