using System.Drawing;
using Tetris.Core;

namespace Tetris.Tetrominos;

public class OTetromino : Tetromino
{
    public OTetromino(Point canvasSize, AudioPlayer audioPlayer, Renderer renderer, TetrisGame game) : base(canvasSize, audioPlayer, renderer, game)
    {
    }

    public override ConsoleColor Color
    {
        get => ConsoleColor.Yellow;
    }

    protected override bool[,] GetShape(ERotationState rotation)
    {
        return new [,]
        {
            { true, true },
            { true, true }
        };
    }
}

