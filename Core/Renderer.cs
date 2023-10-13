using System.Drawing;
using Tetris.Tetrominos;

namespace Tetris.Core;

public class Renderer
{
    private readonly Point matrixSize;
    private static int borderSize = 2;
    
    private readonly int matrixOffsetLeft;
    private readonly int matrixOffsetRight;

    private static char fullTile = '■';
    private static char emptyTile = '◦';

    public Renderer(Point matrixSize)
    {
        this.matrixSize = matrixSize;
        this.matrixOffsetLeft = Console.WindowWidth / 2 - matrixSize.X;
        this.matrixOffsetRight = matrixSize.X * 2 + matrixOffsetLeft + borderSize + 2;
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
}

