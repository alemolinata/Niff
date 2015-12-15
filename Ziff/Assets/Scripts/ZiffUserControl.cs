using UnityEngine;
using System.Collections;
using System.IO.Ports;

[RequireComponent(typeof (ZiffCharacter))]
public class ZiffUserControl : MonoBehaviour {

	public bool serialConnected;
	public SerialPort sp = new SerialPort("/dev/cu.usbserial-DN00CXBV", 9600);
	static string serialReading;

	public static bool buttonState = false;
	public static bool missingLeg = false;
	public static int powerStone = 1;

	private bool prevMissing = false;
	private bool prevButtonReading = false;
	private bool win;

	private Animator ziffAnim;

	private void Awake()
	{
		ziffAnim = GetComponent<Animator> ();
		OpenConnection ();
		win = GetComponent<ZiffCharacter> ().win;
	}

	public void OpenConnection() {
		if(serialConnected){
			if (sp != null) {
				if (sp.IsOpen) {
					sp.Close();
					print("Closing port, because it was already open!");
				}
				else {
					sp.Open();  // opens the connection
					sp.ReadTimeout = 16;  // sets the timeout value before reporting error
					print("Port Opened!");
				}
			}
			else {
				if (sp.IsOpen)
					print("Port is already open");
				else 
					print("Port == null");
			}
		}
	}

	private void Update()
	{
		ReadSerial ();
	}
	public void ReadSerial(){
		if (!ZiffCharacter.dead && !win) {
		//if (!ZiffCharacter.dead) {
			if (serialConnected) {
				try {
					int serialByte = sp.ReadByte ();
					serialReading = System.Convert.ToString (serialByte, 2);
					print (serialReading);
				
					//if(serialReading > 127 && serialReading < 256){
					if (serialReading.Length == 8) {
						int serialMode = 0;
						bool missing = false;
						bool buttonReading = false;
						int stoneReading = 0;
					
						if (serialReading [1] == '1') {
							if (serialReading [2] == '1')
								serialMode = 4;
							else if (serialReading [3] == '1')
								serialMode = 3;
							else if (serialReading [4] == '1') {
								serialMode = 0;
								missing = true;
							} else
								serialMode = 0;
						} else if (serialReading [2] == '1') {
							if (serialReading [3] == '1')
								serialMode = 5;
							else if (serialReading [4] == '1') {
								serialMode = 2;
								missing = true;
							} else
								serialMode = 2;
						} else if (serialReading [3] == '1') {
							if (serialReading [4] == '1') {
								serialMode = 1;
								missing = true;
							} else
								serialMode = 1;
						}
						else{
							serialMode = 6;
							missing = true;
						}
						if (serialReading [5] == '1') {
							stoneReading = 2;
						} else {
							if (serialReading [6] == '1')
								stoneReading = 1;
							else
								stoneReading = 0;
						}
						if (serialReading [7] == '1') {
							buttonReading = true;
						} else {
							buttonReading = false;
						}
						ZiffCharacter.mode = (ZiffCharacter.Mode)(serialMode);
						ziffAnim.SetInteger ("ZiffMode", serialMode);
					
						if (missing == true && prevMissing == true)
							missingLeg = true;
						else
							missingLeg = false;
						ziffAnim.SetBool ("MissingLeg", missingLeg);
						buttonState = buttonReading;
						powerStone = stoneReading;
					
						prevMissing = missing;
					}
				} catch (System.Exception) {
				}
			} else {
				if (Input.GetKey ("space"))
					buttonState = true;
				else
					buttonState = false;
				if (Input.GetKeyDown ("1")) {
					int modeReading = 0;
					ZiffCharacter.mode = (ZiffCharacter.Mode)(modeReading);
					ziffAnim.SetInteger ("ZiffMode", modeReading);
				}
				if (Input.GetKeyDown ("2")) {
					int modeReading = 1;
					ZiffCharacter.mode = (ZiffCharacter.Mode)(modeReading);
					ziffAnim.SetInteger ("ZiffMode", modeReading);
				}
				if (Input.GetKeyDown ("3")) {
					int modeReading = 2;
					ZiffCharacter.mode = (ZiffCharacter.Mode)(modeReading);
					ziffAnim.SetInteger ("ZiffMode", modeReading);
				}
				if (Input.GetKeyDown ("4")) {
					int modeReading = 3;
					ZiffCharacter.mode = (ZiffCharacter.Mode)(modeReading);
					ziffAnim.SetInteger ("ZiffMode", modeReading);
				}
				if (Input.GetKeyDown ("5")) {
					int modeReading = 4;
					ZiffCharacter.mode = (ZiffCharacter.Mode)(modeReading);
					ziffAnim.SetInteger ("ZiffMode", modeReading);
				}
				if (Input.GetKeyDown ("6")) {
					int modeReading = 5;
					ZiffCharacter.mode = (ZiffCharacter.Mode)(modeReading);
					ziffAnim.SetInteger ("ZiffMode", modeReading);
				}
				if (Input.GetKeyDown ("7")) {
					int modeReading = 6;
					ZiffCharacter.mode = (ZiffCharacter.Mode)(modeReading);
					ziffAnim.SetInteger ("ZiffMode", modeReading);
					missingLeg = true;
					ziffAnim.SetBool ("MissingLeg", missingLeg);
				}
				if (Input.GetKeyDown ("z")) {
					powerStone = (powerStone == 1) ? 0 : 1;  
					ziffAnim.SetInteger ("PowerStone", powerStone);
					ziffAnim.SetTrigger ("PowerStoneChange");
				}
				if (Input.GetKeyDown ("x")) {
					missingLeg = ! missingLeg;
					ziffAnim.SetBool ("MissingLeg", missingLeg);
				}
			}
			ziffAnim.SetBool ("Action", buttonState);
		}
	}
	void OnApplicationQuit() {
		if (serialConnected) 
			sp.Close();
		
	}


}
