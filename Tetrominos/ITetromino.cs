using System.Drawing;
namespace Tetris.Tetrominos;

public class ITetromino : Tetromino
{
    public ITetromino(Point canvasSize) : base(canvasSize)
    {
    }

    public override ConsoleColor Color
    {
        get
        {
            return ConsoleColor.Cyan;
        }
    }

    public override bool[,] Render()
    {
        bool[,] shape = {
            { false, false, false, false },
            { true, true, true, true }
        };

        return shape;
    }
}
