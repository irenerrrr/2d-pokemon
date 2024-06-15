using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BeastPanel : MonoBehaviour
{
    public TextMeshProUGUI beastName;
    public TextMeshProUGUI beastLevel;
    public TextMeshProUGUI beastGender;
    public Image beastImage;

    public TextMeshProUGUI beastIntimacy;
    public TextMeshProUGUI beastHP;
    public TextMeshProUGUI beastAttack;
    public TextMeshProUGUI beastArmor;
    public TextMeshProUGUI beastAP;
    public TextMeshProUGUI beastMR;
    public TextMeshProUGUI beastSpeed;


    public void UpdateBeastInfo(SpiritualBeast beast)
    {
        beastImage.sprite = beast.image;
        beastName.text = beast.name;
        beastLevel.text = "Level: " + beast.level;
        beastGender.text = beast.gender;
        
        beastIntimacy.text = "Intimacy: " + beast.intimacy.ToString();
        beastHP.text = "HP: " + beast.hp.ToString();
        beastAttack.text = "Attack: " + beast.attack.ToString();
        beastArmor.text = "Armor: " + beast.armor.ToString();
        beastAP.text = "AP: " + beast.ap.ToString();
        beastMR.text = "MR: " + beast.mr.ToString();
        beastSpeed.text = "Speed: " + beast.speed.ToString();
    }
}
