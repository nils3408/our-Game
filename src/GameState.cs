using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using our_Game;
/**
    Interface das die Schnittstelle zwischen den verschieden Game States implementiert,
    also Menu und Game vor allem.
 
    Man hätte auch nur Upodate und draw vorschreiben können und den rest dem Konstruktur pberlassen können,
    aber so implementiert es genau die Standart Methoden von Game im Framework und so konnte vorherige Arbeit nahtlos übernommen werden
    geschrieben Thies
 */

public interface IGameState
{
    void Initialize();

    void LoadContent();

    void Update(GameTime gameTime);

    void Draw(GameTime gameTime);
}

/**
Jetzt hab ich das als abstrakte Klasse umgesetzt um die Felder die alle Gamestate brauchen oder brauchen könnten mit zu nehmen.
das Interface ist an der Stelle dann eher unnötig, aber hatte ich ja schon.
*/


public abstract class GameState : IGameState
{
    protected Game baseGame;
    protected GraphicsDeviceManager _graphics;
    protected GraphicsDevice _graphicsDevice;
    protected SpriteBatch spriteBatch;
    protected ContentManager Content;

    public GameState(Game game)
    {
        baseGame = game;
        _graphics = Game1.graphics; // Diese Line macht die ganze Klasse leider nicht mehr allgemein anwedbar sondern spezifisch zu out_game
        _graphicsDevice = game.GraphicsDevice;
        spriteBatch = new SpriteBatch(_graphicsDevice);
        Content = game.Content;
    }


    public virtual void Initialize() { }

    public virtual void LoadContent() { }

    public abstract void Update(GameTime gameTime);

    public abstract void Draw(GameTime gameTime);

    public void Exit()
    {
        baseGame.Exit();
    }
}

