

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

public class SimpleButton : UIElement
{



    int outline = 5;

    Color color = Color.White;
    Color colorOnHover = new Color(234, 234, 234);
    Color colorOutline = new Color(96, 96, 96);

    public String text = "";
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
        this.color = color;
        this.colorOnHover = colorOnHover;
        this.outlineColor = colorOutline;
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

    //lässt das Dreieck zukünftig andersherum zeichnen
    public void ToggleFlip()
    {
        flip = true;
    }
}

public class KeySelector : UIElement
{
    public Keys key;
    public KeySelector(Point Size, Keys key) : base(Size)
    {
        this.key = key;
    }

    public Color backgroundColor = Color.White;
    public Color isSelectedColor = Color.LightGoldenrodYellow;

    public bool isSelected = false;
    private bool isHovered = false;
    public event Action OnKeySelected;


    public override void Draw(SpriteBatch spriteBatch)
    {

        if (isSelected)
        {
            PrimitiveDrawer.DrawRectangle(spriteBatch, GetBounds(), isSelectedColor);
            PrimitiveDrawer.DrawText(spriteBatch, GetBounds(), "_", Color.Red);
        }
        else
        {
            PrimitiveDrawer.DrawRectangle(spriteBatch, GetBounds(), backgroundColor);
            PrimitiveDrawer.DrawText(spriteBatch, GetBounds(), getStringfromKey(key), Color.Black);
        }

        Outline(spriteBatch);
    }

    public override void Update()
    {
        MouseState mouseState = Mouse.GetState();
        Point mousePos = new Point(mouseState.X, mouseState.Y);
        isHovered = GetBounds().Contains(mousePos);

        // Check for click
        if (isHovered && InputHandler.IsMouseLeftReleased())
        {
            isSelected = true;
        }
        if (isSelected && Keyboard.GetState().GetPressedKeyCount() > 0)
        {
            key = Keyboard.GetState().GetPressedKeys()[0];
            OnKeySelected.Invoke();
            isSelected = false;
        }
    }

    public String getStringfromKey(Keys key)
    {
        switch (key)
        {
            case Keys.OemPeriod: return ".";
            case Keys.OemComma: return ",";
            case Keys.OemMinus: return "-";
            case Keys.OemBackslash: return ">";
            case Keys.Add: return "+";
            case Keys.LeftControl: return "STRG";
            case Keys.RightControl: return "STRG";
            case Keys.CapsLock: return "Caps";
            case Keys.LeftShift: return "LShift";
            case Keys.RightShift: return "RShift";
            case Keys.NumLock: return "NUM";

            case Keys.D0: return "0";
            case Keys.D1: return "1";
            case Keys.D2: return "2";
            case Keys.D3: return "3";
            case Keys.D4: return "4";
            case Keys.D5: return "5";
            case Keys.D6: return "6";
            case Keys.D7: return "7";
            case Keys.D8: return "8";
            case Keys.D9: return "9";

            case Keys.NumPad0: return "Num0";
            case Keys.NumPad1: return "Num1";
            case Keys.NumPad2: return "Num2";
            case Keys.NumPad3: return "Num3";
            case Keys.NumPad4: return "Num4";
            case Keys.NumPad5: return "Num5";
            case Keys.NumPad6: return "Num6";
            case Keys.NumPad7: return "Num7";
            case Keys.NumPad8: return "Num8";
            case Keys.NumPad9: return "Num9";

            default:
                return key.ToString();
        }
    }
}


public class SwitchButton : UIElement
{
    String[] options;
    int curIndex;

    bool isHovered = false;

    public bool drawOutline = false;
    public Color TextColor = Color.Black;
    public Color BackgroundColor = Color.White;
    public Color HoverColor = new Color(234, 234, 234);

    public event Action onClick;

    public SwitchButton(Point Size, String[] options) : base(Size)
    {
        this.options = options;
        onClick += () => { };
    }

    public override void Draw(SpriteBatch spriteBatch)
    {

        if (isHovered)
        {
            PrimitiveDrawer.DrawRectangle(spriteBatch, GetBounds(), HoverColor);
        }
        else
        {
            PrimitiveDrawer.DrawRectangle(spriteBatch, GetBounds(), BackgroundColor);

        }

        PrimitiveDrawer.DrawText(spriteBatch, GetBounds(), options[curIndex], TextColor);
        base.Outline(spriteBatch);
    }

    public override void Update()
    {
        MouseState mouseState = Mouse.GetState();
        Point mousePos = new Point(mouseState.X, mouseState.Y);
        isHovered = GetBounds().Contains(mousePos);

        // Check for click
        if (isHovered && InputHandler.IsMouseLeftReleased())
        {
            curIndex = ++curIndex % options.Length;
            onClick.Invoke();
        }
    }

    public String getCurOption()
    {
        return options[curIndex];
    }
    public int getCurIndex()
    {
        return curIndex;
    }
}