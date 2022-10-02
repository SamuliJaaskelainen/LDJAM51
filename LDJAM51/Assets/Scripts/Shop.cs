using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    public static bool shopOpen = false;

    bool p1skip = false;
    bool p2skip = false;

    [SerializeField] List<ShopCard> p1cards = new List<ShopCard>();
    [SerializeField] List<ShopCard> p2cards = new List<ShopCard>();
    [SerializeField] List<ShopCard> p1slots = new List<ShopCard>();
    [SerializeField] List<ShopCard> p2slots = new List<ShopCard>();
    [SerializeField] GameObject p1Shop;
    [SerializeField] GameObject p2Shop;
    [SerializeField] GameObject p1Slots;
    [SerializeField] GameObject p2Slots;
    [SerializeField] TextMeshPro p1Skip;
    [SerializeField] TextMeshPro p2Skip;

    [SerializeField] List<GameObject> guns = new List<GameObject>();

    int p1SelectedGunCard;
    int p2SelectedGunCard;

    int maxShopIndex = 3;

    void OnEnable()
    {
        Time.timeScale = 0.0f;
        shopOpen = true;
        maxShopIndex++;
        maxShopIndex = Mathf.Min(maxShopIndex, guns.Count);
        p1Slots.SetActive(false);
        p2Slots.SetActive(false);
        p1Skip.text = "SKIP";
        p2Skip.text = "SKIP";
        p1skip = false;
        if (ManyMouseCrosshair.player2mouseId < 0)
        {
            Skip("2");
        }
        ShowNewCards();
    }

    void OnDisable()
    {
        Time.timeScale = 1.0f;
        shopOpen = false;
    }

    public void Skip(string player)
    {
        Debug.Log("Skip " + player);
        if (player == "1")
        {
            p1skip = true;
            p1Shop.SetActive(false);
            p1Slots.SetActive(false);
            p1Skip.text = "DONE";
        }
        else if (player == "2")
        {
            p2skip = true;
            p2Shop.SetActive(false);
            p2Slots.SetActive(false);
            p2Skip.text = "DONE";
        }

        if (p1skip && p2skip)
        {
            gameObject.SetActive(false);
            // TODO: Continue camera run
        }
    }

    void ShowNewCards()
    {
        p1Shop.SetActive(true);
        p2Shop.SetActive(!p2skip);

        for (int i = 0; i < p1cards.Count; ++i)
        {
            int r = Random.Range(0, maxShopIndex);
            p1cards[i].gunMesh.mesh = guns[r].GetComponent<Gun>().shotMesh;
            p1cards[i].gunName.text = guns[r].name;
        }

        for (int i = 0; i < p2cards.Count; ++i)
        {
            int r = Random.Range(0, maxShopIndex);
            p2cards[i].gunMesh.mesh = guns[r].GetComponent<Gun>().shotMesh;
            p2cards[i].gunName.text = guns[r].name;
        }
    }

    void ShowCurrentGuns(int player)
    {
        ManyMouseCrosshair crosshair = ManyMouseHandler.Instance.GetCrosshairByPlayer(player);
        if (crosshair)
        {
            if (player == 1)
            {
                p1Shop.SetActive(false);
                p1Slots.SetActive(true);
                for (int i = 0; i < p1slots.Count; ++i)
                {
                    p1slots[i].gunMesh.mesh = crosshair.GetGun(i).shotMesh;
                    p1slots[i].gunName.text = crosshair.GetGun(i).name;
                }
            }
            else
            {
                p2Shop.SetActive(false);
                p2Slots.SetActive(true);
            }
        }
    }

    public void SelectGun(string cardId)
    {
        string[] split = cardId.Split('.');

        int playerNumber, cardNumber;
        if (int.TryParse(split[0], out playerNumber) && int.TryParse(split[1], out cardNumber))
        {
            if (playerNumber == 1)
            {
                p1SelectedGunCard = cardNumber;
            }
            else
            {
                p2SelectedGunCard = cardNumber;
            }
        }

        ShowCurrentGuns(playerNumber);
    }

    public void BuyReplacement(string cardId)
    {
        string[] split = cardId.Split('.');

        int playerNumber, slotNumber;
        if (int.TryParse(split[0], out playerNumber) && int.TryParse(split[1], out slotNumber))
        {
            if (ManyMouseHandler.Instance)
            {
                ManyMouseCrosshair crosshair = ManyMouseHandler.Instance.GetCrosshairByPlayer(playerNumber);
                if (crosshair)
                {
                    for (int i = 0; i < guns.Count; ++i)
                    {
                        if (playerNumber == 1)
                        {
                            if (guns[i].name == p1cards[p1SelectedGunCard].gunName.text)
                            {
                                crosshair.ChangeGun(slotNumber, guns[i]);
                            }
                            Skip("1");
                        }
                        else
                        {
                            if (guns[i].name == p2cards[p2SelectedGunCard].gunName.text)
                            {
                                crosshair.ChangeGun(slotNumber, guns[i]);
                            }
                            Skip("2");
                        }
                    }
                }
            }
        }
    }
}
