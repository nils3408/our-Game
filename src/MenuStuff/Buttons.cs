

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class SimpleButton : UIElement
{



    int outline = 5;

    Color color = Color.White;
    Color colorOnHover = new Color(234, 234, 234);
    Color colorOutline = new Color(96, 96, 96);

    String text = "";
    SpriteFont font = null;

    bool isHovered = false;

    bool isSetToStayPressed = false;

    bool isPressed = false;

    public event Action OnClick;




    public SimpleButton(Rectangle rectangle) : base(rectangle) { }
    public SimpleButton(Point Size) : base(Size) { }
    public SimpleButton(Point RelativePosition, Point Size) : base(RelativePosition, Size) { }
    public SimpleButton(Rectangle rectangle, String text, SpriteFont font) : base(rectangle)
    {
        this.text = text;
        this.font = font;
    }
    public SimpleButton(Point size, String text, SpriteFont font) : base(size)
    {
        this.text = text;
        this.font = font;
    }
    public SimpleButton(Point RelativePosition, Point Size, String text, SpriteFont font) : base(RelativePosition, Size)
    {
        this.text = text;
        this.font = font;
    }

    public override void Update()
    {
        MouseState mouseState = Mouse.GetState();
        Point mousePos = new Point(mouseState.X, mouseState.Y);
        isHovered = GetBounds().Contains(mousePos);

        // Check for click
        if (isHovered && InputHandler.IsMouseLeftReleased())
        {
            Click();
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        //Rectangle offsetBounds = new Rectangle(bounds.X + );

        if (!(isHovered || isPressed))
        {
            PrimitiveDrawer.DrawRectangleWithOutline(spriteBatch, GetBounds(), color, colorOutline, outline);
        }
        else
        {
            PrimitiveDrawer.DrawRectangleWithOutline(spriteBatch, GetBounds(), colorOnHover, colorOutline, outline);
        }

        if (text != "")
        {
            Vector2 textSize = font.MeasureString(text);

            Vector2 textPosition = new Vector2(
                GetPosition().X + (Size.X - textSize.X) / 2,
                GetPosition().Y + (Size.Y - textSize.Y) / 2
            );


            spriteBatch.DrawString(font, text, textPosition, Color.Black);
        }
    }

    public void Click()
    {
        if (!isSetToStayPressed)
        {
            isPressed = !isPressed;
            OnClick.Invoke();
        }
        else
        {
            isPressed = !isPressed;
            if (isPressed) OnClick.Invoke();
        }
    }

    public void SetToStayPressed()
    {
        isSetToStayPressed = true;
    }

    public bool GetState()
    {
        return isPressed;
    }

    public void SetColor(Color color, Color colorOnHover, Color colorOutline)
    {
        if (color != null) this.color = color;
        if (colorOnHover != null) this.colorOnHover = colorOnHover;
        if(colorOutline != null) this.outlineColor = colorOutline;
    }
}

public class TriangleButton : UIElement
{
    Color color = Color.White;
    Color colorOnHover = new Color(234, 234, 234);
    Color colorOutline = new Color(96, 96, 96);

    private bool flip = false;

    bool isHovered = false;

    int spacing = 12;

    public event Action OnClick;

    public TriangleButton(Point rectangle) : base(rectangle) { }

    public override void Draw(SpriteBatch spriteBatch)
    {
        PrimitiveDrawer.DrawRectangleWithOutline(spriteBatch, GetBounds(), colorOutline, colorOutline, 5);
        Rectangle _B = GetBounds();
        Vector2 a;
        Vector2 b;
        Vector2 c;
        if (!flip)
        {

            a = new Vector2(_B.Left + spacing, _B.Top + spacing);
            b = new Vector2(_B.Left + spacing, _B.Bottom - spacing);
            c = new Vector2(_B.Right - spacing, _B.Top + (_B.Height / 2));
        }
        else
        {
            a = new Vector2(_B.Left + spacing, _B.Top + (_B.Height / 2));
            b = new Vector2(_B.Right - spacing, _B.Bottom - spacing);
            c = new Vector2(_B.Right - spacing, _B.Top + spacing);
        }
        if (isHovered)
        {
            PrimitiveDrawer.DrawTriangle(spriteBatch, c, a, b, color);
        }
        else
        {
            PrimitiveDrawer.DrawTriangle(spriteBatch, c, a, b, colorOnHover);
        }
    }

    public override void Update()
    {
        MouseState mouseState = Mouse.GetState();
        Point mousePos = new Point(mouseState.X, mouseState.Y);
        isHovered = GetBounds().Contains(mousePos);

        // Check for click
        if (isHovered && InputHandler.IsMouseLeftReleased())
        {
            OnClick?.Invoke();
        }
    }

    public void ToggleFlip()
    {
        flip = true;
    }
}
