using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class StatsCard : MonoBehaviour
{
    public GameObject selectedTarget;

    [Header("Components")]
    public RectTransform maskBar;
    public RectTransform backgroundBar;
    public Text targetName;
    public Text attackValue;
    public Text armorValue;
    public Text scope;
    public Text magicAttackValue;
    public Text magicArmorValue;
    private MouseBehaviour mouse;

    private float startingHealth;

    private float maxWidth = 125f;
    private float newWidth;

    void Start()
    {
        mouse = GameObject.FindGameObjectWithTag("Player").GetComponent<MouseBehaviour>();
    }

    void Update()
    {

        if (mouse.selectedUnit != null || mouse.selectedUnits.Count > 0)
        {
            if (maskBar.gameObject.activeInHierarchy == false)
            {
                maskBar.gameObject.SetActive(true);
                backgroundBar.gameObject.SetActive(true);
            }
            selectedTarget = mouse.selectedUnit;
            if (mouse.selectedUnit == null) selectedTarget = mouse.selectedUnits[0];
            targetName.text = selectedTarget.gameObject.GetComponent<Characters>().characterName;
            attackValue.text = selectedTarget.gameObject.GetComponent<Characters>().attack.ToString();
            armorValue.text = selectedTarget.gameObject.GetComponent<Characters>().armor.ToString();
            scope.text = selectedTarget.gameObject.GetComponent<Characters>().scope.ToString();
            magicAttackValue.text = selectedTarget.gameObject.GetComponent<Characters>().magicAttack.ToString();
            magicArmorValue.text = selectedTarget.gameObject.GetComponent<Characters>().magicArmor.ToString();
            startingHealth = selectedTarget.gameObject.GetComponent<Characters>().startingHitPoints;
            UpdateBar(selectedTarget.gameObject.GetComponent<Characters>().hitPoints);
        }
        else
        {
            maskBar.gameObject.SetActive(false);
            backgroundBar.gameObject.SetActive(false);
            targetName.text = ("");
            attackValue.text = ("");
            armorValue.text = ("");
            scope.text = ("");
            magicAttackValue.text = ("");
            magicArmorValue.text = ("");
        }
    }

    public void UpdateBar(float newLife)
    {
        float newWidth = (maxWidth * newLife) / startingHealth;
        maskBar.sizeDelta = new Vector2(newWidth, maskBar.sizeDelta.y);
    }
}
