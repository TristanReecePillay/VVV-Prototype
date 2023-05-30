using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAIPlayer : IAIPlayer
{

    public Move GetMove(IRepresentation representation, int player)
    {
        List<Move> possibleMove = representation.GetPossibleMoves(player);

        if (possibleMove.Count == 0)
        {
            return null;
        }

        int randomIndex = Random.Range(0, possibleMove.Count);
        return possibleMove[randomIndex];
    }
}
