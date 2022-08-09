using Raylib_cs;

namespace Aston;

public class Game
{
    WindowHandle wh;

    public Game()
    {
        // wh = new WindowHandle(1280, 720, 60, "Test");
        wh = WindowHandle.FromConfig("C:\\Users\\attac\\onedrive\\desktop\\raylibgame\\Config\\config.lua");

        Entity test = new Entity(ref wh)
            .WithComponent(new TransformComponent(new Vector2(1280/2, 720/2), new Vector2(120*4, 80*4), 0))
            .WithComponent(new AnimationComponent());

        using (AnimationComponent? ac = test.GetComponent<AnimationComponent>())
        {
            if (ac != null)
            {
                ac.RegisterAtlasFile("KnightIdle", "C:\\Users\\attac\\onedrive\\desktop\\raylibgame\\Assets\\Knight\\_Run.png");
                ac.SetAtlas("KnightIdle");
                ac.AnimHandler.Active = true;
                ac.AnimHandler.ConformToCurrent(10, 3/4, 120, 80);
            }
        }

        wh.OnUpdate = delegate()
        {
            test.Update();
        };

        wh.OnRender = delegate()
        {
            Raylib.ClearBackground(Color.BLACK);
            Raylib.DrawFPS(4, 4);
            using (AnimationComponent? ac = test.GetComponent<AnimationComponent>())
            {
                if (ac == null) return; 
                ac.Render();
            }
        };
    }

    public void Run() 
    {
        wh.Run();
    }
}