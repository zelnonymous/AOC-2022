using System.Numerics;

class Knot
{
    private Vector2 _currentPosition;
    public List<Vector2> VistedPositions { get; }
    public Vector2 CurrentPosition { 
        get { return _currentPosition; }
    }
    public Knot(Vector2 startingPosition)
    {
        _currentPosition = startingPosition;
        VistedPositions = new List<Vector2>() { startingPosition };
    }
    public void Move(Vector2 newPosition)
    {
        _currentPosition = newPosition;
        if (!VistedPositions.Contains(_currentPosition))
            VistedPositions.Add(_currentPosition);
    }
}