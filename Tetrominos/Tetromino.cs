using System.Drawing;
using Tetris.Core;

namespace Tetris.Tetrominos;

public enum ERotationState
{
    UP,
    RIGHT,
    DOWN,
    LEFT
}

public enum EMoveDirecton
{
    RIGHT,
    DOWN,
    LEFT,
    HARD_DROP
}

public abstract class Tetromino
{
    protected readonly Point canvasSize;
    
    private Point position;
    private ERotationState rotation;
    public bool wasHeld;

    private readonly AudioPlayer audioPlayer;

    public Tetromino(Point canvasSize, AudioPlayer audioPlayer)
    {
        this.canvasSize = canvasSize;
        this.audioPlayer = audioPlayer;
        
        position = new(0, 0);
        wasHeld = false;
    }

    public abstract ConsoleColor Color
    {
        get;
    }

    public Point Position
    {
        get => position;
    }

    public ERotationState Rotation
    {
        get => rotation;
    }

    public void Reset()
    {
        position.X = 0;
        position.Y = 0;
    }

    public void Rotate(bool counterClockwise)
    {
        int newRot = (int) rotation + (counterClockwise ? -1 : 1);

        if (newRot < 0) newRot = 3;
        else if (newRot > 3) newRot = 0;
        
        rotation = (ERotationState) newRot;
    }

    public void Move(EMoveDirecton direction, bool[,] filledTiles)
    {
        switch (direction)
        {
            case EMoveDirecton.RIGHT:
                if (WillCollide(filledTiles, new Point(position.X + 1, position.Y))) break;
                
                position.X++;
                break;
            
            case EMoveDirecton.DOWN:
                if (WillCollide(filledTiles, new Point( position.X, position.Y + 1))) break;
                
                position.Y++;
                break;
            
            case EMoveDirecton.LEFT:
                if (WillCollide(filledTiles, new Point(position.X - 1, position.Y))) break;
                
                position.X--;
                break;
        }
    }

    public bool WillCollide(bool[,] filledTiles, Point newPos)
    {
        for (int y = 0; y < Shape.GetLength(0); y++) {
            for (int x = 0; x < Shape.GetLength(1); x++) {
                if (Shape[y, x])
                {
                    int maxPosX = canvasSize.X - 1;
                    int maxPosY = canvasSize.Y - 1;
                    
                    int pixelPosX = newPos.X + x;
                    int pixelPosY = newPos.Y + y;

                    bool isOutOfBounds = pixelPosX < 0 || pixelPosX > maxPosX || pixelPosY < 0 || pixelPosY > maxPosY;
                    
                    if (isOutOfBounds || filledTiles[pixelPosX, pixelPosY]) {
                        audioPlayer.PlayErrorSound();
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public bool[,] TrimmedShape
    {
        get
        {
            int rows = Shape.GetLength(0);
            int cols = Shape.GetLength(1);

            bool[] nonEmptyRows = new bool[rows];
            for (int i = 0; i < rows; i++)
            {
                nonEmptyRows[i] = false;
                
                for (int j = 0; j < cols; j++)
                {
                    if (Shape[i, j])
                    {
                        nonEmptyRows[i] = true;
                        break;
                    }
                }
            }

            bool[] nonEmptyCols = new bool[cols];
            for (int j = 0; j < cols; j++)
            {
                nonEmptyCols[j] = false;
                
                for (int i = 0; i < rows; i++)
                {
                    if (Shape[i, j])
                    {
                        nonEmptyCols[j] = true;
                        break;
                    }
                }
            }

            bool[,] trimmedShape = new bool[nonEmptyRows.Count(r => r), nonEmptyCols.Count(c => c)];

            int newRow = 0;
            for (int i = 0; i < nonEmptyRows.Length; i++)
            {
                if (nonEmptyRows[i])
                {
                    int newCol = 0;
                    
                    for (int j = 0; j < nonEmptyCols.Length; j++)
                    {
                        if (nonEmptyCols[j])
                        {
                            trimmedShape[newRow, newCol] = Shape[i, j];
                            newCol++;
                        }
                    }
                    
                    newRow++;
                }
            }

            return trimmedShape;
        }
    }
    
    public abstract bool[,] Shape
    {
        get;
    }
}
