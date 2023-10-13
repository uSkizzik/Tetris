using System.Drawing;
namespace Tetris.Tetrominos;

public class STetromino : Tetromino
{
    public STetromino(Point canvasSize) : base(canvasSize)
    {
    }

    public override ConsoleColor Color
    {
        get => ConsoleColor.Green;
    }

    public override bool[,] Render
    {
        get
        {
            bool[,] render = new bool[4, 4];
            
            switch (Rotation)
            {
                case ERotationState.UP:
                    render = new [,]
                    {
                        { false, true, true },
                        { true, true, false },
                        { false, false, false }
                    };

                    break;
                
                case ERotationState.RIGHT:
                    render = new [,]
                    {
                        { false, true, false },
                        { false, true, true },
                        { false, false, true }
                    };

                    break;
                
                
                case ERotationState.DOWN:
                    render = new [,]
                    {
                        { false, false, false },
                        { false, true, true },
                        { true, true, false }
                    };

                    break;
                
                
                case ERotationState.LEFT:
                    render = new [,]
                    {
                        { true, false, false },
                        { true, true, false },
                        { false, true, false }
                    };

                    break;
            }

            return render;
        }
    }
}
