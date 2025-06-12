

using System;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public enum ButtonAnchor
{
    Left = 0,
    Right = 1,
    Top = 2,
    Bottom = 3,
    Middle = 4
}

public class SimpleButton
{

    Vector2 position = new Vector2(0, 0);
    Microsoft.Xna.Framework.Rectangle rectangle;
    
    String text = "";

    public event Action OnClick;

    public SimpleButton(Vector2 pos, Microsoft.Xna.Framework.Rectangle rectangle, String text)
    {
        this.position = pos;
        this.rectangle = rectangle;

    }

    public void Draw(SpriteBatch spriteBatch)
    { 
    }
}