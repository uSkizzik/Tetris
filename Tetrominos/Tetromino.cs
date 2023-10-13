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
    private readonly Renderer renderer;

    public Tetromino(Point canvasSize, AudioPlayer audioPlayer, Renderer renderer)
    {
        this.canvasSize = canvasSize;
        this.audioPlayer = audioPlayer;
        this.renderer = renderer;
        
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

    public void Spawn()
    {
        position.X = (int) Math.Floor((double) canvasSize.X / 2 - (double) GetShape().GetLength(1) / 2);
        position.Y = 0;
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
        
        if (WillCollide((ERotationState) newRot)) return;
        rotation = (ERotationState) newRot;
    }

    public void Move(EMoveDirecton direction)
    {
        switch (direction)
        {
            case EMoveDirecton.RIGHT:
                if (WillCollide(position.X + 1, position.Y)) break;
                
                position.X++;
                break;
            
            case EMoveDirecton.DOWN:
                if (WillCollide(position.X, position.Y + 1)) break;
                
                position.Y++;
                break;
            
            case EMoveDirecton.LEFT:
                if (WillCollide(position.X - 1, position.Y)) break;
                
                position.X--;
                break;
        }
    }

    public bool WillCollide(Point newPos)
    {
        return WillCollide(newPos, rotation);
    }

    public bool WillCollide(ERotationState newRot)
    {
        return WillCollide(position, newRot);
    }

    public bool WillCollide(int x, int y)
    {
        return WillCollide(new Point(x, y), rotation);
    }

    public bool WillCollide(Point newPos, ERotationState newRot)
    {
        bool[,] shape = GetShape(newRot);
            
        for (int y = 0; y < shape.GetLength(0); y++) {
            for (int x = 0; x < shape.GetLength(1); x++) {
                if (shape[y, x])
                {
                    int maxPosX = canvasSize.X - 1;
                    int maxPosY = canvasSize.Y - 1;
                    
                    int pixelPosX = newPos.X + x;
                    int pixelPosY = newPos.Y + y;

                    bool isOutOfBounds = pixelPosX < 0 || pixelPosX > maxPosX || pixelPosY < 0 || pixelPosY > maxPosY;
                    
                    if (isOutOfBounds || renderer.BGCanvas[pixelPosX, pixelPosY]) {
                        audioPlayer.PlayErrorSound();
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public bool[,] GetShape()
    {
        return GetShape(rotation);
    }
    
    protected abstract bool[,] GetShape(ERotationState rotation);
}
