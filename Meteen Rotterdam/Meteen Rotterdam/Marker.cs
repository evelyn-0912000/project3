using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Meteen_Rotterdam
{
  class Marker
  {
    Texture2D texture;
    Vector2 position;

    public Marker(Vector2 position, Texture2D texture)
    {
      this.texture = texture;
    }
  }
}
