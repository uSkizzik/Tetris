using Tetris.Tetrominos;

namespace Tetris.Core;

public class InputHandler
{
    private readonly TetrisGame game;
    
    public InputHandler(TetrisGame game)
    {
        this.game = game;
    }
    
    public void HandleInput()
    {
        if (Console.KeyAvailable)  
        { 
            ConsoleKeyInfo key = Console.ReadKey(true);

            switch (game.Screen)
            {
                case Screen.GAME:  
                    HandleGameInput(key.Key);  
                    break;
            }
        }  
    }

    private void HandleGameInput(ConsoleKey key)
    {
        switch (key)  
        {
            case ConsoleKey.UpArrow:
                game.RotateTetromino(false);
                break;
            
            case ConsoleKey.Z:
                game.RotateTetromino(true);
                break;
            
            case ConsoleKey.LeftArrow:
                game.MoveTetromino(EMoveDirecton.LEFT);
                break;
            
            case ConsoleKey.DownArrow:
                game.MoveTetromino(EMoveDirecton.DOWN);
                break;
            
            case ConsoleKey.RightArrow:
                game.MoveTetromino(EMoveDirecton.RIGHT);
                break;
            
            case ConsoleKey.C:
                game.HoldTetromino();
                break;
        }  
    }
}