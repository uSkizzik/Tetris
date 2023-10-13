using System.Drawing;
using Tetris.Core;

namespace Tetris.Tetrominos;

public class ZTetromino : Tetromino
{
    public ZTetromino(Point canvasSize, AudioPlayer audioPlayer, Renderer renderer) : base(canvasSize, audioPlayer, renderer)
    {
    }

    public override ConsoleColor Color
    {
        get => ConsoleColor.Red;
    }

    protected override bool[,] GetShape(ERotationState rotation)
    {
        bool[,] render = new bool[4, 4];
        
        switch (rotation)
        {
            case ERotationState.UP:
                render = new [,]
                {
                    { true, true, false },
                    { false, true, true },
                    { false, false, false }
                };

                break;
            
            case ERotationState.RIGHT:
                render = new [,]
                {
                    { false, false, true },
                    { false, true, true },
                    { false, true, false }
                };

                break;
            
            
            case ERotationState.DOWN:
                render = new [,]
                {
                    { false, false, false },
                    { true, true, false },
                    { false, true, true }
                };

                break;
            
            
            case ERotationState.LEFT:
                render = new [,]
                {
                    { false, true, false },
                    { true, true, false },
                    { true, false, false }
                };

                break;
        }

        return render;
    }
}
