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
    String text;

    public PowerUp(String abc)
    {
        text = abc;
    }

    public String getText()
    {
        return (String)text;
    }
}