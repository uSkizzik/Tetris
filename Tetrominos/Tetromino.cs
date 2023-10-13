using System.Drawing;
namespace Tetris.Tetrominos;

public enum ERotationState
{
    UP,
    RIGHT,
    DOWN,
    LEFT
}

public abstract class Tetromino
{
    protected readonly Point canvasSize;
    
    private Point position;
    private ERotationState rotation;
    
    public bool wasHeld;

    public Tetromino(Point canvasSize)
    {
        this.canvasSize = canvasSize;
        position = new(0, 0);
        wasHeld = false;
    }

    public abstract ConsoleColor Color
    {
        get;
    }

    public Point Position
    {
        get => position;
    }

    public ERotationState Rotation
    {
        get => rotation;
    }

    public void Tick()
    {
        position.Y++;
    }

    public void Reset()
    {
        position.X = 0;
        position.Y = 0;
    }

    public void Rotate(bool counterClockwise)
    {
        int newRot = (int) rotation + (counterClockwise ? -1 : 1);

        if (newRot < 0) newRot = 3;
        else if (newRot > 3) newRot = 0;
        
        rotation = (ERotationState) newRot;
    }
    
    public abstract bool[,] Render
    {
        get;
    }
}
