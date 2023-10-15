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
    LEFT
}

public abstract class Tetromino
{
    protected readonly Point canvasSize;
    
    private Point position;
    private ERotationState rotation;
    public bool wasHeld;

    private CancellationTokenSource? lockToken;
    private int timesCancelled;

    private readonly AudioPlayer audioPlayer;
    private readonly Renderer renderer;
    private readonly TetrisGame game;

    public Tetromino(Point canvasSize, AudioPlayer audioPlayer, Renderer renderer, TetrisGame game)
    {
        this.canvasSize = canvasSize;
        this.audioPlayer = audioPlayer;
        this.renderer = renderer;
        this.game = game;
        
        position = new(0, 0);
        wasHeld = false;
        timesCancelled = 0;
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
        position.Y = 4;
    }

    public void Reset()
    {
        position.X = 0;
        position.Y = 4;
        rotation = ERotationState.UP;
    }

    public void Tick()
    {
        if (lockToken == null && !WillCollide(Position.X, Position.Y + 1))
            Move(EMoveDirecton.DOWN, false);
        else
            Lock();
    }

    public void Lock()
    {
        if (lockToken == null)
        {
            lockToken = new CancellationTokenSource();
            
            Task.Delay(game.LockTime).ContinueWith(t =>
            {
                if (t.IsCanceled || !WillCollide(position.X, position.Y + 1))
                {
                    lockToken = null;
                    return;
                }
                
                game.LockTetromino(this);
                game.AudioPlayer.PlayBeep();
            }, lockToken.Token);
        }
    }

    private void ResetLock()
    {
        if (lockToken != null && timesCancelled < 15)
        {
            lockToken.Cancel();
            lockToken = null;
            timesCancelled++;
            
            Lock();
        }
    }

    public void Rotate(bool counterClockwise)
    {
        int newRot = (int) rotation + (counterClockwise ? -1 : 1);

        if (newRot < 0) newRot = 3;
        else if (newRot > 3) newRot = 0;
        
        if (WillCollide((ERotationState) newRot))
        {
            if (!WillCollide(position with { X = position.X + 1 }, (ERotationState)newRot))
                position.X++;

            else if (!WillCollide(position with { X = position.X - 1 }, (ERotationState)newRot))
                position.X--;

            else if (!WillCollide(position with { Y = position.Y - 1 }, (ERotationState)newRot))
                position.Y--;

            else if (!WillCollide(position with { X = position.X + 2 }, (ERotationState)newRot))
                position.X += 2;

            else if (!WillCollide(position with { X = position.X - 2 }, (ERotationState)newRot))
                position.X -= 2;

            else if (!WillCollide(position with { Y = position.Y - 2 }, (ERotationState)newRot))
                position.Y--;

            else
                return;
        }
        
        ResetLock();
        
        rotation = (ERotationState) newRot;
        game.AudioPlayer.PlayBeep();
    }

    public void Move(EMoveDirecton direction, bool isUserInput = true)
    {
        switch (direction)
        {
            case EMoveDirecton.RIGHT:
                if (WillCollide(position.X + 1, position.Y)) break;
                ResetLock();
                
                position.X++;
                if (isUserInput) game.AudioPlayer.PlayBeep();
                
                break;
            
            case EMoveDirecton.DOWN:
                if (WillCollide(position.X, position.Y + 1)) break;
                position.Y++;

                if (isUserInput)
                {
                    game.ScoreTracker.TetronimoSoftDropped();
                    game.AudioPlayer.PlayBeep();
                }
                
                break;
            
            case EMoveDirecton.LEFT:
                if (WillCollide(position.X - 1, position.Y)) break;
                ResetLock();
                
                position.X--;
                if (isUserInput) game.AudioPlayer.PlayBeep();
                
                break;
        }
    }

    public void HardDropTetromino()
    {
        int startingY = position.Y;
        
        while (!WillCollide(position.X, position.Y + 1))
            Move(EMoveDirecton.DOWN, false);
        
        game.ScoreTracker.TetronimoHardDropped(startingY, position.Y);
        game.LockTetromino(this);
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
                        audioPlayer.PlayBeep();
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
