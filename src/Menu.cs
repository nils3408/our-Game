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

        _button = new SimpleButton(new Vector2(200,200),new Rectangle(200,200, 200,100), graphicsDevice);
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
        graphicsDevice.Clear(Color.Aquamarine);
        _spriteBatch.Begin();
        _button.Draw(_spriteBatch);
        _spriteBatch.End();
    }
}