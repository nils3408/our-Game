using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using our_Game;
/*

public class GameLogic2 : IGameState
{

    private GraphicsDevice graphicsDevice;
    private  SpriteBatch _spriteBatch;
    private ContentManager contentManager;
    
    public PlayerThies[] playerList { get; set; }

    public Ball football;
    int GroundY = 200;

    private Player[] players;

    SimpleButton _button;

    public GameLogic2(GraphicsDevice graphicsDevice, ContentManager contentManager, PlayerThies[] playerList)
    {
        this.graphicsDevice = graphicsDevice;
        this._spriteBatch = new SpriteBatch(graphicsDevice);
        this.contentManager = contentManager;
        this.playerList = playerList;
        
        Texture2D ballTexture = (Texture2D) contentManager.Load<Texture2D>("football");
        football = new Ball(new Vector2(200, GroundY), ballTexture, GroundY);

        SpriteFont font = contentManager.Load<SpriteFont>("Arial");
        _button = new SimpleButton(new Rectangle(200, 200, 200, 100), "Menu", font);
        _button.OnClick += () => Game1._nextState = Game1.menu;


    }


    public void Initialize()
    {

    }

    public void LoadContent()
    {
        
    }

    public void Update(GameTime gameTime)
    {
        _button.Update(Mouse.GetState());
    }

    public void Draw(GameTime gameTime)
    {
        graphicsDevice.Clear(Color.Crimson);
        _spriteBatch.Begin();
        _button.Draw(_spriteBatch, graphicsDevice);
        _spriteBatch.End();
    }
}*/