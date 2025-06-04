using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class PlayerControls
{
    public enum Action
    {
        Left = 0,
        Right = 1,
        Up = 2,
        Down = 3,
        Shot = 4,
        Special = 5
    }
    //maps the Controlls to the Keys, standartwerte, müssen angepasst werden
    private Keys[] controlKeys = { Keys.A, Keys.D, Keys.W, Keys.S, Keys.Space, Keys.E };

    public PlayerControls(Keys[] keys)
    {
        this.controlKeys = keys;
    }

    public void editKey(Action action, Keys key)
    { 
        controlKeys[(int)action] = key;
    }


    public static PlayerControls getStandartLeft()
    {
        Keys[] keys = { Keys.A, Keys.D, Keys.W, Keys.S, Keys.Space, Keys.E };
        return new PlayerControls(keys);
    }
    
    public static PlayerControls getStandartRight()
    { 
        Keys[] keys = { Keys.Left, Keys.Right, Keys.Up, Keys.Down, Keys.RightShift, Keys.RightControl };
        return new PlayerControls(keys);
    }

}