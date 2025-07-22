using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;
using System.IO;
using System;

namespace our_Game;

public class Game1 : Game
{
    public static GraphicsDeviceManager graphics;
    private SpriteBatch _spriteBatch;

    private static IGameState curState;
    public static IGameState nextState;

    public static List<GameLogic> openGames;
    //public static Menu menu;
    public static Settings settings;
    public static GameSetup setup;
    public static bool GameIsInitialized = false;

    public static PlayerControls leftPlayerControls;
    public static PlayerControls rightPlayerControls;



    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        //_graphics.ToggleFullScreen();


        graphics.PreferredBackBufferWidth = 1920;
        graphics.PreferredBackBufferHeight = 1080;

        openGames = new List<GameLogic>();
        leftPlayerControls = PlayerControls.getStandartLeft();
        rightPlayerControls = PlayerControls.getStandartRight();
    }

    protected override void Initialize()
    {
        PrimitiveDrawer.Initialize(GraphicsDevice, Content);
        PlayerFactory.Initialize(GraphicsDevice, Content);

        LoadSavedData();
        this.Exiting += OnExiting;

        
        setup = new GameSetup(this);
settings = new Settings(this);
        //menu = new Menu(this);


        // Initializing each GameState

        settings.Initialize();
        setup.Initialize();

        curState = new Menu(this);
        nextState = curState;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // load Content for each GameState

        settings.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        InputHandler.Update();

        if (nextState == null)
        {
            SaveData();
            Exit();
        }
        if (nextState != null && nextState != curState) curState = nextState;

        // update current Game State

        curState.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        curState.Draw(gameTime);
        base.Draw(gameTime);
    }

    // kann ich nicht nutzen in den GameStates, da Game1 als einfaches Game übergeben wird
    public void OpenNewGame(Player leftPlayer, Player rightPlayer)
    {
        GameLogic newGame = new GameLogic(this, leftPlayer, rightPlayer);
        newGame.Initialize();
        newGame.LoadContent();
        openGames.Add(newGame);
        nextState = newGame;
    }

    public void GoToMenu()
    { 
        Game1.nextState = new Menu(this);
    }


    private static void LoadSavedData()
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameData));
            using (FileStream fs = new FileStream("GameData.xml", FileMode.Open))
            {
                GameData gameData = (GameData)serializer.Deserialize(fs);

                // Hier musst du die geladenen Daten verwenden:
                leftPlayerControls = gameData.leftPlayerControls;
                rightPlayerControls = gameData.rightPlayerControls;
            }
        }
        catch (FileNotFoundException)
        {
            // Datei existiert noch nicht - verwende Standardwerte
            Console.WriteLine("Keine gespeicherten Daten gefunden.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler beim Laden: {ex.Message}");
        }
    }

    public static void SaveData()
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameData));
            GameData gameData = new GameData(leftPlayerControls, rightPlayerControls);
            using (FileStream fs = new FileStream("GameData.xml", FileMode.Create))
            {
                serializer.Serialize(fs, gameData);
            }
            Console.WriteLine("Daten erfolgreich gespeichert!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler beim Speichern: {ex.Message}");
        }
    }

    private void OnExiting(object sender, EventArgs e)
    {
        SaveData();
    }
}

public class GameData
{
    public PlayerControls leftPlayerControls;
    public PlayerControls rightPlayerControls;
    
    // Parameterloser Konstruktor für XML-Serialisierung erforderlich!
    public GameData() { }
    
    public GameData(PlayerControls leftPlayerControls, PlayerControls rightPlayerControls)
    {
        this.leftPlayerControls = leftPlayerControls;
        this.rightPlayerControls = rightPlayerControls;
    }
}
