using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Meteen_Rotterdam
{
  class Pin
  {
    Texture2D texture;
    Vector2 position;
    public int weight { get; set; }
    public string lat;
    public string lon;
    public bool inside;

    public Pin(Vector2 position, Texture2D texture, string inside, int weight = 1)
    {
      this.lat = position.X.ToString();
      this.lon = position.Y.ToString();
      this.texture = texture;
      this.position = position;
      this.weight = weight;
      if (inside == "1" || inside == "True")
      {
        this.inside = true;
      }
      else
      {
        this.inside = false;
      }
    }
  }
}
