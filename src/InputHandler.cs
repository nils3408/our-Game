using Microsoft.Xna.Framework.Input;
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

public static class InputHandler {

	private static Keys[] prevKeys = Keyboard.GetState().GetPressedKeys();
	private static Keys[] curKeys = Keyboard.GetState().GetPressedKeys();

	public static void Update() { 
		prevKeys = curKeys;
		curKeys = Keyboard.GetState().GetPressedKeys();
		if(IsDown(Keys.Escape)) System.Diagnostics.Debug.WriteLine("escape!");
    }

	public static bool IsDown(Keys key) {
		return curKeys.Contains(key);
	}

	public static bool IsUp(Keys key) {
		return !curKeys.Contains(key);
	}

	public static bool IsReleased(Keys key) {
		return !curKeys.Contains(key) && prevKeys.Contains(key);
	}

	public static bool IsPressed(Keys key) {
		return curKeys.Contains(key) && !prevKeys.Contains(key);
	}
}