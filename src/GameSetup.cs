


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using our_Game;

public class GameSetup : GameState
{

    StackContainer buttonContainer = new StackContainer();
    HorizontalContainer HContainer = new HorizontalContainer();

    public GameSetup(Game game) : base(game)
    {

        SpriteFont font = Content.Load<SpriteFont>("Arial");

        buttonContainer.SetSpacing(7);

        Point ButtonSize = new Point(300, 100);

        SimpleButton settingsButton = new SimpleButton(ButtonSize, "c", font);
        settingsButton.OnClick += () => Game1.nextState = Game1.settings;
        buttonContainer.Add(settingsButton);

        SimpleButton exitButton = new SimpleButton(ButtonSize, "d", font);
        exitButton.OnClick += () => game.Exit();
        buttonContainer.Add(exitButton);

        TriangleButton triangleButton = new TriangleButton(ButtonSize);
        triangleButton.OnClick += () => { };
        buttonContainer.Add(triangleButton);
    }

    public override void Update(GameTime gameTime)
    {
        buttonContainer.Update();
        HContainer.Update();
    }

    public override void Draw(GameTime gameTime)
    {
        Color background = Color.BlueViolet;
        _graphicsDevice.Clear(background);
        spriteBatch.Begin();
        buttonContainer.Draw(spriteBatch);
        HContainer.Draw(spriteBatch);
        spriteBatch.End();
    }
}