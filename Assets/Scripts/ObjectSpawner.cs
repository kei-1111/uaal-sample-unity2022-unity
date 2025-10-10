using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject objectPrefab; // 落とすオブジェクトのPrefab
    [SerializeField] private float spawnHeight = 10f; // 落下開始の高さ
    [SerializeField] private float spawnRangeX = 5f; // X軸のランダム範囲
    [SerializeField] private float spawnRangeZ = 5f; // Z軸のランダム範囲

    [Header("Counter Settings")]
    [SerializeField] private int objectCount = 0; // 落下したオブジェクトのカウント

    /// <summary>
    /// オブジェクトを生成して落下させる
    /// </summary>
    public void SpawnObject()
    {
        // ランダムな位置を計算
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        float randomZ = Random.Range(-spawnRangeZ, spawnRangeZ);
        Vector3 spawnPosition = new Vector3(randomX, spawnHeight, randomZ);

        // オブジェクトを生成（Prefabが設定されていない場合はプリミティブを作成）
        GameObject spawnedObject;
        if (objectPrefab != null)
        {
            spawnedObject = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);
        }
        else
        {
            spawnedObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            spawnedObject.transform.position = spawnPosition;
            spawnedObject.transform.localScale = Vector3.one * 0.5f;
            spawnedObject.name = $"Sphere_{objectCount + 1}";
        }

        // ランダムな色を設定
        var renderer = spawnedObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            // 新しいマテリアルインスタンスを作成してカラーを設定
            renderer.material = new Material(renderer.material);
            renderer.material.color = new Color(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f)
            );
        }

        // Rigidbodyがない場合は追加
        if (spawnedObject.GetComponent<Rigidbody>() == null)
        {
            var rb = spawnedObject.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        // カウントを増やす
        objectCount++;

        // 5の倍数チェック
        if (objectCount % 5 == 0)
        {
            OnMultipleOfFive();
        }

        // 一定時間後にオブジェクトを削除（メモリ節約）
        Destroy(spawnedObject, 10f);
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
