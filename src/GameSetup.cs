


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using our_Game;

public class GameSetup : GameState
{
    ElementContainer container = new ElementContainer();


    Point ScreenCenter;

    Color outlineColor = new Color(96, 96, 96);

    int spacing = 20;
    public GameSetup(Game game) : base(game)
    {

        ScreenCenter = new Point(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);

        SpriteFont font = Content.Load<SpriteFont>("Arial");

        StackContainer buttonContainer = new StackContainer(ScreenCenter, spacing);
        container.Add(buttonContainer);

        Textfield textfield = new Textfield("Choose a Player", new Point(500, 100));
        textfield.SetDrawOutline(outlineColor, 1);
        //container.Add(textfield);
        textfield.MoveCenter(new Point(ScreenCenter.X,spacing+textfield.GetBounds().Height/2));

        SimpleButton homeButton = new SimpleButton(new Point(spacing, spacing), new Point(200, 100), "Home", font);
        homeButton.OnClick += () => Game1.nextState = Game1.menu;
        container.Add(homeButton);


        buttonContainer.SetSpacing(7);
        buttonContainer.ToggleCentralization();

        Point ButtonSize = new Point(300, 100);



        SimpleButton exitButton = new SimpleButton(ButtonSize, "d", font);
        exitButton.OnClick += () => game.Exit();
        buttonContainer.Add(exitButton);

        HorizontalContainer HContainer = new HorizontalContainer(100);

        Point TSize = new Point(100, 100);

        TriangleButton triangleButton = new TriangleButton(TSize);
        triangleButton.OnClick += () => { };
        triangleButton.ToggleFlip();
        HContainer.Add(triangleButton);

        TriangleButton triangleButton2 = new TriangleButton(TSize);
        triangleButton2.OnClick += () => { };

        HContainer.Add(triangleButton2);
        HContainer.SetDrawOutline(outlineColor, 1);

        buttonContainer.Add(HContainer);

        //buttonContainer.MoveCenter(new Point(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2));
        buttonContainer.SetDrawOutline(outlineColor, 3);
    }

    public override void Update(GameTime gameTime)
    {
        //Zurück ins Menu wenn ESC losgelassen wird 
        if (InputHandler.IsReleased(Keys.Escape)) {
            System.Diagnostics.Debug.WriteLine("escape!");
            Game1.nextState = Game1.menu;
        }

        container.Update();
        //HContainer.Update();
    }

    public override void Draw(GameTime gameTime)
    {
        Color background = Color.BlueViolet;
        _graphicsDevice.Clear(background);
        spriteBatch.Begin();
        //HContainer.Draw(spriteBatch);
        container.Draw(spriteBatch);
        spriteBatch.End();
    }
}