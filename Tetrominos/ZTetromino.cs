using System.Drawing;
namespace Tetris.Tetrominos;

public class ZTetromino : Tetromino
{
    public ZTetromino(Point canvasSize) : base(canvasSize)
    {
    }

    public override ConsoleColor Color
    {
        get
        {
            return ConsoleColor.Red;
        }
    }

    public override bool[,] Render()
    {
        bool[,] shape = {
            { true, true, false },
            { false, true, true }
        };

        return shape;
    }
}
