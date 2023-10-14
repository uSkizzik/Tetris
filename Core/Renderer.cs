using System.Drawing;
using Tetris.Screens;
using Tetris.Tetrominos;

namespace Tetris.Core;

public class Renderer : IScreen
{
    private static readonly char fullTile = '■';
    private static readonly char emptyTile = '◦';
    private static readonly int borderSize = 2;
    
    private readonly TetrisGame game;
    
    private readonly Point matrixSize;
    private readonly Point visibilityOffset;
    
    private readonly int matrixOffsetLeft;
    private readonly int matrixOffsetRight;
    
    private bool[,] bgCanvas;
    private ConsoleColor[,] bgColorCanvas;
    
    public Renderer(Point matrixSize, Point visibilityOffset, TetrisGame game)
    {
        this.game = game;
        this.matrixSize = matrixSize;
        this.visibilityOffset = visibilityOffset;
        
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

    private void DrawSidebarTitle(int offsetX, string title)
    {
        Console.CursorTop = 0;
        Console.CursorLeft = offsetX;
        Console.ForegroundColor = ConsoleColor.Gray;
        
        Console.WriteLine(title + ":");
        Console.WriteLine();
    }
    
    private void DrawSidebarItem(int offsetX, Tetromino? tetromino)
    {
        bool[,] canvas = new bool[4, 4];
        ConsoleColor[,] colorCanvas = new ConsoleColor[4, 4];

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
        int cursorLeft = matrixOffsetLeft - 5 - 7;
        
        DrawSidebarTitle(cursorLeft, "HELD");
        DrawSidebarItem(cursorLeft, heldTetromino);
    }
    
    private void DrawQueue(List<Tetromino> queue)
    {
        int cursorLeft = matrixOffsetRight + 5;
        DrawSidebarTitle(cursorLeft, "NEXT");
        
        if (queue.Count != 0)
        {
            DrawSidebarItem(cursorLeft, queue[0]);
            Console.WriteLine();

            DrawSidebarItem(cursorLeft, queue[1]);
            Console.WriteLine();

            DrawSidebarItem(cursorLeft, queue[2]);
        }
        else
        {
            Console.CursorLeft = cursorLeft;
            Console.Write("NONE");
        }
    }

    private void ClearEmptyLines(int startingY)
    {
        for (int i = startingY; i < Console.WindowHeight; i++)
        {
            if (i >= Console.WindowHeight) break;
            int currentLineCursor = Console.CursorTop;
            
            Console.SetCursorPosition(0, i);
            
            Console.Write(new string(' ', Console.WindowWidth)); 
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }

    private T[,] ClearCanvas<T>(List<int> rows, T[,] canvas, T defaultValue)
    {
        int numCols = bgCanvas.GetLength(0);
        int numRows = bgCanvas.GetLength(1);
        T[,] newArray = new T[numCols, numRows];

        int newRow = numRows - 1;
        
        for (int i = numRows - 1; i >= rows.Count; i--)
        {
            if (rows.Contains(i)) continue;
                
            for (int j = 0; j < numCols; j++)
                newArray[j, newRow] = canvas[j, i];

            newRow--;
        }

        for (int i = 0; i < rows.Count; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                newArray[j, i] = defaultValue;
            }
        }

        return newArray;
    }
    
    public void ClearRows(List<int> rows)
    {
        bgCanvas = ClearCanvas(rows, bgCanvas, false);
        bgColorCanvas = ClearCanvas(rows, bgColorCanvas, ConsoleColor.Black);
    }

    public void DrawFrame()
    {
        bool[,] fgCanvas = new bool[matrixSize.X, matrixSize.Y];
        ConsoleColor[,] fgColorCanvas = new ConsoleColor[matrixSize.X, matrixSize.Y];
        
        if (game.ActiveTetromino != null) 
            DrawTetromino(game.ActiveTetromino, fgCanvas, fgColorCanvas);
        
        Console.CursorTop = 0;

        int matrixY = matrixSize.Y + visibilityOffset.Y;
        
        for (int y = visibilityOffset.Y * -1; y <= matrixSize.Y; y++)
        {
            Console.CursorLeft = matrixOffsetLeft;
            Console.ForegroundColor = ConsoleColor.Gray;
            
            Console.Write("<!");
        
            if (y >= 0 && y - visibilityOffset.Y * -1 < matrixY)
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
                if (y < 0) Console.Write(string.Concat(Enumerable.Repeat(emptyTile + " ", matrixSize.X)));
                else Console.Write(new string('=', matrixSize.X * 2));
            }
        
            Console.ForegroundColor = ConsoleColor.Gray; // RIP my fav b-u-g. I'll always remember you.
            
            Console.Write("!>");
            Console.WriteLine();
        }
        
        Console.CursorLeft = matrixOffsetLeft + 2;
        Console.WriteLine(string.Concat(Enumerable.Repeat("\\/", matrixSize.X)));
        
        DrawHeld(game.HeldTetromino);
        DrawQueue(game.TetrominoQueue);
        
        ClearEmptyLines(matrixY + 2);
    }

    public void OnScreenChange()
    {
        Console.Clear();
    }
}

