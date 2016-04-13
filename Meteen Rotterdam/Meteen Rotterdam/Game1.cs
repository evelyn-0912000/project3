using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
    private Vector2 mapPosition;
    private Map map1;
		private Map pointer1;
    private Map pointer2;

    public Game1()
    {
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
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
			spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
      mapimg = Content.Load<Texture2D>("map.png");
      map1 = new Map(mapPosition, mapimg);
			pointer1 = new Map(map1.getMiddle(), Content.Load<Texture2D>("pointer.png"));
      pointer2 = new Map(map1.getMiddle(), Content.Load<Texture2D>("pointer.png"));
      // TODO: use this.Content to load your game content here
    }

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
    {
			// TODO: Unload any non ContentManager content here
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
      mouseState = Mouse.GetState();
      

      if (mouseState.LeftButton == ButtonState.Pressed)
      {        
        Vector2 mousePosition = new Vector2(this.mouseState.X, this.mouseState.Y);
        if (grabOffset == Vector2.Zero)
        {
          grabOffset = new Vector2(mapPosition.X - mousePosition.X, mapPosition.Y - mousePosition.Y);
        }
        else
        {
          mapPosition = new Vector2(mousePosition.X + grabOffset.X, mousePosition.Y + grabOffset.Y);
          map1.UpdatePos(mapPosition);
        }
        
        //System.Console.WriteLine("OFFSET" + grabOffset);
        //System.Console.WriteLine("MOUSE" + mousePosition);
        //System.Console.WriteLine("MAP" + mapPosition);
        
      }
      if (mouseState.LeftButton == ButtonState.Released)
      {
        grabOffset = Vector2.Zero;
      }
			pointer1.UpdatePos(map1.getMiddle() + (map1.GetCoordinates(51.907744, 4.498591)));
      pointer2.UpdatePos(map1.getMiddle() + (map1.GetCoordinates(51.907883, 4.493516)));
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
      map1.Draw(spriteBatch);
			pointer1.Draw(spriteBatch);
      pointer2.Draw(spriteBatch);
      spriteBatch.End();

      // TODO: Add your drawing code here

      base.Draw(gameTime);

      
    }

	}
}

