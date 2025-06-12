using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

public class Menu : IGameState
{
    private GraphicsDevice graphicsDevice;
    private  SpriteBatch _spriteBatch;
    private ContentManager contentManager;

    public Menu(GraphicsDevice graphicsDevice, ContentManager contentManager)
    {
        this.graphicsDevice = graphicsDevice;
        this._spriteBatch = new SpriteBatch(graphicsDevice);
        this.contentManager = contentManager;

    }


    public void Initialize(){}

    public void LoadContent(){}

    public void Update(GameTime gameTime)
    {

    }

    public void Draw(GameTime gameTime)
    {
        graphicsDevice.Clear(Color.White);
        _spriteBatch.Begin();
        
        _spriteBatch.End();
    }
}