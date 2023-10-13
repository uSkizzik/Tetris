using System.Drawing;
namespace Tetris.Tetrominos;

public class ITetromino : Tetromino
{
    public ITetromino(Point canvasSize) : base(canvasSize)
    {
    }

    public override ConsoleColor Color
    {
        get => ConsoleColor.Cyan;
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
}
