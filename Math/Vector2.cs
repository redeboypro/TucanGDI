using System.Drawing;

namespace TucanGDI.Math;

public struct Vector2
{
    public static readonly Vector2 Zero = new (0);
    public static readonly Vector2 One = new (1);
    public static readonly Vector2 Right = new (1, 0);
    public static readonly Vector2 Left = new (-1, 0);
    public static readonly Vector2 Up = new (0, 1);
    public static readonly Vector2 Down = new (0, -1);
    
    public float X;
    public float Y;

    public Vector2() : this(0) {}

    public Vector2(float scalar) : this(scalar, scalar) {}

    public Vector2(float x, float y)
    {
        X = x;
        Y = y;
    }

    public PointF ToPointF()
    {
        return new PointF(X, Y);
    }
}