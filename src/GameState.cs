using Microsoft.Xna.Framework.Graphics;

public interface IGameState
{
    // Method signature (no implementation)
    void Initialize();

    // Property signature
    void LoadContent();

    // Method with parameters and return type
    void Update(float elapsedTime);

    void Draw(SpriteBatch spriteBatch_);
}