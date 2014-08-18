// http://wiki.unity3d.com/index.php/CSharpMessenger_Extended
// MessengerUnitTest.cs v1.0 by Magnus Wolffelt, magnus.wolffelt@gmail.com
//
// Delegates used in Messenger.cs.
 
public delegate void MessengerCallback();
public delegate void MessengerCallback<T>(T arg1);
public delegate void MessengerCallback<T, U>(T arg1, U arg2);
public delegate void MessengerCallback<T, U, V>(T arg1, U arg2, V arg3);