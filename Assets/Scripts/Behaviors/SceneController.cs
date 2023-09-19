using Generator;
using Layout;
using Objects;
using UnityEngine;

namespace Behaviors
{
    /** <summary>Root controller for game scene</summary> */
    public class SceneController : MonoBehaviour
    {
        [Tooltip("Ball Set  to use for the level.")]
        [SerializeField] private BallTheme ballTheme;
        [Tooltip("Tube Set to use for the level.")]
        [SerializeField] private TubeTheme tubeTheme;
        [Tooltip("Template for the ball object.")]
        [SerializeField] private Ball ballTemplate;
        [Tooltip("Template for the tube object.")]
        [SerializeField] private Tube tubeTemplate;

        private readonly BallLayout _layout = new();
        
        /** <summary>Unity start function, instantiates objects.</summary> */
        private void Start()
        {
            var g = new Generator.Generator(4, 4);
            CreateTubes(g.Execute());
        }

        /**
         * <summary>Builds a set of tubes based on the generated objects</summary>
         * <param name="set">Tube set that comes from the generator</param>
         */
        private void CreateTubes(TubeSet set)
        {
            var setup = set.Tubes;
            for (var tube = 0; tube < setup.Length; tube++)
            {
                var t = Instantiate(tubeTemplate, new Vector3(tube * 2, 0, 0), Quaternion.identity);
                t.Setup(setup[tube].Length, tubeTheme);
                _layout.Add(t);
                
                for (var ball = 0; ball < setup[tube].Length; ball++)
                {
                    if(setup[tube][ball] == -1) continue;
                    var b = Instantiate(ballTemplate, t.PositionOf(ball), Quaternion.identity);
                    b.Setup(ballTheme, setup[tube][ball]);
                    _layout.Add(b, t, ball);
                }
            }
        }
    }
}