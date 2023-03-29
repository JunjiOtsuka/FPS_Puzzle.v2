using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour
{
    public static float Coins = 100.0f; //default coins in pocket

    public GameObject gunTowerPrefab;
    public GameObject iceTowerPrefab;
    public GameObject poisonTowerPrefab;

    public static InputAction leftClick;

    public GameObject indicator;
    public bool bIndicator;

    public Text coinText;

    // Start is called before the first frame update
    void Start()
    {
        leftClick = InputManager.inputActions.Player.Fire1;
    }

    // Update is called once per frame
    void Update()
    {
        //update coin text
        coinText.text = "Coins: " + Coins.ToString();

        if (!bIndicator) return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            //shows where the indicator is
            indicator.transform.position = new Vector3(hit.point.x, 4.0f, hit.point.z);

            //when player clicks while indicator is visible
            if (Input.GetMouseButtonDown(0) && bIndicator)
            {
                SpawnTower(indicator, new Vector3(hit.point.x, 4.0f, hit.point.z), Quaternion.identity);
            }
            if (Input.GetMouseButtonDown(1))
            {
                indicator.gameObject.SetActive(false);
                bIndicator = false;
            }
        }

    }

    public void TowerIndicator()
    {
        if (bIndicator) return;
        indicator = Instantiate(gunTowerPrefab);
        bIndicator = true;
    }

    public void SpawnTower(GameObject towerPrefab, Vector3 position, Quaternion rotation)
    {
        var tower = towerPrefab.GetComponent<Tower>();
        if (Coins < tower.cost)
        {
            Debug.Log("You need " + tower.cost + " coins to spawn a tower");
            return;
        }
        Instantiate(towerPrefab, position, rotation);
        tower.PayTowerCost();
    }
}
