/*  Author: Joseph Malibiran
 *  Date Created: January 28, 2021
 *  Last Updated: January 29, 2021
 *  Description: Manages and also retains information regarding the loaded save files and all available save files. 
 *  
 */

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFileManager : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] private string savefileName = "Hamstronaut";       //This is the name of the save file. An indexing number will be appended to this name. This is different from the save file header seen in-game.

    [Header("Temp Settings")]                                           //TODO Remove. Most of these properties are just here for development use and testing; they have no use on final product.
    [SerializeField] private string savefileHeader = ""; 
    [SerializeField] private Vector3 playerLocation = Vector3.zero;
    [SerializeField] private Vector3 playerOrientation = Vector3.zero;
    [SerializeField] private int livesAmount = 3;
    [SerializeField] private int ammoAmount = 100;
    [SerializeField] private int seedsCollected = 0;
    [SerializeField] private int aliensKilled = 0;
    [SerializeField] private int currentLevel = 0;                      //0 means not in a level
    [SerializeField] private int levelsUnlocked = 1;
    [SerializeField] private int selectedSaveSlot = 1; 

    [Header("Temp Debug Controls")]
    [SerializeField] private bool saveButton = false;                   //TODO Remove. This is only used during development to test savefile saving.
    [SerializeField] private bool loadButton = false;                   //TODO Remove. This is only used during development to test savefile loading.

    private string[] availableSaveFiles = new string[9];                //Note: This game will have a maximum 8 save slots hardcoded.
    private SaveData loadedSaveData;                                    //Initial save data being used
    private string gameVersion = "0.1";

    //TODO Remove. This is only used during development to test
    private void Update() 
    {
        //TODO Remove. This is only used during development to test savefile saving.
        if (saveButton) 
        {
            saveButton = false;
            SaveGame(selectedSaveSlot);
        }
        //TODO Remove. This is only used during development to test savefile loading.
        if (loadButton) 
        {
            loadButton = false;
            LoadGame(selectedSaveSlot); 
        }
    }

    public void QuickSave() {
        loadedSaveData = new SaveData();
        loadedSaveData.savefileHeader = "(Quicksave) Marco    Lives: " + loadedSaveData.livesAmount + "; Ammo: " + loadedSaveData.ammoAmount + "; Seeds: " + loadedSaveData.seedsCollected + "; Levels Unlocked: " + loadedSaveData.levelsUnlocked;
        loadedSaveData.gameVersion = this.gameVersion;
        loadedSaveData.playerLocationX = this.playerLocation.x;
        loadedSaveData.playerLocationY = this.playerLocation.y;
        loadedSaveData.playerLocationZ = this.playerLocation.z;
        loadedSaveData.playerOrientationX = this.playerOrientation.x;
        loadedSaveData.playerOrientationY = this.playerOrientation.y;
        loadedSaveData.playerOrientationZ = this.playerOrientation.z;

        loadedSaveData.livesAmount = this.livesAmount;
        loadedSaveData.ammoAmount = this.ammoAmount;
        loadedSaveData.seedsCollected = this.seedsCollected;
        loadedSaveData.aliensKilled = this.aliensKilled;
        loadedSaveData.currentLevel = this.currentLevel; //0 means not in a level
        loadedSaveData.levelsUnlocked = this.levelsUnlocked;

        SaveFileReaderWriter.WriteToSaveFile(Application.persistentDataPath + "/" + savefileName + "0.hamsave", loadedSaveData);
    }

    //Saves game data at given save slot index
    public void SaveGame(int _saveSlotIndex) 
    {
        if (_saveSlotIndex <= 0 || _saveSlotIndex > 8) { //This game will have a maximum 8 (0 to 8) save slots hardcoded. 0 slot should be reserved for quicksave
            Debug.LogError("[Error] Invalid save slot index! Slot number must be between from 1 to 8.");
            return;
        }

        loadedSaveData = new SaveData();
        loadedSaveData.savefileHeader = "Marco    Lives: " + loadedSaveData.livesAmount + "; Ammo: " + loadedSaveData.ammoAmount + "; Seeds: " + loadedSaveData.seedsCollected + "; Levels Unlocked: " + loadedSaveData.levelsUnlocked;
        loadedSaveData.gameVersion = this.gameVersion;
        loadedSaveData.playerLocationX = this.playerLocation.x;
        loadedSaveData.playerLocationY = this.playerLocation.y;
        loadedSaveData.playerLocationZ = this.playerLocation.z;
        loadedSaveData.playerOrientationX = this.playerOrientation.x;
        loadedSaveData.playerOrientationY = this.playerOrientation.y;
        loadedSaveData.playerOrientationZ = this.playerOrientation.z;

        loadedSaveData.livesAmount = this.livesAmount;
        loadedSaveData.ammoAmount = this.ammoAmount;
        loadedSaveData.seedsCollected = this.seedsCollected;
        loadedSaveData.aliensKilled = this.aliensKilled;
        loadedSaveData.currentLevel = this.currentLevel; //0 means not in a level
        loadedSaveData.levelsUnlocked = this.levelsUnlocked;

        SaveFileReaderWriter.WriteToSaveFile(Application.persistentDataPath + "/" + savefileName + _saveSlotIndex + ".hamsave", loadedSaveData);
    }

    //Saves given game data at given save slot index
    public void SaveGame(int _saveSlotIndex, SaveData _saveData) 
    {
        if (_saveSlotIndex <= 0 || _saveSlotIndex > 8) { //This game will have a maximum 8 save slots hardcoded.
            Debug.LogError("[Error] Invalid save slot index! Slot number must be between from 1 to 8.");
            return;
        }

        _saveData.gameVersion = this.gameVersion;
        _saveData.savefileHeader = "Marco    Lives: " + _saveData.livesAmount + "; Ammo: " + _saveData.ammoAmount + "; Seeds: " + _saveData.seedsCollected + "; Levels Unlocked: " + _saveData.levelsUnlocked;

        SaveFileReaderWriter.WriteToSaveFile(Application.persistentDataPath + "/" + savefileName + _saveSlotIndex + ".hamsave", _saveData);
    }

    //Loads save file data at given save slot index
    public SaveData LoadGame(int _saveSlotIndex) 
    {
        if (_saveSlotIndex < 0 || _saveSlotIndex > 8) { //This game will have a maximum 8 save slots hardcoded.
            Debug.LogError("[Error] Invalid save slot index! Slot number must be between from 0 to 8.");
            return null;
        }

        if (!File.Exists(Application.persistentDataPath + "/" + savefileName + _saveSlotIndex + ".hamsave")) {
            Debug.LogError("[Error] File does not exist; Cannot load a save file that does not exist.");
            return null;
        }

        loadedSaveData = SaveFileReaderWriter.ReadFromSaveFile(Application.persistentDataPath + "/" + savefileName + _saveSlotIndex + ".hamsave"); 

        if (this.gameVersion != loadedSaveData.gameVersion) {
            Debug.LogWarning("[Warning] Cannot load save file; incompatible version. ");
            return null;
        }

        this.savefileHeader = loadedSaveData.savefileHeader;
        this.playerLocation = new Vector3(loadedSaveData.playerLocationX, loadedSaveData.playerLocationY, loadedSaveData.playerLocationZ);
        this.playerOrientation = new Vector3(loadedSaveData.playerOrientationX, loadedSaveData.playerOrientationY, loadedSaveData.playerOrientationZ);
        this.livesAmount = loadedSaveData.livesAmount;
        this.ammoAmount = loadedSaveData.ammoAmount;
        this.seedsCollected = loadedSaveData.seedsCollected;
        this.aliensKilled = loadedSaveData.aliensKilled;
        this.currentLevel = loadedSaveData.currentLevel; //0 means not in a level
        this.levelsUnlocked = loadedSaveData.levelsUnlocked;

        return loadedSaveData;
    }

    //TODO Untested
    //Returns the loaded SaveData File.
    public SaveData GetSaveData() 
    {
        if (loadedSaveData == null) 
        {
            Debug.LogError("[Error] No save data loaded yet; returning SaveData with default properties.");
            return new SaveData();
        }

        return loadedSaveData;
    }

    //TODO Untested
    //Returns the header of a specified save slot. 
    //The header contains information about the save file. If the save slot is empty it will return "Empty Save Slot".
    //Usage: Save Slots should be represented by buttons that the user can click and the button text should be a save slot header to display its information. 
    //Pressing the button should call SaveFileManager.LoadGame(saveSlotIndex) where each button represents different save slots.
    public string GetSaveSlotHeader(int _saveSlotIndex) 
    {
        if (_saveSlotIndex < 0 || _saveSlotIndex > 8) 
        { //This game will have a maximum 8 save slots hardcoded.
            Debug.LogError("[Error] Invalid save slot index! Slot number must be between from 1 to 8.");
            return "[Error] Invalid Save slot index!";
        }

        if (availableSaveFiles == null) 
        {
            availableSaveFiles = SaveFileReaderWriter.CheckAvailableSaveFiles(Application.persistentDataPath, savefileName);
        }

        if (availableSaveFiles != null) 
        {
            if (availableSaveFiles.Length <= 0) 
            {
                Debug.LogError("[Error] availableSaveFiles array not initialized!");
                return "[Error] availableSaveFiles array not initialized!";
            }
        }
        else 
        {
            Debug.LogError("[Error] availableSaveFiles array not initialized!");
            return "[Error] availableSaveFiles array not initialized!";
        }

        return availableSaveFiles[_saveSlotIndex - 1];
    }
}
