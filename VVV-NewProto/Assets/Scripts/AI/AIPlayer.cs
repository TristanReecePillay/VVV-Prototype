using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAIPlayer
{


    Move GetMove(IRepresentation representation, int player);
    
        //List<Move> possibleMove = representation.GetPossibleMoves(player);

        //if (possibleMove.Count == 0)
        //{
        //    return null;
        //}

        //if (possibleMove.Count == 1)
        //{
        //    return possibleMove[0];

        //}

        //foreach (Move move in possibleMove)
        //{
        //    IRepresentation next_representation = representation.Duplicate();
        //    next_representation.MakeMove(move, player);
        //    if (next_representation != null)
        //    {
        //        GetMove(next_representation, player);
        //    }


        //}
        //return null;
    



}
