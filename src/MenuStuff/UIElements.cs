using our_Game;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Security.AccessControl;

//Einfaches Interface ds alle UIElemente erf�llen

public abstract class UIElement
{
    public UIElement parent
    {
        get;
        set;
    }

    public Rectangle bounds
    {
        get;
        set;
    }

    public UIElement(Rectangle bounds)
    {
        this.bounds = bounds;
    }

    public UIElement(UIElement parent, Rectangle bounds)
    {
        this.bounds = bounds;
        this.parent = parent;
    }

    public abstract void Update();

    public abstract void Draw(SpriteBatch spriteBatch);

    public Rectangle GetBounds()
    {
        return bounds;
    }

    public void MoveBounds(Vector2 translation)
    {
        this.bounds.Offset(translation);
    }

    public Vector2 GetParentsOffset()
    {
        if (this.parent == null)
        {
            return new Vector2(this.bounds.X, this.bounds.Y);
        }
        else
        {
            return new Vector2(this.bounds.X, this.bounds.Y) + parent.GetParentsOffset();
        }
    }

}

// abstrakte Grundlage f�r alle Container in welchen UIElemente aufgereiht werden

public abstract class ElementContainer : UIElement
{

    protected List<UIElement> elements = new List<UIElement>();

    public ElementContainer(Rectangle bounds) : base(bounds) { }
    public ElementContainer(UIElement parent, Rectangle bounds) : base(bounds) { }

    public List<UIElement> GetChildren()
    {
        return elements;
    }



    public virtual void Add(UIElement element)
    {
        element.parent = this;
        elements.Add(element);
    }

    public override void Update()
    {
        foreach (UIElement e in elements)
        {
            e.Update();
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        foreach (UIElement e in elements)
        {
            e.Draw(spriteBatch);
        }
    }


}

public class StackContainer : ElementContainer
{

    int spacing = 0;

    private Vector2 Offset = Vector2.Zero;

    public StackContainer(UIElement parent, Rectangle bounds, int spacing) : base(parent, bounds)
    {
        this.spacing = spacing;
    }

    public override void Add(UIElement element)
    {
        element.MoveBounds(Offset);
        Offset += new Vector2(0, spacing + element.GetBounds().Height);
        base.Add(element);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        int y = bounds.Top;
        int x = bounds.Left;
        foreach (UIElement e in elements)
        {
            e.Draw(spriteBatch);
            y += e.bounds.Height + spacing;
        }
    }
}

















/*bleibt erstmal nur ne Idee
// Container zum Anordnen in relativen Verh�ltnissen von 0 bis 1, abgekoppelt von den absoluten Pixel Gr��en.
public class RelativeContainer : ElementContainer {
    public ElementContainer(Rectangle bounds) : base(bounds) {
        
    }
}*/