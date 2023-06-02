using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move 
{
    //public int MoveToVictory { get; set; } = 0;
    public int From { get; set; } = 0; //Player is currently on
    public int To { get; set; } = 0;  //Player needs to get to block 
    public int Piece { get; set; } = 0; //Selected piece

    public Vector2Int Position { get; set; } = Vector2Int.zero;

    public Move(int from, int to, int piece)
    {
        From = from;
        To = to;
        Piece = piece;

        //MoveToVictory = 10;
    }

    //public Move(Vector2Int position)
    //{
    //    Position = position;
    //}

    //public override string ToString()
    //{
    //    return "From: " + From + " / To: " + To + " / Piece: " + Piece + " / Position: " + Position;
    //}

    //public override bool Equals(object obj)
    //{
    //    Move other = (Move)obj;
    //    if (other.From != From)
    //    {
    //        return false;
    //    }

    //    if (other.To != To)
    //    {
    //        return false;
    //    }

    //    if (other.Piece != Piece)
    //    {
    //        return false;
    //    }

    //    if (other.Position.x != Position.x)
    //    {
    //        return false;
    //    }

    //    if (other.Position.y != Position.y)
    //    {
    //        return false;
    //    }

    //    return true;
    //}

    //public override int GetHashCode()
    //{
    //    return base.GetHashCode();
    //}
}
