using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Meteen_Rotterdam
{
	public interface Drawable {
		void UpdatePos(Vector2 position);
		void Draw(SpriteBatch spriteBatch, Vector2 position);
		Vector2 printPosition();
		Texture2D printTexture();
		Vector2 getMiddle();
    
	}

  public class Map : Drawable
  {
    Texture2D texture;
    Vector2 position;
    Vector2 virtualPosition;
    double centerLatitude = (51.921045);
    double centerLongitude = (4.493159);
   
    public Map(Vector2 position, Texture2D texture)
    {
      this.texture = texture;
        this.position = position;
      this.virtualPosition = new Vector2(0,0); 
    }

    public void UpdatePos(Vector2 position)
    {
      this.position = position;
    }
    
    public void UpdateVirPos(Vector2 virtualPosition) {
      this.virtualPosition = virtualPosition;
    }

		public Vector2 printPosition() {
			return this.position;
		}

		public Texture2D printTexture() {
			return this.texture;
		}

    public Vector2 GetCoordinates(double latitude, double longitude)
    {
      float scale = 0.00001023f;
      double x = (((longitude * Math.Cos(centerLatitude)) - (centerLongitude * Math.Cos(centerLatitude))) /1.0f)*-1;
      //double x = Math.Round((longitude * Math.Cos(51.907744)) - (4.498591 * Math.Cos(51.907744)), 5)*-1;
      double y = ((latitude - centerLatitude) /7.45f)*-1;
      //double y = Math.Round((latitude - 51.907744), 5)*-1;
      float xf = (float) x;
      float yf = (float) y;
      Vector2 Coordinates = new Vector2(((xf/ scale)), (yf/ scale));
      return Coordinates;
    }


    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
      spriteBatch.Draw(texture, position, Color.White);
    }

    public void DrawMap(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(texture, this.position, Color.White);
    }

		public void DrawPinstyle(SpriteBatch spriteBatch, Vector2 position) {
			float adjustedx = position.X - (texture.Width / 2);
			float adjustedy = position.Y - texture.Height;
			spriteBatch.Draw(texture, new Vector2(adjustedx, adjustedy));
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
	public class buttonOverlay {
		Texture2D texture;
		Color[] color;
		Vector2 pos;
		public int width;
		public int height;
		public bool rightstatus;
		public buttonOverlay(bool right, GraphicsDeviceManager graphics, Color additionalColor) {
			rightstatus = right;
			width = 300;
			height = graphics.PreferredBackBufferHeight;
			this.texture = new Texture2D(graphics.GraphicsDevice, width , height);
			color = new Color[width * height];
			for (int i = 0; i < color.Length; i++) {
				if (i % width == 0) {
					color[i] = new Color(0, 0, 0, 25);
				}else if (i % width == 1) {
					color[i] = new Color(0, 0, 0, 75);
				}
				else if (i % width == 2) {
					color[i] = Color.Black;
				}
				else {
					color[i] = new Color(100,100,100,245);
				}
			}
			this.texture.SetData(color);

			if (right == true) {
				this.pos = new Vector2((graphics.PreferredBackBufferWidth - width), 0);
			}	else {
				this.pos = new Vector2(0, 0);
			}
		}
		public void Draw(SpriteBatch spriteBatch) {
			spriteBatch.Draw(texture, pos, Color.White);
		}

		public bool containsMouse (Vector2 mousePos) {
			Rectangle area = new Rectangle((int) pos.X,(int) pos.Y,(int) pos.X + width, (int) pos.Y + height);
			if (area.Contains(mousePos)){
				return true;
			}
			else {
				return false;
			}
		}
	}
}
