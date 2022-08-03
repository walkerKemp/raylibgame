using Raylib_cs;
using NLua;
using System.Text;
using System.Collections.Generic;

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

    public static WindowHandle FromConfig(string ConfigFile) {
        int w = 640;
        int h = 480;
        int tf = 30;
        string t = "";

        using (Lua state = new Lua()) {
            state.State.Encoding = Encoding.UTF8;
            state.DoFile(ConfigFile);

            LuaTable test = (LuaTable)state["config"];
            if (test == null) { return new WindowHandle(640, 480, 30, ""); }

            foreach (KeyValuePair<Object, Object> item in test) {
                if (item.Value == null) { continue; }

                if (item.Key.ToString() == "title") {
                    t = item.Value.ToString();
                }

                if (item.Key.ToString() == "width") {
                    w = Int32.Parse(item.Value.ToString());
                }

                if (item.Key.ToString() == "height") {
                    h = Int32.Parse(item.Value.ToString());
                }

                if (item.Key.ToString() == "targetfps") {
                    tf = Int32.Parse(item.Value.ToString());
                }
            }
        }

        return new WindowHandle(w, h, tf, t);
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