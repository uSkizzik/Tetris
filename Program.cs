using System.Drawing;
using Tetris.Core;
using Tetris.Tetrominos;

namespace Tetris
{

    class TetrisGame
    {
        private static readonly Point canvasSize = new Point(10, 20);

        private Tetromino? activeTetromino;
        private List<Tetromino> tetrominoQueue = new List<Tetromino>();

        private int moveTime;
        private readonly System.Timers.Timer timer;

        private readonly Randomizer randomizer;
        private readonly Renderer renderer;

        public TetrisGame()
        {
            timer = new System.Timers.Timer();
            //SetMoveTime(1500);
            SetMoveTime(300);
            timer.Elapsed += TickTetromino;

            Console.CursorVisible = false;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            randomizer = new Randomizer(canvasSize);
            renderer = new Renderer(canvasSize);
        }

        private void SetMoveTime(int moveTime)
        {
            this.moveTime = moveTime;
            timer.Interval = this.moveTime;
        }

        private void TickTetromino(Object source, System.Timers.ElapsedEventArgs e)
        {
            int maxPos = canvasSize.Y;

            if (activeTetromino != null)
            {
                if (activeTetromino.pos.Y + activeTetromino.Render().GetLength(0) < maxPos)
                    activeTetromino.pos.Y++;
                else if (tetrominoQueue.Count > 0)
                {
                    renderer.LockTetromino(activeTetromino);

                    activeTetromino = tetrominoQueue[0];
                    tetrominoQueue.Add(randomizer.RandomTetromino());
                    tetrominoQueue.RemoveAt(0);
                }
            }
        }

        public void Run() {
            activeTetromino = randomizer.RandomTetromino();
            tetrominoQueue.Add(randomizer.RandomTetromino());

            timer.Enabled = true;
            // SongPlayer.PlayThemeSong();
            
            while (true)
            {
                renderer.DrawFrame(activeTetromino, tetrominoQueue);
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