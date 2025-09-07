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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
//using SharpDX.XAudio2;
//using System;
//using System.Security.Cryptography.X509Certificates;
//using static System.Windows.Forms.Design.AxImporter;



public class PowerUp
{
    public Ball ball;
    protected ContentManager content;
    protected Texture2D powerUp_texture;
    protected float cooldown; //time how long the PowerUp should be active
    
    public PowerUp(Ball ball1, ContentManager content1)
    {
        ball = ball1;
        content = content1;
    }

    public string get_class_name()
    {
        return this.GetType().Name;
    }

    public virtual void activate(Player player, int powerUp)
    {
       //player is the one the PowerUp will be used on
       //function is overwritten in the erbenden functions
    }

   public float get_cooldown()
   {
        return cooldown;
   }

    public Texture2D get_powerUp_texture()
    {
        return powerUp_texture;
    }

    public virtual int getNewSize()
    {
        // only relevant for BigPlayerPowerUp and SmallPlayerPowerUp
        return 0;
    }

}