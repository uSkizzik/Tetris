using System.Drawing;

namespace Tetris.Tetrominos
{
    abstract class Tetromino
    {
        protected readonly Point canvasSize;

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
