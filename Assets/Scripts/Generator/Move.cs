using Generator;

namespace Generator
{
    /** <summary>Holds the details of a single move</summary> */
    public struct Move
    {
        /** <summary>All possible results of a single move</summary> */
        public enum MoveResult
        {
            /** <summary>Move was successful, moved all balls from the top</summary> */
            MoveAll,
            /** <summary>Move was successful, not enough open slots, moved some balls</summary> */
            MoveSome,
            /** <summary>Move failed, destination tube is full</summary> */
            ToTubeFull,
            /** <summary>Move failed, destination tube top has a different color</summary> */
            WrongColor,
            /** <summary>Move failed, no balls in the originating tube</summary> */
            FromTubeEmpty,
            /** <summary>Move failed, destination tube is full and completed</summary> */
            FromTubeLocked,
            /** <summary>Move failed, index out of range on originating tube</summary> */
            InvalidFromTube,
            /** <summary>Move failed, index out of range on destination tube</summary> */
            InvalidToTube
        }
        
        /** <summary>Index of the originating tube for this move</summary> */
        public readonly int From;
        /** <summary>Index of the destination tube for this move</summary> */
        public readonly int To;
        /** <summary>Result of this move</summary> */
        public readonly MoveResult Result;
        /** <summary>Number of balls moved. 0 on a failed move</summary> */
        public readonly int BallsMoved;

        /** <summary>Basic constructor</summary> */
        public Move(int from, int to, MoveResult result, int ballsMoved)
        {
            From = from;
            To = to;
            Result = result;
            BallsMoved = ballsMoved;
        }
    }
}

public static class MoveResultEx
{ 
    /** <summary>Tests for a successful code</summary> */
    public static bool Success(this Move.MoveResult result)
    {
        return result is Move.MoveResult.MoveAll or Move.MoveResult.MoveSome;
    }
}
