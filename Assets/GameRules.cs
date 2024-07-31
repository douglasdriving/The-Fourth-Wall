public enum Direction
{
  FORWARD,
  BACKWARD,
  LEFT,
  RIGHT
}

public class GameRules
{
  public bool dangerousCharsOn = false;
  public char dangerousChar = 'a';
  public bool directionWallsOn = false;
  public Direction wallDirection = Direction.BACKWARD;

  public GameRules(GameRules rulesToCopy)
  {
    dangerousCharsOn = rulesToCopy.dangerousCharsOn;
    dangerousChar = rulesToCopy.dangerousChar;
    directionWallsOn = rulesToCopy.directionWallsOn;
    wallDirection = rulesToCopy.wallDirection;
  }

  public GameRules()
  {
    dangerousCharsOn = false;
    dangerousChar = 'a';
    directionWallsOn = false;
    wallDirection = Direction.BACKWARD;
  }
}

public class CurrentGameRules
{
  public static GameRules rules = new();
}