using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoom : MonoBehaviour
{
	public GameObject piso;
	public GameObject pared;
	private GameObject pisoClone;
	private GameObject pared1;
	private GameObject pared2;
	private GameObject pared3;
	private GameObject pared4;
	public static float X = 15;
	public static float Y = 15;

    // Start is called before the first frame update
    void Start()
    {
		buildRoom(X, Y);
    }

    // Update is called once per frame
    void Update()
    {
		if(Input.GetKeyDown(KeyCode.E))
			destroyRoom();
    }

	public void buildRoom(float x, float y)
	{
		pisoClone = Instantiate(piso, new Vector3(0, 0, 0), Quaternion.identity);
		pisoClone.transform.localScale = new Vector3(x, 0.1f, y);

		pared1 = Instantiate(pared, new Vector3(x/2-0.1f, 1.35f, 0), Quaternion.identity);
		pared1.transform.localScale = new Vector3(0.2f, 2.6f, y);

		pared2 = Instantiate(pared, new Vector3(-x/2+0.1f, 1.35f, 0), Quaternion.identity);
		pared2.transform.localScale = new Vector3(0.2f, 2.6f, y);

		pared3 = Instantiate(pared, new Vector3(0, 1.35f, y/2-0.1f), Quaternion.identity);
		pared3.transform.localScale = new Vector3(x-0.4f, 2.6f, 0.2f);

		pared4 = Instantiate(pared, new Vector3(0, 1.35f, -y/2+0.1f), Quaternion.identity);
		pared4.transform.localScale = new Vector3(x-0.4f, 2.6f, 0.2f);

		this.transform.position = new Vector3(-x/2, 0, -y/2);
		X = x;
		Y = y;
	}

	public void destroyRoom()
	{
		Destroy(pisoClone);
		Destroy(pared1);
		Destroy(pared2);
		Destroy(pared3);
		Destroy(pared4);

		var eee = GameObject.FindGameObjectsWithTag ("Mesa");
		foreach (var bu in eee){
			Destroy(bu);
		}
	}
}
