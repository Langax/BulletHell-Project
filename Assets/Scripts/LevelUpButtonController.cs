using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpButtonController : MonoBehaviour
{
    /* Variable declaration */
    public TextMeshProUGUI buttonText;
    private PlayerController PC;
    private float selectedStat;
    private float increaseAmount;
    private string statName;
    
    /* Initialize default values */
    void Start()
    {
        gameObject.SetActive(false);
        PC =  GameObject.Find("Player").GetComponent<PlayerController>();
    }
    
    /* if it's active already then deactivate, otherwise activate and randomize the options */
    public void flip()
    {
        
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            randomizeOptions();
        }
    }

    /* Picks a random stat to increase and rolls a random value for it, then sets the text for that card appropriately */
    void randomizeOptions()
    {
        int randOption = Random.Range(0, 4);

        switch (randOption)
        {
            case 0:
                increaseAmount = Random.Range(1, 51);
                buttonText.text = "Increase Max Health \n\n+" + increaseAmount;
                statName = "Max Health";
                break;
            case 1:
                increaseAmount = Random.Range(1, 51);
                buttonText.text = "Increase Attack Range \n\n+" + increaseAmount + "%";
                statName = "Attack Range";
                break;
            case 2:
                increaseAmount = Random.Range(70, 100);
                buttonText.text = "Decrease Attack Cooldown \n\n-" + (100 - increaseAmount) + "%";
                statName = "Attack Speed";
                break;
            case 3:
                increaseAmount = Random.Range(5, 26);
                buttonText.text = "Increase Exp bonus \n\n+" + increaseAmount + "%";
                statName = "Exp Bonus";
                break;
            default:
                break;
        }
    }
    
    /* Depending on which stat was picked from randomizeOptions(), call the correct function from the PlayerController, then flip the cards again (this is called on button press) */
    public void levelUpOption1()
    {
        switch (statName)
        {
            case "Max Health":
                PC.increaseMaxHP((int)increaseAmount);
                break;
            case "Attack Range":
                PC.increaseAttackRange((increaseAmount / 100) + 1);
                break;
            case "Attack Speed":
                PC.increaseAttackSpeed(increaseAmount / 100);
                break;
            case "Exp Bonus":
                PC.increaseExpBonus(increaseAmount / 100);
                break;
            default:
                break;
        }

        PC.flipCards();
    }
}
