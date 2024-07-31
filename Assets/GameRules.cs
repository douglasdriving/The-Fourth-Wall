using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

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

  public bool platformPopOn = false;
  KeyControl platformRespawnKey = Keyboard.current.mKey;

  public GameRules(GameRules rulesToCopy)
  {
    dangerousCharsOn = rulesToCopy.dangerousCharsOn;
    dangerousChar = rulesToCopy.dangerousChar;

    directionWallsOn = rulesToCopy.directionWallsOn;
    wallDirection = rulesToCopy.wallDirection;

    platformPopOn = rulesToCopy.platformPopOn;
    platformRespawnKey = rulesToCopy.platformRespawnKey;
  }

  public GameRules()
  {
    dangerousCharsOn = false;
    dangerousChar = 'a';

    directionWallsOn = false;
    wallDirection = Direction.BACKWARD;

    platformPopOn = false;
    platformRespawnKey = Keyboard.current.mKey;
  }
}

public class CurrentGameRules
{
  public static GameRules rules = new();
}

/// okay so, platform pop.
/// it will be triggered by the player jumping
/// and it will have to know what is a platform and what is just a level piece
/// to do this, it would be very helpful if the pieces was actually added to a platform "parent"
/// so that the entire parent could be toggled off
/// then, it would be easy to check what platform we jumped from
/// and deactivate all the other platforms when the player jumps
/// ok so now we have sorted all the platforms into game objects
/// 
/// a platform pop class listens to it
/// if it is fired, it checks the platform pop rule
/// if its true, it gets all the platforms (direct children of the platform root)
/// if it cant get the direct children, we will have to save the platforms in a separate list in the level generator
/// it then determines which platform is the closest one to the player
/// and sets all other platforms inactive
/// 
/// THEN, we have to bring out the magic callback key
/// the player can bring back the platforms
/// but that is next challenge ;)