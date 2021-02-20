/*  Author: Tyler McMillan
 *  Date Created: February 3, 2021
 *  Last Updated: February 17, 2021
 *  Description: This script is used for managing everything related to the players inventory. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    //Slider for inventory size, can be between 4 & 8 slots
    [Header ("Invetory Settings"), Range(4.0f, 8.0f)]
    [SerializeField] private int inventorySize = 8;
    
    private List<string> playerItems = new List<string>(); //players inventory items 
    [SerializeField] GameObject[] inventoryButtons; //Reference to the buttons in the inventory

    [SerializeField] Sprite[] inventorySprites; //Reference to whatever images you want for the items / inventory blank. 
    private int inventorySlotTemp = 0;
    private int playerSeeds = 0; //how many seeds the player has collected in the level
    [Header ("Seed Settings"), Space]
    [SerializeField] private Text playerSeedTxt; //reference to the player seed textbox
    private int totalSeeds = 0; //how many seed total in the entire level
    [SerializeField] private Text totalSeedTxt; //reference to the total seed textbox
    [SerializeField] private Animator seedTextAnimator; //reference to the animator to make seed count appear and disappear
    private bool showSeedText = false; //are you showing the seed count
    [SerializeField] private float seedShowTime = 3f; //How long to show the seed count for (seconds)
    private float scount = 0f; //Counter just to hold the amount of time elapsed since opened

    //---------------------- Unity Functions ------------------------------
    private void Awake(){
        //count how many seeds / total points the player has collected
        FindTotalSeeds();
    }
    void FixedUpdate(){
        //Counter to close players seeds collected when opened
        scount += Time.deltaTime;
        if(showSeedText){
            if(scount > seedShowTime){
                showSeedText = false;
                seedTextAnimator.SetBool("ShowCount", false);
            }
        }
        
    }

    //------------------- Inventory Functions ---------------------------------
    public void OpenInvetory() //Button calls this function to open the inventory
    {
        gameObject.transform.SetAsLastSibling(); //Makes sure the inventory is in front of all other UI.
        gameObject.GetComponent<Animator>().SetInteger("InventorySize", inventorySize); //changes inventory size in animator so it knows what inventory to animate.
        gameObject.GetComponent<Animator>().SetBool("OpenInventory", true); //Change bool in animator to true so it opens
        seedTextAnimator.SetBool("ShowCount", true);
    }
     public void CloseInvetory() //Middle button in inventory calls this funtion to close the inventory
    {
        
        gameObject.GetComponent<Animator>().SetBool("OpenInventory", false); //Change bool in animator to false so it closes
        seedTextAnimator.SetBool("ShowCount", false);
        showSeedText = false;
    }

    public bool CollectItem(string _name){ //Called when player enters trigger box of item

        if(playerItems.Count < inventorySize){ //check if theres room in inventory
            switch (_name){ //Check item name is existent
                case "seed":  //For example if seed is the items string add it to the players item list
                    AddItemToList(_name);
                    break;
                default:
                    Debug.Log("No string on item or non-existent"); //Error text
                    return false;//tells item that it wasnt collected
            }
            DisplayInventory(); //Display inventory / update images
            return true; //tells item that it was collected
        }else{
            Debug.Log("No room in inventory"); //Error text
            return false;//tells item that it wasnt collected
        }
    }
    private void AddItemToList(string _name){
        for(int item = 0; item < playerItems.Count; item ++){
            if(playerItems[item] == null || playerItems[item] == ""){ //Check if item is empty or null so it can replace that space.
                playerItems[item] = _name;
                return;
            }
        }
            playerItems.Add(_name); //Add item to players item list
    }
    private void DisplayInventory(){ //Called when an item is picked up
         for(int i = 0; i < playerItems.Count; i++){ //Cycle through collected items
             switch(playerItems[i]){ //check strings of items
                 case "seed":  //example if item name is seed display image on button
                    inventoryButtons[i].GetComponent<Image>().sprite = inventorySprites[1]; //Set inventory button image to seed sprite image
                    break;
                default:
                    Debug.Log("No picture for item set up"); //error text 
                    break;
             }
         }
    }
    public void InventoryButtonClick(Button button){ //Function triggered when the player clicks an inventory slot button
        string itemClicked = ""; //temp string to hold item thats in slot that was clicked

        for(int i = 0; i < inventorySize; i++){ //cycle through all slots to check which button was clicked (in terms of 1-8)
            if(button.name == inventoryButtons[i].name){ //check names to find match
                if(playerItems.Count > i){ //check if item even exists
                    itemClicked = playerItems[i]; //set temp string to hold item selected
                    playerItems[i] = "";
                    inventorySlotTemp = i;
                    Invoke("ClearInventorySlot", 0.8f); //wait 1 second before changing button back to nothing so it doesnt show in game
                }
                break;
            }
        }
        switch(itemClicked){ //Do whatever function that item would do
            case "seed":  //for example if item was seed, it would then do its function here. 
                    Debug.Log("seed clicked");
                    
                    break;
                default:
                    Debug.Log("No item in that slot"); //error message/tells user slot is empty
                    break;
        }
        CloseInvetory(); //Close inventory when an item slot is clicked
        
        

    }
    private void ClearInventorySlot(){
        inventoryButtons[inventorySlotTemp].GetComponent<Image>().sprite = inventorySprites[0]; //set button back to default image
    }
    /*
    - checks how many items in slots
    - checks if it already has that item
    - adds to first slot with string
    - clicking that item does a certain function. / some how get whatever button you press and get its name so you can match its name to an inventory slot or something. 
    */

    //----------------------SEED FUNCTIONS -----------------------
    public void CollectSeed(int _amount){
        //Gets called from "seed script" when player steps in a seeds trigger box, it gets sent that seeds value
        //that then is added to the players seed count, displayed, and resets counter to 0 for when the text disappears.
        playerSeeds += _amount;
        playerSeedTxt.text = playerSeeds.ToString();
        seedTextAnimator.SetBool("ShowCount", true);
        showSeedText = true;
        scount = 0f;
        FindObjectOfType<SoundManager>().Play("collect");  //added by salick
    }
    private void FindTotalSeeds(){
        //Find all seeds in level by finding all "seed" tagged objects and adding up their total worth.
        foreach(GameObject _seed in GameObject.FindGameObjectsWithTag("Seed")){
            totalSeeds += _seed.GetComponent<SeedScript>().seedWorth;
        }
        totalSeedTxt.text = totalSeeds.ToString();
    }

    
}

