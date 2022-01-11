using Firebase;
using Firebase.Auth;
using UnityEngine;

public class FirebaseAuthManager : MonoBehaviour
{
    private const string UNIVERSAL_PASSWORD = "123456";

    public static FirebaseUser CurrentUser
    {
        get;
        private set;
    }

    public static string ConvertNameToEmail(string name)
    {
        string nameWithoutSpaces = name.Replace(' ', '-');
        nameWithoutSpaces.ToLower();
        return nameWithoutSpaces += "@email.com";
    }

    public static void LoginUser(string name, bool createUserOnFail = true)
    {
        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(ConvertNameToEmail(name), UNIVERSAL_PASSWORD).ContinueWith(task =>
        {
            if (task.Exception == null)
            {
                CurrentUser = task.Result;
                GameData.FetchDataFromFirebase(name, !createUserOnFail);
            }
            else
            {
                var firebaseException = task.Exception.GetBaseException() as FirebaseException;
                var authError = (AuthError)firebaseException.ErrorCode;

                switch (authError)
                {
                    case AuthError.UserNotFound:
                    {
                        if (createUserOnFail)
                        {
                            CreateNewUser(name);
                        }
                        else
                        {
                            Debug.LogError("Failed to login user after creating one");
                        }
                        break;
                    }
                    default:
                    {
                        Debug.LogError("Login failed: " + task.Exception);
                        break;
                    }
                }
            }
        });
    }

    private static void CreateNewUser(string name)
    {
        FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(ConvertNameToEmail(name), UNIVERSAL_PASSWORD).ContinueWith(task =>
        {
            if (task.Exception == null)
            {
                CurrentUser = task.Result;

                if (CurrentUser != null)
                {
                    var userProfile = new UserProfile
                    {
                        DisplayName = name
                    };

                    CurrentUser.UpdateUserProfileAsync(userProfile).ContinueWith(task =>
                    {
                        if (task.Exception == null)
                        {
                            LoginUser(name, false);
                        }
                        else
                        {
                            Debug.LogError("Failed to register user: " + task.Exception);
                        }
                    });
                }
            }
            else
            {
                var firebaseException = task.Exception.GetBaseException() as FirebaseException;
                var authError = (AuthError)firebaseException.ErrorCode;

                Debug.LogError("Failed to create user: " + task.Exception);
            }
        });
    }
}