using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace our_Game;

public class Game1 : Game
{
    public static GraphicsDeviceManager graphics;
    private SpriteBatch _spriteBatch;

    private static IGameState curState;
    public static IGameState nextState;

    public static List<GameLogic> openGames;
    public static Menu menu;
    public static Settings settings;
    public static GameSetup setup;
    public static bool GameIsInitialized = false;

    public static PlayerControls leftPlayerControls;
    public static PlayerControls rightPlayerControls;



    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        //_graphics.ToggleFullScreen();


        graphics.PreferredBackBufferWidth = 1920;
        graphics.PreferredBackBufferHeight = 1080;

        openGames = new List<GameLogic>();
        leftPlayerControls = PlayerControls.getStandartLeft();
        rightPlayerControls = PlayerControls.getStandartRight();
    }

    protected override void Initialize()
    {
        PrimitiveDrawer.Initialize(GraphicsDevice, Content);
        PlayerFactory.Initialize(GraphicsDevice, Content);

        settings = new Settings(this);
        setup = new GameSetup(this);

        menu = new Menu(this);
        

        // Initializing each GameState
        menu.Initialize();
        settings.Initialize();
        setup.Initialize();

        curState = menu;
        nextState = curState;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // load Content for each GameState
        menu.LoadContent();
        settings.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        InputHandler.Update();

        if (nextState == null) Exit();
        if (nextState != null && nextState != curState) curState = nextState;

        // update current Game State

        curState.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        curState.Draw(gameTime);
        base.Draw(gameTime);
    }

// kann ich nicht nutzen in den GameStates, da Game1 als einfaches Game übergeben wird
    public void OpenNewGame(Player leftPlayer, Player rightPlayer)
    {
        GameLogic newGame = new GameLogic(this, leftPlayer, rightPlayer);
        newGame.Initialize();
        newGame.LoadContent();
        openGames.Add(newGame);
        nextState = newGame;
    }


}
