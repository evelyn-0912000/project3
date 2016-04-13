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
    public double Alongitude = (51.965416);
    public double Alatitude = (4.414179);
    public float Blongitude = (51.864696f);
    public float Blatitude = (4.579965f);
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
    {
      double x = longitude * Math.Cos(latitude);
      double y = latitude;
      System.Console.WriteLine(x);
      System.Console.WriteLine(y);
    }


    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(texture, position, Color.White);
      GetCoordinates(Alongitude, Blongitude);

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
