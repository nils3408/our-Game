using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using our_Game;

public class Settings : GameState
{
    public PlayerControls leftControlls;
    public PlayerControls rightControlls;

    ElementContainer container = new ElementContainer();

    public Settings(Game baseGame) : base(baseGame)
    {
        leftControlls = PlayerControls.getStandartLeft();
        rightControlls = PlayerControls.getStandartRight();

        SpriteFont font = Content.Load<SpriteFont>("Arial");
        SimpleButton homeButton = new SimpleButton(new Point(20, 20), new Point(200, 100), "Home", font);
        homeButton.OnClick += () => Game1.nextState = Game1.menu;
        container.Add(homeButton);

    }


    public override void Update(GameTime gameTime)
    {
        container.Update();


        //Zurück ins Menu wenn ESC losgelassen wird 
        if (InputHandler.IsReleased(Keys.Escape))
        {
            System.Diagnostics.Debug.WriteLine("escape!");
            Game1.nextState = Game1.menu;
        }
        
    }

    public override void Draw(GameTime gameTime)
    {
        Color background = Color.Crimson;
        _graphicsDevice.Clear(background);
        spriteBatch.Begin();
        container.Draw(spriteBatch);
        spriteBatch.End();
    }
}