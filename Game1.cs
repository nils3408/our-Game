//code by Nils 


using  System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace softwareprojekt;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Player player1;
    private Player player2;

    private Ball football;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Erzeuge Spieler als schwarzes Rechteck
        player1 = new Player(GraphicsDevice, new Vector2(100, 100));
        player2 = new Player(GraphicsDevice, new Vector2(700, 100));
        football = new Ball(GraphicsDevice, new Vector2(200, 200));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        HandleMovement();
        football.move_parabel();

        if (Ball_out_of_bounds(football)) football.change_direction();
        if ((football.colliderect_with_player(player1)) | (football.colliderect_with_player(player2)))
            football.change_direction();


            base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        player1.Draw(_spriteBatch);
        player2.Draw(_spriteBatch);
        football.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }


    private void HandleMovement()
    {
        var keyboard = Keyboard.GetState();

        if (keyboard.IsKeyDown(Keys.A)) player1.move_left();
        if (keyboard.IsKeyDown(Keys.D)) player1.move_right();
        if (keyboard.IsKeyDown(Keys.S)) player1.move_down();
        if (keyboard.IsKeyDown(Keys.W)) player1.move_up();


        if (keyboard.IsKeyDown(Keys.Left)) player2.move_left();
        if (keyboard.IsKeyDown(Keys.Right)) player2.move_right();
        if (keyboard.IsKeyDown(Keys.Up)) player2.move_up();
        if (keyboard.IsKeyDown(Keys.Down)) player2.move_down();

    }


    public bool Ball_out_of_bounds(Ball ball1) {
        return ((ball1.Rectangle.X <= 0) | (ball1.Rectangle.X >= 800));
    }

}

