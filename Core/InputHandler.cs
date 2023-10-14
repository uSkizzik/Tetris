using Tetris.Screens;
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

            if (key.Key == ConsoleKey.F5)
            {
                game.RedrawFrame();
            }

            switch (game.ScreenInstance)
            {
                case MainMenu:  
                    HandleMainMenuInput(key.Key);  
                    break;

                case Renderer:
                    HandleGameInput(key.Key);
                    break;
            }
        }
    }


    private void HandleMainMenuInput(ConsoleKey key)
    {
        switch (key)  
        {
            case ConsoleKey.UpArrow:
                ((MainMenu) game.ScreenInstance).PrevActiveOption();
                break;
            
            case ConsoleKey.DownArrow:
                ((MainMenu) game.ScreenInstance).NextActiveOption();
                break;
            
            case ConsoleKey.Enter:
                ((MainMenu) game.ScreenInstance).SelectOption();
                break;
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
            
            case ConsoleKey.RightArrow:
                game.MoveTetromino(EMoveDirecton.RIGHT);
                break;
            
            case ConsoleKey.DownArrow:
                game.MoveTetromino(EMoveDirecton.DOWN);
                break;
            
            case ConsoleKey.Spacebar:
                game.HardDropTetromino();
                break;
            
            case ConsoleKey.C:
                game.HoldTetromino();
                break;
        }  
    }
}