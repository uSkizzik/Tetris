using System.Drawing;
using Tetris.Tetrominos;

namespace Tetris.Core;

public class Renderer
{
    private static readonly char fullTile = '■';
    private static readonly char emptyTile = '◦';
    private static readonly int borderSize = 2;
    
    private readonly Point matrixSize;
    private readonly int matrixOffsetLeft;
    private readonly int matrixOffsetRight;
    
    private readonly bool[,] bgCanvas;
    private readonly ConsoleColor[,] bgColorCanvas;
    
    public Renderer(Point matrixSize)
    {
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
    
    public void DrawTetromino(Tetromino tetromino, bool[,] canvas, ConsoleColor[,] colorCanvas, bool overrideTiles = false)
    {
        bool[,] shape = tetromino.Render();
        ConsoleColor color = tetromino.Color;

        // if (overrideTiles)
        // {
        //     for (int i = 0; i < canvas.GetLength(1); i++)
        //     {
        //         for (int j = 0; j < canvas.GetLength(0); j++)
        //             canvas[j, i] = false;
        //     }
        // }

        for (int i = 0; i < shape.GetLength(1); i++)
        {
            for (int j = 0; j < shape.GetLength(0); j++)
            {
                int canvasX = tetromino.pos.X + i;
                int canvasY = tetromino.pos.Y + j;

                if (shape[j, i])
                {
                    canvas[canvasX, canvasY] = true;
                    colorCanvas[canvasX, canvasY] = color;
                }
            }
        }
    }
    
    public void DrawQueue(List<Tetromino> queue)
    {
        int CursorLeft = matrixOffsetRight + 5;
        
        bool[,] canvas = new bool[4, 2];
        ConsoleColor[,] colorCanvas = new ConsoleColor[4, 2];
        
        Console.CursorTop = 0;
        Console.CursorLeft = CursorLeft;
        Console.Write("NEXT:");
        
        DrawTetromino(queue[0], canvas, colorCanvas, true);
        
        Console.CursorTop = 1;
        Console.CursorLeft = CursorLeft;
        Console.ForegroundColor = colorCanvas[0, 0];
        
        for (int i = 0; i < 4; i++)
            Console.Write(canvas[i, 0] ? fullTile + " " : "");
        
        Console.CursorTop = 2;
        Console.CursorLeft = CursorLeft;
        
        for (int i = 0; i < 4; i++)
            Console.Write(canvas[i, 1] ? fullTile + " " : "");
    }

    public void ClearEmptyLines()
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

    public void DrawFrame(Tetromino activeTetromino, List<Tetromino> tetrominoQueue)
    {
        bool[,] fgCanvas = new bool[matrixSize.X, matrixSize.Y];
        ConsoleColor[,] fgColorCanvas = new ConsoleColor[matrixSize.X, matrixSize.Y];
        
        DrawTetromino(activeTetromino, fgCanvas, fgColorCanvas);

        Console.CursorLeft = 0;
        Console.CursorTop = 0;

        int fieldOffsetLeft = Console.WindowWidth / 2 - matrixSize.X;
        int fieldOffsetRight = Console.WindowWidth - matrixSize.X * 2 - fieldOffsetLeft - 6;

        for (int y = 0; y <= matrixSize.Y; y++)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
                    
            Console.Write(new string(' ', fieldOffsetLeft));
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
            // Console.Write(new string(' ', fieldOffsetRight));

            Console.WriteLine();
        }

        Console.Write(new string(' ', fieldOffsetLeft + 2));
        Console.WriteLine(string.Concat(Enumerable.Repeat("\\/", matrixSize.X)));
        // Console.Write(new string(' ', fieldOffsetRight));

        DrawQueue(tetrominoQueue);
        ClearEmptyLines();
    }
}

