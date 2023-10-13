using System.Drawing;

namespace Tetris.Tetrominos
{
    abstract class Tetromino
    {
        protected readonly Point canvasSize;
        public Point pos = new Point(0, 0);

        public Tetromino(Point canvasSize)
        {
            this.canvasSize = canvasSize;
        }

        public abstract ConsoleColor Color
        {
            get;
        }

        public abstract bool[,] Render();
    }
}
