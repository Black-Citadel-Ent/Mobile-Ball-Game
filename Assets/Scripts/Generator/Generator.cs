using System;
using System.Collections.Generic;
using Random = System.Random;

namespace Generator
{
    /** <summary>Generates a new solvable puzzle based on tube size,
     * number of colors and an optional random seed</summary>
     */
    public class Generator
    {
        /** <summary>Stacked array of balls, starting at the bottom of the first tube</summary> */
        private int[] _balls;
        /** <summary>Height of the tubes used for this level</summary> */
        private readonly int _tubeSize;
        /** <summary>Number of colors used in this level</summary> */
        private readonly int _colors;
        /** <summary>Total number of tubes the generator is solving for</summary> */
        private int _tubeCount;
        /** <summary>Minimum number of moves the solver was able to complete in</summary> */
        private int _minMoves;
        /** <summary>Highest number of moves the solver was able to complete in</summary> */
        private int _maxMoves;
        /** <summary>The seed used for this puzzle</summary> */
        private readonly int _seed;
        
        /** <summary>Creates the base generator</summary>
         * <param name="tubeSize">The height of the tubes to use. Values 3 to 8 only</param>
         * <param name="colors">The number of colors to solve for. Values 2 to 8 only</param>
         * <param name="seed">Optional random seed for this puzzle</param>
         */
        public Generator(int tubeSize, int colors, int? seed = null)
        {
            if (tubeSize is < 4 or > 8)
                throw new Exception("Tube Size must be between 4 and 8");
            if (colors is < 2 or > 8)
                throw new Exception("Must use between 2 and 8 colors");
            
            _tubeSize = tubeSize;
            _colors = colors;
            if (seed != null) _seed = seed.Value;
            else _seed = new Random().Next();
        }

        /** <summary>Generates a random color set and shuffles into a single array</summary> */
        private void CreateColors()
        {
            var rnd = new Random(_seed);
            List<int> balls = new();
            for (var counter = 0; counter < _colors; counter++)
            {
                for (var sizeCounter = 0; sizeCounter < _tubeSize; sizeCounter++)
                    balls.Add(counter);
            }

            _balls = new int[_tubeSize * _colors];
            for (int counter = 0; counter < _balls.Length; counter++)
            {
                var val = rnd.Next(0, balls.Count);
                _balls[counter] = balls[val];
                balls.RemoveAt(val);
            }

            _tubeCount = _colors;
        }

        /** <summary>Starts the generator and creates a single tube set confirmed to be solvable</summary>
         * <returns>A solvable tube set to use for the level, or null if the generator was unable to complete</returns>
         */
        public TubeSet Execute()
        {
            TubeSet finalSet = null;

            int outerLimit = 2, outerCount = 0;
            do
            {
                outerCount++;
                if (outerCount > outerLimit) break;
                
                CreateColors();
                _tubeCount = _colors;

                int limit = 3, count = 0;
                do
                {
                    count++;
                    if (count > limit) break;

                    _tubeCount += 1;

                    var set = new TubeSet(_tubeCount, _tubeSize, _balls);
                    var solver = new Solver(set);
                    solver.Execute();
                    if(solver.Solutions.Count == 0) continue;

                    _minMoves = int.MaxValue;
                    _maxMoves = int.MinValue;
                    foreach (var sol in solver.Solutions)
                    {
                        if (sol.History.Count < _minMoves) _minMoves = sol.History.Count;
                        if (sol.History.Count > _maxMoves) _maxMoves = sol.History.Count;
                    }

                    finalSet = set;
                } while (finalSet == null);
            } while (finalSet == null);

            if (finalSet == null) throw new Exception("Could not generate a puzzle.");
            return finalSet;
        }
    }
    
}