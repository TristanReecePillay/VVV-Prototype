using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;


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

    public GameObject ballInstance;
    private GameObject ballOwner;

    private GameObject currentPlayer;
    private int movesRemaining;

    // Start is called before the first frame update
    void Start()
    {
        CreateField();
         PlaceBall();

        currentPlayer = player1;   
        movesRemaining = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject selectedObject = hit.collider.gameObject;

                // Check if the selected object is a player belonging to the current player
                if (selectedObject.CompareTag("Player") && selectedObject.GetComponent<Player>().Owner == currentPlayer)
                {
                    // Check if there are moves remaining for the current player
                    if (movesRemaining > 0)
                    {
                        // Move the selected player to the clicked position
                        Vector3 newPosition = hit.point;
                        selectedObject.transform.position = new Vector3(newPosition.x, selectedObject.transform.position.y, newPosition.z);

                        // Update moves remaining and switch players if no moves remaining
                        movesRemaining--;
                        if (movesRemaining == 0)
                        {
                            SwitchPlayers();
                        }
                    }
                }
            }
        }
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
                    GameObject player1Instance = Instantiate(player1, position, Quaternion.identity);
                    player1Instance.GetComponent<Player>().SetOwner(player1);
                }

                if (row == 8 && col == 0 || row == 8 && col == 2 || row == 8 && col == 4 || row == 8 && col == 6)
                {
                    Vector3 position = new Vector3(col, 0.5f, row);
                    GameObject player2Instance = Instantiate(player2, position, Quaternion.identity);
                    player2Instance.GetComponent<Player>().SetOwner(player2);
                }
            }
                
            
        }

        
    }

    void SwitchPlayers()
    {
        if (currentPlayer == player1)
        {
            currentPlayer = player2;
        }
        else
        {
            currentPlayer = player1;
        }

        movesRemaining = 2;  // Reset the number of moves remaining for the new current player
    }

    void PlaceBall()
    {
        int ballPositionX = 2;
        int ballPositionZ = 1;
        
        Vector3 position = new Vector3(ballPositionX, 0.6f, ballPositionZ);
        
        ballInstance = Instantiate(ball, position, Quaternion.identity);
        ballOwner = player1;

        ballInstance.GetComponent<Ball>().SetOwner(ballOwner);
    }

    // Function to handle the movement of the ball
    void MoveBall(Vector3 targetPosition)
    {
        if (ballOwner != null)
        {
            // Move the ball to the target position relative to the ball owner
            ballInstance.transform.position = ballOwner.transform.position + targetPosition;
        }
    }

    // Function to handle the passing of the ball
    void PassBall(GameObject targetPlayer)
    {
        if (ballInstance != null)
        {
            // Set the target player as the new ball owner
            ballOwner = targetPlayer;
            ballInstance.transform.SetParent(targetPlayer.transform);
        }
    }
}
