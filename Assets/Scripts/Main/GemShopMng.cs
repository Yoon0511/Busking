using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class GemShopMng : MonoBehaviour, IStoreListener
{

    [SerializeField]
    GameObject _JemShopPopup;
    [SerializeField]
    DataSaveMng _DataSaveMng;

    [SerializeField]
    GameObject _Infinity_Fast_2_Gray;
    [SerializeField]
    GameObject _Infinity_Fast_3_Gray;

    [SerializeField]
    GameObject _Fast_2_BuyPopup;
    [SerializeField]
    Animator _Fast_2_BuyPopup_Ani;

    [SerializeField]
    GameObject _ErrorPopup;
    [SerializeField]
    Animator _ErrorPopupAni;
    [SerializeField]
    UILabel _ErrorLog;

    private static IStoreController storeController;
    private static IExtensionProvider extensionProvider;

    #region 상품ID
    // 상품ID는 구글 개발자 콘솔에 등록한 상품ID와 동일하게 해주세요.
    public const string product_fast_3 = "infinity_fast_3";
    public const string product_gem1 = "gem1";
    public const string product_gem2 = "gem2";
    public const string product_gem3 = "gem3";
    public const string product_gem4 = "gem4";
    #endregion

    void Start()
    {
        InitializePurchasing();
    }
    void Update()
    {
        if (StaticMng.Instance._Infinity_FastValue >= 2)
            _Infinity_Fast_2_Gray.SetActive(true);
        else
            _Infinity_Fast_2_Gray.SetActive(false);

        if (StaticMng.Instance._Infinity_FastValue == 3)
            _Infinity_Fast_3_Gray.SetActive(true);
        else
            _Infinity_Fast_3_Gray.SetActive(false);
    }

    public void OpenFast2BuyPopup()
    {
        _Fast_2_BuyPopup.SetActive(true);
        _Fast_2_BuyPopup_Ani.SetTrigger("open");
    }
    public void CloseFast2BuyPopup()
    {
        _Fast_2_BuyPopup.SetActive(false);
    }
    public void BuyFast2()
    {
        if(StaticMng.Instance._Gem>=100)
        {
            StaticMng.Instance._Gem -= 100;
            StaticMng.Instance._Infinity_FastValue = 2;
            _Fast_2_BuyPopup.SetActive(false);
            ExportError("가속(2배)를\r\n구매하였습니다");
        }
        else
        {
            _Fast_2_BuyPopup.SetActive(false);
            ExportError("젬이 부족합니다");
        }
        _DataSaveMng.WantDataSave();
    }
    void ExportError(string log)
    {
        _ErrorPopup.SetActive(true);
        _ErrorPopupAni.SetTrigger("open");
        _ErrorLog.text = log;
        //Debug.Log(log);
    }


    private bool IsInitialized()
    {
        return (storeController != null && extensionProvider != null);
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
            return;

        var module = StandardPurchasingModule.Instance();

        ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);

        builder.AddProduct(product_gem1, ProductType.Consumable, new IDs
        {
            { product_gem1, AppleAppStore.Name },
            { product_gem1, GooglePlay.Name },
        });
        builder.AddProduct(product_gem2, ProductType.Consumable, new IDs
        {
            { product_gem2, AppleAppStore.Name },
            { product_gem2, GooglePlay.Name },
        });
        builder.AddProduct(product_gem3, ProductType.Consumable, new IDs
        {
            { product_gem3, AppleAppStore.Name },
            { product_gem3, GooglePlay.Name },
        });
        builder.AddProduct(product_gem4, ProductType.Consumable, new IDs
        {
            { product_gem4, AppleAppStore.Name },
            { product_gem4, GooglePlay.Name },
        });

        builder.AddProduct(product_fast_3, ProductType.Consumable, new IDs
        {
            { product_fast_3, AppleAppStore.Name },
            { product_fast_3, GooglePlay.Name }, }
        );
        //
        //builder.AddProduct(productId3, ProductType.Consumable, new IDs
        //{
        //    { productId3, AppleAppStore.Name },
        //    { productId3, GooglePlay.Name },
        //});
        //
        //builder.AddProduct(productId4, ProductType.Consumable, new IDs
        //{
        //    { productId4, AppleAppStore.Name },
        //    { productId4, GooglePlay.Name },
        //});
        //
        //builder.AddProduct(productId5, ProductType.Consumable, new IDs
        //{
        //    { productId5, AppleAppStore.Name },
        //    { productId5, GooglePlay.Name },
        //});

        UnityPurchasing.Initialize(this, builder);
    }

    public void BuyProductID(string productId)
    {
        try
        {
            if (IsInitialized())
            {
                Product p = storeController.products.WithID(productId);

                if (p != null && p.availableToPurchase)
                {
                    Debug.Log(string.Format("Purchasing product asychronously: '{0}'", p.definition.id));
                    storeController.InitiatePurchase(p);
                }
                else
                {
                    Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            else
            {
                Debug.Log("BuyProductID FAIL. Not initialized.");
            }
        }
        catch (Exception e)
        {
            Debug.Log("BuyProductID: FAIL. Exception during purchase. " + e);
        }
    }

    public void RestorePurchase()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            var apple = extensionProvider.GetExtension<IAppleExtensions>();

            apple.RestoreTransactions
                (
                    (result) => { Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore."); }
                );
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController sc, IExtensionProvider ep)
    {
        Debug.Log("OnInitialized : PASS");

        storeController = sc;
        extensionProvider = ep;
    }

    public void OnInitializeFailed(InitializationFailureReason reason)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + reason);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

        switch (args.purchasedProduct.definition.id)
        {
            case product_gem1:
                
                StaticMng.Instance._Gem += 10;
                _DataSaveMng.WantDataSave();
                break;
            case product_gem2:

                StaticMng.Instance._Gem += 50;
                _DataSaveMng.WantDataSave();
                break;
            case product_gem3:

                StaticMng.Instance._Gem += 100;
                _DataSaveMng.WantDataSave();
                break;
            case product_gem4:

                StaticMng.Instance._Gem += 500;
                _DataSaveMng.WantDataSave();
                break;

            case product_fast_3:
                StaticMng.Instance._Infinity_FastValue = 3;
                _DataSaveMng.WantDataSave();
                break;
                
                //case productId3:
                //
                //    // ex) gem 100개 지급
                //
                //    break;
                //
                //case productId4:
                //
                //    // ex) gem 300개 지급
                //
                //    break;
                //
                //case productId5:
                //
                //    // ex) gem 500개 지급
                //
                //    break;
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }




    public void OpenJepShopPopup()
    {
        _JemShopPopup.SetActive(true);
    }
    public void CloseJepShopPopup()
    {
        _JemShopPopup.SetActive(false);
    }
}