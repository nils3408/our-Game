using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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