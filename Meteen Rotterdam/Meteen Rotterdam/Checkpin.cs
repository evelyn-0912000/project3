using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Meteen_Rotterdam
{
  public class Checkpin
  {
    public static int checkPin(string choice)
    {
      if (choice == "Musea")
      {
        return 1;
      }
      if (choice == "Zwembaden")
      {
        return 2;
      }
      if (choice == "Parken")
      {
        return 3;
      }
      if (choice == "Sportcomplexen")
      {
        return 4;
      }
      if (choice == "Recreatieterreinen")
      {
        return 5;
      }
      if (choice == "Kinderboerderijen")
      {
        return 6;
      }
      if (choice == "Bioscopen")
      {
        return 7;
      }
      if (choice == "Markten")
      {
        return 8;
      }
      else
      {
        return 0;
      }
    }
  }
}
