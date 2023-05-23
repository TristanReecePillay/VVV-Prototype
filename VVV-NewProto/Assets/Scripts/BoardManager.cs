using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int numRows = 10;
    public int numColumns = 7;

    private Player currentPlayer;
    private Player player1;
    private Player player2;

    private void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player1").GetComponent<Player>();
        player2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<Player>();
        currentPlayer = player1;
    }

    public bool IsCurrentPlayerTurn(Player player)
    {
        return player == currentPlayer;
    }

    public bool IsMoveValid(Vector3 targetPosition)
    {
        // Get the difference in positions between the current player and the target position
        Vector3 diff = targetPosition - currentPlayer.transform.position;

        // Check if the target position is horizontal or vertical to the current player
        return (Mathf.Abs(diff.x) == 1 && Mathf.Abs(diff.z) == 0) || (Mathf.Abs(diff.x) == 0 && Mathf.Abs(diff.z) == 1);
    }

    public void EndTurn()
    {
        // Switch to the next player
        currentPlayer = (currentPlayer == player1) ? player2 : player1;
    }
}
