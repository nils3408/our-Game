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
        PrimitiveDrawer.Initialize(GraphicsDevice);

        game = new GameLogic(this);
        menu = new Menu(this);
        settings = new Settings(this);

        // Initializing each GameState
        game.Initialize();
        menu.Initialize();
        settings.Initialize();

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
        if (curState == menu)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || InputHandler.IsReleased(Keys.Escape))Exit();
        }
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

    public void ExitGame()
    { 
        base.Exit();
    }

}
