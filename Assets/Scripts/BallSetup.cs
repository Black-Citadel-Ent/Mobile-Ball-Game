using UnityEngine;

public class BallSetup : BaseBehavior, IBall
{
    [SerializeField] private int ballID;

    private Ball _inner;

    public int BallID
    {
        get => ballID;
        set
        {
            ballID = value;
            if(_inner)
                _inner.UpdateBall();
        }
    }

    public void MapInner(Ball b)
    {
        _inner = b;
    }

    public IBall.LocationStatus Status => _inner ? _inner.Status : IBall.LocationStatus.Locked;
}
