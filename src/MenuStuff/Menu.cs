using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using our_Game;
using System;

public class Menu : GameState
{
    StackContainer buttonContainer;

    public Menu(Game game): base(game)
    {

        SpriteFont font = Content.Load<SpriteFont>("Arial");

        buttonContainer = new StackContainer(new Point(500, 500), 20);

        Point ButtonSize = new Point(300, 100);

        //if (Game1.GameIsInitialized)
        
            SimpleButton gameButton = new SimpleButton(ButtonSize, "New Game", font);
            gameButton.OnClick += () => Game1.nextState = Game1.setup;
            buttonContainer.Add(gameButton);
        
        
        
            SimpleButton setupButton = new SimpleButton(ButtonSize, "Resume", font);
            setupButton.OnClick += () => Game1.nextState = Game1.game;
            buttonContainer.Add(setupButton);
        

        SimpleButton settingsButton = new SimpleButton(ButtonSize, "Settings", font);
        settingsButton.OnClick += () => Game1.nextState = Game1.settings;
        buttonContainer.Add(settingsButton);

        SimpleButton exitButton = new SimpleButton(ButtonSize, "Exit", font);
        exitButton.OnClick += () => game.Exit();
        buttonContainer.Add(exitButton);

        TriangleButton triangleButton = new TriangleButton(ButtonSize);
        triangleButton.OnClick += () => { };
        buttonContainer.Add(triangleButton);
        
        buttonContainer.MoveCenter(new Point(_graphics.PreferredBackBufferWidth/2, _graphics.PreferredBackBufferHeight/2));
        buttonContainer.SetDrawOutline(new Color(96, 96, 96), 3);
    }


    public override void Update(GameTime gameTime)
    {
        buttonContainer.Update();

    }

    public override void Draw(GameTime gameTime)
    {
        Color background = new Color(190, 244, 150);
        _graphicsDevice.Clear(background);
        spriteBatch.Begin();
        PrimitiveDrawer.DrawTriangle(spriteBatch, new Vector2(100, 100), new Vector2(300, 300), new Vector2(100, 300), Color.Red);
        buttonContainer.Draw(spriteBatch);
        spriteBatch.End();
    }
}