using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public enum TurnState { START, PLAYER1TURN, PLAYER2TURN, WIN, LOSS, DRAW }
public class BoardMaker : MonoBehaviour
{
    public GameObject plainGrass;
    public GameObject halfwayUpGrass;
    public GameObject halfwayDownGrass;
    public GameObject trylineGrass;

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
    private int movesRemaining;

    public Player player;

    TurnState turnState;


    // Start is called before the first frame update
    void Start()
    {
        //turnState = TurnState.START; 

        CreateField();

        clickedPlayer = null;
        clickedGround = null;
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

            if (Physics.Raycast(ray, out hit) && clickedGround == null)
            {
                GameObject selectedObject = hit.collider.gameObject;

                // Check if the selected object is a player belonging to the current player
                if (selectedObject.CompareTag("Player1") || selectedObject.CompareTag("Player2"))
                {
                    clickedPlayer = selectedObject;
                    Debug.Log(clickedPlayer.name);
                }
            }

            if (Physics.Raycast(ray, out hit) && clickedPlayer != null)
            {
                GameObject selectedFloor = hit.collider.gameObject;

                if (selectedFloor.CompareTag("Ground"))
                {
                    clickedGround = selectedFloor;
                    Debug.Log(clickedGround.name);

                    // Move the selected player to the clicked position
                }
            }

            if (clickedPlayer == null || clickedGround == null)
            {
                Debug.Log("No move made");
                return;
            }

            //clickedPlayer.transform.position = Vector3.MoveTowards(clickedPlayer.transform.position, clickedGround.transform.position, Time.deltaTime * 1f);
            clickedPlayer.transform.position = clickedGround.transform.position;
            movesRemaining--;
            Debug.Log(clickedPlayer.name + clickedGround.name + movesRemaining);

            // If the selected player is the ball owner, move the ball as well

            Player player = clickedPlayer.GetComponent<Player>();
            Debug.Log("Player at " + player.transform.position + " has ball : " + player.hasBall);
            if (player.hasBall)
            {
                MoveBall(clickedPlayer);
            }

            // Update moves remaining and switch players if no moves remaining

            if (movesRemaining == 0)
            {
                SwitchPlayers();
                Debug.Log(currentPlayer.name);
            }
            clickedGround = null;
            clickedPlayer = null;
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
}

