using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public class DESEncryption : IEncryption
{
	private static readonly int ITERATIONS = 500;

	public string Encrypt (string item, string password)
	{
		if (item == null) {
			throw new ArgumentNullException ("item");
		}
		
		if (string.IsNullOrEmpty (password)) {
			throw new ArgumentNullException ("password");
		}

		var des = new DESCryptoServiceProvider ();

		des.GenerateIV ();

		var rfc2898DeriveBytes = new Rfc2898DeriveBytes (password, des.IV, ITERATIONS);

		byte[] key = rfc2898DeriveBytes.GetBytes (8);

		using (var memoryStream = new MemoryStream()) {
			using (var cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(key, des.IV), CryptoStreamMode.Write)) {
				memoryStream.Write (des.IV, 0, des.IV.Length);

				byte[] bytes = Encoding.UTF8.GetBytes (item);

				cryptoStream.Write (bytes, 0, bytes.Length);
				cryptoStream.FlushFinalBlock ();
			
				return Convert.ToBase64String (memoryStream.ToArray ());
			}
		}
	}
	
	public bool TryDecrypt (string encryptedItem, string password, out string decryptedItem)
	{
		if (string.IsNullOrEmpty (encryptedItem) || 
			string.IsNullOrEmpty (password)) {
			decryptedItem = "";
			return false;
		}
		
		try {   
			byte[] cipherBytes = Convert.FromBase64String (encryptedItem);
			
			using (var memoryStream = new MemoryStream(cipherBytes)) {
				var des = new DESCryptoServiceProvider ();

				byte[] iv = new byte[8];
				memoryStream.Read (iv, 0, iv.Length);

				var rfc2898DeriveBytes = new Rfc2898DeriveBytes (password, iv, ITERATIONS);
				
				byte[] key = rfc2898DeriveBytes.GetBytes (8);
				
				using (var cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(key, iv), CryptoStreamMode.Read))
				using (var streamReader = new StreamReader(cryptoStream)) {
					decryptedItem = streamReader.ReadToEnd ();
					return true;
				}
			}
		} catch (Exception ex) {
			Logger.instance.Log (new Logger.Message (this, ex));
			
			decryptedItem = "";
			return false;
		}
	}
	
	
}
