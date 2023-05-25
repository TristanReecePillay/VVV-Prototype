using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject Owner { get; private set; }

    public int movementSpeed;

    private BoardManager boardManager;
    public Ball ball;

    public bool hasBall;
    private bool isMoving;
    private Vector3 targetPosition;

    public bool isBlue;


    public void SetOwner(GameObject owner)
    {
        Owner = owner;
    }
     //Start is called before the first frame update
    void Start()
    {
        boardManager = FindObjectOfType<BoardManager>();
        ball = FindObjectOfType<Ball>();
        hasBall = false;
        

        if ( Owner.transform.position == new Vector3(2f, 0.5f, 1f) )
        {
            hasBall = true;
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            // Move towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * movementSpeed);

            // Check if the player has reached the target position
            if (transform.position == targetPosition)
            {
                isMoving = false;

                // Check if the target position contains the ball
                if (Vector3.Distance(targetPosition, ball.transform.position) < 0.01f)
                {
                    // Player has reached the ball, acquire it
                    hasBall = true;
                    ball.SetBallOwner(gameObject);
                }
            }
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("This has been clicked");
        // Check if the player has the ball and if it's their turn
        if (hasBall && boardManager.IsCurrentPlayerTurn(this))
        {
            // Set the target position to the clicked tile
            targetPosition = GetClickedTilePosition();

            // Check if the target position is a valid move
            if (boardManager.IsMoveValid(targetPosition))
            {
                // Start moving the player towards the target position
                isMoving = true;
                Move(targetPosition);
            }
        }
    }
    private void Awake()
    {
        boardManager = FindObjectOfType<BoardManager>();
        ball = FindObjectOfType<Ball>();
    }

    private Vector3 GetClickedTilePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Tile"))
            {
                return hit.collider.transform.position;
            }
        }

        return Vector3.zero;
    }

    void Move(Vector3 targetPosition)
    {
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (hasBall)
        {
           
            if (distance > 1f)
            {
                // Target position is not valid, it's not adjacent
                return;
            }
            ball.transform.position = targetPosition;
        }
        else
        {
            // Perform movement logic for the player without the ball
            if (distance > 1f)
            {
                // Target position is not valid, it's not adjacent
                return;
            }
        }

        if (distance > 1f)
        {
            // Target position is not valid, it's not adjacent
            return;
        }

        if (hasBall)
        {
            // Move the ball to the target position
            ball.transform.position = targetPosition;
        }

        // Set the target position for the player
        this.targetPosition = targetPosition;
        isMoving = true;
    }

    public bool HasBall()
    {
        return hasBall;
    }
    public void SetBall(bool value)
    {
        hasBall = value;
    }
}
