using UnityEngine;

public class LevelSetup : BaseBehavior
{
    [SerializeField] private int colors;
    [SerializeField] private int extras;
    [SerializeField] private TubeSetup template;
    [SerializeField] private Camera mainCamera;

    private TubeSetup[] _tubes;
    
    private static readonly float TubeWidth = 1.0f;
    private static readonly float TubeHeight = 4.0f;
    private static readonly float Padding = 1.0f;

    private void Start()
    {
        BuildTubeSet();
    }

    private void BuildTubeSet()
    {
        if (_tubes != null && _tubes.Length != 0)
        {
            foreach(var t in _tubes)
                if(t) Destroy(t);
        }
        
        var total = colors + extras;
        _tubes = new TubeSetup[total];
        for (var i = 0; i < total; i++)
            _tubes[i] = Instantiate(template);
        
        Layout();
    }

    private void Layout()
    {
        var total = colors + extras;
        var rowCount = OptimizedRowCount;
        var colCount = total / rowCount + (total % rowCount == 0 ? 0 : 1);

        var width = colCount * TubeWidth + (colCount + 1) * Padding;
        var height = rowCount * TubeHeight + (rowCount + 1) * Padding;

        var rowPos = new float[rowCount];
        var colPos = new float[colCount];
        for (int i = 0; i < rowCount; i++)
            rowPos[i] = Padding * (i + 1) + TubeHeight * (i + 0.5f) - height / 2;
        for (int i = 0; i < colCount; i++)
            colPos[i] = Padding * (i + 1) + TubeWidth * (i + 0.5f) - width / 2;
        
        for (int i = 0; i < _tubes.Length; i++)
        {
            var row = i / colCount;
            var col = i % colCount;
            _tubes[i].transform.position = new Vector2(colPos[col], rowPos[row]);
        }

        var camHeight = height / 2;
        mainCamera.orthographicSize = camHeight;
        // TODO add safe area
        // TODO balance rows/col counts
    }

    private int OptimizedRowCount
    {
        get
        {
            var safe = Screen.safeArea;
            var aspect = safe.height / safe.width;
            var nearAspect = float.MaxValue;
            var bestRows = 1;

            // Brute force calculation - tired of thinking about this
            var total = colors + extras;
            for (int counter = 1; counter <= total; counter++)
            {
                var rows = (float)counter;
                var cols = Mathf.Ceil(total / (float)counter);
                var width = cols * TubeWidth + (cols + 1) * Padding;
                var height = rows * TubeHeight + (rows + 1) * Padding;
                var testAspect = height / width;
                var diff = testAspect - aspect;
                if (diff >= 0 && diff < nearAspect)
                {
                    nearAspect = diff;
                    bestRows = counter;
                }
            }

            return bestRows;
        }
    }
}
