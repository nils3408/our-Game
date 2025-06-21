

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class SimpleButton
{


    Microsoft.Xna.Framework.Rectangle rect;
    int outline = 5;

    Color color = Color.White;
    Color colorOnHover = new Color(234,234,234);
    Color colorOutline = new Color(96, 96, 96);

    String text = "";
    SpriteFont font = null;

    bool isHovered = false;

    int timeout = 0;
    const int FramesPerTimeout = 5;

    public event Action OnClick;


    public SimpleButton(Rectangle rectangle)
    {
        this.rect = rectangle;

    }

    public SimpleButton(Rectangle rectangle, String text, SpriteFont font)
    {
        this.rect = rectangle;
        this.text = text;
        this.font = font;
    }

    public void Update(MouseState mouseState)
    {

        Point mousePos = new Point(mouseState.X, mouseState.Y);
        isHovered = rect.Contains(mousePos);

        // Check for click
        if (isHovered && mouseState.LeftButton == ButtonState.Pressed)
        {
            if (timeout == 0)
            {
                System.Diagnostics.Debug.WriteLine("click!");
                timeout += FramesPerTimeout;
                OnClick?.Invoke();
            }
            else {
                timeout--;
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
    {
        Texture2D pixelTexture = new Texture2D(graphicsDevice, 1, 1);
        pixelTexture.SetData(new[] { Color.White });

        // In Draw()
        // x, y, width, height
        spriteBatch.Draw(pixelTexture, rect, colorOutline);

        Rectangle innerRect = new Rectangle(rect.X + outline, rect.Y + outline, rect.Width - 2 * outline, rect.Height - 2 * outline);
        if (!isHovered)
        {
            spriteBatch.Draw(pixelTexture, innerRect, color);
        }
        else
        {
            spriteBatch.Draw(pixelTexture, innerRect, colorOnHover);
        }

        if (text != "") {
            Vector2 textSize = font.MeasureString(text);
        
            Vector2 textPosition = new Vector2(
                rect.X + (rect.Width - textSize.X) / 2,
                rect.Y + (rect.Height - textSize.Y) / 2
            );

        
            spriteBatch.DrawString(font, text, textPosition, Color.Black, 0 , Vector2.Zero,2,SpriteEffects.None,0); 
        }
    }
}