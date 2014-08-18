using UnityEngine;
using System.Collections;

public class Md5 {

	public static string Hash(string strToEncrypt) {
		System.Text.Encoding encoding = new System.Text.UTF8Encoding();
		byte[] bytes = encoding.GetBytes(strToEncrypt);

		// encrypt bytes
		System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] hashBytes = md5.ComputeHash(bytes);
		
		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";
		
		for (int i = 0; i < hashBytes.Length; i++)
		{
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, "0"[0]);
		}
		
		return hashString.PadLeft(32, "0"[0]);
	}
}
