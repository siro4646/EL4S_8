using System;
using UnityEngine;

/// <summary>
/// Player側に付ける「隠すシステム」
/// ・同じ仕組みを最大3回まで実行可能
/// ・3回すべて使い切ったら IsAllHidden が true
/// ・オブジェクト固有のコンポーネントには依存しない
/// </summary>
public sealed class Hide : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int maxHideCount = 3;

    [Header("Effect")]
    [SerializeField] private ParticleSystem hideEffectPrefab;

    private int currentHideCount = 0;

    /// <summary>
    /// 3回すべて隠し切ったかどうか
    /// </summary>
    public bool IsAllHidden { get; private set; } = false;

    /// <summary>
    /// 残り回数（UI表示など用）
    /// </summary>
    public int RemainingHideCount => Mathf.Max(0, maxHideCount - currentHideCount);

    /// <summary>
    /// 当たり判定OK & 決定入力時に呼ぶ。
    /// 成功したら true。
    /// </summary>
    public bool TryHide(GameObject target)
    {
        // すでに隠し切っている
        if (IsAllHidden)
        {
            Debug.Log("すでにすべて隠し切っています。");
            return false;
        }

        // 残り回数なし
        if (currentHideCount >= maxHideCount)
        {
            Debug.Log("隠す回数が残っていません。");
            return false;
        }

        if (target == null)
            return false;

        // 1) オブジェクト非表示
        // TODO: 後でオブジェクトの隠されているフラグをオンにする
        target.SetActive(false);

        // 2) エフェクト再生
        PlayEffectAt(target.transform.position);

        // 3) 回数消費
        currentHideCount++;

        // 4) 隠し切り判定
        if (currentHideCount >= maxHideCount)
        {
            IsAllHidden = true;
        }

        return true;
    }

    private void PlayEffectAt(Vector3 position)
    {
        if (hideEffectPrefab == null)
            return;

        var ps = Instantiate(hideEffectPrefab, position, Quaternion.identity);
        ps.Play();

        Destroy(
            ps.gameObject,
            ps.main.duration + ps.main.startLifetime.constantMax + 0.5f
        );
    }
}
