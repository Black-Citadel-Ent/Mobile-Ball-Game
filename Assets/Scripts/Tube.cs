using UnityEngine;

public class Tube : BaseBehavior
{
    [SerializeField] private TubeSetup rootObject;
    [SerializeField] private TubeLocation[] locations;

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
}
