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
    private int lockTime;
    private readonly System.Timers.Timer timer;

    private readonly AudioPlayer audioPlayer;
    private readonly InputHandler inputHandler;
    private readonly Randomizer randomizer;
    private readonly Renderer renderer;
    private readonly ScoreTracker scoreTracker;

    public TetrisGame()
    {
        screenInstance = new MainMenu(this);
        
        timer = new System.Timers.Timer();
        SetMoveTime(1200);
        timer.Elapsed += TickTetromino;

        audioPlayer = new AudioPlayer();
        inputHandler = new InputHandler(this);
        renderer = new Renderer(canvasSize, visibilityOffset, this);
        randomizer = new Randomizer(canvasSize, audioPlayer, renderer, this);
        scoreTracker = new ScoreTracker(this);

        Console.CursorVisible = false;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
    }

    public int LockTime
    {
        get => lockTime;
        set => lockTime = value;
    }

    public AudioPlayer AudioPlayer
    {
        get => audioPlayer;
    }

    public ScoreTracker ScoreTracker
    {
        get => scoreTracker;
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

    public void SetMoveTime(int moveTime)
    {
        this.moveTime = Math.Clamp(moveTime, 1, Int32.MaxValue);
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
            audioPlayer.PlayBeep();
            return;
        }

        activeTetromino.Rotate(counterClockwise);
    }

    public void MoveTetromino(EMoveDirecton direction)
    {
        if (activeTetromino == null)
        {
            audioPlayer.PlayBeep();
            return;
        }

        activeTetromino.Move(direction);
    }

    public void HardDropTetromino()
    {
        if (activeTetromino == null)
        {
            audioPlayer.PlayBeep();
            return;
        }

        activeTetromino.HardDropTetromino();
    }

    public void HoldTetromino()
    {
        if (activeTetromino == null || activeTetromino.wasHeld)
        {
            audioPlayer.PlayBeep();
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
        if (tetromino == activeTetromino && tetrominoQueue.Count > 0) SpawnTetromino(MoveQueue());
        
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

        scoreTracker.TetronimoLocked(fullRows.Count, renderer.BGCanvas);
    }

    private void TickTetromino(Object source, System.Timers.ElapsedEventArgs e)
    { 
        // There's used to be a racing condition somewhere that causes a newly spawned tetromino to get locked in the air.
        // Took me some debugging to find out. I was confused as to why Lock gets called on it but the last WillCollide check used the previous tetromino.
        // Anyways, I'm not sure if it still exists but just in case we solve it by saving the tetromino at the start of the function and using the saved value.
        Tetromino? tetromino = activeTetromino;
        
        if (tetromino != null)
        {
            tetromino.Tick();
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

        int lastWidth = Console.WindowWidth;
        int lastHeight = Console.WindowHeight;
        
        while (true)
        {
            if (Console.WindowWidth != lastWidth || Console.WindowHeight != lastHeight)
            {
                lastWidth = Console.WindowWidth;
                lastHeight = Console.WindowHeight;
                screenInstance.RefreshScreenSize(Console.WindowWidth, Console.WindowHeight);
                Console.Clear();
            }
                
            inputHandler.HandleInput();

            try
            {
                screenInstance.DrawFrame();
            }
            catch (Exception err)
            {
                Console.WriteLine("Error drawing frame!");
                Console.WriteLine(err);
            }
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
