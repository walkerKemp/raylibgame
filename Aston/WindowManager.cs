using Raylib_cs;

namespace Aston;

public class WindowHandle
{
    public int Width;
    public int Height;
    public int TargetFPS;
    public string Title;
    public bool Running;
    public float DeltaTime;

    public Action OnEnter;
    public Action OnUpdate;
    public Action OnRender;
    public Action OnExit;

    public WindowHandle(int width, int height, int targetfps, string title)
    {
        Width = width;
        Height = height;
        TargetFPS = targetfps;
        Title = title;
        Running = false;

        DeltaTime = 0.0f;

        OnEnter = delegate() {};
        OnUpdate = delegate() {};

        OnRender = delegate() {
            Raylib.ClearBackground(Color.BLACK);
            Raylib.DrawFPS(4, 4);
        };

        OnExit = delegate() {};

        Raylib.InitWindow(Width, Height, Title);
        Raylib.SetTargetFPS(TargetFPS);
        Running = true;
    }

    public void Run() {
        OnEnter();

        while (!Raylib.WindowShouldClose()) {
            DeltaTime = Raylib.GetFrameTime();
            OnUpdate();
            Raylib.BeginDrawing();
            OnRender();
            Raylib.EndDrawing();
        }

        OnExit();
        Raylib.CloseWindow();

        Running = false;
    }
}