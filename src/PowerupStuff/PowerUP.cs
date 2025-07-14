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
    Ball ball;
    
    public PowerUp(Ball ball1)
    {
        ball = ball1;
    }
    

    public virtual void activate()
    {
        //hello there 
    }
}