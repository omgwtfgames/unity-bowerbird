#pragma strict
// From: http://unifycommunity.com/wiki/index.php?title=ScaledRect

static var customWidth : float = 1024; //Set this value to the Width of your Game Tab in the Editor
static var customHeight : float = 600; //Set this value to the Height of your Game Tab in the Editor

static function scaledRect (x : float, y : float, width : float, height : float) {
    var returnRect : Rect;
    x = (x*Screen.width) / customWidth;
    y = (y*Screen.height) / customHeight;
    width = (width*Screen.width) / customWidth;
    height = (height*Screen.height) / customHeight;
    
    returnRect = Rect(x,y,width,height);
    return returnRect;
}