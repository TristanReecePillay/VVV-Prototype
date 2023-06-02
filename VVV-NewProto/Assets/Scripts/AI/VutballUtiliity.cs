using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VutballUtiliity : IEvaluator
{
    public int CheckIfScored(IRepresentation representation) // Tests whether this move may end the game for either player
    {
        GameOutcome outcome = representation.GetGameOutcome();

        if (outcome == GameOutcome.PLAYER1)
        {
            return 1;
        }
        if (outcome == GameOutcome.PLAYER2)
        {
            return -1;
        }

        return 0;
    }

    public int GetEvaluation(IRepresentation representation)
    {
        GameObject[] playerACharacters = GameObject.FindGameObjectsWithTag("Player1");
        GameObject[] playerBCharacters = GameObject.FindGameObjectsWithTag("Player2");

        int scorePlayerA = 0;
        int scorePlayerB = 0;
        int bestPlayer = 0;

        foreach (GameObject character in playerACharacters)
        {
            if (character.transform.position.z > 0)
            {
                scorePlayerA += (int)character.transform.position.z;
            }
            
        }

        foreach (GameObject character in playerBCharacters)
        {
            if (character.transform.position.z > 0)
            {
                scorePlayerB += (int)character.transform.position.z;
            }
            
        }


        scorePlayerB = (36 - scorePlayerB);

        bestPlayer = scorePlayerA - scorePlayerB;
        Debug.Log("Winning player: " +  bestPlayer);
        return bestPlayer; // Give an int {bigger the positive value, more player 1 is winning || bigger the negative value the more player 2 is winning}
    }


}
