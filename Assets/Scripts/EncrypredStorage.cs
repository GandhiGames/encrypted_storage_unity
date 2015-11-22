using UnityEngine;
using System.Collections;

public class EncrypredStorage : MonoBehaviour
{
	private SecurePlayerPrefs _securePlayerPref;

	/// <summary>
	/// Change password to suit your needs.
	/// </summary>
	private static readonly string PASSWORD = "90qwdkbbkj12DSA114efasd00m";

	private static EncrypredStorage _instance;
	public static EncrypredStorage instance {
		get {
			if (!_instance) {
				_instance = GameObject.FindObjectOfType<EncrypredStorage> ();
				_instance._securePlayerPref = new SecurePlayerPrefs ();
			}

			return _instance;
		}
	}

	public void StoreContent (string id, string content)
	{
		Logger.instance.Log (new Logger.Message (this, "Saving content: " + content + " with id: " + id));
		_securePlayerPref.SetString (id, content, PASSWORD);
		PlayerPrefs.Save ();
	}

	public bool TryRetrieveContent (string id, out string decryptedItem)
	{
		Logger.instance.Log (new Logger.Message (this, "attempting retrieval of item with id: " + id));
		decryptedItem = _securePlayerPref.GetString (id, PASSWORD);

		return !string.IsNullOrEmpty (decryptedItem);
	}

}
