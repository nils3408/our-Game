using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using our_Game;

public class Settings : GameState
{


    ElementContainer container = new ElementContainer();

    public Settings(Game baseGame) : base(baseGame)
    {

        SpriteFont font = Content.Load<SpriteFont>("Arial");
        SimpleButton homeButton = new SimpleButton(new Point(20, 20), new Point(200, 100), "Home", font);
        homeButton.OnClick += () =>
        {
            Game1.nextState = new Menu(baseGame);
        };
        container.Add(homeButton);

        HorizontalContainer H1 = new HorizontalContainer();
        H1.SetSpacing(30);

        Point TextSize = new Point(400, 100);

        StackContainer S1 = new StackContainer();
        S1.Add(new Textfield("Left Player", TextSize));
        ControlsEditor leftPlayerControls = new ControlsEditor(Game1.leftPlayerControls);
        S1.Add(leftPlayerControls);
        H1.Add(S1);

        StackContainer S2 = new StackContainer();
        S2.Add(new Textfield("Right Player", TextSize));
        ControlsEditor rightPlayerControls = new ControlsEditor(Game1.rightPlayerControls);
        S2.Add(rightPlayerControls);
        H1.Add(S2);

        H1.MoveCenter(new Point(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2));
        container.Add(H1);

    }


    public override void Update(GameTime gameTime)
    {
        container.Update();


        //Zurück ins Menu wenn ESC losgelassen wird 
        if (InputHandler.IsReleased(Keys.Escape))
        {
            System.Diagnostics.Debug.WriteLine("escape!");
            Game1.nextState = new Menu(baseGame);
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