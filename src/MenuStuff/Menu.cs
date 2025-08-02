using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using our_Game;
using System;
using System.Linq;

public class Menu : GameState
{
    StackContainer buttonContainer;
    Texture2D backgroundPicture;
    Texture2D pixel;

    public Menu(Game game) : base(game)
    {

        SpriteFont font   = Content.Load<SpriteFont>("Arial");
        backgroundPicture = Content.Load<Texture2D> ("background");

        buttonContainer = new StackContainer(new Point(500,500), 20);
        Point ButtonSize = new Point(300, 100);

        if (Game1.openGames.Count() == 0)
        {

            SimpleButton gameButton = new SimpleButton(ButtonSize, "New Game", font);
            gameButton.OnClick += () => Game1.nextState = new GameSetup(baseGame);
            buttonContainer.Add(gameButton);

        } else {

            SimpleButton setupButton = new SimpleButton(ButtonSize, "Resume", font);
            setupButton.OnClick += () => Game1.nextState = Game1.openGames.First();
            buttonContainer.Add(setupButton);
        }

        SimpleButton settingsButton = new SimpleButton(ButtonSize, "Settings", font);
        settingsButton.OnClick += () => Game1.nextState = Game1.settings;
        buttonContainer.Add(settingsButton);

        SimpleButton exitButton = new SimpleButton(ButtonSize, "Exit", font);
        exitButton.OnClick += () => { Game1.SaveData(); game.Exit(); };
        buttonContainer.Add(exitButton);
        
        buttonContainer.MoveCenter(new Point(_graphics.PreferredBackBufferWidth/2, _graphics.PreferredBackBufferHeight/2+140));
        buttonContainer.SetDrawOutline(new Color(96, 96, 96), 3);
    }


    public override void Update(GameTime gameTime)
    {
        buttonContainer.Update();
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || InputHandler.IsReleased(Keys.Escape))Exit();
    }

    public override void Draw(GameTime gameTime)
    {
        Color background_color = new Color(190, 244, 150);
        Texture2D pixel= new Texture2D(_graphicsDevice, 1, 1);
        pixel.SetData(new[] { background_color });

        spriteBatch.Begin();
        
        spriteBatch.Draw(backgroundPicture, new Rectangle(0, 0, 1920, 1080), Color.White);
        spriteBatch.Draw(pixel, new Rectangle(770, 470, 333, 380), background_color);
        buttonContainer.Draw(spriteBatch);
        
        spriteBatch.End();
    }
}