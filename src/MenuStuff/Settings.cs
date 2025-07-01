using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using our_Game;

public class Settings : GameState
{
    public Settings(Game baseGame) : base(baseGame) { }


    public override void Update(GameTime gameTime)
    {
            //Zurück ins Menu wenn ESC losgelassen wird 
            if (InputHandler.IsReleased(Keys.Escape)) {
                System.Diagnostics.Debug.WriteLine("escape!");
                Game1.nextState = Game1.menu;
            }
    }

    public override void Draw(GameTime gameTime)
    {
        Color background = Color.Crimson;
        _graphicsDevice.Clear(background);
        spriteBatch.Begin();
        
        spriteBatch.End();
    }
}