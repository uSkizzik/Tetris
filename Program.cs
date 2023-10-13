using System.Drawing;
using Tetris.Tetrominos;

namespace Tetris
{

    class TetrisGame
    {
        private static Point canvasSize = new Point(10, 20);

        private bool[,] filledPixels = new bool[canvasSize.X, canvasSize.Y];
        private ConsoleColor[,] pixelColors = new ConsoleColor[canvasSize.X, canvasSize.Y];

        private Tetrominos.Tetromino? activeTetromino;
        private Point tetrominoPos = new Point(0, 0);

        private int moveTime;
        private System.Timers.Timer timer;

        public TetrisGame()
        {
            timer = new System.Timers.Timer();
            SetMoveTime(1500);
            timer.Elapsed += TickTetromino;

            Console.CursorVisible = false;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }

        private void SetMoveTime(int moveTime)
        {
            this.moveTime = moveTime;
            timer.Interval = this.moveTime;
        }

        private void TickTetromino(Object source, System.Timers.ElapsedEventArgs e)
        {
            int maxPos = canvasSize.Y - 1;

            if (activeTetromino != null && tetrominoPos.Y + activeTetromino.Render().GetLength(1) < maxPos)
                tetrominoPos.Y++;
        }

        public void Run() {
            timer.Enabled = true;
            activeTetromino = new LTetromino(canvasSize);

            while (true)
            {
                bool[,] canvas = new bool[canvasSize.X, canvasSize.Y];
                bool[,] tetromino = activeTetromino.Render();
                ConsoleColor tetrominoColor = activeTetromino.Color;

                for (int i = 0; i < tetromino.GetLength(1); i++)
                {
                    for (int j = 0; j < tetromino.GetLength(0); j++)
                    {
                        int canvasX = tetrominoPos.X + i;
                        int canvasY = tetrominoPos.Y + j;

                        if (tetromino[j, i])
                        {
                            canvas[canvasX, canvasY] = true;
                            pixelColors[canvasX, canvasY] = tetrominoColor;
                        }
                    }
                }

                Console.CursorLeft = 0;
                Console.CursorTop = 0;

                int fieldOffsetLeft = Console.WindowWidth / 2 - canvasSize.X;
                int fieldOffsetRight = Console.WindowWidth - canvasSize.X * 2 - fieldOffsetLeft - 6;

                for (int y = 0; y <= canvasSize.Y; y++)
                {
                    Console.Write(new string(' ', fieldOffsetLeft));
                    Console.Write("<!");

                    if (y < canvasSize.Y)
                    {
                        for (int x = 0; x < canvasSize.X; x++)
                        {
                            bool isPixelFilled = canvas[x, y] || filledPixels[x, y];

                            Console.ForegroundColor = isPixelFilled && pixelColors[x, y] != ConsoleColor.Black ? pixelColors[x, y] : ConsoleColor.Gray;
                            Console.Write(isPixelFilled ? "■ " : "◦ ");
                        }
                    }
                    else
                    {
                        Console.Write(new string('=', canvasSize.X * 2));
                    }

                    Console.Write("!>");
                    Console.Write(new string(' ', fieldOffsetRight));

                    Console.WriteLine();
                }

                Console.Write(new string(' ', fieldOffsetLeft + 2));
                Console.WriteLine(string.Concat(Enumerable.Repeat("\\/", canvasSize.X)));
                Console.Write(new string(' ', fieldOffsetRight));
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            TetrisGame game = new TetrisGame();
            game.Run();
        }
    }
}