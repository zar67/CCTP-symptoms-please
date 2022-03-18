using Firebase.Database;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardTabContent : MonoBehaviour
{
    [SerializeField] private ScrollRect m_scrollView = default;
    [SerializeField] private RectTransform m_scrollViewContent = default;
    [SerializeField] private LeaderboardItem m_leaderboardItemPrefab = default;

    private void OnEnable()
    {
        foreach (Transform child in m_scrollViewContent)
        {
            Destroy(child.gameObject);
        }

        StartCoroutine(RefreshData());
    }

    private void SnapTo(RectTransform child)
    {
        if (child == null)
        {
            return;
        }

        Canvas.ForceUpdateCanvases();

        var contentPos = (Vector2)m_scrollView.transform.InverseTransformPoint(m_scrollViewContent.position);
        var childPos = (Vector2)m_scrollView.transform.InverseTransformPoint(child.position);
        var endPos = contentPos - childPos;

        if (!m_scrollView.horizontal)
        {
            endPos.x = contentPos.x + (m_scrollViewContent.rect.width / 2);
        }

        if (!m_scrollView.vertical)
        {
            endPos.y = contentPos.y + (m_scrollViewContent.rect.height / 2);
        }

        m_scrollViewContent.anchoredPosition = endPos;
    }

    private IEnumerator RefreshData()
    {
        var task = FirebaseDatabase.DefaultInstance.RootReference.Child(FirebaseDatabaseManager.USERS_REFERENCE).OrderByChild(FirebaseDatabaseManager.SCORE_REFERENCE).GetValueAsync();
        
        yield return new WaitUntil(predicate: () => task.IsCompleted);

        if (task.Exception == null)
        {
            DataSnapshot dataSnapshot = task.Result;
            RectTransform playerTransform = null;

            int count = 1;
            foreach (DataSnapshot childSnapshot in dataSnapshot.Children.Reverse())
            {
                bool onlineEnabled = (bool)childSnapshot.Child(FirebaseDatabaseManager.ONLINE_PERMISSION_REFERENCE).Value;

                if (onlineEnabled)
                {
                    string username = childSnapshot.Child(FirebaseDatabaseManager.USERNAME_REFERENCE).Value.ToString();
                    int score = Convert.ToInt32(childSnapshot.Child(FirebaseDatabaseManager.SCORE_REFERENCE).Value);

                    LeaderboardItem newItem = Instantiate(m_leaderboardItemPrefab, m_scrollViewContent);
                    newItem.SetPostitionText(count);
                    newItem.SetNameText(username);
                    newItem.SetScoreText(score);

                    if (username == GameData.PlayerName)
                    {
                        playerTransform = newItem.GetComponent<RectTransform>();
                    }

                    count++;
                }
            }

            SnapTo(playerTransform);
        }
        else
        {
            Debug.LogError("Cannot load leaderboard scores from database");
        }
    }
}