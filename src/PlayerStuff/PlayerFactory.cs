using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using System;


public static class PlayerFactory
{

    /**
    Zum erzeugen von PlayerTypes mit CreatePlayer(), muss stetig angepasst werden! 
    Erspart mir aber doppelt gemoppelt das selbe zu tun weil ich die Texturen im SetupGame brauche, Texturen können mit GetPlayerTexture nachgefragt werden.
    Man könnte sagen das es dem Factory Pattern entspricht, aber sicher bin ich mir da nicht.
    //beim hinzufügen von PlayerTypen den enum sowie TypesCount anpassen, wenn euch n besserer Name einfällt für TypesCount bitte komentieren lol
    //Das und dann nicht vergessen, die Texture zu laden in der Initialize Methode
    */


    public const int TypesCount = 6;
    public enum Types
    {
        Standart = 0,
        Spiderman = 1,
        Sonic = 2,
        Knight = 3,
        Ninja = 4,
        Mario = 5,
    }

    private static GraphicsDevice _graphicsDevice;

    private static Texture2D[] playerTextures = new Texture2D[TypesCount];

    public static void Initialize(GraphicsDevice graphicsDevice, ContentManager Content)
    {
        _graphicsDevice = graphicsDevice;

        playerTextures[(int)Types.Spiderman] = Content.Load<Texture2D>("Spiderman");
        playerTextures[(int)Types.Knight] = Content.Load<Texture2D>("Knightord");
        playerTextures[(int)Types.Sonic] = Content.Load<Texture2D>("sonic");
        playerTextures[(int)Types.Standart] = Content.Load<Texture2D>("KopfkickerChar1_neu");
        playerTextures[(int)Types.Ninja] = Content.Load<Texture2D>("KopfkickerChar2_neu");
        playerTextures[(int)Types.Mario] = Content.Load<Texture2D>("Mario");
    }

    public static Player CreatePlayer(Types playerType, Vector2 position, int id)
    {
        switch (playerType)
        {
            case Types.Standart:
                return new Player(_graphicsDevice, position, GetPlayerTexture(playerType), id);
                break;
            case Types.Sonic:
                return new Sonic(_graphicsDevice, position, GetPlayerTexture(playerType), id);
                break;
            case Types.Spiderman:
                return new Spiderman(_graphicsDevice, position, GetPlayerTexture(playerType), id);
                break;
            case Types.Knight:
                return new Knight(_graphicsDevice, position, GetPlayerTexture(playerType), id);
                break;
            case Types.Ninja:
                return new Player(_graphicsDevice, position, GetPlayerTexture(playerType), id);
                break;
            case Types.Mario:
                return new Mario(_graphicsDevice, position, GetPlayerTexture(playerType), id);
                break;
            default:
                return new Player(_graphicsDevice, position, GetPlayerTexture(playerType), id);
        }
    }

    //erstellt Player, Position ist da erstmal egal, weil das in GameLogic abhängig ist von GroundY, korrigiere ich die Positionen dann auch dort
    public static Player CreatePlayer(Types playerType, bool left)
    {
        if (left)
        {
            return CreatePlayer(playerType, Vector2.Zero, 1);
        }
        else
        {
            return CreatePlayer(playerType, Vector2.Zero, 2);
        }
    }

    public static Texture2D GetPlayerTexture(Types playerType)
    {
        return playerTextures[(int)playerType];
    }
}