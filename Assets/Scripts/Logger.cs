using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Logger : MonoBehaviour
{

	private const string PRE_TEXT = "encryption_log: ";

	private static Logger _instance;
	public static Logger instance {
		get {
			if (!_instance) {
				_instance = GameObject.FindObjectOfType<Logger> ();
			}
			
			return _instance;
		}
	}
	
	void Awake ()
	{
		if (!_instance) {
			_instance = this;
		}
	}

	public void Log (object msg)
	{
		Debug.Log (PRE_TEXT + msg.ToString ());
	}

	public class Message
	{
		private object _sendingObj;
		private object _msg;
		
		public Message (object sendingObj, object msg)
		{
			_sendingObj = sendingObj;
			_msg = msg;
		}
		
		public override string ToString ()
		{
			return string.Format ("{0} [{1}]: {2}", System.DateTime.Now, _sendingObj.GetType ().Name, _msg.ToString ());
		}
	}
}
