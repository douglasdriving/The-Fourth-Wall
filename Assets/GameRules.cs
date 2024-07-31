using System;
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
  public delegate void PlatformSpawnKeySet(KeyControl newKey);
  public event PlatformSpawnKeySet OnPlatformSpawnKeySet;

  public GameRules(GameRules rulesToCopy)
  {
    dangerousCharsOn = rulesToCopy.dangerousCharsOn;
    dangerousChar = rulesToCopy.dangerousChar;

    directionWallsOn = rulesToCopy.directionWallsOn;
    wallDirection = rulesToCopy.wallDirection;

    platformPopOn = rulesToCopy.platformPopOn;
    SetPlatformSpawnKey(rulesToCopy.platformRespawnKey);
  }

  public GameRules()
  {
    dangerousCharsOn = false;
    dangerousChar = 'a';

    directionWallsOn = false;
    wallDirection = Direction.BACKWARD;

    platformPopOn = true;
    SetPlatformSpawnKey(Keyboard.current.mKey);
  }

  public void SetPlatformSpawnKey(KeyControl newKey)
  {
    platformRespawnKey = newKey;
    OnPlatformSpawnKeySet?.Invoke(platformRespawnKey);
  }

  public KeyControl GetPlatformSpawnKey()
  {
    return platformRespawnKey;
  }
}

public class CurrentGameRules
{
  public static GameRules rules = new();
}

/// THEN, we have to bring out the magic callback key
/// the player can bring back the platforms
/// but that is next challenge ;)
/// 
///okay so the platform pop works now
///next up: making them come back!
///so... how do we do this?
///it seems reasonable that there is a function for re-assigning the respawn key
///and that when this key is re-assigned, it also binds the key
///HOWEVER, it does not make sense that the game rules class listens to key commands
///so, perhaps the game rules can call an event when the respawn key is reassigned
///and then have the player listen to this event
///and rebind the key
///and then the player can have a class that makes the platforms re-appear
///which should be quite simply to program :)