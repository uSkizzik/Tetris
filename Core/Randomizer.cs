﻿using System.Drawing;
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

    public Randomizer(Point canvasSize)
    {
        this.canvasSize = canvasSize;
        this.random = new Random();
        
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

        return Activator.CreateInstance(tetromino, canvasSize) as Tetromino;
    }
}