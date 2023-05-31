using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public enum TurnState { START, PLAYER1TURN, PLAYER2TURN, WIN, LOSS, DRAW }
public class BoardMaker : MonoBehaviour
{
    public GameObject plainGrass;
    public GameObject halfwayUpGrass;
    public GameObject halfwayDownGrass;
    public GameObject trylineRed;
    public GameObject trylineBlue;

    public GameObject player1;
    public GameObject player2;

    private GameObject ball;

    public int numRows = 10;
    public int numColumns = 7;

    public GameObject ballPrefab;
    private GameObject ballOwner;

    private GameObject currentPlayer;

    public GameObject clickedPlayer;
    public GameObject clickedGround;

    private int redScore;
    private int blueScore;

    GameObject[] bluePlayers;
    GameObject[] redPlayers;
    private int movesRemaining;

    
    public Player player;

    public Player tackledPlayer;

    private Vector3 oblivion = new Vector3(100, 100, 100);

    public TextMeshProUGUI textRed;
    public TextMeshProUGUI textBlue;

    public TextMeshProUGUI textInvalidPlayer;
    public TextMeshProUGUI textScore;

    private float blockDist;
    private float diagonalBlockDist;

    

    
    TurnState turnState;


    // Start is called before the first frame update
    void Start()
    {
        //turnState = TurnState.START; 

        CreateField();
        blockDist = Vector3.Distance(new Vector3(0, 0, 0), new Vector3(0, 0, 1));
        diagonalBlockDist = Vector3.Distance(new Vector3(0, 0, 0), new Vector3(1, 0, 1));

        bluePlayers = GameObject.FindGameObjectsWithTag("Player1");
        redPlayers = GameObject.FindGameObjectsWithTag("Player2");

        clickedPlayer = null;
        clickedGround = null;
        currentPlayer = player1;
        movesRemaining = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if ((movesRemaining > 0) && (Input.GetMouseButtonDown(1)))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray,out hit) && clickedGround == null)
            {
                GameObject selectedObject = hit.collider.gameObject;

                // Check if the selected object is a player belonging to the current player
                if (selectedObject.CompareTag("Player1") || selectedObject.CompareTag("Player2"))
                {
                    if (selectedObject.GetComponent<Player>().isBlue == currentPlayer.GetComponent<Player>().isBlue)
                    {
                        ballOwner = FindBall(bluePlayers, redPlayers);
                        if (ballOwner.GetComponent<Player>().isBlue == selectedObject.GetComponent<Player>().isBlue)
                        {
                            if ((ballOwner.GetComponent<Player>().isBlue && (ballOwner.transform.position.z >= selectedObject.transform.position.z)) || (!ballOwner.GetComponent<Player>().isBlue && (ballOwner.transform.position.z <= selectedObject.transform.position.z)))
                            {
                                selectedObject.GetComponent<Player>().hasBall = true;
                                ballOwner.GetComponent<Player>().hasBall = false;
                                MoveBall(selectedObject);
                                movesRemaining--;
                            }
                            else
                            {
                                textInvalidPlayer.text = "Thats Offside!!";
                            }
                           
                        }
                        else
                        {
                            textInvalidPlayer.text = "Other team owns the Ball!";
                        }
                    }
                }
            }
        }

        if ((movesRemaining > 0) && (Input.GetMouseButtonDown(0))) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && clickedGround == null)
            {
                GameObject selectedObject = hit.collider.gameObject;


                // Check if the selected object is a player belonging to the current player
                if (selectedObject.CompareTag("Player1") || selectedObject.CompareTag("Player2"))
                {
                    if (selectedObject.GetComponent<Player>().isBlue == currentPlayer.GetComponent<Player>().isBlue)
                    {
                        clickedPlayer = selectedObject;
                        player = clickedPlayer.GetComponent<Player>();
                        Debug.Log(clickedPlayer.name);
                    }
                    else
                    {
                        textInvalidPlayer.text = "You clicked the Wrong Player!!";
                        Debug.Log("Wrong Player selected");
                    }
                }
            }

            if (Physics.Raycast(ray, out hit) && clickedPlayer != null)
            {
                GameObject selectedFloor = hit.collider.gameObject;

                bool isOccupied = false;

               

                if (selectedFloor.CompareTag("Ground") || selectedFloor.CompareTag("FinishBlue") || selectedFloor.CompareTag("FinishRed"))
                {
                    float dist = Vector3.Distance(clickedPlayer.transform.position, selectedFloor.transform.position);
                  
                        if ((dist == blockDist) || ((dist == diagonalBlockDist) && (movesRemaining == 2)))
                        {
                            foreach (GameObject obj in bluePlayers)
                            {
                                if (obj.transform.position == selectedFloor.transform.position)
                                {
                                    tackledPlayer = obj.GetComponent<Player>();
                                    if (tackledPlayer.hasBall && (tackledPlayer.isBlue != player.isBlue))
                                    {
                                        tackledPlayer.hasBall = false;
                                        obj.transform.position = oblivion;
                                        player.hasBall = true;
                                        if (AllInOblivion(bluePlayers))
                                        {
                                            resetBoard();
                                        movesRemaining = 0;
                                            Debug.Log("Red scored!");
                                        }
                                    }
                                    else
                                    {
                                        isOccupied = true;
                                    }
                                }
                            }

                            foreach (GameObject obj in redPlayers)
                            {
                                if (obj.transform.position == selectedFloor.transform.position)
                                {
                                    tackledPlayer = obj.GetComponent<Player>();
                                    if (tackledPlayer.hasBall && (tackledPlayer.isBlue != player.isBlue))
                                    {
                                        tackledPlayer.hasBall = false;
                                        obj.transform.position = oblivion;
                                        player.hasBall = true;
                                    if (AllInOblivion(redPlayers))
                                    {
                                        resetBoard();
                                        movesRemaining = 0;
                                        
                                    }
                                }
                                    else
                                    {
                                        isOccupied = true;
                                    }
                                }
                            }
                            if (!isOccupied)
                            {
                                // Move the selected player to the clicked position
                                clickedGround = selectedFloor;
                                movesRemaining--;
                                if(dist == diagonalBlockDist)
                                {
                                   movesRemaining--;
                                }

                                clickedPlayer.transform.position = clickedGround.transform.position;
                                Debug.Log(clickedPlayer.name + clickedGround.name + movesRemaining);
                                Debug.Log("Player currently on: " + clickedGround.name + "Number of moves reamining: " + movesRemaining);

                                textInvalidPlayer.text = string.Empty;

                                if (player.hasBall && ((player.isBlue && selectedFloor.CompareTag("FinishBlue")) || (!player.isBlue && selectedFloor.CompareTag("FinishRed"))))
                                {
                                    if(player.isBlue)
                                    {
                                        blueScore++;
                                       
                                        textInvalidPlayer.text = "Blue scored: " + blueScore;
                                    }
                                    else
                                    {
                                        redScore++;
                                        textInvalidPlayer.text = "Red scored: " + redScore;
                                    }
                                    resetBoard();
                                    movesRemaining = 0;                                   
                                    
                                }
                            }

                            else
                            {
                               textInvalidPlayer.text = "Space is Occupied!";
                            }

                        }

                        else
                        {
                           textInvalidPlayer.text = "You can't move that far!!";
                        }
                }
            }

            if (clickedPlayer == null || clickedGround == null)
            {
                Debug.Log("No move made");
                return;
            }

            //clickedPlayer.transform.position = Vector3.MoveTowards(clickedPlayer.transform.position, clickedGround.transform.position, Time.deltaTime * 1f);

            

            // If the selected player is the ball owner, move the ball as well
               

            Debug.Log("Player at " + player.transform.position + " has ball : " + player.hasBall);
            if (player.hasBall)
            {
                MoveBall(clickedPlayer);
            }

            // Update moves remaining and switch players if no moves remaining
            clickedGround = null;
            clickedPlayer = null;
        }
        if (movesRemaining <= 0)
        {
            SwitchPlayers();
            Debug.Log(currentPlayer.name);
        }
        textScore.text = " Blue " + blueScore + " : " + redScore + " Red ";

    }

    void CreateField()
    {
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                if (row == 0)
                {
                    Vector3 position = new Vector3(col, 0.5f, row);
                    Instantiate(trylineRed, position, Quaternion.identity);
                }
                else if (row == 9)
                {
                    Vector3 position = new Vector3(col, 0.5f, row);
                    Instantiate(trylineBlue, position, Quaternion.identity);
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
                }

                if (row == 8 && col == 0 || row == 8 && col == 2 || row == 8 && col == 4 || row == 8 && col == 6)
                {
                    Vector3 position = new Vector3(col, 0.5f, row);
                    GameObject player2Instance = Instantiate(player2, position, Quaternion.identity);
                }
            }

        }

        PlaceBall();
        
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

        textRed.enabled = !textRed.IsActive();
        textBlue.enabled = !textBlue.IsActive();

        movesRemaining = 2;  // Reset the number of moves remaining for the new current player
    }

    void PlaceBall()
    {
        Vector3 ballPosition = Vector3.zero;  // Default position if no player is the ball owner

        if (currentPlayer == player1)
        {
            ballPosition = new Vector3(2, 0.6f, 1);
        }
        else if (currentPlayer == player2)
        {
            ballPosition = new Vector3(2, 0.6f, 8);
        }

        ball = Instantiate(ballPrefab, ballPosition, Quaternion.identity);
        ballOwner = player1;

        ball.GetComponent<Ball>().SetOwner(ballOwner);
        //ball.transform.SetParent(ballOwner.transform);
    }

    // Function to handle the movement of the ball
    public void MoveBall(GameObject targetPlayer)
    {
        ball.transform.position = targetPlayer.transform.position;
    }

    // Function to handle the passing of the ball
    void PassBall(GameObject targetPlayer)
    {
        if (ball != null)
        {
            // Set the target player as the new ball owner
            ballOwner = targetPlayer;
            ball.GetComponent<Ball>().SetOwner(ballOwner);

            MoveBall(ballOwner);
        }
    }

   bool AllInOblivion(GameObject[] players)
   {
        bool result = true;

        foreach (GameObject obj in players)
        {
            if(obj.transform.position != oblivion)
            {
                result = false;
                break;
            }
        }

            return result;
   }

    void resetBoard()
    {
        int xCoordinate = 0;
        player.hasBall = false;
        foreach (GameObject obj in bluePlayers)
        {
            obj.transform.position = new Vector3(xCoordinate, 0.5f, 1);
            if ((xCoordinate == 2) && (!player.isBlue))
            {
                obj.GetComponent<Player>().hasBall = true;
                MoveBall(obj);
            }
            xCoordinate += 2;
        }
        xCoordinate = 0;

        foreach (GameObject obj in redPlayers)
        {
            obj.transform.position = new Vector3(xCoordinate, 0.5f, 8);
            if ((xCoordinate == 2) && (player.isBlue))
            {
                obj.GetComponent<Player>().hasBall = true;
                MoveBall(obj);
            }

            xCoordinate += 2;
        }
    }

    GameObject FindBall(GameObject[] bluePlayers, GameObject[] redPlayers)
    {
        foreach (GameObject obj in bluePlayers )
        {
            if (obj.GetComponent<Player>().hasBall)
            {
                return obj;
            }
        }
        foreach (GameObject obj in redPlayers)
        {
            if (obj.GetComponent<Player>().hasBall)
            {
                return obj;
            }
        }
        return null;
    }
    

}

