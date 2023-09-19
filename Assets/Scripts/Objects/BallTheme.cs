using UnityEngine;

namespace Objects
{
    /** <summary>Container for themed sets of ball sprites</summary> */
    [CreateAssetMenu(fileName = "Ball Theme", menuName = "Ball Game/Ball Theme")]
    public class BallTheme : ScriptableObject
    {
        [Tooltip("List of balls that appear in this set. Must have at least 8 to prevent errors.")]
        [SerializeField] private Sprite[] balls;

        /** <summary>Array of balls in this container</summary> */
        public Sprite[] Balls => balls;
    }
}