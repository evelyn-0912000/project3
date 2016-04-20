using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Meteen_Rotterdam
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	
	public class Game1 : Game
  {
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
    private Texture2D mapimg;
    private Vector2 grabOffset;
    private MouseState mouseState;
    private MouseState oldMouseState;
    private Vector2 mapPosition;
    private Map map1;
    private List<Map> points = new List<Map>();
		private buttonOverlay overlay1;
		private ApplyButton applyButton;
		private List<IButton> buttons = new List<IButton>();
    private List<Banner> banners = new List<Banner>();
		private legendOverlay legend;
		private List<Map> clouds = new List<Map>();
		private WeatherButton weatherButton;
		private string oldWeatherStatus;
		private string WeatherStatus;
		bool applyWeather = false;
		private AbstractionButton abstractionButton;
		public Game1(int width, int height,bool fullsc)
    {
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			var content = Content;
      graphics.PreferredBackBufferHeight = height;
      graphics.PreferredBackBufferWidth = width;
			graphics.IsFullScreen = fullsc;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
    {
      // TODO: Add your initialization logic here
      this.IsMouseVisible = true;
      base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
    {
      // Create a new SpriteBatch, which can be used to draw textures.
      Console.WriteLine("BUTTONS\nPurple\t...\tToggle Mood\nYellow\t...\tAdd to Min/Max Age\nGreen\t...\tAdd to Min/Max Persons\nLight Blue\tChange Inside/Outside\nRed\t...\tApply changes");
	    spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
      mapimg = Content.Load<Texture2D>("map.gif");
      map1 = new Map(GetCenter(mapimg, graphics), mapimg,"1");
			Color a = new Color(100, 100, 100, 100);
	    overlay1 = new buttonOverlay(true, graphics, new Color(100, 100, 100, 235));
			legend = new legendOverlay(graphics, new Color(100, 100, 100, 235));
			weatherButton = new WeatherButton(overlay1, graphics, Content);
			buttons.Add(new PersonsButton(false, overlay1, graphics, Content));
      buttons.Add(new PersonsButton(true, overlay1, graphics, Content));
      applyButton = new ApplyButton(overlay1, graphics, Content);
	    buttons.Add(new MoodButton(overlay1, graphics, Content));
	    buttons.Add(new OutsideButton(overlay1, graphics, Content));
	    buttons.Add(new AgeButton(false,overlay1, graphics, Content));
      buttons.Add(new AgeButton(true, overlay1, graphics, Content));
			abstractionButton = new AbstractionButton(overlay1, graphics, Content);
      banners.Add(new Banner(1, overlay1, graphics, Content));
      banners.Add(new Banner(2, overlay1, graphics, Content));
      banners.Add(new Banner(3, overlay1, graphics, Content));

			WeatherStatus = weatherButton.printValue();
      // TODO: use this.Content to load your game content here
      List<List<string>> pointsFromDB = new List<List<string>>();
      pointsFromDB = Filter.initialMap("server = 127.0.0.1; uid = root; pwd = SZ3omhSQ; database = rotterdamDB;");
      foreach (List<string> row in pointsFromDB)
      {
        float lat = Convert.ToSingle(row[0]);
        float lon = Convert.ToSingle(row[1]);
				string inside = row[2];
				points.Add(new Map(new Vector2(lat, lon), Content.Load<Texture2D>("pin.png"),inside));
				
      }

      Console.WriteLine(points.Count);
      // points = Abstraction.createAbstractedMap(points, Content);
      Console.WriteLine(points.Count);
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
    {
			// TODO: Unload any non ContentManager content here
		}

    public static Vector2 GetCenter(Texture2D mapimg, GraphicsDeviceManager graphics)
    {
      int a = (graphics.PreferredBackBufferWidth/ 2) - (mapimg.Width / 2);
      int b = (graphics.PreferredBackBufferHeight / 2) - (mapimg.Height / 2);
      return new Vector2(a, b);
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			// TODO: Add your update logic here

			base.Update(gameTime);
      oldMouseState = mouseState;
      mouseState = Mouse.GetState();
			if (mouseState.LeftButton == ButtonState.Pressed) {
				Vector2 mousePosition = new Vector2(this.mouseState.X, this.mouseState.Y);
				if (overlay1.containsMouse(mousePosition) == false && legend.containsMouse(mousePosition) == false) {
					if (grabOffset == Vector2.Zero) {
						grabOffset = new Vector2(map1.printPosition().X - mousePosition.X, map1.printPosition().Y - mousePosition.Y);
					}
					else {
						mapPosition = new Vector2(mousePosition.X + grabOffset.X, mousePosition.Y + grabOffset.Y);
						map1.UpdatePos(mapPosition);
					}
				}
        //System.Console.WriteLine("OFFSET" + grabOffset);
        //System.Console.WriteLine("MOUSE" + mousePosition);
        //System.Console.WriteLine("MAP" + mapPosition);
        // dummy comment

      }
      if (mouseState.LeftButton == ButtonState.Released)
      {
        grabOffset = Vector2.Zero;
      }

      foreach (IButton button in buttons) {
        button.Update(mouseState, oldMouseState);
      }
			oldWeatherStatus = WeatherStatus;
			weatherButton.Update(mouseState, oldMouseState);
			WeatherStatus = weatherButton.printValue();
			if (oldWeatherStatus != WeatherStatus) {
				applyWeather = true;
			}
			abstractionButton.Update(mouseState,oldMouseState);
      Tuple<bool,string> applyResult = applyButton.Update(mouseState, oldMouseState, buttons);
      if (applyResult.Item1) {
        points.Clear();
        var pointsFromDB = Filter.executeQuery(applyResult.Item2, "server = 127.0.0.1; uid = root; pwd = SZ3omhSQ; database = rotterdamDB;",3);
        foreach (List<string> row in pointsFromDB) {
          float lat = Convert.ToSingle(row[0]);
          float lon = Convert.ToSingle(row[1]);
					string inside = row[2];
          points.Add(new Map(new Vector2(lat, lon), Content.Load<Texture2D>("pin.png"),inside));
        }
				if (applyWeather) {
					if (WeatherStatus == "0") {
						clouds.Clear();
					}
					else {
						foreach (Map point in points) {
							if (point.inside != true) {
								var weatherResult = Fetcher.FetchWeather(point.lat, point.lon);
								if (weatherResult.Item2 == Weather.raining) {
									clouds.Add(new Map(point.position, Content.Load<Texture2D>("cloud.png"), "1"));
								}
								else if (weatherResult.Item3 == true) {
									clouds.Add(new Map(point.position, Content.Load<Texture2D>("cloud2.png"), "1"));
								}
							}
						}
					}
					applyWeather = false;
				}
				else {
					weatherButton.overwriteValue(0);
					clouds.Clear();
				}
      }

      // TODO: uncomment this once sixth button (abstraction) implemented
      /*
      for (int i = 0; i < buttons[6].abstractionLevel; i++)
      {
        points = Abstraction.createAbstractedMap(points);
      }
      */

      //System.Console.WriteLine("test" + GetCenter(mapimg, graphics));
    }

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
    {
			GraphicsDevice.Clear(Color.White);
      spriteBatch.Begin();
      //spriteBatch.Draw(map, new Vector2(0, 0), Color.White);
      map1.DrawMap(spriteBatch);
      // TODO: Add your drawing code here
      foreach (Map point in points)
      {
        point.DrawPinstyle(spriteBatch, map1.getMiddle() + point.GetCoordinates(point.printPosition().X, point.printPosition().Y), point.weight);
      }
			foreach (Map cloud in clouds) {
				cloud.Draw(spriteBatch, map1.getMiddle() + cloud.GetCoordinates(Convert.ToDouble(cloud.lat), Convert.ToDouble(cloud.lon)) + new Vector2(-17, -50));
			}
			overlay1.Draw(spriteBatch);
			foreach(IButton button in buttons) {
				button.Draw(spriteBatch);
			}
      foreach(Banner banner in banners) {
        banner.Draw(spriteBatch);
      }
      applyButton.Draw(spriteBatch, mouseState);
			weatherButton.Draw(spriteBatch);
			legend.Draw(spriteBatch);
			abstractionButton.Draw(spriteBatch);
			spriteBatch.End();			
      base.Draw(gameTime);
		}
	}
}
