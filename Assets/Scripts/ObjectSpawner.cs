using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private float spawnHeight = 10f;
    [SerializeField] private float spawnRangeX = 5f;
    [SerializeField] private float spawnRangeZ = 5f;

    private int objectCount = 0;

    public void SpawnObject()
    {
        // ランダムな位置を計算
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        float randomZ = Random.Range(-spawnRangeZ, spawnRangeZ);
        Vector3 spawnPosition = new Vector3(randomX, spawnHeight, randomZ);

        // Sphereを生成
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = spawnPosition;

        // Rigidbodyを追加
        Rigidbody rb = sphere.AddComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // カウントを増やす
        objectCount++;

        // 5の倍数チェック
        if (objectCount % 5 == 0)
        {
            OnMultipleOfFive();
        }

        // 10秒後に削除
        Destroy(sphere, 10f);
    }

    /// <summary>
    /// カウントが5の倍数になった時の処理
    /// </summary>
    private void OnMultipleOfFive()
    {
        // ここでAndroidに通知する（後で実装）
        // 例: AndroidBridge経由で通知
    }

    /// <summary>
    /// 現在のカウントを取得
    /// </summary>
    public int GetCount()
    {
        return objectCount;
    }

    /// <summary>
    /// カウントをリセット
    /// </summary>
    public void ResetCount()
    {
        objectCount = 0;
    }

    // テスト用: Spaceキーで落下テスト
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnObject();
        }
    }

    // デバッグ用: スポーン範囲を可視化
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = transform.position + Vector3.up * spawnHeight;
        Gizmos.DrawWireCube(center, new Vector3(spawnRangeX * 2, 0.1f, spawnRangeZ * 2));
    }
}
