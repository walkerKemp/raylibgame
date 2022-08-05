using Raylib_cs;

namespace Aston;

public class Game
{
    WindowHandle wh;

    public Game()
    {
        string fileLocation = "C:\\Users\\Walkers-Work-Machine\\desktop\\programs\\raylibgame\\Assets\\Knight\\_Idle.png";
        AnimationHandler KnightWalking = new AnimationHandler(fileLocation, 10, 1/5f, 120, 80);

        wh = new WindowHandle(1280, 720, 60, "Test");

        wh.OnEnter = delegate()
        {
            KnightWalking.DeferredLoad();
        };

        wh.OnUpdate = delegate()
        {
            KnightWalking.Update(ref wh);
        };

        wh.OnRender = delegate()
        {
            Raylib.ClearBackground(Color.BLACK);
            Raylib.DrawFPS(4, 4);
            KnightWalking.Render(1280/2, 720/2, 480, 320);
        };
    }

    public void Run() 
    {
        wh.Run();
    }
}