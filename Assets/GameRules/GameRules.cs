using System;
using UnityEngine;
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
  public KeyControl platformRespawnKey = Keyboard.current.mKey;

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
    platformRespawnKey = null;
  }
}

public class CurrentGameRules
{
  public static GameRules rules = new();

  public static void UpdateRules(GameRules newRules)
  {
    rules = new GameRules(newRules);
    // GameObject.FindObjectOfType<PlatformToggler>().SetActivationKey(rules.platformRespawnKey);
  }

  public static void SetDangerousLetter(char letter)
  {
    rules.dangerousCharsOn = true;
    rules.dangerousChar = letter;
  }

  public static void SetWallSpawningDirection(Direction dir)
  {
    rules.directionWallsOn = true;
    rules.wallDirection = dir;
  }

  public static void SetDissapearingPlatformsWithKey(KeyControl key)
  {
    rules.platformPopOn = true;
    rules.platformRespawnKey = key;
    // GameObject.FindObjectOfType<PlatformToggler>().SetActivationKey(key);
  }
}