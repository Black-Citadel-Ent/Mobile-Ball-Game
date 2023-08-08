using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class Tube : BaseBehavior
{
    [SerializeField] private TubeSetup rootObject;
    [SerializeField] private TubeLocation[] locations;
    [SerializeField] private Collider2D touchCollider;

    // Prevents occasional double tap - 0.05 seconds
    private float _tapHoldTime;
    
    private void Start()
    {
        rootObject.RegisterInner(this);
    }

    public int TopBallType
    {
        get
        {
            foreach (var loc in locations)
            {
                if (loc.Ball)
                    return loc.Ball.BallID;
            }

            return -1;
        }
    }

    public void OnTap(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<TouchState>();
        if (!Camera.main || value.phase != TouchPhase.Began || Time.time < _tapHoldTime)
            return;
        
        var pos = (Vector2)Camera.main.ScreenToWorldPoint(value.position);
        if (touchCollider.bounds.Contains(pos))
        {
            _tapHoldTime = Time.time + 0.05f;
            print("Touch");
        }
    }
}
