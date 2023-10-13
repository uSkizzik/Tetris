using System.Drawing;
using Tetris.Tetrominos;

namespace Tetris.Core
{
    class Renderer
    {
        private Point fieldSize;
        private const int borderSize = 2;

        public Renderer(Point fieldSize)
        {
            this.fieldSize = fieldSize;
        }

        private int getFieldOffset()
        {
            return Console.WindowWidth / 2 - fieldSize.X;
        }

        public void renderQueue(List<Tetromino> queue) {
            Console.CursorTop = 0;
            Console.CursorLeft = getFieldOffset() + fieldSize.X + borderSize + 5;

            Console.Write("######");
        }
    }
}
