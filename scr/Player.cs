using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Player
{
    private Texture2D texture;
    public Rectangle Rectangle { get; private set; }

    public Player(GraphicsDevice graphicsDevice, Vector2 position, int width = 50, int height = 50)
    {
        // Erstelle eine schwarze 1x1-Textur
        texture = new Texture2D(graphicsDevice, 1, 1);
        texture.SetData(new[] { Color.Black });

        // Erstelle das Rechteck mit Position und Größe
        Rectangle = new Rectangle((int)position.X, (int)position.Y, width, height);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, Rectangle, Color.White);
    }

    // Optional: Bewegung oder andere Logik
    public void move_left()
    {
        Rectangle = new Rectangle(
            Rectangle.X - 10,
            Rectangle.Y,
            Rectangle.Width,
            Rectangle.Height
        );
    }
    public void move_right()
    {
        Rectangle = new Rectangle(
            Rectangle.X + 10,
            Rectangle.Y,
            Rectangle.Width,
            Rectangle.Height
        );
    }

    public void move_up()
    {
        Rectangle = new Rectangle(
            Rectangle.X ,
            Rectangle.Y- 10,
            Rectangle.Width,
            Rectangle.Height
        );
    }
    
     public void move_down()
    {
        Rectangle = new Rectangle(
            Rectangle.X,
            Rectangle.Y+10,
            Rectangle.Width,
            Rectangle.Height
        );
    }
}
