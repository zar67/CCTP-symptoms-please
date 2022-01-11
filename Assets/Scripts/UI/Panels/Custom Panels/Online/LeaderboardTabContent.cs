using Firebase.Database;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class LeaderboardTabContent : MonoBehaviour
{
    [SerializeField] private Transform m_scrollViewContent = default;
    [SerializeField] private LeaderboardItem m_leaderboardItemPrefab = default;

    private void OnEnable()
    {
        foreach (Transform child in m_scrollViewContent)
        {
            Destroy(child.gameObject);
        }

        StartCoroutine(RefreshData());
    }

    private IEnumerator RefreshData()
    {
        var task = FirebaseDatabase.DefaultInstance.RootReference.Child(FirebaseDatabaseManager.USERS_REFERENCE).OrderByChild(FirebaseDatabaseManager.SCORE_REFERENCE).GetValueAsync();

        yield return new WaitUntil(predicate: () => task.IsCompleted);

        if (task.Exception == null)
        {
            DataSnapshot dataSnapshot = task.Result;

            int count = 1;
            foreach (DataSnapshot childSnapshot in dataSnapshot.Children.Reverse<DataSnapshot>())
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

                    count++;
                }
            }
        }
        else
        {
            Debug.LogError("Cannot load leaderboard scores from database");
        }
    }
}