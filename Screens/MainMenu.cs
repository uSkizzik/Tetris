namespace Tetris.Screens;

public class MainMenu : IScreen
{
    private static readonly int SpaceSize = 2;
    private static readonly ConsoleColor[] Colors = { ConsoleColor.Red, ConsoleColor.DarkYellow, ConsoleColor.Yellow, ConsoleColor.Green, ConsoleColor.Cyan, ConsoleColor.Magenta };

    private static readonly string[] T =
    {
        "##########",
        "##########",
        "   ####   ",
        "   ####   ",
        "   ####   ",
        "   ####   ",
        "   ####   "
    };
        
        
    private static readonly string[] E =
    {
        "#########",
        "#########",
        "###      ",
        "#########",
        "###      ",
        "#########",
        "#########"
    };
        
        
    private static readonly string[] R =
    {
        "########  ",
        "######### ",
        "###    ###",
        "######### ",
        "########  ",
        "###   ### ",
        "###    ###"
    };
        
        
    private static readonly string[] I =
    {
        "####",
        "####",
        "####",
        "####",
        "####",
        "####",
        "####"
    };
        
        
    private static readonly string[] S =
    {
        "############",
        "############",
        "###         ",
        "############",
        "         ###",
        "############",
        "############"
    };

    private static readonly int LogoWidth = T[0].Length + E[0].Length + T[0].Length + R[0].Length + I[0].Length + S[0].Length + SpaceSize * 6;
    private static readonly int LogoHeight = T.Length;

    private static string[] _options = { "Play", "Exit" };
    private int _selectedOpt;

    private int _windowWidth;
    private int _windowHeight;
    
    private readonly TetrisGame _game;

    public MainMenu(TetrisGame game)
    {
        _game = game;
        RefreshScreenSize(Console.WindowWidth, Console.WindowHeight);
    }

    private int DrawLogoLetter(int x, int y, int index, string[] letter)
    {
        Console.CursorTop = y;
        Console.ForegroundColor = Colors[index];
        
        for (int i = 0; i < letter.Length; i++)
        {
            Console.CursorLeft = x;
            Console.WriteLine(letter[i]);
        }

        return letter[0].Length;
    }
    
    private void DrawLogo(int x, int y)
    {
        Console.CursorLeft = x;
        Console.CursorTop = y;

        int logoLength = 0;
        
        logoLength += DrawLogoLetter(x, y, 0, T) + SpaceSize;
        logoLength += DrawLogoLetter(x + logoLength, y, 1, E) + SpaceSize;
        logoLength += DrawLogoLetter(x + logoLength, y, 2, T) + SpaceSize;
        logoLength += DrawLogoLetter(x + logoLength, y, 3, R) + SpaceSize;
        logoLength += DrawLogoLetter(x + logoLength, y, 4, I) + SpaceSize;
        DrawLogoLetter(x + logoLength, y, 5, S);
    }

    public void NextActiveOption()
    {
        if (_selectedOpt != _options.Length - 1)
            _selectedOpt++;
    }

    public void PrevActiveOption()
    {
        if (_selectedOpt != 0) 
            _selectedOpt--;
    }

    public void SelectOption()
    {
        switch (_selectedOpt)
        {
            case 0:
                PlayGame();
                break;
            
            case 1:
                Exit();
                break;
        }
    }

    private void PlayGame()
    {
        _game.StartGame();
    }

    private void Exit()
    {
        _game.Exit();
    }
    
    private int DrawOption(int x, int y, int index)
    {
        Console.CursorLeft = x;
        Console.CursorTop = y + index * 2;

        if (_selectedOpt != index)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("  ");
        } else {
            Console.ForegroundColor = Colors[index];
            Console.Write("> ");
        }
        
        Console.Write(_options[index]);
        return 2;
    }
    
    private void DrawOptions(int x, int y)
    {
        Console.CursorLeft = x;
        Console.CursorTop = y;
        Console.ForegroundColor = ConsoleColor.Gray;

        DrawOption(x, y, 0);
        DrawOption(x, y, 1);
    }

    private void DrawHighScore()
    {
        string title = "High Score";
        string highScore = 1000.ToString();

        int logoEnd = _windowWidth / 2 + LogoWidth / 2;
        int center = logoEnd + (_windowWidth - logoEnd) / 2;
        
        Console.CursorTop = LogoHeight / 2 + 1;
        
        Console.CursorLeft = center - title.Length / 2;
        Console.ForegroundColor = ConsoleColor.White;
        
        Console.WriteLine(title);
        
        Console.CursorLeft = center - highScore.Length / 2;
        Console.ForegroundColor = ConsoleColor.Cyan;
        
        Console.Write(highScore);
    }

    private void DrawInstructions()
    {
        string[] str =
        {
            "Up Arrow - Rotate Clockwise",
            "Down Arrow - Move Down",
            "Left Arrow - Move Left",
            "Right Arrow - Move Right",
            "",
            "Space Bar - Hard Drop",
            "",
            "C - Hold Tetromino",
            "Z - Rotate Counter-Clockwise",
            "",
            "M - Toggle Audio",
            "F5 - Redraw Frame"
        };
        
        Console.CursorLeft = 1;
        Console.CursorTop = _windowHeight - str.Length - 1;
        Console.ForegroundColor = ConsoleColor.DarkGray;
        
        for (int i = 0; i < str.Length; i++)
        {
            Console.CursorLeft = 1;
            Console.WriteLine(str[i]);
        }
    }

    private void DrawCredit()
    {
        string creditStr = "Made by Radostin \"uSkizzik\" Stoyanov";
        
        Console.CursorLeft = _windowWidth - creditStr.Length;
        Console.CursorTop = _windowHeight - 1;
        Console.ForegroundColor = ConsoleColor.DarkGray;
        
        Console.Write(creditStr);
    }

    public void RefreshScreenSize(int width, int height)
    {
        _windowWidth = width;
        _windowHeight = height;
    }
    
    public void DrawFrame()
    {
        int centerX = _windowWidth / 2;

        DrawLogo(centerX - LogoWidth / 2, 1);
        DrawOptions(centerX - 2, LogoHeight + 3);
        
        DrawHighScore();
        DrawInstructions();
        DrawCredit();
    }
}