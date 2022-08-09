using Raylib_cs;

namespace Aston;

public class Game
{
    WindowHandle wh;

    public Game()
    {
        // wh = new WindowHandle(1280, 720, 60, "Test");
        wh = WindowHandle.FromConfig("C:\\Users\\Walkers-Work-Machine\\Desktop\\programs\\raylibgame\\Config\\config.lua");

        Entity test = new Entity(ref wh)
            .WithComponent(new TransformComponent(new Vector2(1280/2, 720/2), new Vector2(120*4, 80*4), 0))
            .WithComponent(new AnimationComponent())
            .WithComponent(new InputComponent())
            .WithComponent(new DataComponent());

        using (DataComponent? dc =  test.GetComponent<DataComponent>())
        {
            if (dc != null)
            {
                dc.RegisterVariable("sprinting", false);
            }
        }

        using (AnimationComponent? ac = test.GetComponent<AnimationComponent>())
        {
            if (ac != null)
            {
                ac.RegisterAtlasFile("KnightIdle", "C:\\Users\\Walkers-Work-Machine\\Desktop\\programs\\raylibgame\\Assets\\Knight\\_Idle.png");
                ac.SetAtlas("KnightIdle");
                ac.AnimHandler.Active = true;
                ac.AnimHandler.ConformToCurrent(10, 1/15f, 120, 80);
            }
        }

        using (InputComponent? ic = test.GetComponent<InputComponent>())
        {
            if (ic != null)
            {
                ic.RegisterKey(KeyboardKey.KEY_W, InputEvent.Held, delegate(Entity e)
                {
                    using (TransformComponent? tc = e.GetComponent<TransformComponent>())
                    {
                        if (tc != null)
                        {
                            if (tc.Entity == null) { return; }
                            float deltaTime = tc.Entity.wh.DeltaTime;
                            tc.Position += new Vector2(0.0f, -1.0f);
                        }
                    }
                });
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