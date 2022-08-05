using Raylib_cs;

namespace Aston;

public class Component
{
    public Entity Entity;
    public virtual void Update(float DeltaTime) {}
}

public class Transform : Component
{
    public Vector2 Position = new Vector2();
    public Vector2 Scale = new Vector2();
    public float LayerDepth = 0;
}

public class Entity
{
    public int ID { get; set; }
    public List<Component> Components = new List<Component>();

    public Entity() {}

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
