using System.Drawing;
namespace Tetris.Tetrominos;

public class JTetromino : Tetromino
{
    public JTetromino(Point canvasSize) : base(canvasSize)
    {
    }

    public override ConsoleColor Color
    {
        get
        {
            return ConsoleColor.DarkBlue;
        }
    }

    public override bool[,] Render()
    {
        bool[,] shape = {
            { true, false, false },
            { true, true, true }
        };

        return shape;
    }
}
