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

    SimpleButton _button1;

    public Menu(GraphicsDevice graphicsDevice, ContentManager contentManager)
    {
        this.graphicsDevice = graphicsDevice;
        this._spriteBatch = new SpriteBatch(graphicsDevice);
        this.contentManager = contentManager;

        SpriteFont font = contentManager.Load<SpriteFont>("Arial");

        ElementContainer stackCobtainer = new StackContainer(null, new Rectangle(200,200, 200,100), 20);
        _button1 = new SimpleButton(null, new Rectangle(0,0, 200,100), "Button", font);
        _button1.OnClick += () => Game1._nextState = Game1.game;
        stackCobtainer.Add(_button1);

    }


    public void Initialize(){}

    public void LoadContent(){}

    public void Update(GameTime gameTime)
    {
        _button1.Update();

    }

    public void Draw(GameTime gameTime)
    {
        Color background = new Color(190, 244, 150);
        graphicsDevice.Clear(background);
        _spriteBatch.Begin();
        Geometry.DrawLine(_spriteBatch, new Vector2(0, 0), new Vector2(500, 500), Color.Red, 20);
        _button1.Draw(_spriteBatch);
        _spriteBatch.End();
    }
}