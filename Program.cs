using System.Drawing;
using Tetris.Core;
using Tetris.Screens;
using Tetris.Tetrominos;

namespace Tetris;

public class TetrisGame
{
    private static readonly Point canvasSize = new (10, 26);
    private static readonly Point visibilityOffset = new (0, -6);
    
    private IScreen screenInstance;
    
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
        screenInstance = new MainMenu(this);
        
        timer = new System.Timers.Timer();
        //SetMoveTime(1500);
        SetMoveTime(300);
        timer.Elapsed += TickTetromino;

        audioPlayer = new AudioPlayer();
        inputHandler = new InputHandler(this);
        renderer = new Renderer(canvasSize, visibilityOffset, this);
        randomizer = new Randomizer(canvasSize, audioPlayer, renderer, this);

        Console.CursorVisible = false;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
    }

    public Tetromino? ActiveTetromino
    {
        get => activeTetromino;
    }

    public Tetromino? HeldTetromino
    {
        get => heldTetromino;
    }

    public List<Tetromino> TetrominoQueue
    {
        get => tetrominoQueue;
    }

    public IScreen ScreenInstance
    {
        get => screenInstance;
    }

    public Renderer Renderer
    {
        get => renderer;
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
        activeTetromino.Spawn();
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

    public void MoveTetromino(EMoveDirecton direction)
    {
        if (activeTetromino == null)
        {
            audioPlayer.PlayErrorSound();
            return;
        }

        activeTetromino.Move(direction);
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

        if (heldTetromino == null)
        {
            heldTetromino = MoveQueue();
            heldTetromino.wasHeld = true;
        }

        SpawnTetromino(heldTetromino);
        
        toBeHeld.Reset();
        heldTetromino = toBeHeld;
    }

    public void LockTetromino(Tetromino tetromino)
    {
        renderer.LockTetromino(tetromino);
        if (tetromino == activeTetromino) SpawnTetromino(MoveQueue());
    }

    private void TickTetromino(Object source, System.Timers.ElapsedEventArgs e)
    {
        if (activeTetromino != null)
        {
            if (!activeTetromino.WillCollide(activeTetromino.Position.X, activeTetromino.Position.Y + 1))
                activeTetromino.Move(EMoveDirecton.DOWN);
            else if (tetrominoQueue.Count > 0)
                activeTetromino.Lock();
        }
    }

    public void RedrawFrame()
    {
        Console.Clear();
        screenInstance.DrawFrame();
    }

    public void StartGame()
    {
        screenInstance = renderer;
        RedrawFrame();
        
        SpawnTetromino(randomizer.RandomTetromino());
        
        tetrominoQueue.Add(randomizer.RandomTetromino());
        tetrominoQueue.Add(randomizer.RandomTetromino());
        tetrominoQueue.Add(randomizer.RandomTetromino());

        timer.Enabled = true;
    }

    public void Exit()
    {
        Environment.Exit(0);
    }

    public void Run(bool skipMenu) {
        AudioPlayer.PlayThemeSong();
        if (skipMenu) StartGame();
        
        while (true)
        {
            inputHandler.HandleInput();
            screenInstance.DrawFrame();
            
            List<int> fullRows = new List<int>();
            
            for (int y = 0; y < renderer.BGCanvas.GetLength(1); y++)
            {
                bool isRowFull = true;
                
                for (int x = 0; x < renderer.BGCanvas.GetLength(0); x++)
                {
                    if (!renderer.BGCanvas[x, y])
                    {
                        isRowFull = false;
                        break;
                    }
                }

                if (isRowFull) fullRows.Add(y);
            }

            if (fullRows.Count != 0)
                renderer.ClearRows(fullRows);
        }
    }
}

internal class Program
{
    static void Main(string[] args)
    {
        TetrisGame game = new TetrisGame();
        game.Run(args.Contains("--skipMenu"));
    }
}
