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

public class MovementComponent : Component
{
    public Vector2 Position = new Vector2();
    public Vector2 Velocity = new Vector2();
    public Vector2 Acceleration = new Vector2();
    public float WalkSpeed = 128.0f;
    public float SprintSpeed = 256.0f;
    public float CurrentSpeed = 128.0f;
    public bool IsSprinting = false;
    public float AccelerationSpeed = 128.0f;

    public override void Update(ref WindowHandle wh)
    {
        this.CurrentSpeed = this.IsSprinting ? this.SprintSpeed : this.WalkSpeed;

        if (this.Entity == null) return;
        float DeltaTime = this.Entity.wh.DeltaTime;

        if (this.Velocity.GetMag() >= this.CurrentSpeed)
        {
            this.Velocity.SetMag(this.CurrentSpeed);
        }

        this.Position += this.Velocity;
        this.Velocity += this.Acceleration;
        this.Velocity *= DeltaTime;
        this.Acceleration.Clear();

        using (TransformComponent? tc = this.Entity?.GetComponent<TransformComponent>())
        {
            if (tc != null) { tc.Position = this.Position; }
        }
    }

    public void Impulse(Vector2 Force)
    {
        this.Acceleration += Force;
    }
}

public class Entity
{
    public WindowHandle wh;
    public int ID { get; set; }
    public List<Component> Components = new List<Component>();
    public Dictionary<string, object> Data = new Dictionary<string, object>();

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
