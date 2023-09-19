using Objects;
using UnityEngine;

namespace Behaviors
{
    /** <summary>Root Ball object.</summary> */
    public class Ball : MonoBehaviour
    {
        [Tooltip("Link to Sprite Renderer object.")]
        [SerializeField] private SpriteRenderer sprite;

        /** <summary>Enumeration containing the various states of movement</summary> */
        public enum BallState
        {
            /** <summary>Ball is sitting in its tube slot without movement</summary> */
            InTube, 
            /** <summary>Ball is selected from a tube, either moving above or sitting above</summary> */
            AboveTube, 
            /** <summary>Ball is translating from one tube to another</summary> */
            MovingToTube, 
            /** <summary>Ball is moving back into a tube after coming from another</summary> */
            MovingIntoTube
        }
        
        /** <summary>The ball set used for this level</summary> */
        private BallTheme _theme;
        /** <summary>The color id for the ball, equating to the theme's array</summary> */
        private int _color;
        /** <summary>State context for this ball</summary> */
        private StateContext _ballContext;

        /** <summary>Ball color ID</summary> */
        public int Color => _color;
        /** <summary>Current State of the ball</summary> */
        public BallState CurrentState => _ballContext.CurrentState.StateName;

        /**
         * <summary>Setup function for the ball. Should occur between Awake and Start</summary>
         * <param name="theme">The ball theme set</param>
         * <param name="color">The color ID for this ball</param>
         * <param name="startingTube">Initial tube this ball starts in</param>
         */
        public void Setup(BallTheme theme, int color)
        {
            _theme = theme;
            _color = color;
        }
        
        /** <summary>Unity Start function, begins the state machine</summary> */
        private void Start()
        {
            _ballContext = new StateContext();
            UpdateVisual();
        }

        /** <summary>Updates the visual to the correct sprite</summary> */
        private void UpdateVisual()
        {
            sprite.sprite = _theme.Balls[_color];
        }

        /** <summary>Context for the ball state machine</summary> */
        private class StateContext
        {
            /** <summary>Currently running state</summary> */
            private BaseState _currentState;

            /** <summary>Default constructor. Starts the default state</summary> */
            public StateContext()
            {
                _currentState = Default;
            }

            /** <summary>Currently running state. Setting will invoke Begin and End</summary> */
            public BaseState CurrentState => _currentState;

            /** <summary>Changes to a new state. This invokes begin and end</summary> */
            public void ChangeState(BallState state)
            {
                _currentState.EndState();
                _currentState = _states[(int)state];
                _currentState.BeginState(this);
            }
            
            /** <summary>Cached list of states</summary> */
            private readonly BaseState[] _states =
            {
                new InTubeState(),
                new AboveTubeState(),
                new MovingToTubeState(),
                new MovingIntoTubeState()
            };

            /** <summary>Default startup state</summary> */
            private BaseState Default => _states[0];
        }

        /** <summary>Base class for all ball states</summary> */
        private abstract class BaseState
        {
            /** <summary>Context for this state</summary> */
            private StateContext _context;

            /** <summary>Called when the state initially starts up</summary> */
            public virtual void BeginState(StateContext context) { _context = context; }
            /** <summary>Called in the unity update loop</summary> */
            public virtual void Update() { }
            /** <summary>Called when changing to a new state</summary> */
            public virtual void EndState() { }
            
            /** <summary>The currently running state</summary> */
            public abstract BallState StateName { get; }

            /** <summary>Link to the running context</summary> */
            protected StateContext Context => _context;
        }
        
        private class InTubeState : BaseState
        {
            public override BallState StateName => BallState.InTube;
        }

        private class AboveTubeState : BaseState
        {
            public override BallState StateName => BallState.AboveTube;
        }

        private class MovingToTubeState : BaseState
        {
            public override BallState StateName => BallState.MovingToTube;
        }

        private class MovingIntoTubeState : BaseState
        {
            public override BallState StateName => BallState.MovingIntoTube;
        }
    }
}