using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;

using Hyperledger.Indy.WalletApi;
using Hyperledger.Indy.DidApi;
using Hyperledger.Indy.PoolApi;
using Hyperledger.Indy.CryptoApi;
using System.Text;
using System.Threading.Tasks;

public class IndyTest : MonoBehaviour
{
    private string wallet_config;
    private string wallet_credentials;
    private string wallet_name;
    private string genesis_file_path = null;
    private string test_url;
    private string pool_name;
    private string pool_config;
    private Wallet wallet_handle = null;
    private CreateAndStoreMyDidResult did = null;
    private Pool pool_handle = null;
    public Text text;


    public void StartIndy()
    {
        System.Random random = new System.Random();
        int randomNumber = random.Next(10000, 99999);
        wallet_name = "wallet" + randomNumber.ToString();
        pool_name = "pool" + randomNumber.ToString();

        test_url = "http://220.68.5.139:9000/genesis";
        wallet_config = "{\"id\":\"" + wallet_name + "\"}";
        wallet_credentials = "{\"key\":\"wallet_key\"}";
        genesis_file_path = Application.dataPath + "/genesis.txn";
        pool_config = "{\"genesis_txn\":\"" + genesis_file_path + "\"}";       
        HttpClient httpClient = HttpClient.GetInstance();
        string genesis_file_ = httpClient.CreateGenesisFile(genesis_file_path, test_url);
        Debug.Log("genesis_file_: " + genesis_file_);


        IndyWalletApiTestFun();

        IndyPoolApiTestFun();
    }

    public void StopIndy()
    {
        pool_handle.CloseAsync().Wait();
        Debug.Log("Indy Close Pool Ledger");

        Pool.DeletePoolLedgerConfigAsync(pool_name).Wait();
        Debug.Log("Indy Delete Pool Ledger Config");

        wallet_handle.CloseAsync().Wait();
        Debug.Log("Indy Close Wallet");

        Wallet.DeleteWalletAsync(wallet_config, wallet_credentials).Wait();
        Debug.Log("Indy Delete Wallet");
 
    }


    public void IndyWalletApiTestFun()
    {

        //string wallet_config = "{\"id\":\"" + wallet_name + "\", \"storage_type\": {\"path\": \""
        //+ Application.dataPath + "/indy/wallet\"}}";
        //string wallet_credentials = "{\"key\":\"wallet_key\"}";

        Debug.Log("Wallet Config: " + wallet_config);
        Debug.Log("Wallet Credentials: " + wallet_credentials);

        
        
        string did_list = null;

        
         Debug.Log("Indy Create Wallet");
         Wallet.CreateWalletAsync(wallet_config, wallet_credentials).Wait();

         Debug.Log("Indy Open Wallet");
         wallet_handle = Wallet.OpenWalletAsync(wallet_config, wallet_credentials).Result;
         Debug.Log("Wallet Handle: " + wallet_handle.ToString());

         Debug.Log("Indy Create DID");
         string did_json = "{\"seed\":\"test0000000000000000000000000000\"}";
         did = Did.CreateAndStoreMyDidAsync(wallet_handle, did_json).Result;
         Debug.Log("DID: " + did);

         Debug.Log("Indy List DID");
         did_list = Did.ListMyDidsWithMetaAsync(wallet_handle).Result;
         Debug.Log("DID List: " + did_list);    
        
    }

    public void IndyPoolApiTestFun()
    {
        if (false == File.Exists(genesis_file_path))
        {
            Debug.Log("Genesis File is Null");
            return;
        }

        
         Debug.Log("Pool Config: " + pool_config);
        
         Debug.Log("Indy Create Pool Ledger Config");
         Pool.CreatePoolLedgerConfigAsync(pool_name, pool_config).Wait();

         Debug.Log("Indy Open Pool Ledger");
         pool_handle = Pool.OpenPoolLedgerAsync(pool_name, pool_config).Result;
         Debug.Log("Pool Handle: " + pool_handle.ToString());

    }

    public string PackMessage(string message, string targetDid)
    {
        Debug.Log("PackMessage");
        byte[] packedMessage = Crypto.PackMessageAsync(wallet_handle, did.VerKey, targetDid,
            System.Text.Encoding.UTF8.GetBytes(message)).Result;
        Debug.Log("packedMessage: " + packedMessage.ToString());


        return Encoding.UTF8.GetString(packedMessage);
    }

    public async Task<string> SignDataAsync(string data)
    {

        try
        {
            // �����͸� ����Ʈ �迭�� ��ȯ
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            // �����Ϳ� ���� ���� ����
            byte[] signature = await Crypto.SignAsync(wallet_handle, did.VerKey, dataBytes);

            // ����Ʈ �迭�� �� ������ Base64 ���ڿ��� ��ȯ�Ͽ� ��ȯ
            return Convert.ToBase64String(signature);
        }
        catch (Exception ex)
        {
           Debug.Log(ex.Message);
            
            return null;
        }
    }

    public bool VerifySignature(string signedMessage, string message)
    {
        try
        {
            // Base64 ���ڿ��� �� ������ ����Ʈ �迭�� ��ȯ
            byte[] signature = Convert.FromBase64String(signedMessage);

            // �޽����� ����Ʈ �迭�� ��ȯ
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            // ���� ����
            bool isValid = Crypto.VerifyAsync(did.VerKey, messageBytes, signature).Result;

            return isValid;
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            return false;
        }
    }
}