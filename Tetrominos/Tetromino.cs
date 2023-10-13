using System.Drawing;
namespace Tetris.Tetrominos;

public abstract class Tetromino
{
    protected readonly Point canvasSize;
    private Point position = new (0, 0);

    public Tetromino(Point canvasSize)
    {
        this.canvasSize = canvasSize;
    }

    public abstract ConsoleColor Color
    {
        get;
    }

    public Point Position
    {
        get => position;
    }

    public void Reset()
    {
        position.X = 0;
        position.Y = 0;
    }

    public void Tick()
    {
        position.Y++;
    }

    public abstract bool[,] Render();
}
