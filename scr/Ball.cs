// von Nils 


using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Ball
{
    private Texture2D texture;
    public Rectangle Rectangle { get; private set; }
    public int velocity;

    public Ball(GraphicsDevice graphicsDevice, Vector2 position, int width = 20, int height = 20)
    {
        // Erstelle eine rote 1x1-Textur
        texture = new Texture2D(graphicsDevice, 1, 1);
        texture.SetData(new[] { Color.Red });  // <- ROT statt schwarz

        Rectangle = new Rectangle((int)position.X, (int)position.Y, width, height);
        velocity = 7;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, Rectangle, Color.White);
    }

    public void move()
    {
        Rectangle = new Rectangle(
            Rectangle.X + velocity,
            Rectangle.Y,
            Rectangle.Width,
            Rectangle.Height
        );
    }



    public void change_direction(){
        velocity *= -1;
    }


    public bool colliderect_with_player(Player player){
        return Rectangle.Intersects(player.Rectangle);
    }

}
