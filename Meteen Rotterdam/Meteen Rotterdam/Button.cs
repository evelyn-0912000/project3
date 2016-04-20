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

    // TODO: Use the overlay variable and set position properly
    class AbstractionButton : IButton
    {
        public int abstractionLevel;
        public Vector2 pos;
        private Texture2D texture;
        private List<Texture2D> textureList = new List<Texture2D>();

        public AbstractionButton(buttonOverlay overlay, GraphicsDeviceManager graphics, ContentManager content) {
            this.abstractionLevel = 0;

            for (int i = 0; i < 3; i++)
            {
                textureList.Add(content.Load<Texture2D>("buttons/abstraction" + i.ToString() + ".png"));
            }

            texture = textureList[abstractionLevel];
        }

        public bool checkMouse(MouseState mouseState)
        {
            Rectangle area = new Rectangle((int)pos.X, (int)pos.Y, (int)texture.Width, (int)texture.Height);
            if (area.Contains(mouseState.Position))
            {
                return true;
            }
            else {
                return false;
            }
        }

        public void click()
        {
            abstractionLevel++;
            if (abstractionLevel == 3)
            {
                abstractionLevel = 0;
            }

            texture = textureList[abstractionLevel];
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, pos, Color.White);
        }
        public string printValue()
        {
            return abstractionLevel.ToString();
        }
        public void Update(MouseState mousestate, MouseState oldmousestate)
        {
            if (checkMouse(mousestate))
            {
                if (mousestate.LeftButton == ButtonState.Pressed && oldmousestate.LeftButton == ButtonState.Released)
                {
                    click();
                }
            }
        }

	class PersonsButton : IButton {
		public int persons;
		public Vector2 pos;
		private Texture2D texture;
    private bool max;
    private List<Texture2D> textureList = new List<Texture2D>();

		public PersonsButton(bool max, buttonOverlay overlay, GraphicsDeviceManager graphics, ContentManager content) { 
      this.max = max;
			persons = 0;
			float posx;
			float posy;
      for (int i = 0; i < 13; i++) {
        if (max) {
          textureList.Add(content.Load<Texture2D>("buttons/personsmax" + i.ToString() + ".png"));
        }else {
          textureList.Add(content.Load<Texture2D>("buttons/persons" + i.ToString() + ".png"));
        }
      }
      texture = textureList[persons];
      if (overlay.rightstatus == true) {
        if (max) {
          posx = graphics.PreferredBackBufferWidth - (overlay.width - 155);
          
        } else {
          posx = graphics.PreferredBackBufferWidth - (overlay.width - 50);
        }
      }
      else {
        if (max) {
          posx = 155;

        }
        else {
          posx = 50;
        }        
      }
      posy = (Game1.GetCenter(texture, graphics).Y - 25);
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
      if (max) {
        Console.WriteLine("Max persons: " + persons.ToString());
      } else {
        Console.WriteLine("Min persons: " + persons.ToString());
      }
      texture = textureList[persons];
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
				posx = 155;
			}
			posy = (Game1.GetCenter(texture, graphics).Y + 140);
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
    public bool max;
		public Vector2 pos;
		private Texture2D texture;
    private List<Texture2D> textureList = new List<Texture2D>();
    public AgeButton(bool max, buttonOverlay overlay, GraphicsDeviceManager graphics, ContentManager content) {
      age = 0;
      this.max = max;
			float posx;
			float posy;
      for (int i = 0; i < 85; i += 5) {
        if (max) {
          textureList.Add(content.Load<Texture2D>("buttons/agemax" + i.ToString() + ".png"));
        }else {
          textureList.Add(content.Load<Texture2D>("buttons/age" + i.ToString() + ".png"));
        }
      }
      texture = textureList[age];
			if (overlay.rightstatus) {
        if (max) {
          posx = graphics.PreferredBackBufferWidth - (overlay.width - 155);
        } else {
          posx = graphics.PreferredBackBufferWidth - (overlay.width - 50);
        }
				
			}
			else {
        if (max) {
          posx = 155;
        }
        else {
          posx = 50;
        }
        
			}
			posy = (Game1.GetCenter(texture, graphics).Y -190);
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
      if (max) {
        Console.WriteLine("Max Age: " + age.ToString());
      }
      else {
        Console.WriteLine("Min Age: " + age.ToString());
      }

      texture = textureList[age / 5];
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
				posx = graphics.PreferredBackBufferWidth - (overlay.width - 155);
			}
			else {
				posx = 50;
			}
			posy = (Game1.GetCenter(texture, graphics).Y + 140);
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
