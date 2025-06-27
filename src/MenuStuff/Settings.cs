using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using our_Game;

public class Settings : GameState
{
    public Settings(Game baseGame) : base(baseGame) { }


    public override void Update(GameTime gameTime)
    {

    }

    public override void Draw(GameTime gameTime)
    {
        Color background = Color.Crimson;
        _graphicsDevice.Clear(background);
        spriteBatch.Begin();
        PrimitiveDrawer.DrawLine(spriteBatch, new Vector2(0, 0), new Vector2(500, 500), Color.Red, 20);
        spriteBatch.End();
    }
}