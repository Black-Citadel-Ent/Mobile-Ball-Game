using Objects;
using UnityEngine;

namespace Behaviors
{
    /** <summary>Base class for the tube object</summary> */
    public class Tube : MonoBehaviour
    {
        [Tooltip("Reference to the object that will contain the sprites.")]
        [SerializeField] private Transform visualRoot;
        
        /** <summary>Sprite set for the tube</summary> */
        private TubeTheme _theme;
        /** <summary>Number of balls this tube can hold</summary> */
        private int _height;
        /** <summary>Holds the various ball positions in this tube from bottom to top </summary> */
        private Transform[] _positions;

        /** <summary>Height of this tube in terms of number of balls it can hold</summary> */
        public int Height => _height;

        /**
         * <summary>Setup for the tube, called between Awake and Start</summary>
         * <param name="height">Capacity of balls for this tube</param>
         * <param name="theme">Theme for this tube</param>
         */
        public void Setup(int height, TubeTheme theme)
        {
            _theme = theme;
            _height = height;
            _positions = new Transform[height];
            Layout();
        }

        /**
         * <summary>Returns the absolute position of a particular ball slot in this tube</summary>
         * <param name="slot">The number of the slot to fine, with zero being the bottom of the tube</param>
         */
        public Vector3 PositionOf(int slot)
        {
            return _positions[slot].position;
        }

        /** <summary>Builds the visual tube objects</summary> */
        private void Layout()
        {
            var obj = new GameObject("Bottom");
            var rend = obj.AddComponent<SpriteRenderer>();
            rend.sprite = _theme.Bottom;
            obj.transform.parent = visualRoot;
            obj.transform.localPosition = new Vector3(0, 0, 0);
            _positions[0] = obj.transform;

            for (int counter = 0; counter < _height - 1; counter++)
            {
                obj = new GameObject($"Fill {counter+1}");
                rend = obj.AddComponent<SpriteRenderer>();
                rend.sprite = _theme.Fill;
                obj.transform.parent = visualRoot;
                obj.transform.localPosition = new Vector3(0, counter + 1, 0);
                _positions[counter + 1] = obj.transform;
            }

            obj = new GameObject("Top");
            rend = obj.AddComponent<SpriteRenderer>();
            rend.sprite = _theme.Top;
            obj.transform.parent = visualRoot;
            obj.transform.localPosition = new Vector3(0, _height - 0.5f, 0);
        }
    }
}