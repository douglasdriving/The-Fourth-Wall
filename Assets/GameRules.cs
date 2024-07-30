public class GameRules
{
  public bool dangerousCharsOn = false;
  public char dangerousChar = 'a';

  public GameRules(GameRules rulesToCopy)
  {
    dangerousCharsOn = rulesToCopy.dangerousCharsOn;
    dangerousChar = rulesToCopy.dangerousChar;
  }

  public GameRules()
  {
    dangerousCharsOn = false;
    dangerousChar = 'a';
  }
}

public class CurrentGameRules
{
  public static GameRules currentGameRules = new();
}