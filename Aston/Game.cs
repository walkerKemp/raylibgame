using Raylib_cs;

namespace Aston;

public class Game
{
    WindowHandle wh;

    public Game()
    {
        wh = new WindowHandle(1280, 720, 60, "Test");
        Entity test = new Entity();
        test.WithComponent(new Transform());

        Transform? tc = test.GetComponent<Transform>();
        if (tc != null)
        {
            Console.WriteLine(tc.Position);
        }
    }

    public void Run() 
    {
        wh.Run();
    }
}