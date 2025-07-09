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

    public static GameLogic game;
    public static Menu menu;
    public static Settings settings;
    public static GameSetup setup;
    public static bool GameIsInitialized = false;



    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        //_graphics.ToggleFullScreen();


        graphics.PreferredBackBufferWidth = 1920;
        graphics.PreferredBackBufferHeight = 1080;
    }

    protected override void Initialize()
    {
        PrimitiveDrawer.Initialize(GraphicsDevice, Content);
        PlayerFactory.Initialize(GraphicsDevice, Content);

        game = new GameLogic(this);
        setup = new GameSetup(this);
 
        menu = new Menu(this);
        settings = new Settings(this);

        // Initializing each GameState
        game.Initialize();
        menu.Initialize();
        settings.Initialize();
        setup.Initialize();

        curState = menu;
        nextState = menu;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // load Content for each GameState
        menu.LoadContent();
        game.LoadContent();
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

}
