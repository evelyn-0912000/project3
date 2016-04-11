using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class Map
{
  Texture2D texture;
  Vector2 position;

  public Map(Vector2 position, Texture2D texture)
  {
    this.texture = texture;
    //this.position = position;
  }

  public void UpdatePos(Vector2 position)
  {
    this.position = position;
  }

  public void Draw(SpriteBatch spriteBatch)
  {
    spriteBatch.Draw(texture, position, Color.White);
  }

}
