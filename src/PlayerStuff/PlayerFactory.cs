using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using System;
using our_Game;


public static class PlayerFactory
{

    /**
    Zum erzeugen von PlayerTypes mit CreatePlayer(), muss stetig angepasst werden! 
    Erspart mir aber doppelt gemoppelt das selbe zu tun weil ich die Texturen im SetupGame brauche, Texturen k�nnen mit GetPlayerTexture nachgefragt werden.
    Man k�nnte sagen das es dem Factory Pattern entspricht, aber sicher bin ich mir da nicht.
    //beim hinzuf�gen von PlayerTypen den enum sowie TypesCount anpassen, wenn euch n besserer Name einf�llt f�r TypesCount bitte komentieren lol
    //Das und dann nicht vergessen, die Texture zu laden in der Initialize Methode
    */


    public const int TypesCount = 7;
    public enum Types
    {
        Standart = 0,
        Spiderman = 1,
        Sonic = 2,
        Knight = 3,
        Ninja = 4,
        Mario = 5,
        Wizzard = 6
    }

    private static GraphicsDevice _graphicsDevice;

    private static Texture2D[] playerTextures        = new Texture2D[TypesCount];
    private static Texture2D[] smt = new Texture2D[TypesCount];  //special_move_textures


    public static void Initialize(GraphicsDevice graphicsDevice, ContentManager Content)
    {
        _graphicsDevice = graphicsDevice;

        playerTextures[(int)Types.Spiderman] = Content.Load<Texture2D>("players/Spiderman");
        playerTextures[(int)Types.Knight]    = Content.Load<Texture2D>("players/Knightord");
        playerTextures[(int)Types.Sonic]     = Content.Load<Texture2D>("players/sonic");
        playerTextures[(int)Types.Standart]  = Content.Load<Texture2D>("players/KopfkickerChar1_neu");
        playerTextures[(int)Types.Ninja]     = Content.Load<Texture2D>("players/KopfkickerChar2_neu");
        playerTextures[(int)Types.Mario]     = Content.Load<Texture2D>("players/Mario2");
        playerTextures[(int)Types.Wizzard]   = Content.Load<Texture2D>("players/Wizzard");


        smt[(int)Types.Spiderman]  = Content.Load<Texture2D>("special_move_textures/special_move_texture_spiderman");
        smt[(int)Types.Knight]     = Content.Load<Texture2D>("special_move_textures/special_move_texture_knight");
        smt[(int)Types.Sonic]      = Content.Load<Texture2D>("special_move_textures/special_move_texture_sonic");
        smt[(int)Types.Standart]   = Content.Load<Texture2D>("special_move_textures/special_move_texture_player");
        smt[(int)Types.Ninja]      = Content.Load<Texture2D>("special_move_textures/special_move_texture_ninja");
        smt[(int)Types.Mario]      = Content.Load<Texture2D>("special_move_textures/special_move_texture_mario");
        smt[(int)Types.Wizzard]    = Content.Load<Texture2D>("special_move_textures/special_move_texture_wizzard");
    }

    public static Player CreatePlayer(Types playerType, Vector2 position, int id, PlayerControls controls)
    {
        switch (playerType)
        {
            case Types.Standart:
                return new Player(_graphicsDevice,    position, GetPlayerTexture(playerType), getSMT(playerType), id, controls);
            case Types.Sonic:
                return new Sonic(_graphicsDevice,     position, GetPlayerTexture(playerType), getSMT(playerType), id, controls);
            case Types.Spiderman:
                return new Spiderman(_graphicsDevice, position, GetPlayerTexture(playerType), getSMT(playerType), id, controls);
            case Types.Knight:
                return new Knight(_graphicsDevice,    position, GetPlayerTexture(playerType), getSMT(playerType), id, controls);
            case Types.Ninja:
                return new Ninja(_graphicsDevice,     position, GetPlayerTexture(playerType), getSMT(playerType), id, controls);
            case Types.Mario:
                return new Mario(_graphicsDevice,     position, GetPlayerTexture(playerType), getSMT(playerType), id, controls);
            case Types.Wizzard:
                return new Wizzard(_graphicsDevice,   position, GetPlayerTexture(playerType), getSMT(playerType), id, controls);
            default:
                return new Player(_graphicsDevice,    position, GetPlayerTexture(playerType), getSMT(playerType), id, controls);
        }
    }

    //erstellt Player, Position ist da erstmal egal, weil das in GameLogic abh�ngig ist von GroundY, korrigiere ich die Positionen dann auch dort
    public static Player CreatePlayer(Types playerType, bool left)
    {
        if (left)
        {
            return CreatePlayer(playerType, Vector2.Zero, 1, Game1.leftPlayerControls);
        }
        else
        {
            return CreatePlayer(playerType, Vector2.Zero, 2, Game1.rightPlayerControls);
        }
    }

    public static Texture2D GetPlayerTexture(Types playerType)
    {
        return playerTextures[(int)playerType];
    }

    public static Texture2D getSMT(Types playerType)
    {
        return smt[(int)playerType];
    }
}