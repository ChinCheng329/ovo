using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Follower : MonoBehaviour
{
    public float 探測範圍 = 10f; // 探測敵人的距離
    public string 敵人Tag = "Enemy"; // 敵人的 Tag
    private NavMeshAgent 導航;
    private Transform 目標;
    private Transform 原目標; // 保存初始目標（玩家）

    void Start()
    {
        導航 = GetComponent<NavMeshAgent>();
        原目標 = GameObject.FindGameObjectWithTag("Player").transform;
        目標 = 原目標; // 預設目標為玩家
    }

    void Update()
    {
        // 如果當前目標是敵人，但敵人已被摧毀，恢復玩家為目標
        if (目標 != null && !目標.gameObject.activeInHierarchy && 目標.CompareTag(敵人Tag))
        {
            目標 = 原目標;
        }

        // 探測周圍的敵人
        Collider[] 範圍內物件 = Physics.OverlapSphere(transform.position, 探測範圍);
        foreach (var 物件 in 範圍內物件)
        {
            if (物件.CompareTag(敵人Tag))
            {
                目標 = 物件.transform; // 將敵人設置為目標
                break;
            }
        }

        // 如果目標超出範圍，恢復初始目標
        if (目標 == null || Vector3.Distance(transform.position, 目標.position) > 探測範圍)
        {
            目標 = 原目標;
        }

        // 移動到目標位置
        if (目標 != null)
        {
            導航.SetDestination(目標.position);
        }

        // 檢查敵人數量並判斷是否獲勝
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        GameObject[] 所有敵人 = GameObject.FindGameObjectsWithTag(敵人Tag);
        if (所有敵人.Length == 0)
        {
            Debug.Log("恭喜！你贏了遊戲！");
            GameWin();
        }
    }

    private void GameWin()
    {
        // 實現遊戲勝利邏輯，例如跳轉到勝利場景、顯示勝利畫面等
        Debug.Log("遊戲勝利！");
        // Example: UnityEngine.SceneManagement.SceneManager.LoadScene("WinScene");
    }

    private void OnDrawGizmosSelected()
    {
        // 繪製探測範圍（僅在選中物件時顯示）
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 探測範圍);
    }
}
