using Raylib_cs;

namespace Aston;

public class AnimationHandler
{
    private string FileName;
    public Texture2D SpriteAtlas;
    public int Index;
    public int MaxIndex;
    private int NumImages;
    public float PlaybackSpeed;
    private float PlaybackPoint;
    public int TileWidth;
    public int TileHeight;
    private Rectangle RenderRec;
    private System.Numerics.Vector2 RenderPos;

    public AnimationHandler(string FileName, int NumImages, float PlaybackSpeed, int TileWidth, int TileHeight)
    {
        this.FileName = FileName;
        this.SpriteAtlas = new Texture2D();
        this.Index = 0;
        this.MaxIndex = NumImages - 1;
        this.NumImages = NumImages;
        this.PlaybackSpeed = PlaybackSpeed;
        this.PlaybackPoint = 0.0f;
        this.TileWidth = TileWidth;
        this.TileHeight = TileHeight;
        this.RenderRec = new Rectangle(0, 0, TileWidth, TileHeight);
    }

    public void DeferredLoad()
    {
        this.SpriteAtlas = Raylib.LoadTexture(this.FileName);
    }

    public void Update(ref WindowHandle wh)
    {
        this.PlaybackPoint += wh.DeltaTime;

        if (this.PlaybackPoint >= this.PlaybackSpeed)
        {
            this.PlaybackPoint = 0.0f;

            this.Index++;

            if (this.Index > this.MaxIndex)
            {
                this.Index = 0;
            }
        }

        this.RenderRec.x = this.Index * this.TileWidth;
    }

    public void Render(int X, int Y)
    {
        this.RenderPos.X = X;
        this.RenderPos.Y = Y;
        Raylib.DrawTextureRec(this.SpriteAtlas, this.RenderRec, this.RenderPos, Color.WHITE);
    }
}