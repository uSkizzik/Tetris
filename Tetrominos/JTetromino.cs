using System.Drawing;
namespace Tetris.Tetrominos;

public class JTetromino : Tetromino
{
    public JTetromino(Point canvasSize) : base(canvasSize)
    {
    }

    public override ConsoleColor Color
    {
        get => ConsoleColor.DarkBlue;
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
                        { true, false, false },
                        { true, true, true },
                        { false, false, false }
                    };

                    break;
                
                case ERotationState.RIGHT:
                    render = new [,]
                    {
                        { false, true, true },
                        { false, true, false },
                        { false, true, false }
                    };

                    break;
                
                
                case ERotationState.DOWN:
                    render = new [,]
                    {
                        { false, false, false },
                        { true, true, true },
                        { false, false, true }
                    };

                    break;
                
                
                case ERotationState.LEFT:
                    render = new [,]
                    {
                        { false, true, false },
                        { false, true, false },
                        { true, true, false }
                    };

                    break;
            }

            return render;
        }
    }
}
