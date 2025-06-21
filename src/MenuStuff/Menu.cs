using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using our_Game;

public class Menu : IGameState
{
    private GraphicsDevice graphicsDevice;
    private  SpriteBatch _spriteBatch;
    private ContentManager contentManager;

    SimpleButton _button;

    public Menu(GraphicsDevice graphicsDevice, ContentManager contentManager)
    {
        this.graphicsDevice = graphicsDevice;
        this._spriteBatch = new SpriteBatch(graphicsDevice);
        this.contentManager = contentManager;

        SpriteFont font = contentManager.Load<SpriteFont>("Arial");
        _button = new SimpleButton(new Rectangle(200,200, 200,100), "Button", font);
        _button.OnClick += () => Game1._nextState = Game1.game;
        

    }


    public void Initialize(){}

    public void LoadContent(){}

    public void Update(GameTime gameTime)
    {
        _button.Update(Mouse.GetState());

    }

    public void Draw(GameTime gameTime)
    {
        Color background = new Color(190, 244, 150);
        graphicsDevice.Clear(background);
        _spriteBatch.Begin();
        _button.Draw(_spriteBatch, graphicsDevice);
        _spriteBatch.End();
    }
}