using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Meteen_Rotterdam {
	public interface IButton {
		bool checkMouse(MouseState mouseState);
		void Update(MouseState mousestate, MouseState oldmousestate);
		void Draw(SpriteBatch spriteBatch);
    string printValue();
  }
	class PersonsButton : IButton {
		public int persons;
		public Vector2 pos;
		private Texture2D texture;
		private List<Texture2D> textureList = new List<Texture2D>();
		public PersonsButton(buttonOverlay overlay, GraphicsDeviceManager graphics, ContentManager content) {
			persons = 0;
			float posx;
			float posy;
			
			for (int i = 0; i < 13; i++) {
				textureList.Add(content.Load<Texture2D>("buttons/persons" + i.ToString() + ".png"));
			}
			texture = textureList[persons];
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
      Rectangle area = new Rectangle((int)pos.X, (int)pos.Y, (int)texture.Width, (int)texture.Height);
      if (area.Contains(mouseState.Position)) {
        return true;
      }
      else {
        return false;
      }
    }
		public void click() {
      persons++;
      if (persons == 13) {
        persons = 0;
      }
			texture = textureList[persons];
      Console.WriteLine("Persons: " + persons.ToString());
		}
		public void Draw(SpriteBatch spriteBatch) {
			spriteBatch.Draw(texture, pos, Color.White);
		}
    public string printValue() {
      return persons.ToString();
    }
    public void Update(MouseState mousestate, MouseState oldmousestate) {
      if (checkMouse(mousestate)) {
        if (mousestate.LeftButton == ButtonState.Pressed && oldmousestate.LeftButton == ButtonState.Released) {
          click();
        }
      }
    }
	}
	class ApplyButton {
		public Vector2 pos;
		private Texture2D texture;
		private Texture2D hovertexture;
		public ApplyButton(buttonOverlay overlay, GraphicsDeviceManager graphics, Color color) {
			float posx;
			float posy;
			texture = Rectangler.makeRect(200, 50, color, graphics);
			hovertexture = Rectangler.makeRect(200, 50, Color.Gray, graphics);
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
			Rectangle area = new Rectangle((int)pos.X, (int)pos.Y, (int)texture.Width, (int)texture.Height);
			if (area.Contains(mouseState.Position)) {
				return true;
			}
			else {
				return false;
			}
		}
    public Tuple<bool, string> click(List<IButton> list) { // Tuple<bool,string>
      List<string> results = new List<string>();
      List<IButton> valueButtons = new List<IButton>();
      foreach (IButton button in list) {
        results.Add(button.printValue());
      }
      ///<remarks>
      /// results[0] is Persons -> IfEmpty it is 0
      /// results[1] is Mood -> IfEmpty it is None
      /// results[2] is Outside -> IfEmpty it is 2
      /// results[3] is Age -> IfEmpty it is 0
      /// </remarks>
      string query = "SELECT a.x, a.y FROM attractions AS a INNER JOIN occasions AS o ON(o.occasion_name = a.occasion)";
      bool firstItem = true;
      if (results[0] != "0") {
        firstItem = false;
        query += " WHERE o.amount_min <= " + results[0] + " AND o.amount_max >= " + results[0];
      }
      if (results[1] != "None") {
        if (firstItem) {
          query += " WHERE ";
          firstItem = false;
        }
        else {
          query += " AND ";
        }
        query += "o.mood = '" + results[1] + "'";
      }
      if (results[2] != "2") {
        if (firstItem) {
          query += " WHERE ";
          firstItem = false;
        }
        else {
          query += " AND ";
        }
        query += "o.indoors = " + results[2];
      }
      if (results[3] != "0") {
        if (firstItem) {
          query += " WHERE ";
          firstItem = false;
        }
        else {
          query += " AND ";
        }
        query += "o.age_min <= " + results[3] + " AND o.age_max >= " + results[3];
      }
			Console.WriteLine(query);
      return new Tuple<bool,string>(true,query);
    }
		public void Draw(SpriteBatch spriteBatch, MouseState mouseState) {
			if (checkMouse(mouseState)) {
				spriteBatch.Draw(hovertexture, pos, Color.White);
			}
			else {
				spriteBatch.Draw(texture, pos, Color.White);
			}
		}
    public string printValue() {
      return null;
    }
    public Tuple<bool, string> Update(MouseState mousestate, MouseState oldmousestate, List<IButton> list) {
      if (checkMouse(mousestate)) {
        if (mousestate.LeftButton == ButtonState.Pressed && oldmousestate.LeftButton == ButtonState.Released) {
          return click(list);
        }
        else {
          return new Tuple<bool, string>(false, "");
        }
      }
      else {
        return new Tuple<bool, string>(false, "");
      }
    }
  }
	class MoodButton : IButton {
    public int mood;
    public string moodname;
		public Vector2 pos;
		private Texture2D texture;
		private List<Texture2D> textureList = new List<Texture2D>();
		public MoodButton(buttonOverlay overlay, GraphicsDeviceManager graphics, ContentManager content) {
      mood = 0;
      moodname = "None";
			float posx;
			float posy;
			
			for (int i = 0; i < 5; i++) {
				textureList.Add(content.Load<Texture2D>("buttons/mood" + i.ToString() + ".png"));
			}
			texture = textureList[mood];
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
      Rectangle area = new Rectangle((int)pos.X, (int)pos.Y, (int)texture.Width, (int)texture.Height);
      if (area.Contains(mouseState.Position)) {
        return true;
      }
      else {
        return false;
      }
    }    
		public void click() {
      mood++;
      if (mood == 5) {
        mood = 0;
      }
      if (mood == 0){
        moodname = "None";
      }else if (mood == 1) {
        moodname = "relaxation";
      }else if (mood == 2) {
        moodname = "education";
      }else if (mood == 3) {
        moodname = "commerce";
      }else if (mood == 4) {
        moodname = "sport";
      }
			texture = textureList[mood];
      Console.WriteLine("Mood: " + mood.ToString() + ". " + moodname);
		}
		public void Draw(SpriteBatch spriteBatch) {
			spriteBatch.Draw(texture, pos, Color.White);
		}
    public string printValue() {
      return moodname;
    }
    public void Update(MouseState mousestate, MouseState oldmousestate) {
      if (checkMouse(mousestate)) {
        if (mousestate.LeftButton == ButtonState.Pressed && oldmousestate.LeftButton == ButtonState.Released) {
          click();
        }
      }
    }
  }
	class AgeButton : IButton {
    public int age;
		public Vector2 pos;
		private Texture2D texture;
		public AgeButton(buttonOverlay overlay, GraphicsDeviceManager graphics, Color color) {
      age = 0;
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
      Rectangle area = new Rectangle((int)pos.X, (int)pos.Y, (int)texture.Width, (int)texture.Height);
      if (area.Contains(mouseState.Position)) {
        return true;
      }
      else {
        return false;
      }
    }
		public void click() {
      age += 5;
      if (age == 85) {
        age = 0;
      }
      Console.WriteLine("Age: " + age.ToString());
		}
		public void Draw(SpriteBatch spriteBatch) {
			spriteBatch.Draw(texture, pos, Color.White);
		}
    public string printValue() {
      return age.ToString();
    }
    public void Update(MouseState mousestate, MouseState oldmousestate) {
      if (checkMouse(mousestate)) {
        if (mousestate.LeftButton == ButtonState.Pressed && oldmousestate.LeftButton == ButtonState.Released) {
          click();
        }
      }
    }
  }
	class OutsideButton : IButton { //IS ACTUALLY AN INSIDE BUTTON
    public int inside;
		public Vector2 pos;
		private Texture2D texture;
		private List<Texture2D> textureList = new List<Texture2D>();
		public OutsideButton(buttonOverlay overlay, GraphicsDeviceManager graphics, ContentManager content) {
			float posx;
			float posy;
      inside = 2;
			for (int i = 0; i < 3; i++) {
				textureList.Add(content.Load<Texture2D>("buttons/inside" + i.ToString() + ".png"));
			}
			texture = textureList[inside];
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
      Rectangle area = new Rectangle((int)pos.X, (int)pos.Y, (int)texture.Width, (int)texture.Height);
      if (area.Contains(mouseState.Position)) {
        return true;
      }
      else {
        return false;
      }
    }
		public void click() {
      inside++;
      if (inside == 3) {
        inside = 0;
      }
			texture = textureList[inside];
      Console.WriteLine("Inside: " + inside.ToString());
		}
		public void Draw(SpriteBatch spriteBatch) {
			spriteBatch.Draw(texture, pos, Color.White);
		}
    public string printValue() {
      return inside.ToString();
    }
    public void Update(MouseState mousestate, MouseState oldmousestate) {
      if (checkMouse(mousestate)) {
        if (mousestate.LeftButton == ButtonState.Pressed && oldmousestate.LeftButton == ButtonState.Released) {
          click();
        }
      }
    }
  }
}
