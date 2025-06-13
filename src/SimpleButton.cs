

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class SimpleButton
{

    Vector2 position = new Vector2(0, 0);
    Microsoft.Xna.Framework.Rectangle rectangle;

    public event Action OnClick;

    private Texture2D pixelTexture;

    private Microsoft.Xna.Framework.Color buttonColor = Color.Black;

    public SimpleButton(Vector2 pos, Microsoft.Xna.Framework.Rectangle rectangle, GraphicsDevice graphicsDevice)
    {
        this.position = pos;
        this.rectangle = rectangle;

        pixelTexture = new Texture2D(graphicsDevice, 1, 1);
        pixelTexture.SetData(new[] { Color.Black });
    }

    public void Update(MouseState mouseState)
    {

        Point mousePos = new Point(mouseState.X, mouseState.Y);
        bool isHovered = rectangle.Contains(mousePos);

        // Check for click
        if (isHovered)
        {
            System.Diagnostics.Debug.WriteLine("hello");
            buttonColor = Color.Gray;
            
            if (mouseState.LeftButton == ButtonState.Pressed) {
                OnClick?.Invoke();
            }
        } else {
            buttonColor = Color.Black;
        }
        pixelTexture.SetData(new[] { buttonColor });
    }

    public void Draw(SpriteBatch spriteBatch)
    {



        // In Draw()
         // x, y, width, height
        spriteBatch.Draw(pixelTexture, rectangle, buttonColor);
        
    }
}