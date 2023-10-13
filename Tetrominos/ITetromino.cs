using System.Drawing;
using Tetris.Core;

namespace Tetris.Tetrominos;

public class ITetromino : Tetromino
{
    public ITetromino(Point canvasSize, AudioPlayer audioPlayer, Renderer renderer) : base(canvasSize, audioPlayer, renderer)
    {
    }

    public override ConsoleColor Color
    {
        get => ConsoleColor.Cyan;
    }

    protected override bool[,] GetShape(ERotationState rotation)
    {
        bool[,] render = new bool[4, 4];
        
        switch (rotation)
        {
            case ERotationState.UP:
                render = new [,]
                {
                    { false, false, false, false },
                    { true, true, true, true },
                    { false, false, false, false },
                    { false, false, false, false }
                };

                break;
            
            case ERotationState.RIGHT:
                render = new [,]
                {
                    { false, false, true, false },
                    { false, false, true, false },
                    { false, false, true, false },
                    { false, false, true, false }
                };

                break;
            
            
            case ERotationState.DOWN:
                render = new [,]
                {
                    { false, false, false, false },
                    { false, false, false, false },
                    { true, true, true, true },
                    { false, false, false, false }
                };

                break;
            
            
            case ERotationState.LEFT:
                render = new [,]
                {
                    { false, true, false, false },
                    { false, true, false, false },
                    { false, true, false, false },
                    { false, true, false, false }
                };

                break;
        }

        return render;
    }
}
