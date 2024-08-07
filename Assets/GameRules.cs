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

    platformPopOn = false;
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