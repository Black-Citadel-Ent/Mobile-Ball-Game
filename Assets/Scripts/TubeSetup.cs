public class TubeSetup : BaseBehavior, ITube
{
    private Tube _inner;

    public void RegisterInner(Tube t)
    {
        _inner = t;
    }

    public int TopBallType => _inner ? _inner.TopBallType : -1;
}
