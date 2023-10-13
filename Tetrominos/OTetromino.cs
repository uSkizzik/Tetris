using System.Drawing;
namespace Tetris.Tetrominos;

public class OTetromino : Tetromino
{
    public OTetromino(Point canvasSize) : base(canvasSize)
    {
    }

    public override ConsoleColor Color
    {
        get => ConsoleColor.Yellow;
    }

    public override bool[,] Render
    {
        get
        {
            return new [,]
            {
                { true, true },
                { false, true }
            };
        }
    }
}

