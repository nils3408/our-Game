//nils 


/*-------------------------------------------------------------------------------------------------------------
 * PowerUp that are integrated in the Map
 *     
 * 
 * ------------------------------------------------------------------------------------------------------------
 */

using System;
using System.ComponentModel.Design;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using SharpDX.XAudio2;
//using System;
//using System.Security.Cryptography.X509Certificates;
//using static System.Windows.Forms.Design.AxImporter;



public class PowerUp
{
    public Ball ball;
    protected float cooldown; //time how long the PowerUp should be active
    
    public PowerUp(Ball ball1)
    {
        ball = ball1;
    }

    public string get_class_name()
    {
        return this.GetType().Name;
    }

    public virtual void activate(Player player)
    {
       //player is the one the PowerUp will be used on
    }

   public float get_cooldown()
   {
        return cooldown;
   }
}