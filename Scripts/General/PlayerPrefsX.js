#pragma strict
// ArrayPrefs2 v 1.3
import System.Collections.Generic;
 
static private var endianDiff1 : int;
static private var endianDiff2 : int;
static private var idx : int;
static private var byteBlock : byte[];
 
enum ArrayType {Float, Int32, Bool, String, Vector2, Vector3, Quaternion, Color}
 
static function SetBool (name : String, value : boolean) : boolean {
	try {
		PlayerPrefs.SetInt(name, value? 1 : 0);
	}
	catch (err) {
		return false;
	}
	return true;
}
 
static function GetBool (name : String) : boolean {
	return PlayerPrefs.GetInt(name) == 1;
}
 
static function GetBool (name : String, defaultValue : boolean) : boolean {
	if (PlayerPrefs.HasKey(name)) {
		return GetBool(name);
	}
	return defaultValue;
}
 
static function SetVector2 (key : String, vector : Vector2) : boolean {
	return SetFloatArray(key, [vector.x, vector.y]);
}
 
static function GetVector2 (key : String) : Vector2 {
	var floatArray = GetFloatArray(key);
	if (floatArray.Length < 2) {
		return Vector2.zero;
	}
	return Vector2(floatArray[0], floatArray[1]);
}
 
static function GetVector2 (key : String, defaultValue : Vector2) : Vector2 {
	if (PlayerPrefs.HasKey(key)) {
		return GetVector2(key);
	}
	return defaultValue;
}
 
static function SetVector3 (key : String, vector : Vector3) : boolean {
	return SetFloatArray(key, [vector.x, vector.y, vector.z]);
}
 
static function GetVector3 (key : String) : Vector3 {
	var floatArray = GetFloatArray(key);
	if (floatArray.Length < 3) {
		return Vector3.zero;
	}
	return Vector3(floatArray[0], floatArray[1], floatArray[2]);
}
 
static function GetVector3 (key : String, defaultValue : Vector3) : Vector3 {
	if (PlayerPrefs.HasKey(key)) {
		return GetVector3(key);
	}
	return defaultValue;
}
 
static function SetQuaternion (key : String, vector : Quaternion) : boolean {
	return SetFloatArray(key, [vector.x, vector.y, vector.z, vector.w]);
}
 
static function GetQuaternion (key : String) : Quaternion {
	var floatArray = GetFloatArray(key);
	if (floatArray.Length < 4) {
		return Quaternion.identity;
	}
	return Quaternion(floatArray[0], floatArray[1], floatArray[2], floatArray[3]);
}
 
static function GetQuaternion (key : String, defaultValue : Quaternion) : Quaternion {
	if (PlayerPrefs.HasKey(key)) {
		return GetQuaternion(key);
	}
	return defaultValue;
}
 
static function SetColor (key : String, color : Color) : boolean {
	return SetFloatArray(key, [color.r, color.g, color.b, color.a]);
}
 
static function GetColor (key : String) : Color {
	var floatArray = GetFloatArray(key);
	if (floatArray.Length < 4) {
		return Color(0.0, 0.0, 0.0, 0.0);
	}
	return Color(floatArray[0], floatArray[1], floatArray[2], floatArray[3]);
}
 
static function GetColor (key : String, defaultValue : Color) : Color {
	if (PlayerPrefs.HasKey(key)) {
		return GetColor(key);
	}
	return defaultValue;
}
 
static function SetBoolArray (key : String, boolArray : boolean[]) : boolean {
	if (boolArray.Length == 0) {
		Debug.LogError ("The bool array cannot have 0 entries when setting " + key);
		return false;
	}
	// Make a byte array that's a multiple of 8 in length, plus 5 bytes to store the number of entries as an int32 (+ identifier)
	// We have to store the number of entries, since the boolArray length might not be a multiple of 8, so there could be some padded zeroes
	var bytes = new byte[(boolArray.Length + 7)/8 + 5];
	bytes[0] = System.Convert.ToByte (ArrayType.Bool);	// Identifier
	var bits = new BitArray(boolArray);
	bits.CopyTo (bytes, 5);
	Initialize();
	ConvertInt32ToBytes (boolArray.Length, bytes); // The number of entries in the boolArray goes in the first 4 bytes
 
	return SaveBytes (key, bytes);	
}
 
static function GetBoolArray (key : String) : boolean[] {
	if (PlayerPrefs.HasKey(key)) {
		var bytes = System.Convert.FromBase64String (PlayerPrefs.GetString(key));
		if (bytes.Length < 6) {
			Debug.LogError ("Corrupt preference file for " + key);
			return new boolean[0];
		}
		if (bytes[0] != ArrayType.Bool) {
			Debug.LogError (key + " is not a boolean array");
			return new boolean[0];
		}
		Initialize();
 
		// Make a new bytes array that doesn't include the number of entries + identifier (first 5 bytes) and turn that into a BitArray
		var bytes2 = new byte[bytes.Length-5];
		System.Array.Copy(bytes, 5, bytes2, 0, bytes2.Length);
		var bits = new BitArray(bytes2);
		// Get the number of entries from the first 4 bytes after the identifier and resize the BitArray to that length, then convert it to a boolean array
		bits.Length = ConvertBytesToInt32 (bytes);
		var boolArray = new boolean[bits.Count];
		bits.CopyTo (boolArray, 0);
 
		return boolArray;
	}
	return new boolean[0];
}
 
static function GetBoolArray (key : String, defaultValue : boolean, defaultSize : int) : boolean[] {
	if (PlayerPrefs.HasKey(key)) {
		return GetBoolArray(key);
	}
	var boolArray = new boolean[defaultSize];
	for (b in boolArray) {
		b = defaultValue;
	}
	return boolArray;
}
 
static function SetStringArray (key : String, stringArray : String[]) : boolean {
	if (stringArray.Length == 0) {
		Debug.LogError ("The string array cannot have 0 entries when setting " + key);
		return false;
	}
	var bytes = new byte[stringArray.Length + 1];
	bytes[0] = System.Convert.ToByte (ArrayType.String);	// Identifier
	Initialize();
 
	// Store the length of each string that's in stringArray, so we can extract the correct strings in GetStringArray
	for (var i = 0; i < stringArray.Length; i++) {
		if (stringArray[i] == null) {
			Debug.LogError ("Can't save null entries in the string array when setting " + key);
			return false;
		}
		if (stringArray[i].Length > 255) {
			Debug.LogError ("Strings cannot be longer than 255 characters when setting " + key);
			return false;
		}
		bytes[idx++] = stringArray[i].Length;
	}
 
	try {
		PlayerPrefs.SetString (key, System.Convert.ToBase64String (bytes) + "|" + String.Join("", stringArray));
	}
	catch (err) {
		return false;
	}
	return true;
}
 
static function GetStringArray (key : String) : String[] {
	if (PlayerPrefs.HasKey(key)) {
		var completeString = PlayerPrefs.GetString(key);
		var separatorIndex = completeString.IndexOf("|"[0]);
		if (separatorIndex < 4) {
			Debug.LogError ("Corrupt preference file for " + key);
			return new String[0];
		}
		var bytes = System.Convert.FromBase64String (completeString.Substring(0, separatorIndex));
		if (bytes[0] != ArrayType.String) {
			Debug.LogError (key + " is not a string array");
			return new String[0];
		}
		Initialize();
 
		var numberOfEntries = bytes.Length-1;
		var stringArray = new String[numberOfEntries];
		var stringIndex = separatorIndex + 1;
		for (var i = 0; i < numberOfEntries; i++) {
			var stringLength : int = bytes[idx++];
			if (stringIndex + stringLength > completeString.Length) {
				Debug.LogError ("Corrupt preference file for " + key);
				return new String[0];
			}
			stringArray[i] = completeString.Substring(stringIndex, stringLength);
			stringIndex += stringLength;
		}
 
		return stringArray;
	}
	return new String[0];
}
 
static function GetStringArray (key : String, defaultValue : String, defaultSize : int) : String[] {
	if (PlayerPrefs.HasKey(key)) {
		return GetStringArray(key);
	}
	var stringArray = new String[defaultSize];
	for (s in stringArray) {
		s = defaultValue;
	}
	return stringArray;
}
 
static function SetIntArray (key : String, intArray : int[]) : boolean {
	return SetValue (key, intArray, ArrayType.Int32, 1, ConvertFromInt);
}
 
static function SetFloatArray (key : String, floatArray : float[]) : boolean {
	return SetValue (key, floatArray, ArrayType.Float, 1, ConvertFromFloat);
}
 
static function SetVector2Array (key : String, vector2Array : Vector2[]) : boolean {
	return SetValue (key, vector2Array, ArrayType.Vector2, 2, ConvertFromVector2);
}
 
static function SetVector3Array (key : String, vector3Array : Vector3[]) : boolean {
	return SetValue (key, vector3Array, ArrayType.Vector3, 3, ConvertFromVector3);
}
 
static function SetQuaternionArray (key : String, quaternionArray : Quaternion[]) : boolean {
	return SetValue (key, quaternionArray, ArrayType.Quaternion, 4, ConvertFromQuaternion);
}
 
static function SetColorArray (key : String, colorArray : Color[]) : boolean {
	return SetValue (key, colorArray, ArrayType.Color, 4, ConvertFromColor);
}
 
private static function SetValue (key : String, array : IList, arrayType : ArrayType, vectorNumber : int, convert : function(IList, byte[], int)) : boolean {
	if (array.Count == 0) {
		Debug.LogError ("The " + arrayType.ToString() + " array cannot have 0 entries when setting " + key);
		return false;
	}
	var bytes = new byte[(4*array.Count)*vectorNumber + 1];
	bytes[0] = System.Convert.ToByte (arrayType);	// Identifier
	Initialize();
 
	for (var i = 0; i < array.Count; i++) {
		convert (array, bytes, i);	
	}
	return SaveBytes (key, bytes);
}
 
private static function ConvertFromInt (array : int[], bytes : byte[], i : int) {
	ConvertInt32ToBytes (array[i], bytes);
}
 
private static function ConvertFromFloat (array : float[], bytes : byte[], i : int) {
	ConvertFloatToBytes (array[i], bytes);
}
 
private static function ConvertFromVector2 (array : Vector2[], bytes : byte[], i : int) {
	ConvertFloatToBytes (array[i].x, bytes);
	ConvertFloatToBytes (array[i].y, bytes);
}
 
private static function ConvertFromVector3 (array : Vector3[], bytes : byte[], i : int) {
	ConvertFloatToBytes (array[i].x, bytes);
	ConvertFloatToBytes (array[i].y, bytes);
	ConvertFloatToBytes (array[i].z, bytes);
}
 
private static function ConvertFromQuaternion (array : Quaternion[], bytes : byte[], i : int) {
	ConvertFloatToBytes (array[i].x, bytes);
	ConvertFloatToBytes (array[i].y, bytes);
	ConvertFloatToBytes (array[i].z, bytes);
	ConvertFloatToBytes (array[i].w, bytes);
}
 
private static function ConvertFromColor (array : Color[], bytes : byte[], i : int) {
	ConvertFloatToBytes (array[i].r, bytes);
	ConvertFloatToBytes (array[i].g, bytes);
	ConvertFloatToBytes (array[i].b, bytes);
	ConvertFloatToBytes (array[i].a, bytes);
}
 
static function GetIntArray (key : String) : int[] {
	var intList = new List.<int>();
	GetValue (key, intList, ArrayType.Int32, 1, ConvertToInt);
	return intList.ToArray();
}
 
static function GetIntArray (key : String, defaultValue : int, defaultSize : int) : int[] {
	if (PlayerPrefs.HasKey(key)) {
		return GetIntArray(key);
	}
	var intArray = new int[defaultSize];
	for (i in intArray) {
		i = defaultValue;
	}
	return intArray;
}
 
static function GetFloatArray (key : String) : float[] {
	var floatList = new List.<float>();
	GetValue (key, floatList, ArrayType.Float, 1, ConvertToFloat);
	return floatList.ToArray();
}
 
static function GetFloatArray (key : String, defaultValue : float, defaultSize : int) : float[] {
	if (PlayerPrefs.HasKey(key)) {
		return GetFloatArray(key);
	}
	var floatArray = new float[defaultSize];
	for (f in floatArray) {
		f = defaultValue;
	}
	return floatArray;
}
 
static function GetVector2Array (key : String) : Vector2[] {
	var vector2List = new List.<Vector2>();
	GetValue (key, vector2List, ArrayType.Vector2, 2, ConvertToVector2);
	return vector2List.ToArray();
}
 
static function GetVector2Array (key : String, defaultValue : Vector2, defaultSize : int) : Vector2[] {
	if (PlayerPrefs.HasKey(key)) {
		return GetVector2Array(key);
	}
	var vector2Array = new Vector2[defaultSize];
	for (v in vector2Array) {
		v = defaultValue;
	}
	return vector2Array;
}
 
static function GetVector3Array (key : String) : Vector3[] {
	var vector3List = new List.<Vector3>();
	GetValue (key, vector3List, ArrayType.Vector3, 3, ConvertToVector3);
	return vector3List.ToArray();
}
 
static function GetVector3Array (key : String, defaultValue : Vector3, defaultSize : int) : Vector3[] {
	if (PlayerPrefs.HasKey(key)) {
		return GetVector3Array(key);
	}
	var vector3Array = new Vector3[defaultSize];
	for (v in vector3Array) {
		v = defaultValue;
	}
	return vector3Array;
}
 
static function GetQuaternionArray (key : String) : Quaternion[] {
	var quaternionList = new List.<Quaternion>();
	GetValue (key, quaternionList, ArrayType.Quaternion, 4, ConvertToQuaternion);
	return quaternionList.ToArray();
}
 
static function GetQuaternionArray (key : String, defaultValue : Quaternion, defaultSize : int) : Quaternion[] {
	if (PlayerPrefs.HasKey(key)) {
		return GetQuaternionArray(key);
	}
	var quaternionArray = new Quaternion[defaultSize];
	for (v in quaternionArray) {
		v = defaultValue;
	}
	return quaternionArray;
}
 
static function GetColorArray (key : String) : Color[] {
	var colorList = new List.<Color>();
	GetValue (key, colorList, ArrayType.Color, 4, ConvertToColor);
	return colorList.ToArray();
}
 
static function GetColorArray (key : String, defaultValue : Color, defaultSize : int) : Color[] {
	if (PlayerPrefs.HasKey(key)) {
		return GetColorArray(key);
	}
	var colorArray = new Color[defaultSize];
	for (v in colorArray) {
		v = defaultValue;
	}
	return colorArray;
}
 
private static function GetValue (key : String, list : IList, arrayType : ArrayType, vectorNumber : int, convert : function(IList, byte[])) {
	if (PlayerPrefs.HasKey(key)) {
		var bytes = System.Convert.FromBase64String (PlayerPrefs.GetString(key));
		if ((bytes.Length-1) % (vectorNumber*4) != 0) {
			Debug.LogError ("Corrupt preference file for " + key);
			return;
		}
		if (bytes[0] != arrayType) {
			Debug.LogError (key + " is not a " + arrayType.ToString() + " array");
			return;
		}
		Initialize();
 
		var end = (bytes.Length-1) / (vectorNumber*4);
		for (var i = 0; i < end; i++) {
			convert (list, bytes);
		}
	}
}
 
private static function ConvertToInt (list : List.<int>, bytes : byte[]) {
	list.Add (ConvertBytesToInt32(bytes));
}
 
private static function ConvertToFloat (list : List.<float>, bytes : byte[]) {
	list.Add (ConvertBytesToFloat(bytes));
}
 
private static function ConvertToVector2 (list : List.<Vector2>, bytes : byte[]) {
	list.Add (Vector2(ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes)));
}
 
private static function ConvertToVector3 (list : List.<Vector3>, bytes : byte[]) {
	list.Add (Vector3(ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes)));
}
 
private static function ConvertToQuaternion (list : List.<Quaternion>, bytes : byte[]) {
	list.Add (Quaternion(ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes)));
}
 
private static function ConvertToColor (list : List.<Color>, bytes : byte[]) {
	list.Add (Color(ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes), ConvertBytesToFloat(bytes)));
}
 
static function ShowArrayType (key : String) {
	var bytes = System.Convert.FromBase64String (PlayerPrefs.GetString(key));
	if (bytes.Length > 0) {
		var arrayType : ArrayType = bytes[0];
		Debug.Log (key + " is a " + arrayType.ToString() + " array");
	}
}
 
private static function Initialize () {
	if (System.BitConverter.IsLittleEndian) {
		endianDiff1 = 0;
		endianDiff2 = 0;
	}
	else {
		endianDiff1 = 3;
		endianDiff2 = 1;
	}
	if (byteBlock == null) {
		byteBlock = new byte[4];
	}
	idx = 1;
}
 
private static function SaveBytes (key : String, bytes : byte[]) : boolean {
	try {
		PlayerPrefs.SetString (key, System.Convert.ToBase64String (bytes));
	}
	catch (err) {
		return false;
	}
	return true;
}
 
private static function ConvertFloatToBytes (f : float, bytes : byte[]) {
	byteBlock = System.BitConverter.GetBytes (f);
	ConvertTo4Bytes (bytes);
}
 
private static function ConvertBytesToFloat (bytes : byte[]) : float {
	ConvertFrom4Bytes (bytes);
	return System.BitConverter.ToSingle (byteBlock, 0);
}
 
private static function ConvertInt32ToBytes (i : int, bytes : byte[]) {
	byteBlock = System.BitConverter.GetBytes (i);
	ConvertTo4Bytes (bytes);
}
 
private static function ConvertBytesToInt32 (bytes : byte[]) : int {
	ConvertFrom4Bytes (bytes);
	return System.BitConverter.ToInt32 (byteBlock, 0);
}
 
private static function ConvertTo4Bytes (bytes : byte[]) {
	bytes[idx  ] = byteBlock[    endianDiff1];
	bytes[idx+1] = byteBlock[1 + endianDiff2];
	bytes[idx+2] = byteBlock[2 - endianDiff2];
	bytes[idx+3] = byteBlock[3 - endianDiff1];
	idx += 4;
}
 
private static function ConvertFrom4Bytes (bytes : byte[]) {
	byteBlock[    endianDiff1] = bytes[idx  ];
	byteBlock[1 + endianDiff2] = bytes[idx+1];
	byteBlock[2 - endianDiff2] = bytes[idx+2];
	byteBlock[3 - endianDiff1] = bytes[idx+3];
	idx += 4;
}
