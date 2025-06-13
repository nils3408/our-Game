using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using nkast.Aether.Physics2D.Dynamics;


public class GameLogic : IGameState
{

    private GraphicsDevice graphicsDevice;
    private  SpriteBatch _spriteBatch;
    private ContentManager contentManager;
    private float GroundY;
    
    public Player[] playerList { get; set; }

    public Project8.Ball football;
    private GraphicsDeviceManager graphics;
    private ContentManager content;
    private Player[] players;

    public GameLogic(GraphicsDevice graphicsDevice, ContentManager contentManager, Player[] playerList)
    {
        this.graphicsDevice = graphicsDevice;
        this._spriteBatch = new SpriteBatch(graphicsDevice);
        this.contentManager = contentManager;
        this.playerList = playerList;
        
        Texture2D ballTexture = (Texture2D) contentManager.Load<Texture2D>("football");
        football = new Project8.Ball(new Vector2(200, GroundY), ballTexture, GroundY);

    }


    public void Initialize()
    {

    }

    public void LoadContent()
    {
        
    }

    public void Update(GameTime gameTime)
    {
        
    }

    public void Draw(GameTime gameTime)
    {
        graphicsDevice.Clear(Color.Crimson);
        _spriteBatch.Begin();

        _spriteBatch.End();
    }
}