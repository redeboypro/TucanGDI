using System.Drawing;
using System.Runtime.InteropServices;

namespace TucanGDI;

public abstract class GDIContext : IDisposable
{
    #region [ExternalLibs]
    private const string Kernel = "kernel32.dll";
    private const string User = "user32.dll";
    private const string WinGDI = "gdi32.dll";
    #endregion
    
    #region [ExternFunctions]
    [DllImport(Kernel, ExactSpelling = true)]
    private static extern IntPtr GetConsoleWindow();

    [DllImport(User, SetLastError = true)]
    private static extern IntPtr GetDC(IntPtr winHandle);

    [DllImport(User)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ReleaseDC(IntPtr winHandle, IntPtr deviceContextHandle);

    [DllImport(WinGDI)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool BitBlt(
        IntPtr deviceContextHandle, 
        int xDest, int yDest,
        int nWidth, int nHeight,
        IntPtr hdcSrc,
        int xSrc, int ySrc,
        TernaryRasterOperation rasterOperation);
    
    [DllImport(WinGDI, SetLastError = true)]
    private static extern IntPtr CreateCompatibleDC(IntPtr deviceContextHandle);

    [DllImport(WinGDI, SetLastError = true)]
    private static extern IntPtr SelectObject(IntPtr deviceContextHandle, IntPtr objHandle);
    
    [DllImport(User)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetWindowRect(IntPtr winHandle, out Rectangle rectangle);
    #endregion

    private Bitmap? _backgroundBitmap;
    private bool _isRunning;

    protected GDIContext()
    {
        var consoleWindow = GetConsoleWindow();

        if (GetWindowRect(consoleWindow, out var rect))
        { 
            return;
        }
        
        var consoleDeviceContextHandle = GetDC(consoleWindow);

        _isRunning = true;
        var time = new Time();
        
        using (_backgroundBitmap)
        {
            if (_backgroundBitmap != null)
            {
                using var blackGraphics = Graphics.FromImage(_backgroundBitmap);
                using var backgroundGraphics = Graphics.FromHdc(consoleDeviceContextHandle);
                
                Load();
                
                while (_isRunning)
                {
                    time.Begin();
                    var displayWidth = rect.Width;
                    var displayHeight = rect.Height;
                        
                    backgroundGraphics.DrawImage(_backgroundBitmap, 0, 0, displayWidth, displayHeight);
                    Paint(time, backgroundGraphics);

                    var backgroundDeviceContext = backgroundGraphics.GetHdc();
                    BitBlt(
                        backgroundDeviceContext,
                        0, 0,
                        displayWidth, displayHeight,
                        blackGraphics.GetHdc(),
                        0, 0,
                        TernaryRasterOperation.SrcCopy);

                    time.End();
                }
            }
        }

        ReleaseDC(consoleWindow, consoleDeviceContextHandle);
    }

    ~GDIContext()
    {
        Dispose(false);
    }

    public void SetBackground(Bitmap? bitmap)
    {
        _backgroundBitmap = bitmap;
    }
    
    protected abstract void Load();
    
    protected abstract void Paint(Time time, Graphics graphics);

    public void Stop()
    {
        _isRunning = false;
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _backgroundBitmap?.Dispose();
        }
    }
}