using System;
using System.Diagnostics;

/*
 *  Use like: 
 *  #define DEBUG
 *  DebugUtils.Assert(FindAllMiceInTheCity().Length < 1000000);
 * 
 *  From: http://answers.unity3d.com/questions/19122/assert-function.html
 */
public class DebugUtils
{
    [Conditional("DEBUG")]
    static void Assert(bool condition)
    {
        if (!condition) throw new Exception();
    }
}