// von Nils 
using System.Threading;


using System;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Ball
{
    private Texture2D texture;
    public Rectangle Rectangle { get; private set; }
    public float velocityX =(float) 73.5;
    public float g = 9.81f;    //gravity

    public float x_pos;
    public float y_pos;
    public float alpha = MathHelper.ToRadians(40f);
    public float h;   //starting postion of y
    public float starting_x_pos;


    public Ball(GraphicsDevice graphicsDevice, Vector2 position, int width = 20, int height = 20)
    {
        // Erstelle eine rote 1x1-Textur
        texture = new Texture2D(graphicsDevice, 1, 1);
        texture.SetData(new[] { Color.Red });  // <- ROT statt schwarz

        x_pos = position.X;
        y_pos = position.Y;
        h = position.Y;
        starting_x_pos = position.X;
        Rectangle = new Rectangle((int)position.X, (int)position.Y, width, height);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, Rectangle, Color.White);
    }

    public void move_linear()
    {
        x_pos += velocityX;

        Rectangle = new Rectangle(
            (int)x_pos,
            (int)y_pos,
            Rectangle.Width,
            Rectangle.Height
        );
    }


    // schräger Wurf
    // https://www.leifiphysik.de/mechanik/waagerechter-und-schraeger-wurf/grundwissen/schraeger-wurf-nach-oben-mit-anfangshoehe

    // x(t) = x₀ + v₀ₓ * t

    public void move_parabel()
    {
        x_pos += velocityX/12;
        float dx = Math.Abs(x_pos - starting_x_pos);

        float cosAlpha = (float)Math.Cos(alpha);
        float tanAlpha = (float)Math.Tan(alpha);

        y_pos = (-0.5f * (g / (velocityX * velocityX * cosAlpha * cosAlpha)) * (dx * dx)
                        + tanAlpha * dx
                        + h);


        //opposite direction as in Grafik G1
        float distance_to_middle = y_pos - h;
        float y2 = h - distance_to_middle;
        y_pos = y2;


        Rectangle = new Rectangle(
            (int)x_pos,
            (int)y_pos,
            Rectangle.Width,
            Rectangle.Height
        );

        //Console.WriteLine("x: " + x_pos + "  y: " + y_pos);
        Console.WriteLine("X_pos: " + x_pos + "  y_pos: " + y_pos);

        
        
    }



    public void change_direction()
    {
        Console.WriteLine("change:direction()");
        velocityX *= -1;
        starting_x_pos = x_pos;
        h = y_pos;
    }


    public bool colliderect_with_player(Player player)
    {
        return Rectangle.Intersects(player.Rectangle);
    }

}
