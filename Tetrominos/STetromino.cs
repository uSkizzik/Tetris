using System.Drawing;
namespace Tetris.Tetrominos;

public class STetromino : Tetromino
{
    public STetromino(Point canvasSize) : base(canvasSize)
    {
    }

    public override ConsoleColor Color
    {
        get
        {
            return ConsoleColor.Green;
        }
    }

    public override bool[,] Render()
    {
        bool[,] shape = {
            { false, true, true },
            { true, true, false }
        };

        return shape;
    }
}
