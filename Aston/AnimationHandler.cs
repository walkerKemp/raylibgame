using Raylib_cs;

namespace Aston;

public class AnimationHandler
{
    private string? FileName;
    public Texture2D SpriteAtlas;
    public int Index;
    public int MaxIndex;
    private int NumImages;
    public float PlaybackSpeed;
    private float PlaybackPoint;
    public int TileWidth;
    public int TileHeight;
    private Rectangle SourceRec;
    private Rectangle DestRec;
    private System.Numerics.Vector2 RenderOrigin = new System.Numerics.Vector2(0.0f, 0.0f);
    private float Rotation = 0.0f;
    private Color RenderColor = Color.WHITE;
    public bool Active;
    public bool DebugRender;
    public Action OnEnter;
    public Action OnExit;

    public AnimationHandler()
    {
        this.FileName = null;
        this.SpriteAtlas = new Texture2D();
        this.Index = 0;
        this.MaxIndex = 0;
        this.PlaybackSpeed = 0.0f;
        this.PlaybackPoint = 0.0f;
        this.TileWidth = 0;
        this.TileHeight = 0;
        this.SourceRec = new Rectangle(0, 0, 0, 0);
        this.DestRec = new Rectangle(0, 0, 0, 0);
        this.Active = false;
        this.DebugRender = false;
        
        this.OnEnter = delegate() {};
        this.OnExit = delegate() { this.Index = 0; };
    }

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
        this.SourceRec = new Rectangle(0, 0, TileWidth, TileHeight);
        this.DestRec = new Rectangle(0, 0, TileWidth, TileHeight);
        this.Active = false;
        this.DebugRender = false;

        this.OnEnter = delegate() {};
        this.OnExit = delegate() { this.Index = 0; };
    }

    public AnimationHandler(Texture2D SpriteAtlas, int NumImages, float PlaybackSpeed, int TileWidth, int TileHeight)
    {
        this.FileName = null;
        this.SpriteAtlas = SpriteAtlas;
        this.Index = 0;
        this.MaxIndex = NumImages - 1;
        this.NumImages = NumImages;
        this.PlaybackSpeed = PlaybackSpeed;
        this.TileWidth = TileWidth;
        this.TileHeight = TileHeight;
        this.SourceRec = new Rectangle(0, 0, TileWidth, TileHeight);
        this.DestRec = new Rectangle(0, 0, TileWidth, TileHeight);
        this.Active = false;
        this.DebugRender = false;

        this.OnEnter = delegate() {};
        this.OnExit = delegate() { this.Index = 0; };
    }

    public void ConformToCurrent(int NumImages, float PlaybackSpeed, int TileWidth, int TileHeight)
    {
        this.Index = 0;
        this.MaxIndex = NumImages - 1;
        this.NumImages = NumImages;
        this.PlaybackSpeed = PlaybackSpeed;
        this.PlaybackPoint = 0.0f;
        this.TileWidth = TileWidth;
        this.TileHeight = TileHeight;
        this.SourceRec = new Rectangle(0, 0, TileWidth, TileHeight);
        this.DestRec = new Rectangle(0, 0, TileWidth, TileHeight);
    }

    public void DeferredLoad()
    {
        this.SpriteAtlas = Raylib.LoadTexture(this.FileName);
    }

    public void Update(ref WindowHandle wh)
    {
        if (!this.Active)
        {
            return;
        }

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

        this.SourceRec.x = this.Index * this.TileWidth;
    }

    public void Render(int X, int Y, int renderWidth, int renderHeight)
    {
        this.DestRec.x = X;
        this.DestRec.y = Y;
        this.DestRec.width = renderWidth;
        this.DestRec.height = renderHeight;

        Raylib.DrawTexturePro(this.SpriteAtlas, this.SourceRec, this.DestRec, this.RenderOrigin, this.Rotation, this.RenderColor);
    }
}