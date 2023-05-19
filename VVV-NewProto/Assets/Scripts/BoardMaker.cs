using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoardMaker : MonoBehaviour
{
    public GameObject plainGrass;
    public GameObject halfwayUpGrass;
    public GameObject halfwayDownGrass;
    public GameObject trylineGrass;

    public GameObject player1;
    public GameObject player2;

    public GameObject ball;

    public int numRows = 10;
    public int numColumns = 7;

     


    // Start is called before the first frame update
    void Start()
    {
        CreateField();
         PlaceBall();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateField()
    {
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                if (row == 0 || row == 9)
                {
                    Vector3 position = new Vector3(col, 0.5f, row);
                    Instantiate(trylineGrass, position, Quaternion.identity);
                }
                else if (row == 4)
                {
                    Vector3 position = new Vector3(col, 0.5f, row);
                    Vector3 rotation = new Vector3(0, 180, 0);
                    GameObject go = Instantiate(halfwayUpGrass, position, Quaternion.identity);
                    go.transform.Rotate(rotation); 
                }
                else if (row == 5)
                {
                    Vector3 position = new Vector3(col, 0.5f, row);
                    Instantiate(halfwayDownGrass, position, Quaternion.identity);
                }
                else
                {
                    Vector3 position = new Vector3(col, 0.5f, row);
                    Instantiate(plainGrass, position, Quaternion.identity);
                }

                if (row == 1 && col == 0 || row == 1 && col == 2 || row == 1 && col == 4 || row == 1 && col == 6)
                {
                    Vector3 position = new Vector3(col, 0.5f, row);
                    Instantiate(player1, position, Quaternion.identity);
                }

                if (row == 8 && col == 0 || row == 8 && col == 2 || row == 8 && col == 4 || row == 8 && col == 6)
                {
                    Vector3 position = new Vector3(col, 0.5f, row);
                    Instantiate(player2, position, Quaternion.identity);
                }
            }
                
            
        }

        
    }

    void PlaceBall()
    {
        int ballPositionX = 2;
        int ballPositionZ = 1;
        
        Vector3 position = new Vector3(ballPositionX, 0.6f, ballPositionZ);
        
        Instantiate(ball, position, Quaternion.identity);

        
    }
}
