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
        Console.WriteLine("We are hitting this");
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
