using System;
using System.Collections.Generic;

namespace Generator
{
    /** <summary>Tube set built for the generator and solver. This fully describes
     * a level</summary>
     */
    public class TubeSet: ICloneable
    {
        /** <summary>Holds all tubes/balls that make up this grid</summary> */
        private readonly int[][] _tubes;
        /** <summary>Move history from the initial state that built this grid</summary> */
        private readonly Queue<Move> _history;
        
        /** <summary>Creates a tube set from initial values</summary>
         * <param name="count">The number of tubes to use in this set</param>
         * <param name="size">The height of each tube</param>
         * <param name="balls">The array of balls starting at the bottom of the first tube.
         * Empty spaces use -1</param>
         */
        public TubeSet(int count, int size, int[] balls)
        {
            _tubes = new int[count][];
            for (var counter = 0; counter < count; counter++)
            {
                _tubes[counter] = new int[size];
                var ball = counter * size;
                for (var bCounter = 0; bCounter < size; bCounter++)
                {
                    if (ball + bCounter < balls.Length)
                        _tubes[counter][bCounter] = balls[ball + bCounter];
                    else
                        _tubes[counter][bCounter] = -1;
                }
            }

            _history = new();
        }

        /** <summary>Convenience constructor for the clone operation. Makes a copy of a tube set</summary>
         * <param name="tubes">The original set's tube array</param>
         * <param name="history">The original set's history</param>
         */
        private TubeSet(int[][] tubes, Queue<Move> history)
        {
            _tubes = new int[tubes.Length][];
            for (var tube = 0; tube < tubes.Length; tube++)
            {
                _tubes[tube] = new int[tubes[tube].Length];
                for (var ball = 0; ball < tubes[tube].Length; ball++)
                    _tubes[tube][ball] = tubes[tube][ball];
            }

            _history = new Queue<Move>(history);
        }

        /** <summary>Queue of move history for this set, with the most recent move at the front</summary> */
        public Queue<Move> History => _history;
        /** <summary>The total number of tubes in this set</summary> */
        public int TubeCount => _tubes.Length;
        /** <summary>The Tube/Ball layout for this set</summary> */
        public int[][] Tubes => _tubes;

        /** <summary>Encoded value for this set, disregarding history</summary> */
        public ulong[] Encoded
        {
            get
            {
                var ret = new ulong[_tubes.Length];
                for (var tube = 0; tube < _tubes.Length; tube++)
                    for (var ball = 0; ball < _tubes[tube].Length; ball++)
                        ret[tube] |= (ulong)_tubes[tube][ball] << (8 * ball);
                return ret;
            }
        }

        /** <summary>Tests if this set is complete, all full tubes of the same color or empty</summary> */
        public bool IsFinished
        {
            get
            {
                foreach (var tube in _tubes)
                {
                    var ball = tube[0];
                    foreach(var b in tube)
                        if (b != ball)
                            return false;
                }

                return true;
            }
        }

        /** <summary>Simulates a move and returns the result of it. This does not
         * execute the move</summary>
         * <param name="from">Index of the origin tube</param>
         * <param name="to">Index of the destination tube</param>
         * <returns>The result of this move</returns>
         */
        public Move.MoveResult TestMove(int from, int to)
        {
            if (from < 0 || from >= _tubes.Length) return Move.MoveResult.InvalidFromTube;
            if (to < 0 || to >= _tubes.Length) return Move.MoveResult.InvalidToTube;
            if (EmptyCount(from) == _tubes[0].Length) return Move.MoveResult.FromTubeEmpty;
            if (Finished(from)) return Move.MoveResult.FromTubeLocked;
            var toEmptyCount = EmptyCount(to);
            if (toEmptyCount == 0) return Move.MoveResult.ToTubeFull;
            
            Top(from, out var fromColor, out var fromCount);
            Top(to, out var toColor, out var toCount);
            if (fromColor != toColor && toColor != -1) return Move.MoveResult.WrongColor;
            return fromCount > toEmptyCount ? Move.MoveResult.MoveSome : Move.MoveResult.MoveAll;
        }

        /** <summary>Executes a move without testing it</summary>
         * <param name="from">Index of the originating tube</param>
         * <param name="to">Index of the destination tube</param>
         * <returns>The number of balls moved</returns>
         */
        public int ExecuteMove(int from, int to)
        {
            var test = TestMove(from, to);
            if (!test.Success()) return 0;

            var empty = EmptyCount(to);
            Top(from, out var color, out var count);
            var move = Math.Min(empty, count);
            PopTop(from, move);
            Fill(to, color, move);
            return move;
        }

        public object Clone()
        {
            return new TubeSet(_tubes, _history);
        }

        /** <summary>Checks the top of a tube for color and depth</summary>
         * <param name="tube">Index of the tube to take from</param>
         * <param name="color">Color of the top ball. -1 if tube is empty</param>
         * <param name="count">Number of balls in this set. 0 if tube is empty</param>
         * <returns>True if there are balls present, false otherwise</returns>
         */
        private bool Top(int tube, out int color, out int count)
        {
            color = -1;
            count = 0;
            for (var ball = _tubes[tube].Length - 1; ball >= 0; ball--)
            {
                var c = _tubes[tube][ball];
                if (color == -1 && c != -1)
                {
                    color = c;
                    count = 1;
                }
                else if (color != -1 && c == color)
                    count++;
                else if (color != -1 && c != color)
                    break;
            }

            return count != 0;
        }

        /** <summary>Finds the number of empty spaces at the top of a tube</summary>
         * <param name="tube">Index of the tube to search</param>
         * <returns>The count of empty spaces. 0 if full</returns>
         */
        private int EmptyCount(int tube)
        {
            var count = 0;
            for (var ball = _tubes[tube].Length - 1; ball >= 0; ball--)
            {
                if (_tubes[tube][ball] == -1) count++;
                else break;
            }

            return count;
        }

        /** <summary>Checks a tube to see if it is finished, meaning full with the same color</summary>
         * <param name="tube">Index of the tube to search</param>
         * <returns>true if this tube is full with the same color, false otherwise</returns>
         */
        private bool Finished(int tube)
        {
            var color = _tubes[tube][0];
            if (color == -1) return false;
            foreach(var c in _tubes[tube])
                if (c != color) return false;
            return true;
        }

        /** <summary>Removes balls from the top of a tube</summary>
         * <param name="tube">Index of the tube</param>
         * <param name="count">Maximum number of balls to remove</param>
         * <returns>The number of balls removed</returns>
         */
        private int PopTop(int tube, int count)
        {
            var endCount = 0;
            for (var ball = _tubes[tube].Length - 1; ball >= 0; ball--)
            {
                if (_tubes[tube][ball] != -1 && count != 0)
                {
                    _tubes[tube][ball] = -1;
                    count -= 1;
                    endCount++;
                }
            }

            return endCount;
        }

        /** <summary>Adds ball of a specific color to the top of a tube</summary>
         * <param name="tube">Index of the tube</param>
         * <param name="color">Color of the ball to add</param>
         * <param name="count">Max number of balls to add</param>
         * <returns>Number of balls added</returns>
         */
        private int Fill(int tube, int color, int count)
        {
            var endCount = 0;
            for (var ball = 0; ball < _tubes[tube].Length; ball++)
            {
                if (_tubes[tube][ball] == -1 && count != 0)
                {
                    _tubes[tube][ball] = color;
                    count -= 1;
                    endCount++;
                }
            }

            return endCount;
        }

        /** <summary>Overridden for debug purposes</summary> */
        public override string ToString()
        {
            var str = "--------\n";
            foreach (var tube in _tubes)
            {
                foreach (var ball in tube)
                {
                    str += ball;
                }

                str += "\n";
            }

            str += "--------";
            return str;
        }
    }
}