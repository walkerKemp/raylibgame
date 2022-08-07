using Raylib_cs;

namespace Aston;

public class Game
{
    WindowHandle wh;

    public Game()
    {
        wh = new WindowHandle(1280, 720, 60, "Test");

        Entity test = new Entity(ref wh);
        test
            .WithComponent(new TransformComponent(new Vector2(1280/2, 720/2), new Vector2(120*4, 80*4), 0))
            .WithComponent(new AnimationComponent());
        

        using (AnimationComponent? ac = test.GetComponent<AnimationComponent>())
        {
            if (ac != null)
            {
                ac.RegisterAtlasFile("KnightIdle", "C:\\Users\\attac\\onedrive\\desktop\\raylibgame\\Assets\\Knight\\_Idle.png");
                ac.SetAtlas("KnightIdle");
                ac.AnimHandler.Active = true;
                ac.AnimHandler.ConformToCurrent(10, 1/2, 120, 80);
            }
        }

        wh.OnUpdate = delegate()
        {
            test.Update();
        };

        wh.OnRender = delegate()
        {
            Raylib.ClearBackground(Color.BLACK);
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