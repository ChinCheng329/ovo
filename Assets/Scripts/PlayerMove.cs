using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 引入UI相關的命名空間

public class PlayerMove : MonoBehaviour
{
    public float speed = 3.0f;
    public GameObject 小兵;
    Text 顯示目前數量文字;
    public Text 結果文字; // 用於顯示勝利或失敗的文字
    private bool 遊戲結束 = false; // 遊戲是否結束

    private void Start()
    {
        顯示目前數量文字 = GameObject.Find("/Canvas/Text").GetComponent<Text>();
        更新顯示文字();
    }

    void Update()
    {
        if (遊戲結束) return; // 遊戲結束後不再進行任何操作

        // 玩家移動邏輯
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.back * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        }

        更新顯示文字();

        // 檢查是否勝利
        CheckWinCondition();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "資源")
        {
            string 字首 = other.name.Substring(0, 1); // 操作符號
            string 數值字串 = other.name.Substring(1); // 數值部分

            if (!int.TryParse(數值字串, out int 數值))
            {
                Debug.LogWarning("無法解析數值，請檢查資源名稱格式！");
                return;
            }

            GameObject[] 所有小兵 = GameObject.FindGameObjectsWithTag("小兵");
            int 目前小兵數量 = 所有小兵.Length;

            Debug.Log($"操作符號: {字首}, 數值: {數值}, 目前士兵數量: {目前小兵數量}");

            // 處理操作邏輯
            switch (字首)
            {
                case "+":
                    生成小兵(數值);
                    break;

                case "-":
                    if (數值 > 目前小兵數量) 數值 = 目前小兵數量;
                    銷毀小兵(所有小兵, 數值);
                    break;

                case "x":
                    if (目前小兵數量 == 0) 目前小兵數量 = 1;
                    int 倍增數量 = (目前小兵數量 * 數值) - 目前小兵數量;
                    生成小兵(倍增數量);
                    break;

                case "/":
                    if (數值 > 0 && 目前小兵數量 > 0)
                    {
                        int 保留數量 = Mathf.FloorToInt((float)目前小兵數量 / 數值);
                        int 移除數量 = 目前小兵數量 - 保留數量;

                        Debug.Log($"除法操作: 保留 {保留數量}，移除 {移除數量}");

                        銷毀小兵(所有小兵, 移除數量);
                    }
                    else
                    {
                        Debug.LogWarning("無法執行除法操作，因為數值或士兵數量不足！");
                    }
                    break;

                default:
                    Debug.LogWarning($"未知操作符號: {字首}");
                    break;
            }

            更新顯示文字();
            Destroy(other.gameObject); // 銷毀資源物體
        }
    }

    // 方法: 生成士兵
    private void 生成小兵(int 數量)
    {
        for (int i = 0; i < 數量; i++)
        {
            Instantiate(小兵, this.transform.position, Quaternion.identity);
        }
        Debug.Log($"新增士兵數量: {數量}");
    }

    // 方法: 銷毀士兵
    private void 銷毀小兵(GameObject[] 小兵陣列, int 數量)
    {
        for (int i = 0; i < 數量; i++)
        {
            if (i < 小兵陣列.Length)
            {
                Destroy(小兵陣列[i]);
            }
        }
        Debug.Log($"移除士兵數量: {數量}");
    }

    // 方法: 更新顯示文字
    private void 更新顯示文字()
    {
        int 目前小兵數量 = GameObject.FindGameObjectsWithTag("小兵").Length;
        顯示目前數量文字.text = $"場上士兵數量: {目前小兵數量}";
    }

    // 方法: 檢查是否勝利
    private void CheckWinCondition()
    {
        GameObject[] 所有敵人 = GameObject.FindGameObjectsWithTag("Enemy");
        if (所有敵人.Length == 0 && !遊戲結束) // 如果敵人數量為零且遊戲未結束
        {
            遊戲結束 = true; // 遊戲結束
            Time.timeScale = 0; // 停止遊戲時間
            結果文字.text = "贏遊戲"; // 顯示勝利文本
        }
    }
}
