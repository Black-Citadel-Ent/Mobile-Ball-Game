using UnityEngine;

namespace Objects
{
    /** <summary>Container of tube sprites used to build a tube in this theme</summary> */
    [CreateAssetMenu(fileName = "Tube Theme", menuName = "Ball Game/Tube Theme")]
    public class TubeTheme : ScriptableObject
    {
        [Tooltip("Top sprite of the tube.")]
        [SerializeField] private Sprite top;
        [Tooltip("Fill (center) segment of the tube.")]
        [SerializeField] private Sprite fill;
        [Tooltip("Bottom segment of the tube.")]
        [SerializeField] private Sprite bottom;

        /** <summary>Small top segment of the tube</summary> */
        public Sprite Top => top;
        /** <summary>Bottom segment of the tube</summary> */
        public Sprite Bottom => bottom;
        /** <summary>Fill (center) segments of the tube</summary> */
        public Sprite Fill => fill;
    }
}