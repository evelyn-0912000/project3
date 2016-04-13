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
    double centerLatitude = (51.921045);
    double centerLongitude = (4.493159);
   
    public Map(Vector2 position, Texture2D texture)
    {
      this.texture = texture;
			this.position = position;
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

    public Vector2 GetCoordinates(double latitude, double longitude)
    {
      float scale = 0.0000102f;
      double x = ((longitude * Math.Cos(centerLatitude)) - (centerLongitude * Math.Cos(centerLatitude)))*-1;
      //double x = Math.Round((longitude * Math.Cos(51.907744)) - (4.498591 * Math.Cos(51.907744)), 5)*-1;
      double y = ((latitude - centerLatitude) /7.47f)*-1;
      //double y = Math.Round((latitude - 51.907744), 5)*-1;
      float xf = (float) x;
      float yf = (float) y;
      Vector2 Coordinates = new Vector2(((xf/ scale)), (yf/ scale));
      return Coordinates;
    }


    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(texture, position, Color.White);
      System.Console.WriteLine(GetCoordinates(51.907744, 4.498591));
      System.Console.WriteLine(GetCoordinates(51.907883, 4.493516)); 
      System.Console.WriteLine(GetCoordinates(51.913171, 4.493527));
      System.Console.WriteLine(GetCoordinates(51.911503, 4.512609));
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
