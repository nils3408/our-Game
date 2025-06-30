using our_Game;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;

//Einfaches Interface ds alle UIElemente erf�llen

public abstract class UIElement
{
    public UIElement parent = null;

    protected Point relPos = Point.Zero;
    public Point Size = Point.Zero;

    public UIElement(Point Size)
    {
        this.Size = Size;
    }

    public UIElement(Point relativePosition, Point Size)
    {
        this.relPos = relativePosition;
        this.Size = Size;
    }

    public UIElement(Rectangle bounds)
    {
        this.relPos = bounds.Location;
        this.Size = bounds.Size;
    }

    public abstract void Update();

    public abstract void Draw(SpriteBatch spriteBatch);

    public virtual Rectangle GetBounds()
    {
        Point position = this.GetPosition();
        return new Rectangle(position, Size);
    }

    public Point GetOffset()
    {
        return this.relPos;
    }

    public void Offset(Point translation)
    {
        this.relPos += translation;
    }

    public Point GetPosition()
    {
        if (this.parent == null)
        {
            return relPos;
        }
        else
        {
            return relPos + parent.GetPosition();
        }
    }

    public void MoveCenter(Point newCenterPos)
    {
        relPos = newCenterPos- new Point(GetBounds().Width/2, GetBounds().Height/2);
    }

}

// abstrakte Grundlage f�r alle Container in welchen UIElemente aufgereiht werden

public abstract class ElementContainer : UIElement
{

    protected List<UIElement> elements = new List<UIElement>();

    public ElementContainer(Rectangle bounds) : base(bounds) { }
    public ElementContainer(Point RelativePosition) : base(RelativePosition) { }
    public ElementContainer(Point RelativePosition, Point Size) : base(RelativePosition, Size) { }

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

    /**
    gibt die Bounds, also den Umriss zurück als Recteck,
    mit also der richtigen absoluten Position aufm Bildschirm und der Größe
    berechnet die größe und da buttons auch nach links vom origin gesetzt werden könnten relPos=new Point(-10,0) also auch die relPos neu
    */

    public override Rectangle GetBounds()
    {
        
        int maxX = GetPosition().X;
        int maxY = GetPosition().Y;
        int minX = GetPosition().X;
        int minY = GetPosition().Y;
        for (int i = 0; i < elements.Count; i++)
        {
            int left = elements[i].GetBounds().Left;
            if (left < minX) minX = left;
            int bottom = elements[i].GetBounds().Bottom;
            if (bottom > maxY) maxY = bottom;
            int right = elements[i].GetBounds().Right;
            if (right > maxX) maxX = right;
            int top = elements[i].GetBounds().Top;
            if (top < minY) minY = top;

        }
        return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        //return new Rectangle(GetPosition(), Size);
    }


}

public class StackContainer : ElementContainer
{

    int spacing = 0;

    private Point Offset = Point.Zero;

    bool IsSetToDrawOutline = false;
    Color outlineColor = Color.White;
    int thickness = 1;

    public StackContainer(Point position, int spacing) : base(position, Point.Zero)
    {
        this.spacing = spacing;
    }

    public override void Add(UIElement element)
    {
        element.Offset(Offset);
        Offset += new Point(0, spacing + element.Size.Y);
        base.Add(element);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if(IsSetToDrawOutline)
        PrimitiveDrawer.DrawRectangleOutline(spriteBatch, GetBounds(), outlineColor, thickness);

        foreach (UIElement e in elements)
        {
            e.Draw(spriteBatch);
        }

    }

    public override Rectangle GetBounds()
    {
        Rectangle nB = base.GetBounds();
        Rectangle boundsWSpacing = new Rectangle(nB.X - spacing, nB.Y - spacing, nB.Width + 2 * spacing, nB.Height + 2 * spacing);
        Debug.WriteLine($"Elements Count: {boundsWSpacing}");
        return boundsWSpacing;
    }

    public void SetDrawOutline(Color color, int thickness)
    {
        IsSetToDrawOutline = true;
        outlineColor = color;
        this.thickness = thickness;
    }
    
}

















/*bleibt erstmal nur ne Idee
// Container zum Anordnen in relativen Verh�ltnissen von 0 bis 1, abgekoppelt von den absoluten Pixel Gr��en.
public class RelativeContainer : ElementContainer {
    public ElementContainer(Rectangle bounds) : base(bounds) {
        
    }
}*/