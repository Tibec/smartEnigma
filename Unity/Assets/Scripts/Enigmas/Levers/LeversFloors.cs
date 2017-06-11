using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class LeversFloors : InteractableElementBehaviour {

	public GameObject floor1;
	public GameObject floor2;
	public GameObject floor3;
	public GameObject floor4;
	public GameObject floor5;

	// number of floors activated
	private int i = 0;

	private void Awake()
	{

	}

	public override void OnInteraction(InteractableElement ie, Player p)
	{
		if (ie.name == "Lever1") {
			if (floor1.activeSelf) {
				floor1.SetActive (false);
				i--;
			} else if (i<2){
				floor1.SetActive (true);
				i++;
			}
		}
		else if (ie.name == "Lever2") {
			if (floor2.activeSelf) {
				floor2.SetActive (false);
				i--;
			} else if (i<2){
				floor2.SetActive (true);
				i++;
			}
		}
		else if (ie.name == "Lever3") {
			if (floor3.activeSelf) {
				floor3.SetActive (false);
				i--;
			} else if (i<2){
				floor3.SetActive (true);
				i++;
			}
		}
		else if (ie.name == "Lever4") {
			if (floor4.activeSelf) {
				floor4.SetActive (false);
				i--;
			} else if (i<2){
				floor4.SetActive (true);
				i++;
			}
		}
		else if (ie.name == "Lever5") {
			if (floor5.activeSelf) {
				floor5.SetActive (false);
				i--;
			} else if (i<2){
				floor5.SetActive (true);
				i++;
			}
		}
	} 


	void Update()
	{

	}
}
