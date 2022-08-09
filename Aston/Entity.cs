using Raylib_cs;
using System.Collections.Generic;

namespace Aston;

public class Component : IDisposable
{
    public Entity? Entity;
    public virtual void Update(ref WindowHandle wh) {}
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

public class DataComponent : Component
{
    public Dictionary<string, object> Data = new Dictionary<string, object>();
    public void RegisterVariable(string Key, object Base)
    {
        if (!this.Data.Keys.Contains<string>(Key))
        {
            this.Data.Add(Key, Base);
        }
    }

    public void DeregisterVariable(string Key)
    {
        if (this.Data.Keys.Contains<string>(Key))
        {
            this.Data.Remove(Key);
        }
    }

    public bool ContainsVariable(string Key) { return this.Data.Keys.Contains<String>(Key); }
}

public class TransformComponent : Component
{
    public Vector2 Position = new Vector2();
    public Vector2 Scale = new Vector2();
    public float LayerDepth = 0;

    public TransformComponent() : base() {}
    public TransformComponent(Vector2 Position, Vector2 Scale, float LayerDepth) : base()
    {
        this.Position = Position;
        this.Scale = Scale;
        this.LayerDepth = LayerDepth;
    }
}

public class AnimationComponent : Component
{
    public AnimationHandler AnimHandler = new AnimationHandler();
    public Dictionary<string, Texture2D> SpriteAtlases = new Dictionary<string, Texture2D>();

    public bool RegisterAtlas(string Name, Texture2D Atlas)
    {
        if (this.SpriteAtlases.Keys.Contains<string>(Name))
        {
            return false;
        }

        this.SpriteAtlases.Add(Name, Atlas);

        return true;
    }

    public bool RegisterAtlasFile(string Name, string FileName)
    {
        if (this.SpriteAtlases.Keys.Contains<string>(Name))
        {
            return false;
        }

        Texture2D Atlas = Raylib.LoadTexture(FileName);
        this.SpriteAtlases.Add(Name, Atlas);

        return true;
    }

    public bool SetAtlas(string AtlasName)
    {
        if (!this.SpriteAtlases.Keys.Contains<string>(AtlasName))
        {
            return false;
        }

        this.AnimHandler.SpriteAtlas = this.SpriteAtlases[AtlasName];

        return true;
    }

    public override void Update(ref WindowHandle wh)
    {
        this.AnimHandler.Update(ref wh);
    }

    public void Render()
    {
        if (this.Entity == null) return;
        TransformComponent? tc = this.Entity.GetComponent<TransformComponent>();
        if (tc == null) return;
        this.AnimHandler.Render((int)tc.Position.X, (int)tc.Position.Y, (int)tc.Scale.X, (int)tc.Scale.Y);
    }
}

public enum InputEvent
{
    Pressed,
    Released,
    Held,
    Up
}

public class InputComponent : Component
{
    private Dictionary<(KeyboardKey, InputEvent), Action<Entity>> Events = new Dictionary<(KeyboardKey, InputEvent), Action<Entity>>();

    public void RegisterKey(KeyboardKey key, InputEvent ie, Action<Entity> action)
    {
        if (this.Events.ContainsKey((key, ie)))
        {
            this.Events[(key, ie)] = action;
        } else
        {
            this.Events.Add((key, ie), action);
        }
    }

    public void DeregisterKey(KeyboardKey key, InputEvent ie)
    {
        if (this.Events.ContainsKey((key, ie)))
        {
            this.Events.Remove((key, ie));
        }
    }

    public override void Update(ref WindowHandle wh)
    {
        foreach ((KeyboardKey, InputEvent) key in this.Events.Keys)
        {
            switch (key.Item2)
            {
                case InputEvent.Pressed:
                    if (Raylib.IsKeyPressed(key.Item1))
                    {
                        if (this.Entity == null) { break; }
                        this.Events[key](this.Entity);
                    }

                    break;
                case InputEvent.Held:
                    if (Raylib.IsKeyDown(key.Item1))
                    {
                        if (this.Entity == null) { break; }
                        this.Events[key](this.Entity);
                    }

                    break;
                case InputEvent.Released:
                    if (Raylib.IsKeyReleased(key.Item1))
                    {
                        if (this.Entity == null) { break; }
                        this.Events[key](this.Entity);
                    }

                    break;
                case InputEvent.Up:
                    if (Raylib.IsKeyUp(key.Item1))
                    {
                        if (this.Entity == null) { break; }
                        this.Events[key](this.Entity);
                    }

                    break;
            }
        }
    }
}

public class Entity
{
    public WindowHandle wh;
    public int ID { get; set; }
    public List<Component> Components = new List<Component>();

    public Entity(ref WindowHandle wh)
    {
        this.wh = wh;
    }

    public void Update()
    {
        foreach (Component c in Components)
        {
            c.Update(ref wh);
        }
    }

    public Entity WithComponent(Component c)
    {
        this.Components.Add(c);
        c.Entity = this;

        return this;
    }

    public T? GetComponent<T>() where T : Component
    {
        foreach (Component c in Components)
        {
            if (c.GetType().Equals(typeof(T)))
            {
                return (T)c;
            }
        }

        return null;
    }
}
