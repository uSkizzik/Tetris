﻿using System.Drawing;
namespace Tetris.Tetrominos;

public class LTetromino : Tetromino
{
    public LTetromino(Point canvasSize) : base(canvasSize)
    {
    }

    public override ConsoleColor Color
    {
        get
        {
            return ConsoleColor.DarkYellow;
        }
    }

    public override bool[,] Render()
    {
        bool[,] shape = {
            { false, false, true },
            { true, true, true }
        };

        return shape;
    }
}
