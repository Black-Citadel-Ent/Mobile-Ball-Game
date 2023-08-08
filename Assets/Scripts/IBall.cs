
public interface IBall 
{
    public enum LocationStatus { Placed, Locked, Transit }
    
    public int BallID { get; set; }
    public LocationStatus Status { get; }
}
