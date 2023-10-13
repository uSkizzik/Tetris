using System.Drawing;
namespace Tetris.Tetrominos;

public class ZTetromino : Tetromino
{
    public ZTetromino(Point canvasSize) : base(canvasSize)
    {
    }

    public override ConsoleColor Color
    {
        get => ConsoleColor.Red;
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
}
