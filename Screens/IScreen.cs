namespace Tetris.Screens;

public interface IScreen
{
    public void RefreshScreenSize(int width, int height);
    public void DrawFrame();
}