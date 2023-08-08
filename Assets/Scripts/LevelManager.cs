using System.Collections.Generic;
using UnityEngine;

public class LevelManager : BaseBehavior, ILevel
{
    [SerializeField] private TubeSetup tubeTemplate;
    [SerializeField] private BallSetup ballTemplate;

    private ITube[] _tubes;
    private IBall[] _balls;

    private void Start()
    {
        _balls = CreateBallSet(2, ballTemplate);
    }
    
    public void ClickTube(ITube t)
    {
        
    }
    
    private static IBall[] CreateBallSet(int colors, BallSetup ballTemplate)
    {
        List<int> colorList = new();
        for (int c = 0; c < colors; c++)
        for(int i=0; i < 4; i++)
            colorList.Add(c);

        List<int> shuffledColorList = new();
        while (colorList.Count != 0)
        {
            var rnd = Random.Range(0, colorList.Count);
            shuffledColorList.Add(colorList[rnd]);
            colorList.RemoveAt(rnd);
        }

        IBall[] balls = new IBall[shuffledColorList.Count];
        for (int counter = 0; counter < shuffledColorList.Count; counter++)
        {
            balls[counter] = Instantiate(ballTemplate);
            balls[counter].BallID = shuffledColorList[counter];
        }

        return balls;
    }

    private static int CalculateRequiredTubes(IBall[] balls)
    {
        return 0;
    }
}
