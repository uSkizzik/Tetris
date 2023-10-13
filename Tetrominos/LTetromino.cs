using System.Drawing;
namespace Tetris.Tetrominos;

public class LTetromino : Tetromino
{
    public LTetromino(Point canvasSize) : base(canvasSize)
    {
    }

    public override ConsoleColor Color
    {
        get => ConsoleColor.DarkYellow;
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
                        { false, false, true },
                        { true, true, true },
                        { false, false, false }
                    };

                    break;
                
                case ERotationState.RIGHT:
                    render = new [,]
                    {
                        { false, true, false },
                        { false, true, false },
                        { false, true, true }
                    };

                    break;
                
                
                case ERotationState.DOWN:
                    render = new [,]
                    {
                        { false, false, false },
                        { true, true, true },
                        { true, false, false }
                    };

                    break;
                
                
                case ERotationState.LEFT:
                    render = new [,]
                    {
                        { true, true, false },
                        { false, true, false },
                        { false, true, false }
                    };

                    break;
            }

            return render;
        }
    }
}
