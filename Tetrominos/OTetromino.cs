using System.Drawing;
namespace Tetris.Tetrominos;

public class OTetromino : Tetromino
{
    public OTetromino(Point canvasSize) : base(canvasSize)
    {
    }

    public override ConsoleColor Color
    {
        get
        {
            return ConsoleColor.Yellow;
        }
    }

    public override bool[,] Render()
    {
        bool[,] shape = {
            { true, true },
            { true, true }
        };

        return shape;
    }
}

