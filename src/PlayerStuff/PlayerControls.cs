using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public enum PlayerAction
    {
        Left = 0,
        Right = 1,
        Jump = 2,
        Special = 3,
        PowerUp_1 = 4,
        PowerUp_2 = 5
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
        Keys[] keys = { Keys.A, Keys.D, Keys.W, Keys.E, Keys.R, Keys.F };
        return new PlayerControls(keys);
    }

    //standart besetzung wie zuvor in GameLogic.handel_player_movement()
    public static PlayerControls getStandartRight()
    {
        Keys[] keys = { Keys.J, Keys.L, Keys.I, Keys.O, Keys.P, Keys.OemPeriod };
        return new PlayerControls(keys);
    }

    public PlayerAction GetAction(Keys key)
    {
        return keyToAction[key];
    }

    public Keys getKey(PlayerAction action)
    {
        return controlKeys[(int)action];
    } 

    
}

public class ContraolsEditor : UIElement
{
    public PlayerControls controls;

    ElementContainer container = new ElementContainer();

    public ContraolsEditor(PlayerControls controls) : base()
    {
        this.controls = controls;

        
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        container.Draw(spriteBatch);
    }

    public override void Update()
    {
        container.Update();
    }
}

public class KeyEditor : UIElement
{
    Keys key;
    String description;

    HorizontalContainer HContainer;


    public KeyEditor(Keys key, String description) : base()
    {
        this.key = key;
        this.description = description;

        HContainer = new HorizontalContainer();

        Point TextfielSize = new Point(250, 100);
        Textfield text = new Textfield(description, TextfielSize);
        HContainer.Add(text);

        Textfield selKey = new Textfield(key.ToString(), TextfielSize);
        HContainer.Add(text);

    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        HContainer.Draw(spriteBatch);
    }

    public override void Update()
    {
        HContainer.Update();
    }
}