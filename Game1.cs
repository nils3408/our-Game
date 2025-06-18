using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace our_Game;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private  SpriteBatch _spriteBatch;

    private static IGameState curState;
    public static IGameState _nextState;

    public static GameLogic game;
    public static Menu menu;
    public static Settings settings;



    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        //_graphics.ToggleFullScreen();
        
        
        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.PreferredBackBufferHeight = 1080;
    }

    protected override void Initialize()
    {
        game = new GameLogic(_graphics ,GraphicsDevice, Content);
        menu = new Menu(GraphicsDevice, Content);
        settings = new Settings();
        // Initializing each GameState
        game.Initialize();
        menu.Initialize();
        settings.Initialize();

        curState = menu;

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
        if (curState == menu)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
        }
        if (_nextState != null && _nextState != curState) curState = _nextState;

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
