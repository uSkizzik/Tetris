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
        }  
    }
}