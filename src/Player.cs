


using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class Player
{
    public string name;
    //int id;

    Vector2 position;
    Vector2 velocity = new Vector2(0, 0);

    const float gravity = 10;
    const float jump_impuls = 50;
    Vector2 footForce = new Vector2(5, 10);


    private enum Actions
    {
        Left = 0,
        Right = 1,
        Up = 2,
        Down = 3,
        Shot = 4,
        Special = 5
    }
    //maps the Controlls to the Keys, standartwerte, müssen angepasst werden
    private Keys[] controllKeys = { Keys.A, Keys.D, Keys.W, Keys.S, Keys.Space, Keys.E };

    public Player(string name)
    {
        this.name = name;
    }

    public void setControlls(Keys[] keys) { }

    public void ActOnInput() { }
    

}