using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpButtonController : MonoBehaviour
{
    public TextMeshProUGUI buttonText;

    private PlayerController PC;
    private float selectedStat;
    private float increaseAmount;
    private string statName;
    
    void Start()
    {
        gameObject.SetActive(false);
        PC =  GameObject.Find("Player").GetComponent<PlayerController>();
    }
    
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
                increaseAmount = Random.Range(1, 51);
                buttonText.text = "Increase Attack Speed \n\n+" + increaseAmount + "%";
                statName = "Attack Speed";
                break;
            case 3:
                increaseAmount = Random.Range(50, 101);
                buttonText.text = "Increase Exp \n\n+" + increaseAmount;
                statName = "Exp Bonus";
                break;
            default:
                break;
        }
    }
    
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
                PC.increaseAttackSpeed((increaseAmount / 100) + 1);  ;
                break;
            case "Exp Bonus":
                PC.IncreaseExp((int)increaseAmount);
                break;
            default:
                break;
        }

        PC.flipCards();
    }
}
