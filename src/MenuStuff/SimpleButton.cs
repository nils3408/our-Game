

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class SimpleButton : UIElement
{


  
    int outline = 5;

    Color color = Color.White;
    Color colorOnHover = new Color(234,234,234);
    Color colorOutline = new Color(96, 96, 96);

    String text = "";
    SpriteFont font = null;

    bool isHovered = false;

    public event Action OnClick;


    public SimpleButton(Rectangle rectangle):base(rectangle){}

    public SimpleButton(UIElement parent, Rectangle rectangle, String text, SpriteFont font):base(rectangle)
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
        if (isHovered && InputHandler.IsMouseLeftReleased())
        {
            OnClick?.Invoke();
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        //Rectangle offsetBounds = new Rectangle(bounds.X + );

        if (!isHovered)
        {
            Geometry.DrawRectangleWithOutline(spriteBatch, bounds, color, colorOutline, outline);
        }
        else
        {
            Geometry.DrawRectangleWithOutline(spriteBatch, bounds, colorOnHover, colorOutline, outline);
        }

        if (text != "") {
            Vector2 textSize = font.MeasureString(text);
        
            Vector2 textPosition = new Vector2(
                bounds.X + (bounds.Width - textSize.X) / 2,
                bounds.Y + (bounds.Height - textSize.Y) / 2
            );

        
            spriteBatch.DrawString(font, text, textPosition, Color.Black); 
        }
    }
}