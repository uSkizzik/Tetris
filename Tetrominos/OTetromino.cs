using System.Drawing;
using Tetris.Core;

namespace Tetris.Tetrominos;

public class OTetromino : Tetromino
{
    public OTetromino(Point canvasSize, AudioPlayer audioPlayer) : base(canvasSize, audioPlayer)
    {
    }

    public override ConsoleColor Color
    {
        get => ConsoleColor.Yellow;
    }

    public override bool[,] Shape
    {
        get
        {
            return new [,]
            {
                { true, true },
                { true, true }
            };
        }
    }
}

