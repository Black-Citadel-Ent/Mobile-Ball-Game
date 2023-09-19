using System.Collections.Generic;
using Behaviors;
using UnityEngine;

namespace Layout
{
    public class BallLayout
    {
        /*
         * Tube Segments are 1.0 units high and wide.
         * Spacing between tubes must be 1.2 units horizontally.
         * Spacing between tubes must be 1.4 units vertically.
         * No spacing necessary along the overall margin.
         * Top of tube extends up to 0.2 units.
         * Origin of tube is at center of bottom segment (0.5, 0.5).
         */
        private const float TubeWidth = 1.0f;
        private const float SegmentHeight = 1.0f;
        private const float PaddingX = 1.2f;
        private const float PaddingY = 1.4f;
        private const float OriginX = 0.5f;
        private const float OriginY = 0.5f;
        private const float TopPadding = 0.2f;
        
        private readonly List<Tube> _tubes = new();
        private readonly List<TubeBallLink> _balls = new();

        public void Add(Tube tube)
        {
            _tubes.Add(tube);
        }

        public void Add(Ball ball, Tube tube, int position)
        {
            if(!_tubes.Contains(tube))
                _tubes.Add(tube);
            _balls.Add(new TubeBallLink()
            {
                Ball = ball,
                Tube = tube,
                Position = position
            });
        }

        /** <summary>Builds the layout within the rectangular aspect ratio</summary>
         * <param name="area">The area to create the proper aspect ratio for</param>
         * <returns>The height in unity units of the overall grid</returns>
         */
        public float BuildLayout(Rect area)
        {
            var tubeHeight = _tubes[0].Height * SegmentHeight + TopPadding;
            var aspect = area.height / area.width;
            var nearAspect = float.MaxValue;
            var nearRow = 0;
            var nearCol = 0;
            
            for (var row = 1; row <= _tubes.Count; row++)
            {
                var col = (int)Mathf.Ceil(row / (float)_tubes.Count);
                var height = row * tubeHeight + (row - 1) * PaddingY;
                var width = col * TubeWidth + (col - 1) * PaddingX;
                var a = height / width;
                var nearA = Mathf.Abs(aspect - a);
                if (nearA < nearAspect)
                {
                    nearAspect = nearA;
                    nearRow = row;
                    nearCol = col;
                }
            }

            var unitSize = nearRow * nearCol;
            var extra = _tubes.Count - unitSize;
            
            return 0.0f;
        }

        private struct TubeBallLink
        {
            public Tube Tube;
            public Ball Ball;
            public int Position;
        }
    }
}