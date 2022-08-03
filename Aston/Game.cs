namespace Aston;

public class Game
{
    WindowHandle wh;

    public Game()
    {
        wh = new WindowHandle(1280, 720, 60, "Test");
    }
}