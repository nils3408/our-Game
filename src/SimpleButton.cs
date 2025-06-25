

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

public class SimpleButton : UIElement
{

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


    public SimpleButton(UIElement Parent, Rectangle bounds):base(Parent,bounds){}

    public SimpleButton(UIElement Parent, Rectangle bounds, String text, SpriteFont font):base(Parent,bounds)
    {
        this.text = text;
        this.font = font;
    }

    public override void Update()
    {
        MouseState mouseState = Mouse.GetState();
        Point mousePos = new Point(mouseState.X, mouseState.Y);
        isHovered = bounds.Contains(mousePos);

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

    public override void Draw(SpriteBatch spriteBatch)
    {
        Texture2D pixelTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
        pixelTexture.SetData(new[] { Color.White });

        // In Draw()
        // x, y, width, height
        spriteBatch.Draw(pixelTexture, bounds, colorOutline);

        Rectangle innerRect = new Rectangle(bounds.X + outline, bounds.Y + outline, bounds.Width - 2 * outline, bounds.Height - 2 * outline);
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
                bounds.X + (bounds.Width - textSize.X) / 2,
                bounds.Y + (bounds.Height - textSize.Y) / 2
            );

        
            spriteBatch.DrawString(font, text, textPosition, Color.Black
                //, 0 , Vector2.Zero,1,SpriteEffects.None,0
                 ); 
        }
    }


}