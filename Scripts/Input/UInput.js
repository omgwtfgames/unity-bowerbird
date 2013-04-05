#pragma downcast
// a modified Singleton version of:
// http://answers.unity3d.com/questions/263881/is-there-a-way-to-manually-change-the-inputaxis-va.html
//
// This class lets you set/get and get an Input axes, just like Input.GetAxis,
// except as UInput.events.GetAxis("Horizontal") and UInput.events.SetAxis("Horizontal", 1.0)
//
// Rather than a regular constructor, create an instance with:
// var input : UInput = UInput.GetInputManager();
//
// If a non-zero value of an axis has been explicitly set, that value is returned.
// Otherwise, the value from the regular Input.GetAxis call is returned (that may or may no be zero).
//
// Why would you ever want this ? It allows you to use touch screen events to SetAxis values,
// (eg via virtual joysticks and touchscreen buttons) but also catch regular key/gamepad
// controls using the same code path, if present. Since with touch events you would need to
// explicitly call SetAxis to reset the axis to zero when no touch is present, another option is to
// use PopAxis which resets the axis value to zero after it's value is returned.

class UInput extends MonoBehaviour {
    static var events : UInput;
	static var inputs : Hashtable;

    public static function GetInputManager() : UInput {
        if (events == null)
            events = new UInput();
        return events;
    }

    private function UInput() {
		inputs = new Hashtable();
    }
    
    static function GetAxis(_axis:String) : float {
       if(!inputs.ContainsKey(_axis)) {
         inputs.Add(_axis, 0);
       }
	   
       if (inputs[_axis] != 0) {
         return inputs[_axis];
       } else {
         return Input.GetAxis(_axis);
       }
    }
    
    // returns the axis value and then resets it to zero
    // TODO: buggy ?
    static function PopAxis(_axis:String) : float {
    	var v : float = GetAxis(_axis);
    	SetAxis(_axis, 0);
    	return v;
    }

    static function SetAxis(_axis:String, _value:float) : float {
       if(!inputs.ContainsKey(_axis)) {
         inputs.Add(_axis, _value);
       }
       inputs[_axis] = _value;
    }
}