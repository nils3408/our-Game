using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public enum PlayerAction
    {
        Left = 0,
        Right = 1,
        Up = 2,
        Down = 3,
        Shot = 4,
        Special = 5
    }
public class PlayerControls
{


    //maps the Controlls to the Keys, standartwerte, müssen angepasst werden
    private Keys[] controlKeys = { Keys.A, Keys.D, Keys.W, Keys.S, Keys.Space, Keys.E };
    private Dictionary<Keys, PlayerAction> keyToAction = new Dictionary<Keys, PlayerAction>();

    public PlayerControls(Keys[] keys)
    {
        this.controlKeys = keys;
        for (int i = 0; i < keys.Length; i++)
        {
            this.keyToAction[keys[i]] = (PlayerAction)i;
        }
    }

    public void editKey(PlayerAction action, Keys key)
    {
        controlKeys[(int)action] = key;
        if (keyToAction.ContainsKey(key))
        {
            keyToAction[key] = action;
        }
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

    public PlayerAction GetAction(Keys key)
    {
        return keyToAction[key];
    }



}