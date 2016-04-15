using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Meteen_Rotterdam {
	public interface IButton {
		void click();
		bool checkMouse(MouseState mouseState);
		void Update(MouseState mouseState);
		void Draw(SpriteBatch spriteBatch);
	}
	class PersonsButton : IButton {
		public int persons;
		public Vector2 pos;
		private Texture2D texture;
		public PersonsButton(buttonOverlay overlay, GraphicsDeviceManager graphics, Color color) {
			persons = 1;
			float posx;
			float posy;
			texture = Rectangler.makeRect(95, 95, color, graphics);
			if (overlay.rightstatus == true) {
				posx = graphics.PreferredBackBufferWidth - (overlay.width - 155);
			}
			else {
				posx = 100;
			}
			posy = (Game1.GetCenter(texture, graphics).Y + 20);
			pos = new Vector2(posx, posy);
		}

		public bool checkMouse(MouseState mouseState) {
			throw new NotImplementedException();
		}

		public void click() {
			throw new NotImplementedException();
		}

		public void Draw(SpriteBatch spriteBatch) {
			spriteBatch.Draw(texture, pos, Color.White);
		}

		public void Update(MouseState mousestate) {
			
		}
	}

	class ApplyButton : IButton {
		public Vector2 pos;
		private Texture2D texture;

		public ApplyButton(buttonOverlay overlay, GraphicsDeviceManager graphics, Color color) {
			float posx;
			float posy;
			texture = Rectangler.makeRect(200, 50, color, graphics);
			if (overlay.rightstatus == true) {
				posx = graphics.PreferredBackBufferWidth - (overlay.width - 50);
			}
			else {
				posx = 100;
			}
			posy = (Game1.GetCenter(texture, graphics).Y + 100);
			pos = new Vector2(posx, posy);
		}

		public bool checkMouse(MouseState mouseState) {
			Rectangle area = new Rectangle((int)pos.X, (int)pos.Y, (int)pos.X + texture.Width, (int)pos.Y + texture.Height);
			if (area.Contains(mouseState.Position)) {
				return true;
			}
			else {
				return false;
			}
		}

		public void click() {
			throw new NotImplementedException();
		}

		public void Draw(SpriteBatch spriteBatch) {
			spriteBatch.Draw(texture, pos, Color.White);
		}

		public void Update(MouseState mouseState) {
			
		}
	}

	class MoodButton : IButton {
		public Vector2 pos;
		private Texture2D texture;
		public MoodButton(buttonOverlay overlay, GraphicsDeviceManager graphics, Color color) {
			float posx;
			float posy;
			texture = Rectangler.makeRect(95, 95, color, graphics);
			if (overlay.rightstatus == true) {
				posx = graphics.PreferredBackBufferWidth - (overlay.width - 50);
			}
			else {
				posx = 100;
			}
			posy = (Game1.GetCenter(texture, graphics).Y + 20);
			pos = new Vector2(posx, posy);
		}


		public bool checkMouse(MouseState mouseState) {
			throw new NotImplementedException();
		}

		

		public void click() {
			throw new NotImplementedException();
		}

		public void Draw(SpriteBatch spriteBatch) {
			spriteBatch.Draw(texture, pos, Color.White);
		}

		public void Update(MouseState mouseState) {
			throw new NotImplementedException();
		}
	}

	class AgeButton : IButton {
		public Vector2 pos;
		private Texture2D texture;
		public AgeButton(buttonOverlay overlay, GraphicsDeviceManager graphics, Color color) {
			float posx;
			float posy;
			texture = Rectangler.makeRect(95, 95, color, graphics);
			if (overlay.rightstatus == true) {
				posx = graphics.PreferredBackBufferWidth - (overlay.width - 155);
			}
			else {
				posx = 100;
			}
			posy = (Game1.GetCenter(texture, graphics).Y -85);
			pos = new Vector2(posx, posy);
		}

		public bool checkMouse(MouseState mouseState) {
			throw new NotImplementedException();
		}

		public void click() {
			throw new NotImplementedException();
		}

		public void Draw(SpriteBatch spriteBatch) {
			spriteBatch.Draw(texture, pos, Color.White);
		}

		public void Update(MouseState mouseState) {
			throw new NotImplementedException();
		}
	}

	class OutsideButton : IButton {
		public Vector2 pos;
		private Texture2D texture;

		public OutsideButton(buttonOverlay overlay, GraphicsDeviceManager graphics, Color color) {
			float posx;
			float posy;
			texture = Rectangler.makeRect(95, 95, color, graphics);
			if (overlay.rightstatus == true) {
				posx = graphics.PreferredBackBufferWidth - (overlay.width - 50);
			}
			else {
				posx = 100;
			}
			posy = (Game1.GetCenter(texture, graphics).Y - 85);
			pos = new Vector2(posx, posy);
		}

		public bool checkMouse(MouseState mouseState) {
			throw new NotImplementedException();
		}

		public void click() {
			throw new NotImplementedException();
		}

		public void Draw(SpriteBatch spriteBatch) {
			spriteBatch.Draw(texture, pos, Color.White);
		}

		public void Update(MouseState mouseState) {
			
		}
	}
}
