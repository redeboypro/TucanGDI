using System.Drawing;
using System.Drawing.Drawing2D;
using TucanGDI.Math;

namespace TucanGDI;

public class Sprite
{
    public readonly Bitmap Bitmap;
    
    private Matrix _matrix;
    private Vector2 _position;
    private float _rotationAngle;
    private float _scale;

    public Sprite(Bitmap bitmap)
    {
        _matrix = new Matrix();
        _position = Vector2.Zero;
        _rotationAngle = 0;
        _scale = 1;
        Bitmap = bitmap;
    }

    public Vector2 Position
    {
        get
        {
            return _position;
        }
        set
        {
            _position = value;
            Transform();
        }
    }
    
    public float Angle
    {
        get
        {
            return _rotationAngle;
        }
        set
        {
            _rotationAngle = value;
            Transform();
        }
    }
    
    public float Scale
    {
        get
        {
            return _scale;
        }
        set
        {
            _scale = value;
            Transform();
        }
    }

    private void Transform()
    {
        _matrix = new Matrix();
        _matrix.RotateAt(_rotationAngle, _position.ToPointF());
        _matrix.Scale(_scale, _scale);
    }

    public void Paint(Graphics graphics)
    {
        var width = Bitmap.Width;
        var height = Bitmap.Height;
        
        graphics.Transform = _matrix;
        graphics.DrawImage(Bitmap, Position.X - width * 0.5f, Position.Y - width * 0.5f, width, height);
    }
}