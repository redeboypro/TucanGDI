namespace TucanGDI;

public class Time
{
    private DateTime _lastTime, _currentTime;
    private float _frameTime;
    private int _fpsProc;

    public Time()
    {
        _lastTime = DateTime.Now;
        _currentTime = DateTime.Now;
        _frameTime = 0.0f;
        _fpsProc = 0;
        DeltaTime = float.Epsilon;
    }

    public float DeltaTime { get; private set; }
        
    public int FramesPerSecond { get; private set; }

    public void Begin()
    {
        _currentTime = DateTime.Now;
        DeltaTime = (_currentTime.Ticks - _lastTime.Ticks) / 10000000f;
            
        _frameTime += DeltaTime;
        _fpsProc++;

        if (_frameTime >= 1.0f)
        {
            FramesPerSecond = _fpsProc;
            _frameTime = 0.0f;
            _fpsProc = 0;
        }
    }
        
    public void End()
    {
        _lastTime = _currentTime;
    }
}