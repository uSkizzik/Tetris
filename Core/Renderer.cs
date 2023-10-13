using System.Drawing;
using Tetris.Tetrominos;

namespace Tetris.Core;

public class Renderer
{
    private static readonly char fullTile = '■';
    private static readonly char emptyTile = '◦';
    private static readonly int borderSize = 2;
    
    private readonly TetrisGame game;
    
    private readonly Point matrixSize;
    private readonly int matrixOffsetLeft;
    private readonly int matrixOffsetRight;
    
    private readonly bool[,] bgCanvas;
    private readonly ConsoleColor[,] bgColorCanvas;
    
    public Renderer(Point matrixSize, TetrisGame game)
    {
        this.game = game;
        this.matrixSize = matrixSize;
        
        matrixOffsetLeft = Console.WindowWidth / 2 - matrixSize.X;
        matrixOffsetRight = matrixSize.X * 2 + matrixOffsetLeft + borderSize + 2;

        bgCanvas = new bool[matrixSize.X, matrixSize.Y];
        bgColorCanvas = new ConsoleColor[matrixSize.X, matrixSize.Y];
    }

    public bool[,] BGCanvas
    {
        get => bgCanvas;
    }

    public ConsoleColor[,] BGColorCanvas
    {
        get => bgColorCanvas;
    }

    public void LockTetromino(Tetromino tetromino)
    {
        DrawTetromino(tetromino, bgCanvas, bgColorCanvas);
    }
    
    public void DrawTetromino(Tetromino tetromino, bool[,] canvas, ConsoleColor[,] colorCanvas, bool staticRender = false)
    {
        bool[,] shape = tetromino.GetShape();
        ConsoleColor color = tetromino.Color;

        for (int i = 0; i < shape.GetLength(1); i++)
        {
            for (int j = 0; j < shape.GetLength(0); j++)
            {
                int canvasX = i;
                int canvasY = j;

                if (!staticRender)
                {
                    canvasX += tetromino.Position.X;
                    canvasY += tetromino.Position.Y;
                }
                
                if (shape[j, i])
                {
                    canvas[canvasX, canvasY] = true;
                    colorCanvas[canvasX, canvasY] = color;
                }
            }
        }
    }

    private void DrawSidebar(int offsetX, string title, Tetromino? tetromino)
    {
        bool[,] canvas = new bool[4, 2];
        ConsoleColor[,] colorCanvas = new ConsoleColor[4, 2];
        
        Console.CursorTop = 0;
        Console.CursorLeft = offsetX;
        Console.ForegroundColor = ConsoleColor.Gray;
        
        Console.WriteLine(title + ":");
        Console.WriteLine();

        if (tetromino == null)
        {
            Console.CursorLeft = offsetX;
            Console.WriteLine("NONE");
            
            return;
        }
        
        DrawTetromino(tetromino, canvas, colorCanvas, true);
        Console.ForegroundColor = colorCanvas[0, 0];
        
        for (int y = 0; y < canvas.GetLength(1); y++)
        {
            Console.CursorLeft = offsetX;

            for (int x = 0; x < canvas.GetLength(0); x++)
            {
                bool isTileFilled = canvas[x, y];
                ConsoleColor tileColor = colorCanvas[x, y];
                
                Console.ForegroundColor = isTileFilled && tileColor != ConsoleColor.Black ? tileColor : ConsoleColor.Gray;
                Console.Write(isTileFilled ? fullTile + " " : "  ");
            }
            
            Console.WriteLine();
        }
    }
    
    private void DrawHeld(Tetromino? heldTetromino)
    {
        int cursorLeft = matrixOffsetLeft - 5 - 5;
        DrawSidebar(cursorLeft, "HELD", heldTetromino);
    }
    
    private void DrawQueue(List<Tetromino> queue)
    {
        int cursorLeft = matrixOffsetRight + 5;
        DrawSidebar(cursorLeft, "NEXT", queue[0]);
    }

    private void ClearEmptyLines()
    {
        for (int i = matrixSize.Y + 2; i < Console.WindowHeight; i++)
        {
            if (i >= Console.WindowHeight) break;
            int currentLineCursor = Console.CursorTop;
            
            Console.SetCursorPosition(0, i);
            
            Console.Write(new string(' ', Console.WindowWidth)); 
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }

    public void DrawFrame()
    {
        bool[,] fgCanvas = new bool[matrixSize.X, matrixSize.Y];
        ConsoleColor[,] fgColorCanvas = new ConsoleColor[matrixSize.X, matrixSize.Y];
        
        DrawTetromino(game.ActiveTetromino, fgCanvas, fgColorCanvas);
        
        Console.CursorTop = 0;
        
        for (int y = 0; y <= matrixSize.Y; y++)
        {
            Console.CursorLeft = matrixOffsetLeft;
            Console.ForegroundColor = ConsoleColor.Gray;
            
            Console.Write("<!");
        
            if (y < matrixSize.Y)
            {
                for (int x = 0; x < matrixSize.X; x++)
                {
                    bool isTileFilled = fgCanvas[x, y] || bgCanvas[x, y];
                    ConsoleColor tileColor = fgColorCanvas[x, y] != ConsoleColor.Black ? fgColorCanvas[x, y] : bgColorCanvas[x, y];
        
                    Console.ForegroundColor = isTileFilled && tileColor != ConsoleColor.Black ? tileColor : ConsoleColor.Gray;
                    Console.Write(isTileFilled ? fullTile + " " : emptyTile + " ");
                }
            }
            else
            {
                Console.Write(new string('=', matrixSize.X * 2));
            }
        
            Console.Write("!>");
            Console.WriteLine();
        }
        
        Console.CursorLeft = matrixOffsetLeft + 2;
        Console.WriteLine(string.Concat(Enumerable.Repeat("\\/", matrixSize.X)));
        
        DrawHeld(game.HeldTetromino);
        DrawQueue(game.TetrominoQueue);
        
        ClearEmptyLines();
    }
}

