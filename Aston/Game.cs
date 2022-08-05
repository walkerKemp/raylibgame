using Raylib_cs;

namespace Aston;

public class Game
{
    WindowHandle wh;

    public Game()
    {
        string fileLocation = "C:\\Users\\Walkers-Work-Machine\\desktop\\programs\\raylibgame\\Assets\\Knight\\_Run.png";
        AnimationHandler KnightRunning = new AnimationHandler(fileLocation, 10, 1/30f, 120, 80);

        wh = new WindowHandle(1280, 720, 144, "Test");

        wh.OnEnter = delegate()
        {
            KnightRunning.DeferredLoad();
        };

        wh.OnUpdate = delegate()
        {
            KnightRunning.Update(ref wh);
        };

        wh.OnRender = delegate()
        {
            Raylib.ClearBackground(Color.BLACK);
            Raylib.DrawFPS(4, 4);
            KnightRunning.Render(1280/2, 720/2);
        };
    }

    public void Run() 
    {
        wh.Run();
    }
}