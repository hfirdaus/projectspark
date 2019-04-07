using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardTestManager : Singleton<KeyboardTestManager>
{
	public event Action Numpad1 = new Action(() => { });
	public event Action Numpad2 = new Action(() => { });
	public event Action Numpad3 = new Action(() => { });
	public event Action Numpad4 = new Action(() => { });
	public event Action Numpad5 = new Action(() => { });
	public event Action Numpad6 = new Action(() => { });
	public event Action Numpad7 = new Action(() => { });
	public event Action Numpad8 = new Action(() => { });
	public event Action Numpad9 = new Action(() => { });
	public event Action Numpad0 = new Action(() => { });

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
			Numpad1();

		if (Input.GetKeyDown(KeyCode.Alpha2))
			Numpad2();

		if (Input.GetKeyDown(KeyCode.Alpha3))
			Numpad3();

		if (Input.GetKeyDown(KeyCode.Alpha4))
			Numpad4();

		if (Input.GetKeyDown(KeyCode.Alpha5))
			Numpad5();

		if (Input.GetKeyDown(KeyCode.Alpha6))
			Numpad6();

		if (Input.GetKeyDown(KeyCode.Alpha7))
			Numpad7();

		if (Input.GetKeyDown(KeyCode.Alpha8))
			Numpad8();

		if (Input.GetKeyDown(KeyCode.Alpha9))
			Numpad9();

		if (Input.GetKeyDown(KeyCode.Alpha0))
			Numpad0();
	}
}
