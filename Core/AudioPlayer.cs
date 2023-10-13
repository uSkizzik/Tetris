namespace Tetris.Core;

public class AudioPlayer
{
    private bool canPlaySound;
    
    public AudioPlayer()
    {
        this.canPlaySound = true;
    }
    
    public void PlayErrorSound()
    {
        if (!canPlaySound) return;
        
        canPlaySound = false;
        Console.Beep(500, 250);
        
        Task.Delay(250).ContinueWith(t => canPlaySound = true);
    }
    
    public static void PlayThemeSong()
    {
        Task.Run(PlayThemeSongThreaded);
    }
    
    // I cannot be bothered to make this myself.
    // https://gist.github.com/erineccleston/0d65a78e9df88d68a13a421d2a298f10
    private static void PlayThemeSongThreaded()
    {
        Console.Beep(1320,500);
                
        Console.Beep(990,250);
                
        Console.Beep(1056,250);
        Console.Beep(1188,250);
        Console.Beep(1320,125);
        Console.Beep(1188,125);
        Console.Beep(1056,250);
        Console.Beep(990,250);
        Console.Beep(880,500);
        Console.Beep(880,250);
        Console.Beep(1056,250);
        Console.Beep(1320,500);
        Console.Beep(1188,250);
        Console.Beep(1056,250);
        Console.Beep(990,750);
        Console.Beep(1056,250);
        Console.Beep(1188,500);
        Console.Beep(1320,500);
        Console.Beep(1056,500);
        Console.Beep(880,500);
        Console.Beep(880,500);

        Thread.Sleep(500);

        Console.Beep(1188,500);
        Console.Beep(1408,250);
        Console.Beep(1760,500);
        Console.Beep(1584,250);
        Console.Beep(1408,250);
        Console.Beep(1320,750);
        Console.Beep(1056,250);
        Console.Beep(1320,500);
        Console.Beep(1188,250);
        Console.Beep(1056,250);
        Console.Beep(990,500);
        Console.Beep(990,250);
        Console.Beep(1056,250);
        Console.Beep(1188,500);
        Console.Beep(1320,500);
        Console.Beep(1056,500);
        Console.Beep(880,500);
        Console.Beep(880,500);

        Thread.Sleep(500);
        PlayThemeSong();
    }
}