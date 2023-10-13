using System.Drawing;
namespace Tetris.Tetrominos;

public class TTetromino : Tetromino
{
    public TTetromino(Point canvasSize) : base(canvasSize)
    {
    }

    public override ConsoleColor Color
    {
        get
        {
            return ConsoleColor.Magenta;
        }
    }

    public override bool[,] Render()
    {
        bool[,] shape = {
            { false, true, false },
            { true, true, true }
        };

        return shape;
    }
}
