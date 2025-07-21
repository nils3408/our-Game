


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

        Textfield textfield = new Textfield("Choose a Player", new Point(500, 100));
        textfield.SetDrawOutline(outlineColor, 1);
        container.Add(textfield);
        textfield.MoveCenter(new Point(ScreenCenter.X, spacing + textfield.GetBounds().Height / 2));

        SimpleButton homeButton = new SimpleButton(new Point(spacing, spacing), new Point(200, 100), "Home", font);
        homeButton.OnClick += () => Game1.nextState = Game1.menu;
        
        container.Add(homeButton);


        HorizontalContainer H1 = new HorizontalContainer();
        H1.SetSpacing(30);
        PlayerSelection leftSelection = new PlayerSelection(Content);
        PlayerSelection rightSelection = new PlayerSelection(Content);
        H1.Add(leftSelection);
        H1.Add(rightSelection);
        H1.MoveCenter(new Point(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2));
        container.Add(H1);


        SimpleButton startButton = new SimpleButton(new Point(500, 100), "Start New Game", font);
        startButton.OnClick += () =>
        {
            if (leftSelection.isChoosen && rightSelection.isChoosen)
            {
                Console.WriteLine($"start Game");
                Player leftPlayer = PlayerFactory.CreatePlayer(leftSelection.playerType, true);
                Player rightPlayer = PlayerFactory.CreatePlayer(rightSelection.playerType, false);

                GameLogic newGame = new GameLogic(game, leftPlayer, rightPlayer);
                newGame.Initialize();
                newGame.LoadContent();
                Game1.openGames.Add(newGame);
                
                Game1.nextState = newGame;
            }
            else
            {
                //Text anzeigen : beide Charaktere müssen ausgewählt sein
            }
        };
        startButton.MoveCenter(new Point(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight - spacing - startButton.GetBounds().Height / 2));
        container.Add(startButton);

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



public class ShowPlayerType : UIElement
{
    ContentManager Content;

    int curIndex = 0;

    int maxTypes = 0;

    public ShowPlayerType(Point Size, ContentManager Content) : base(Size)
    {
        maxTypes = PlayerFactory.TypesCount;
        this.Content = Content;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        //PrimitiveDrawer.DrawRectangleOutline(spriteBatch, GetBounds(), Color.Red, 1);
        Texture2D texture = PlayerFactory.GetPlayerTexture((PlayerFactory.Types)curIndex);
        spriteBatch.Draw(texture, GetBounds(), null, Color.White);
    }

    public void SwipeRight()
    {
        curIndex = (curIndex + 1) % maxTypes;
    }

    public void SwipeLeft()
    {
        curIndex = (curIndex + 1) % maxTypes;
    }

    public PlayerFactory.Types GetCurPlayer()
    {
        return (PlayerFactory.Types) curIndex;
    }

    public override void Update()
    {

    }
}

//Die Box wo man den Player auswählen kann
public class PlayerSelection : StackContainer
{
    Color outlineColor = new Color(96, 96, 96);
    int spacing = 7;

    public bool isChoosen = false;

    public PlayerFactory.Types playerType = PlayerFactory.Types.Standart;

    SimpleButton chooseButton;

    public PlayerSelection(ContentManager Content)
    {
        SpriteFont font = Content.Load<SpriteFont>("Arial");



        base.SetSpacing(spacing);

        Point ButtonSize = new Point(300, 100);

        ShowPlayerType playerDisplay = new ShowPlayerType(new Point(300, 500), Content);
        base.Add(playerDisplay);

        chooseButton = new SimpleButton(ButtonSize, "Choose", font);
        chooseButton.OnClick += () =>
        {
            //isChoosen = !isChoosen;
            playerType = playerDisplay.GetCurPlayer();
        };
        chooseButton.SetToStayPressed();
        chooseButton.SetColor(Color.White, Color.Green, outlineColor);
        base.Add(chooseButton);

        HorizontalContainer HContainer = new HorizontalContainer(100);

        Point TSize = new Point(100, 100);

        TriangleButton leftButton = new TriangleButton(TSize);
        leftButton.OnClick += () => { if (!chooseButton.GetState()) playerDisplay.SwipeRight(); };
        leftButton.ToggleFlip();
        HContainer.Add(leftButton);

        TriangleButton rightButton = new TriangleButton(TSize);
        rightButton.OnClick += () => { if (!chooseButton.GetState()) playerDisplay.SwipeRight(); };

        HContainer.Add(rightButton);
        HContainer.SetDrawOutline(outlineColor, 1);

        base.Add(HContainer);

        //buttonContainer.MoveCenter(new Point(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2));
        base.SetDrawOutline(outlineColor, 3);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }

    public override void Update()
    {
        base.Update();
        isChoosen = chooseButton.GetState();
    }

 

}