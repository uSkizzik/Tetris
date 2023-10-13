using System.Drawing;
using Tetris.Core;

namespace Tetris.Tetrominos;

public class TTetromino : Tetromino
{
    public TTetromino(Point canvasSize, AudioPlayer audioPlayer, Renderer renderer) : base(canvasSize, audioPlayer, renderer)
    {
    }

    public override ConsoleColor Color
    {
        get => ConsoleColor.Magenta;
    }

    protected override bool[,] GetShape(ERotationState rotation)
    {
        bool[,] render = new bool[4, 4];
        
        switch (rotation)
        {
            case ERotationState.UP:
                render = new [,]
                {
                    { false, true, false },
                    { true, true, true },
                    { false, false, false }
                };

                break;
            
            case ERotationState.RIGHT:
                render = new [,]
                {
                    { false, true, false },
                    { false, true, true },
                    { false, true, false }
                };

                break;
            
            
            case ERotationState.DOWN:
                render = new [,]
                {
                    { false, false, false },
                    { true, true, true },
                    { false, true, false }
                };

                break;
            
            
            case ERotationState.LEFT:
                render = new [,]
                {
                    { false, true, false },
                    { true, true, false },
                    { false, true, false }
                };

                break;
        }

        return render;
    }
}
