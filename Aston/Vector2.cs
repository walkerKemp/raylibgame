namespace Aston;

public class Vector2
{
    public float X;
    public float Y;

    public Vector2()
    {
        X = 0.0f;
        Y = 0.0f;
    }

    public Vector2(float Base)
    {
        X = Base;
        Y = Base;
    }

    public Vector2(float Ux, float Uy)
    {
        X = Ux;
        Y = Uy;
    }

    public static Vector2 FromRadians(float R)
    {
        Vector2 ret = new Vector2();

        ret.X = (float)Math.Round((float)Math.Cos(R), 6);
        ret.Y = (float)Math.Round((float)Math.Sin(R), 6);

        return ret;
    }

    public static Vector2 operator+(Vector2 s, Vector2 r)
    {
        Vector2 ret = new Vector2();
        ret.X = s.X + r.X;
        ret.Y = s.Y + r.Y;
        return ret;
    }
    
    public static Vector2 operator-(Vector2 s, Vector2 r)
    {
        Vector2 ret = new Vector2();
        ret.X = r.X - s.X;
        ret.Y = r.Y - s.Y;
        return ret;
    }

    public static Vector2 operator*(Vector2 s, Vector2 r)
    {
        Vector2 ret = new Vector2();
        ret.X = s.X * r.X;
        ret.Y = s.Y * r.Y;
        return ret;
    }

    public static Vector2 operator*(Vector2 s, float r) {
        Vector2 ret = new Vector2();
        ret.X = s.X * r;
        ret.Y = s.Y * r;
        return ret;
    }

    public static Vector2 operator/(Vector2 s, Vector2 r)
    {
        Vector2 ret = new Vector2();
        ret.X = r.X == 0.0f ? 0.0f : s.X / r.X;
        ret.Y = r.Y == 0.0f ? 0.0f : s.Y / r.Y;
        return ret;
    }

    public static Vector2 operator/(Vector2 s, float r) {
        Vector2 ret = new Vector2();
        if (r == 0.0f) { return ret; }
        ret.X = s.X / r;
        ret.Y = s.Y / r;
        return ret;
    }

    public void Clear()
    {
        this.X = 0.0f;
        this.Y = 0.0f;
    }

    public float GetMag()
    {
        return (float)Math.Sqrt((this.X * this.X) + (this.Y * this.Y));
    }

    public void SetMag(float Scale)
    {
        float div = this.GetMag();
        if (div == 0.0f) { return; }
        this.X /= div;
        this.Y /= div;
    }

    public override string ToString()
    {
        return $"<{X}, {Y}>";
    }
}