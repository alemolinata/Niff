using UnityEngine;
//using System.Collections;
using System.IO.Ports;

[RequireComponent(typeof (NiffCharacter))]
public class NiffUserControl : MonoBehaviour {

	public SerialPort sp;
	//sp = new SerialPort("/dev/tty.usbmodem1451", 9600);
	static string serialReading;
	public static bool buttonState = false;
	public static bool missingLeg = false;
	
	public bool serialConnected;
	public static bool powerStoneOn = true;
	private bool prevMissing = false;
	private bool prevButtonReding = false;

	private Animator niffAnim;

	private void Awake()
	{
		niffAnim = GetComponent<Animator> ();
		OpenConnection ();
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
		if (serialConnected) {
			try {
				serialReading = sp.ReadLine ();
				if(serialReading.Length == 8){
					int serialMode = 0;
					bool missing = false;
					bool buttonReading;
					if(serialReading[1] == '1' || serialReading[4] == '1'){
						if(serialReading[1] == serialReading[4]){
							serialMode = 0;
						}
						else if(serialReading[2] == '1' || serialReading[5] == '1'){
							serialMode = 4;
						}
						else if(serialReading[3] == '1' || serialReading[6] == '1'){
							serialMode = 3;
						}
						else{
							missing = true;
						}
					}
					else if(serialReading[2] == '1' || serialReading[5] == '1'){
						if(serialReading[2] == serialReading[5]){
							serialMode = 2;
						}
						else if(serialReading[3] == '1' || serialReading[6] == '1'){
							serialMode = 5;
						}
						else{
							missing = true;
						}
					}
					else if(serialReading[3] == '1' || serialReading[6] == '1'){
						if(serialReading[3] == serialReading[6]){
							serialMode = '1';
						}
						else{
							missing = true;
						}
					}
					if(serialReading[7] == '1'){
						buttonReading = true;
					}
					else{
						buttonReading = false;
					}
					NiffCharacter.mode = (NiffCharacter.Mode)(serialMode);
					if(missing == true && prevMissing == true)
						missingLeg = true;
					prevMissing = missing;
				}


				string[] stringReadings = serialReading.Split (',');
				if(stringReadings [0]=="1")
					buttonState = true;
				else
					buttonState = false;
				int modeReading = System.Int32.Parse (stringReadings [1]);
				NiffCharacter.mode = (NiffCharacter.Mode)(modeReading);
				niffAnim.SetInteger ("NiffMode", modeReading);
				//print(serialReading);
			} catch (System.Exception) {
			}
		} else {
			if (Input.GetKey("space"))
				buttonState = true;
			else
				buttonState = false;
			if (Input.GetKeyDown("1")){
				int modeReading = 0;
				NiffCharacter.mode = (NiffCharacter.Mode)(modeReading);
				niffAnim.SetInteger ("NiffMode", modeReading);
			}
			if (Input.GetKeyDown("2")){
				int modeReading = 1;
				NiffCharacter.mode = (NiffCharacter.Mode)(modeReading);
				niffAnim.SetInteger ("NiffMode", modeReading);
			}
			if (Input.GetKeyDown("3")){
				int modeReading = 2;
				NiffCharacter.mode = (NiffCharacter.Mode)(modeReading);
				niffAnim.SetInteger ("NiffMode", modeReading);
			}
			if(Input.GetKeyDown("z")){
				powerStoneOn = !powerStoneOn;
				niffAnim.SetBool("PowerStoneOn", powerStoneOn);
				niffAnim.SetTrigger("PowerStoneChange");
			}
		}
		niffAnim.SetBool ("Action", buttonState);
	}
	void OnApplicationQuit() {
		if (serialConnected) 
			sp.Close();
		
	}


}
