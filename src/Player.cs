


using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Player
{
    public string name;
    //int id;

    Texture2D texture2D;

    Vector2 position;
    Vector2 velocity = new Vector2(0, 0);

    const float gravity = 10;
    const float jump_impuls = 50;
    Vector2 footForce = new Vector2(5, 10);

    PlayerControls controls = null;
    

    public Player(string name, PlayerControls controls)
    {
        this.name = name;
        this.controls = controls;
    }

    public void setControlls(Keys[] keys) { }

    public void ActOnInput() { }
    

}