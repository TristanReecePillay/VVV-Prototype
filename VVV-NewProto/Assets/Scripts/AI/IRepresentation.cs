using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRepresentation
{
    int[] GetAs1DArray();
    int[,] GetAs2DArray();

    IRepresentation Duplicate();

    List<Move> GetPossibleMoves(int player);
    bool MakeMove(Move move, int player);
    bool IsValidMove(Move move, int player);

    GameOutcome GetGameOutcome();
}

public enum GameOutcome
{
    PLAYER1,
    PLAYER2,
    DRAW,
    UNDETERMINED
}
