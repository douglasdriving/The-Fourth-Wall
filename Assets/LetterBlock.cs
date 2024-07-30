using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class LetterBlock : MonoBehaviour
{
    [SerializeField] TMP_Text textElement;
    KeyControl keyToDestroyOn = null;

    private void FixedUpdate()
    {
        if (keyToDestroyOn.isPressed)
        {
            Debug.Log("destroying!");
            Destroy(gameObject);
        }
    }

    public void SetLetter(char letter)
    {
        textElement.text = letter.ToString();
        AssignDestroyKey(letter);
    }

    private void AssignDestroyKey(char letter)
    {
        Keyboard keyboard = Keyboard.current;
        string letterString = letter.ToString();
        letterString = letterString.ToUpper();

        switch (letterString)
        {
            case "A":
                keyToDestroyOn = keyboard.aKey;
                break;
            case "B":
                keyToDestroyOn = keyboard.bKey;
                break;
            case "C":
                keyToDestroyOn = keyboard.cKey;
                break;
            case "D":
                keyToDestroyOn = keyboard.dKey;
                break;
            case "E":
                keyToDestroyOn = keyboard.eKey;
                break;
            case "F":
                keyToDestroyOn = keyboard.fKey;
                break;
            case "G":
                keyToDestroyOn = keyboard.gKey;
                break;
            case "H":
                keyToDestroyOn = keyboard.hKey;
                break;
            case "I":
                keyToDestroyOn = keyboard.iKey;
                break;
            case "J":
                keyToDestroyOn = keyboard.jKey;
                break;
            case "K":
                keyToDestroyOn = keyboard.kKey;
                break;
            case "L":
                keyToDestroyOn = keyboard.lKey;
                break;
            case "M":
                keyToDestroyOn = keyboard.mKey;
                break;
            case "N":
                keyToDestroyOn = keyboard.nKey;
                break;
            case "O":
                keyToDestroyOn = keyboard.oKey;
                break;
            case "P":
                keyToDestroyOn = keyboard.pKey;
                break;
            case "Q":
                keyToDestroyOn = keyboard.qKey;
                break;
            case "R":
                keyToDestroyOn = keyboard.rKey;
                break;
            case "S":
                keyToDestroyOn = keyboard.sKey;
                break;
            case "T":
                keyToDestroyOn = keyboard.tKey;
                break;
            case "U":
                keyToDestroyOn = keyboard.uKey;
                break;
            case "V":
                keyToDestroyOn = keyboard.vKey;
                break;
            case "W":
                keyToDestroyOn = keyboard.wKey;
                break;
            case "X":
                keyToDestroyOn = keyboard.xKey;
                break;
            case "Y":
                keyToDestroyOn = keyboard.yKey;
                break;
            case "Z":
                keyToDestroyOn = keyboard.zKey;
                break;
            case ",":
                keyToDestroyOn = keyboard.commaKey;
                break;
            case ".":
                keyToDestroyOn = keyboard.periodKey;
                break;
            case " ":
                keyToDestroyOn = keyboard.spaceKey;
                break;
            case "'":
                keyToDestroyOn = keyboard.quoteKey;
                break;
            default:
                keyToDestroyOn = null;
                Debug.LogError("unable to assign a key for letter string: " + letter);
                Destroy(gameObject);
                break;
        }
    }
}
