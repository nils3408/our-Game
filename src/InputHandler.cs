using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Linq;

/**
	Implementation eines simplen InputHandlers fürs Keyboard,
	der nur die Funktionalität ergänzt von IsReleased(Key key).
	Das ist nativ nicht möglich mit Microsoft.Xna.Framework.Input.
 
	InputHandler muss immer geupdated werden sofern man aktuelle Eingaben prüfen möchte.
	Ich denke Game1.cs ist die richtige Stelle dafür.
 
	Klasse ist evtl. erweiterbar um Zeiten, falls man schauen möchte wie lange der Button gedrückt wurde z.B. beim Schuss.
	geschrieben Thies
 */

public static class InputHandler
{

	private static Keys[] prevKeys = Keyboard.GetState().GetPressedKeys();
	private static Keys[] curKeys = Keyboard.GetState().GetPressedKeys();

	private static MouseState prevMouse = Mouse.GetState();
	private static MouseState curMouse = Mouse.GetState();

	private static GamePadState prevPS4 = GamePad.GetState(PlayerIndex.One);
	private static GamePadState curPS4 = GamePad.GetState(PlayerIndex.One);

	private static GamePadState prevPS4_P2 = GamePad.GetState(PlayerIndex.Two);
	private static GamePadState curPS4_P2 = GamePad.GetState(PlayerIndex.Two);


	public static void Update()
	{
		prevKeys = curKeys;
		curKeys = Keyboard.GetState().GetPressedKeys();

		prevMouse = curMouse;
		curMouse = Mouse.GetState();

        prevPS4 = curPS4;
        curPS4 = GamePad.GetState(PlayerIndex.One);

        prevPS4_P2 = curPS4_P2;
        curPS4_P2 = GamePad.GetState(PlayerIndex.Two);


    }

	public static bool IsDown(Keys key)
	{
		return curKeys.Contains(key);
	}

	public static bool IsUp(Keys key)
	{
		return !curKeys.Contains(key);
	}

	public static bool IsReleased(Keys key)
	{
		return !curKeys.Contains(key) && prevKeys.Contains(key);
	}

	public static bool IsPressed(Keys key)
	{
		return curKeys.Contains(key) && !prevKeys.Contains(key);
	}

	public static bool IsMouseLeftReleased() {
		return prevMouse.LeftButton == ButtonState.Pressed && curMouse.LeftButton != ButtonState.Pressed;
	}



	//---------------------------------------------------------------------------------
	// ps4 controller 

	public static bool IsGamePadButtonDown(Buttons button, int playergroup)
	{
		//playergroup is one or 2

		PlayerIndex player = getPlayerIndex(playergroup);

		GamePadState state = GamePad.GetState(player);
		return (state.IsConnected && state.IsButtonDown(button));

	}

	public static bool isStickDown(string side, int playergroup)
	{
		//side is "l" or "r"
		PlayerIndex player = getPlayerIndex(playergroup);

		GamePadState state = GamePad.GetState(player);
		if (side == "l")
		{
			float z = -0.5f;
			return state.ThumbSticks.Left.X < z;
		}
		else
		{
			float z = 0.5f;
			return state.ThumbSticks.Left.X > z;
		}
	}

	public static bool isLeftTriggerDown(int playergroup){
		//L2 Tasten ist ein sogenannten Trigger
		
		PlayerIndex player = getPlayerIndex(playergroup);
        GamePadState state = GamePad.GetState(player);
		return state.Triggers.Left > 0.5;
    }

    public static bool isRightTriggerDown(int playergroup)
    {
        //R2 Tasten ist ein sogenannten Trigger

        PlayerIndex player = getPlayerIndex(playergroup);
        GamePadState state = GamePad.GetState(player);
        return state.Triggers.Right > 0.5;
    }



    public static bool IsGamePadButtonReleased(Buttons button)
	{
		return prevPS4.IsButtonDown(button) && curPS4.IsButtonUp(button);
	}

    public static bool IsGamePadButtonPressed(Buttons button)
    {
        return prevPS4.IsButtonUp(button) && curPS4.IsButtonDown(button);
    }


	public static PlayerIndex getPlayerIndex(int playergroup)
	{
 
        switch (playergroup)
        {
            case 1:
                return PlayerIndex.One;
            case 2:
                return PlayerIndex.Two;
            default:
                return PlayerIndex.One;   //should never end here, but implementetd to prevent error 
        }
    }
}