using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject Owner { get; private set; }

    public int movementSpeed;

    private BoardManager boardManager;
    private Ball ball;

    private bool hasBall;
    private bool isMoving;
    private Vector3 targetPosition;


    public void SetOwner(GameObject owner)
    {
        Owner = owner;
    }
     //Start is called before the first frame update
    void Start()
    {
        
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
                if (targetPosition == ball.transform.position)
                {
                    // Player has reached the ball, acquire it
                    hasBall = true;
                    ball.SetBallOwner(this);
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
        if (hasBall)
        {
            float distance = Vector3.Distance(transform.position, targetPosition);
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
            float distance = Vector3.Distance(transform.position, targetPosition);
            if (distance > 1f)
            {
                // Target position is not valid, it's not adjacent
                return;
            }
        }
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
