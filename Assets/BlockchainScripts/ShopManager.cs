using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using Thirdweb.Unity;
using TMPro;
using UnityEngine.UI;
using System.Numerics;
using System;
using System.Data;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Unity.Collections.LowLevel.Unsafe;

public class ShopManager : MonoBehaviour
{
    public string Address { get; private set; }
    public static BigInteger ChainId = 1001;

    public UnityEngine.UI.Button playButton;
    public UnityEngine.UI.Button coinButton;

    public TMP_Text goldBoughtText;
    public TMP_Text buyingStatusText;

    string receiverAddress = "0xA24d7ECD79B25CE6C66f1Db9e06b66Bd11632E00";

    int coinAmount = 100;

    int itemPrice = 1;

    private void HideAllButtons()
    {
        playButton.interactable = false;
        coinButton.interactable = false;
    }

    private void ShowAllButtons()
    {
        playButton.interactable = true;
        coinButton.interactable = true;
    }

    private void UpdateStatus(string messageShow)
    {
        buyingStatusText.text = messageShow;
        buyingStatusText.gameObject.SetActive(true);
    }

    private BigInteger ConvertToWei(decimal amount)
    {
        // 1 token = 10^18 Wei
        BigInteger wei = new BigInteger(amount * (decimal)Math.Pow(10, 18));
        return wei;
    }

    private void BoughtSuccessFully(int indexValue)
    {
        BlockchainEffect.Instance.coins += 100;
        goldBoughtText.text = "Coins Bought: " + BlockchainEffect.Instance.coins.ToString();
        UpdateStatus("Bought 100 Coins");
    }

    public async void BuyCoins(int indexValue)
    {
        HideAllButtons();
        UpdateStatus("Buying...");
        BigInteger weiAmount = ConvertToWei(itemPrice);
        var wallet = ThirdwebManager.Instance.GetActiveWallet();
        var balance = await wallet.GetBalance(chainId: ChainId);
        var balanceEth = Utils.ToEth(wei: balance.ToString(), decimalsToDisplay: 4, addCommas: true);
        Debug.Log("balanceEth1: " + balanceEth);
        if (float.Parse(balanceEth) < itemPrice)
        {
            UpdateStatus("Not Enough Token...");
            ShowAllButtons();
            return;
        }

        //Bắt đầu Coroutine
        StartCoroutine(WaitAndExecute(indexValue));
        try
        {
            await wallet.Transfer(ChainId, receiverAddress, weiAmount);
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred during the transfer: {ex.Message}");
        }
    }

    IEnumerator WaitAndExecute(int indexValue)
    {
        Debug.Log("Coroutine started, waiting for 3 seconds...");
        yield return new WaitForSeconds(3f); // Chờ 3 giây
        Debug.Log("3 seconds have passed!");
        BoughtSuccessFully(indexValue);
        ShowAllButtons();
    }

    public async void GetWalletBalance()
    {
        var wallet = ThirdwebManager.Instance.GetActiveWallet();
        var balance = await wallet.GetBalance(chainId: ChainId);
        var balanceEth = Utils.ToEth(wei: balance.ToString(), decimalsToDisplay: 4, addCommas: true);
        UpdateStatus("Wallet Balance: " + balanceEth + " KLAY");
    }

    public void ChangeToScenePlay()
    {
        SceneManager.LoadScene("main");
    }
}
