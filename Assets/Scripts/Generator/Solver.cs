using System.Collections.Generic;

namespace Generator
{
    /** <summary>Solves for a tube set</summary> */
    public class Solver
    {
        /** <summary>List of pending grids to test</summary> */
        private readonly Stack<TubeSet> _pending;
        /** <summary>List of fully solved grids</summary> */
        private readonly List<TubeSet> _solved;
        /** <summary>List of encoded previous states to prevent duplicate grid sets</summary> */
        private readonly List<ulong[]> _prevStates;

        /** <summary>Creates a solver</summary>
         * <param name="initialState">A tube set with an initial placement of balls</param>
         */
        public Solver(TubeSet initialState)
        {
            _pending = new Stack<TubeSet>();
            _solved = new List<TubeSet>();
            _pending.Push(initialState);
            _prevStates = new List<ulong[]>();
        }

        /** <summary>The final list of all solutions with their move history</summary> */
        public List<TubeSet> Solutions => _solved;

        /** <summary>Starts the solver. Runs 500 iterations before stopping</summary> */
        public void Execute()
        {
            int count = 0, limit = 500;

            while (_pending.Count != 0)
            {
                count++;
                if (count >= limit) break;
                Iterate();
            }
        }

        /** <summary>Runs a single iteration of the solver. Duplicates the next grid in
         * pending and generates a list of possible moves. Removes duplicates and failures,
         * and stores the rest back into pending for future iterations. Fully resolved
         * grids go into solved</summary>
         */
        private void Iterate()
        {
            var set = _pending.Pop();
            for (var fromCounter = 0; fromCounter < set.TubeCount; fromCounter++)
            {
                for (var toCounter = 0; toCounter < set.TubeCount; toCounter++)
                {
                    if(fromCounter == toCounter) continue;
                    var result = set.TestMove(fromCounter, toCounter);
                    if(!result.Success()) continue;
                    
                    var newSet = (TubeSet)set.Clone();
                    if(newSet.ExecuteMove(fromCounter, toCounter) == 0) continue;
                    if(IsPreviousState(newSet)) continue;
                    _prevStates.Add(newSet.Encoded);
                    
                    if (newSet.IsFinished) _solved.Add(newSet);
                    else _pending.Push(newSet);
                }
            }
        }

        /** <summary>Goes through recorded previous states to find a match. Uses encoded
         * values for the grid</summary>
         */
        private bool IsPreviousState(TubeSet set)
        {
            var test = set.Encoded;
            foreach (var prev in _prevStates)
            {
                var match = true;
                for (var counter = 0; counter < test.Length; counter++)
                {
                    if (prev[counter] != test[counter])
                    {
                        match = false;
                        break;
                    }
                }

                if (match) return true;
            }

            return false;
        }
    }
}