using System.Drawing;
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

        inputHandler = new InputHandler(this);
        randomizer = new Randomizer(canvasSize);
        renderer = new Renderer(canvasSize);

        Console.CursorVisible = false;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
    }

    public Tetromino ActiveTetromino
    {
        get => activeTetromino;
    }

    public Screen Screen
    {
        get => Screen;
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

    private void TickTetromino(Object source, System.Timers.ElapsedEventArgs e)
    {
        int maxPos = canvasSize.Y;

        if (activeTetromino != null)
        {
            if (activeTetromino.Position.Y + activeTetromino.Render().GetLength(0) < maxPos)
                activeTetromino.Tick();
            else if (tetrominoQueue.Count > 0)
            {
                renderer.LockTetromino(activeTetromino);

                SpawnTetromino(tetrominoQueue[0]);
                tetrominoQueue.Add(randomizer.RandomTetromino());
                tetrominoQueue.RemoveAt(0);
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
            renderer.DrawFrame(activeTetromino, tetrominoQueue);
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
