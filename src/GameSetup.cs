


using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        container.Add(textfield);
        textfield.MoveCenter(new Point(ScreenCenter.X, spacing + textfield.GetBounds().Height / 2));

        SimpleButton homeButton = new SimpleButton(new Point(spacing, spacing), new Point(200, 100), "Home", font);
        homeButton.OnClick += () => Game1.nextState = Game1.menu;
        container.Add(homeButton);


        buttonContainer.SetSpacing(7);
        //buttonContainer.ToggleCentralization();

        Point ButtonSize = new Point(300, 100);

        ShowePlayerTyper playerDisplay = new ShowePlayerTyper(new Point(300, 500), Content);
        buttonContainer.Add(playerDisplay);

        SimpleButton exitButton = new SimpleButton(ButtonSize, "Choose", font);
        exitButton.OnClick += () => game.Exit();
        buttonContainer.Add(exitButton);

        HorizontalContainer HContainer = new HorizontalContainer(100);

        Point TSize = new Point(100, 100);

        TriangleButton leftButton = new TriangleButton(TSize);
        leftButton.OnClick += () => { playerDisplay.SwipeRight(); };
        leftButton.ToggleFlip();
        HContainer.Add(leftButton);

        TriangleButton rightButton = new TriangleButton(TSize);
        rightButton.OnClick += () => { playerDisplay.SwipeRight(); };

        HContainer.Add(rightButton);
        HContainer.SetDrawOutline(outlineColor, 1);

        buttonContainer.Add(HContainer);

        buttonContainer.MoveCenter(new Point(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2));
        buttonContainer.SetDrawOutline(outlineColor, 3);
    }

    public override void Update(GameTime gameTime)
    {
        //Zurück ins Menu wenn ESC losgelassen wird 
        if (InputHandler.IsReleased(Keys.Escape))
        {
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



public class ShowePlayerTyper : UIElement
{
    ContentManager Content;

    int curIndex = 0;

    public Texture2D[] players = new Texture2D[2];

    public ShowePlayerTyper(Point Size, ContentManager Content) : base(Size)
    {
        this.Content = Content;
        players[0] = Content.Load<Texture2D>("KopfkickerChar2_neu");
        players[1] = Content.Load<Texture2D>("Spiderman");
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        //PrimitiveDrawer.DrawRectangleOutline(spriteBatch, GetBounds(), Color.Red, 1);
        spriteBatch.Draw(players[curIndex], GetBounds(), null, Color.White);
    }

    public void SwipeRight()
    {
        curIndex = (curIndex + 1) % players.Length;
    }

    public void SwipeLeft()
    {
        curIndex = (curIndex + 1) % players.Length;
    }

    public Texture2D GetCurPlayer()
    {
        return players[curIndex];
    }

    public override void Update()
    {

    }
}