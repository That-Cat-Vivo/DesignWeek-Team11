using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //private static vari to refer to this GameManager (foreward GM)
    //Static = there is only ever one instance that does not change.
    private static GameManager _instance;

    public static GameManager Instance
    {
        //Get = Keyword that returns val.
        //Here we are returning to the _instance vari
        //We don't want _instance to be changed, but we still want to access its info. 
        //Instance allows us to access this info from anywhere else without changing the script.
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            //If the GM does not exist, this is the GM.
            //If a GM already exists, delete this object.
            //DontDestroyOnLoad makes an object persistent across scenes.
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Array of Materials
    public Material[] colourOptions;

    //Vari for the renderer on the car
    //Renderer is the category that includes materials, color, shader type, etc.
    private Renderer carRenderer;

    public TMP_Text lapText, speedText;

    public bool checkpointPassed;
    public int currentLap = 0;

    private CarController carRef;

    // Start is called before the first frame update
    void Start()
    {
        lapText.text = "Lap: 0";

        //The Renderer is on the body object of the car
        //Find the Body GameObject, and get its Renderer
        carRenderer = GameObject.Find("Body").GetComponent<Renderer>();

        //Set a default material.
        carRenderer.material = colourOptions[0];

        //Locate GameObject called PlayerCar and component "carController"
        carRef = GameObject.Find("PlayerCar").GetComponent<CarController>();
    }

    //Public func that will be accessed from the CarController.
    //Has an int parameter to store the index of the dropdown.
    //Passes that int to the colourOptions array.
    public void PaintShop(int selectedColour)
    {
        carRenderer.material = colourOptions[selectedColour];
    }

    // Update is called once per frame
    void Update()
    {
        //Access currentSpeed of car and display it
            //F0 means no decimals
        speedText.text = carRef.currentSpeed.ToString("F0") + "km/h";
    }

    public void Lap()
    {
        if (!checkpointPassed && currentLap == 0)
        {
            currentLap = 1;
        }

        //If the player is currentlyin a lap and gotten a checkpoint increase the laps!
        if (currentLap > 0 && checkpointPassed)
        {
            currentLap++;
            checkpointPassed = false;
        }
        //Update the lap text
        lapText.text = "Lap: " + currentLap.ToString();
    }
}
