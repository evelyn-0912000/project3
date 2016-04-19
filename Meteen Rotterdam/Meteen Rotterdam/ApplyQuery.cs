using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Meteen_Rotterdam {
  class ApplyButton {
    public Vector2 pos;
    private Texture2D texture;
    private Texture2D offtexture;
    public ApplyButton(buttonOverlay overlay, GraphicsDeviceManager graphics, ContentManager content) {
      float posx;
      float posy;
      offtexture = content.Load<Texture2D>("buttons/apply2.png");
      texture = content.Load<Texture2D>("buttons/apply1.png");
      if (overlay.rightstatus == true) {
        posx = graphics.PreferredBackBufferWidth - (overlay.width - 50);
      }
      else {
        posx = 50;
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
      bool success = true;
      if (Int32.Parse(results[0]) > Int32.Parse(results[1]) && results[1] != "0") {
        success = false;
        Console.WriteLine("Err: Person min is higher than person max");
      }
      if (Int32.Parse(results[4]) > Int32.Parse(results[5]) && results[5] != "0"){
        success = false;
        Console.WriteLine("Err: Age min is higher than age max");
      }
      if (success) {
        string query = "SELECT a.x, a.y FROM attractions AS a INNER JOIN occasions AS o ON(o.occasion_name = a.occasion)";
        bool firstItem = true;
        if (results[0] != "0") {
          firstItem = false;
          query += " WHERE o.amount_min <= " + results[0];
        }
        if (results[1] != "0") {
          if (firstItem) {
            query += " WHERE ";
            firstItem = false;
          }
          else {
            query += " AND ";
          }
          query += "o.amount_max >= " + results[1];
        }
        if (results[2] != "None") {
          if (firstItem) {
            query += " WHERE ";
            firstItem = false;
          }
          else {
            query += " AND ";
          }
          query += "o.mood = '" + results[2] + "'";
        }
        if (results[3] != "2") {
          if (firstItem) {
            query += " WHERE ";
            firstItem = false;
          }
          else {
            query += " AND ";
          }
          query += "o.indoors = " + results[3];
        }
        if (results[4] != "0") {
          if (firstItem) {
            query += " WHERE ";
            firstItem = false;
          }
          else {
            query += " AND ";
          }
          query += "o.age_min <= " + results[4];
        }
        if (results[5] != "0") {
          if (firstItem) {
            query += " WHERE ";
            firstItem = false;
          }
          else {
            query += " AND ";
          }
          query += "o.age_max >= " + results[5];
        }
        Console.WriteLine("<----><----> APPLY <----><---->");
        return new Tuple<bool, string>(true, query);
      }
      else {
        return new Tuple<bool, string>(false, "");
      }      
    }
    public void Draw(SpriteBatch spriteBatch, MouseState mouseState) {
      if (checkMouse(mouseState)) {
        spriteBatch.Draw(offtexture, pos, Color.White);
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
}
