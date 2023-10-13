﻿using System.Drawing;
using Tetris.Core;
using Tetris.Tetrominos;

namespace Tetris;

public enum Screen
{
    GAME
}

public class TetrisGame
{
    private static readonly Point canvasSize = new (10, 20);
    private static Screen screen;
    
    private Tetromino? activeTetromino;
    private Tetromino? heldTetromino;
    private List<Tetromino> tetrominoQueue = new ();

    private int moveTime;
    private readonly System.Timers.Timer timer;

    private readonly AudioPlayer audioPlayer;
    private readonly InputHandler inputHandler;
    private readonly Randomizer randomizer;
    private readonly Renderer renderer;

    public TetrisGame()
    {
        screen = Screen.GAME;
        
        timer = new System.Timers.Timer();
        //SetMoveTime(1500);
        SetMoveTime(300);
        timer.Elapsed += TickTetromino;

        audioPlayer = new AudioPlayer();
        inputHandler = new InputHandler(this);
        randomizer = new Randomizer(canvasSize);
        renderer = new Renderer(canvasSize, this);

        Console.CursorVisible = false;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
    }

    public Tetromino ActiveTetromino
    {
        get => activeTetromino;
    }

    public Tetromino HeldTetromino
    {
        get => heldTetromino;
    }

    public List<Tetromino> TetrominoQueue
    {
        get => tetrominoQueue;
    }

    public Screen Screen
    {
        get => screen;
    }

    private void SetMoveTime(int moveTime)
    {
        this.moveTime = moveTime;
        timer.Interval = this.moveTime;
    }

    private void SpawnTetromino(Tetromino tetromino)
    {
        tetromino.Reset();
        activeTetromino = tetromino;
    }

    private Tetromino MoveQueue()
    {
        Tetromino toSpawn = tetrominoQueue[0];
        
        tetrominoQueue.Add(randomizer.RandomTetromino());
        tetrominoQueue.RemoveAt(0);

        return toSpawn;
    }

    public void RotateTetromino(bool counterClockwise)
    {
        if (activeTetromino == null)
        {
            audioPlayer.PlayErrorSound();
            return;
        }

        activeTetromino.Rotate(counterClockwise);
    }

    public void HoldTetromino()
    {
        if (activeTetromino == null || activeTetromino.wasHeld)
        {
            audioPlayer.PlayErrorSound();
            return;
        }
        
        Tetromino toBeHeld = activeTetromino;
        toBeHeld.wasHeld = true;
        toBeHeld.Reset();

        if (heldTetromino == null)
        {
            heldTetromino = MoveQueue();
            heldTetromino.wasHeld = true;
        }

        SpawnTetromino(heldTetromino);
        heldTetromino = toBeHeld;
    }

    private void TickTetromino(Object source, System.Timers.ElapsedEventArgs e)
    {
        int maxPos = canvasSize.Y;

        if (activeTetromino != null)
        {
            if (activeTetromino.Position.Y + activeTetromino.Render.GetLength(0) < maxPos)
                activeTetromino.Tick();
            else if (tetrominoQueue.Count > 0)
            {
                renderer.LockTetromino(activeTetromino);
                SpawnTetromino(MoveQueue());
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
            renderer.DrawFrame();
            inputHandler.HandleInput();
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
