using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Dynamics;


public class GameLogic : IGameState
{

    private GraphicsDevice graphicsDevice;
    private float GroundY;
    
    public Player[] playerList { get; set; }

    public Project8.Ball football;

    public GameLogic(GraphicsDevice graphicsDevice, Player[] playerList)
    {

        this.playerList = playerList;
        football = new Project8.Ball(new Vector2(200, GroundY), Content.Load<Texture2D>("ball"), GroundY);

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

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch_)
    {
        
    }
}