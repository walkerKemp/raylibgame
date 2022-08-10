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
            .WithComponent(new TransformComponent(new Vector2(1280/2, 720/2), new Vector2(120*2, 80*2), 0))
            .WithComponent(new AnimationComponent())
            .WithComponent(new InputComponent())
            .WithComponent(new MovementComponent());

        test.Data.Add("issprinting", false);

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
                    using (MovementComponent? mc = test.GetComponent<MovementComponent>())
                    {
                        if (mc == null) return;
                        mc.Impulse(new Vector2(0.0f, -mc.AccelerationSpeed));
                    }
                });

                ic.RegisterKey(KeyboardKey.KEY_S, InputEvent.Held, delegate(Entity e)
                {
                    using (MovementComponent? mc = test.GetComponent<MovementComponent>())
                    {
                        if (mc == null) return;
                        mc.Impulse(new Vector2(0.0f, mc.AccelerationSpeed));
                    }
                });

                ic.RegisterKey(KeyboardKey.KEY_LEFT_SHIFT, InputEvent.Pressed, delegate(Entity e)
                {
                    using (MovementComponent? mc = test.GetComponent<MovementComponent>())
                    {
                        if (mc == null) return;
                        mc.IsSprinting = true;
                    }
                });

                ic.RegisterKey(KeyboardKey.KEY_LEFT_SHIFT, InputEvent.Released, delegate(Entity e)
                {
                    using (MovementComponent? mc = test.GetComponent<MovementComponent>())
                    {
                        if (mc == null) return;
                        mc.IsSprinting = false;
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