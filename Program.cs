﻿using System.Drawing;
using System.Linq;
using Tetris.Core;
using Tetris.Tetrominos;

namespace Tetris
{

    class TetrisGame
    {
        private static Point canvasSize = new Point(10, 20);

        private bool[,] filledPixels = new bool[canvasSize.X, canvasSize.Y];
        private ConsoleColor[,] pixelColors = new ConsoleColor[canvasSize.X, canvasSize.Y];

        private Tetromino? activeTetromino;
        private List<Tetromino> tetrominoQueue = new List<Tetromino>();

        private int moveTime;
        private System.Timers.Timer timer;

        private Renderer renderer;

        public TetrisGame()
        {
            timer = new System.Timers.Timer();
            //SetMoveTime(1500);
            SetMoveTime(300);
            timer.Elapsed += TickTetromino;

            Console.CursorVisible = false;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            renderer = new Renderer(canvasSize);
        }

        private void SetMoveTime(int moveTime)
        {
            this.moveTime = moveTime;
            timer.Interval = this.moveTime;
        }

        private Tetromino RandomTetromino()
        {
            Type[] opts = { typeof(OTetromino), typeof(LTetromino) };
            return Activator.CreateInstance(opts[new Random().Next(opts.Length)], canvasSize) as Tetromino;
        }

        private void TickTetromino(Object source, System.Timers.ElapsedEventArgs e)
        {
            int maxPos = canvasSize.Y - 1;

            if (activeTetromino != null)
            {
                if (activeTetromino.pos.Y + activeTetromino.Render().GetLength(1) < maxPos)
                    activeTetromino.pos.Y++;
                else if (tetrominoQueue.Count > 0)
                {
                    RenderTetromino(activeTetromino, filledPixels);

                    activeTetromino = tetrominoQueue[0];
                    tetrominoQueue.Add(RandomTetromino());
                    tetrominoQueue.RemoveAt(0);
                }
            }
        }

        private void RenderTetromino(Tetromino tetromino, bool[,] canvas)
        {
            bool[,] shape = activeTetromino.Render();
            ConsoleColor color = activeTetromino.Color;

            for (int i = 0; i < shape.GetLength(1); i++)
            {
                for (int j = 0; j < shape.GetLength(0); j++)
                {
                    int canvasX = activeTetromino.pos.X + i;
                    int canvasY = activeTetromino.pos.Y + j;

                    if (shape[j, i])
                    {
                        canvas[canvasX, canvasY] = true;
                        pixelColors[canvasX, canvasY] = color;
                    }
                }
            }
        }

        public void Run() {
            activeTetromino = RandomTetromino();
            Console.WriteLine(activeTetromino);
            tetrominoQueue.Add(RandomTetromino());

            timer.Enabled = true;

            while (true)
            {
                bool[,] canvas = new bool[canvasSize.X, canvasSize.Y];
                RenderTetromino(activeTetromino, canvas);

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

                renderer.renderQueue(tetrominoQueue);
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