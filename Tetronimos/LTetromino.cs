using System.Drawing;

namespace Tetris.Tetrominos
{
    class LTetromino : Tetromino
    {
        public LTetromino(Point canvasSize) : base(canvasSize)
        {
        }

        public override ConsoleColor Color
        {
            get
            {
                return ConsoleColor.DarkYellow;
            }
        }

        public override bool[,] Render()
        {
            bool[,] shape = {
                { true, false },
                { true, false },
                { true, true }
            };

            return shape;
        }
    }
}
