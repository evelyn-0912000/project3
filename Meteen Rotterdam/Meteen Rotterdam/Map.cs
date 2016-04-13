using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace Meteen_Rotterdam
{
	public interface Drawable {
		void UpdatePos(Vector2 position);
		void Draw(SpriteBatch spriteBatch);
		Vector2 printPosition();
		Texture2D printTexture();
		Vector2 getMiddle();
	}

  public class Map : Drawable
  {
    Texture2D texture;
    Vector2 position;
    private double Clongitude = (51.907744);
    private double Clatitude = (4.498591);
    

    public float test = ((51.965416f - 51.864696f)+ 10);
    public float test2 = ((4.414179f - 4.579965f)+ 10);
   
    public Map(Vector2 position, Texture2D texture)
    {
      this.texture = texture;
    }

    public void UpdatePos(Vector2 position)
    {
      this.position = position;
    }

		public Vector2 printPosition() {
			return this.position;
		}

		public Texture2D printTexture() {
			return this.texture;
		}

    public void GetCoordinates(double latitude, double longitude)
    public Vector2 GetCoordinates(double latitude, double longitude)
    {
      float scale = 0.0001f;
      //double x = Math.Round((longitude * Math.Cos(51.907744)), 5);
      double x = Math.Round((longitude * Math.Cos(51.907744)) - (4.498591 * Math.Cos(51.907744)), 5)*-1;
      //double y = Math.Round(latitude, 5);
      double y = Math.Round((latitude - 51.907744), 5);
      float xf = (float) x;
      float yf = (float) y;
      Vector2 Coordinates = new Vector2((xf/ scale), (yf/ scale));
      return Coordinates;
    }


    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(texture, position, Color.White);
      System.Console.WriteLine(GetCoordinates(51.907744, 4.498591));
      System.Console.WriteLine(GetCoordinates(51.907883, 4.493516)); 

      int tst = (int)test;
      //public Rectangle square = new Rectangle(0, 0, tst, 100);
    }

		//public Vector2 getMiddle() {
		//	float x = (image.printTexture().Width / 2);
		//	float y = (image.printTexture().Height / 2);
		//	return new Vector2(x, y);
		//}
		public Vector2 getMiddle() {
			float x = (this.position.X + this.texture.Width / 2);
			float y = (this.position.Y + this.texture.Height / 2);
			return new Vector2(x, y);
			}
  }
}
