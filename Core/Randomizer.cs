using System.Drawing;
using Tetris.Tetrominos;

namespace Tetris.Core;

public class Randomizer
{
    private static readonly Type[] allTetrominos = {
        typeof(ITetromino), 
        typeof(JTetromino), 
        typeof(LTetromino), 
        typeof(OTetromino), 
        typeof(STetromino), 
        typeof(ZTetromino), 
        typeof(TTetromino)
    };
    
    private List<Type> possibleTetrominnos = allTetrominos.ToList();

    private readonly Point canvasSize; 
    private readonly Random random;
    
    private readonly AudioPlayer audioPlayer;
    private readonly Renderer renderer;

    public Randomizer(Point canvasSize, AudioPlayer audioPlayer, Renderer renderer)
    {
        this.canvasSize = canvasSize;
        this.random = new Random();
        
        this.audioPlayer = audioPlayer;
        this.renderer = renderer;
        
        ResetPossibleTetrominos();
    }

    private void ResetPossibleTetrominos()
    {
        possibleTetrominnos = allTetrominos.ToList();   
    }
    
    public Tetromino RandomTetromino()
    {
        if (possibleTetrominnos.Count == 0) ResetPossibleTetrominos();
        
        Type tetromino = possibleTetrominnos[random.Next(possibleTetrominnos.Count)];
        possibleTetrominnos.Remove(tetromino);

        return Activator.CreateInstance(tetromino, canvasSize, audioPlayer, renderer) as Tetromino;
    }
}