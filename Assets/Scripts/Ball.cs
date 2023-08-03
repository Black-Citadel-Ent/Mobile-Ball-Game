using UnityEngine;

public class Ball : BaseBehavior
{
    [SerializeField] private BallSetup rootObject;
    [SerializeField] private Sprite[] spriteList;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        rootObject.MapInner(this);
        UpdateBall();
    }

    public void UpdateBall()
    {
        if (rootObject.BallID < 0 || rootObject.BallID >= spriteList.Length)
            spriteRenderer.sprite = null;
        else
            spriteRenderer.sprite = spriteList[rootObject.BallID];
    }
}
