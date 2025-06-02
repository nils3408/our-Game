

using System.ComponentModel;
using System.Numerics;
using System.Reflection;

public class Ball
{
    Vector2 velocity;
    Vector2 position;
    public Ball(Vector2 position)
    {
        this.position = position;
    }

    public void reflect(Vector2 factor)
    { 
        //ist noch nicht ganz richtig, eher symbolisch
        velocity *= factor;
    }
    
}
