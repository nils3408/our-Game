using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace our_Game;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private  SpriteBatch _spriteBatch;

    private static IGameState curState;

    private static GameLogic game;
    private static Menu menu;
    private static Settings settings;





    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;



        
        _graphics.PreferredBackBufferWidth = 800;
        _graphics.PreferredBackBufferHeight = 480;
    }

    protected override void Initialize()
    {
        game = new GameLogic(GraphicsDevice, Content, new Player[] { });
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
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // update current Game State

        curState.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        //GraphicsDevice.Clear(Color.CornflowerBlue);

        curState.Draw(gameTime);

    }
}
