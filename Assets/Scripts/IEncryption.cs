using UnityEngine;
using System.Collections;

public interface IEncryption  
{
	string Encrypt (string item, string password);
	bool TryDecrypt (string encryptedItem, string password, out string item);
}
