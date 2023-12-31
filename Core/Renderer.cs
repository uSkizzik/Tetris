﻿using System.Drawing;
using System.Xml.Schema;
using Tetris.Screens;
using Tetris.Tetrominos;

namespace Tetris.Core;

public class Renderer : IScreen
{
    private static readonly char fullTile = '■';
    private static readonly char halfTile = '#';
    private static readonly char emptyTile = '◦';
    private static readonly int borderSize = 2;
    
    private readonly TetrisGame game;
    
    private readonly Point matrixSize;
    private readonly Point visibilityOffset;

    private int windowWidth;
    private int windowHeight;
    
    private bool[,] bgCanvas;
    private ConsoleColor[,] bgColorCanvas;
    
    public Renderer(Point matrixSize, Point visibilityOffset, TetrisGame game)
    {
        this.game = game;
        this.matrixSize = matrixSize;
        this.visibilityOffset = visibilityOffset;

        bgCanvas = new bool[matrixSize.X, matrixSize.Y];
        bgColorCanvas = new ConsoleColor[matrixSize.X, matrixSize.Y];

        RefreshScreenSize(Console.WindowWidth, Console.WindowHeight);
    }

    public bool[,] BGCanvas
    {
        get => bgCanvas;
    }

    public ConsoleColor[,] BGColorCanvas
    {
        get => bgColorCanvas;
    }

    private int matrixOffsetLeft
    {
        get => windowWidth / 2 - matrixSize.X;
    }

    private int matrixOffsetRight
    {
        get => matrixSize.X * 2 + matrixOffsetLeft + borderSize + 2;
    }

    public void RefreshScreenSize(int width, int height)
    {
        windowWidth = width;
        windowHeight = height;
    }
    
    public void LockTetromino(Tetromino tetromino)
    {
        DrawTetromino(tetromino, bgCanvas, bgColorCanvas);
    }

    public void DrawTetromino(Tetromino tetromino, bool[,] canvas, ConsoleColor[,] colorCanvas, bool staticRender = false)
    {
        DrawTetromino(tetromino, canvas, colorCanvas, tetromino.Position, staticRender);
    }

    public void DrawGhostTetromino(Tetromino tetromino, bool[,] canvas, ConsoleColor[,] colorCanvas, bool staticRender = false)
    {
        DrawTetromino(tetromino, canvas, colorCanvas, tetromino.GhostPosition, staticRender);
    }
    
    private void DrawTetromino(Tetromino tetromino, bool[,] canvas, ConsoleColor[,] colorCanvas, Point position, bool staticRender = false)
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
                    canvasX += position.X;
                    canvasY += position.Y;
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

    private void DrawTitle()
    {
        string title = "TETRIS";
        Console.CursorLeft = windowWidth / 2 - title.Length / 2 + 1;

        for (int i = 0; i < title.Length; i++)
        {
            Console.ForegroundColor = MainMenu.Colors[i];
            Console.Write(title[i]);
        }

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine();
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
    
    private void DrawScore()
    {
        int cursorLeft = matrixOffsetLeft - 5 - 7;
        
        Console.CursorTop = 6;
        Console.ForegroundColor = ConsoleColor.Gray;
        
        Console.CursorLeft = cursorLeft;
        Console.Write("SCORE:");
        Console.WriteLine();
        
        Console.CursorLeft = cursorLeft;
        Console.Write(game.ScoreTracker.Score);
        Console.WriteLine();
    }
    
    private void DrawLevelProgress()
    {
        int cursorLeft = matrixOffsetLeft - 5 - 7;
        
        Console.CursorTop = 4 + 6;
        Console.ForegroundColor = ConsoleColor.Gray;
        
        Console.CursorLeft = cursorLeft;
        Console.Write("LEVEL:");
        Console.WriteLine();
        
        Console.CursorLeft = cursorLeft;
        Console.Write(game.ScoreTracker.Level);
        Console.WriteLine();
        
        Console.WriteLine();

        Console.CursorLeft = cursorLeft;
        Console.Write("PROGRESS:");
        Console.WriteLine();
        
        Console.CursorLeft = cursorLeft;
        
        Console.Write(game.ScoreTracker.LevelProgress);
        Console.Write(" / ");
        Console.Write(game.ScoreTracker.LineClearsRequired);
        
        Console.WriteLine();
    }

    public void ClearCanvas()
    {
        for (int x = 0; x < BGCanvas.GetLength(0); x++)
        {
            for (int y = 0; y < BGCanvas.GetLength(1); y++)
            {
                BGCanvas[x, y] = false;
                BGColorCanvas[x, y] = ConsoleColor.Black;
            }
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
        bool[,] ghostCanvas = new bool[matrixSize.X, matrixSize.Y];
        ConsoleColor[,] fgColorCanvas = new ConsoleColor[matrixSize.X, matrixSize.Y];
        
        if (game.ActiveTetromino != null)
        {
            DrawTetromino(game.ActiveTetromino, fgCanvas, fgColorCanvas);
            DrawGhostTetromino(game.ActiveTetromino, ghostCanvas, fgColorCanvas);
        }
        
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
        
                    Console.ForegroundColor = (ghostCanvas[x, y] || isTileFilled) && tileColor != ConsoleColor.Black ? tileColor : ConsoleColor.Gray;
                    
                    if (isTileFilled) Console.Write(fullTile + " ");
                    else if (ghostCanvas[x, y]) Console.Write(halfTile + " ");
                    else Console.Write(emptyTile + " ");
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

        Console.WriteLine();
        DrawTitle();

        if (game.IsGameOver)
        {
            string text = "GAME OVER";
            Console.WriteLine();
            
            Console.CursorLeft = windowWidth / 2 - text.Length / 2 + 1;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);

            string score = "SCORE: " + game.ScoreTracker.Score;
            Console.CursorLeft = windowWidth / 2 - score.Length / 2 + 1;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(score);
        }
        
        DrawHeld(game.HeldTetromino);
        DrawQueue(game.TetrominoQueue);
        
        DrawScore();
        DrawLevelProgress();
        
        string instruction = "Press ESC to return to menu";
        Console.CursorTop = windowHeight - 2;
        Console.CursorLeft = windowWidth / 2 - instruction.Length / 2 + 1;
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(instruction);
    }
}

