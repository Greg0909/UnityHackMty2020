using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
	public GameObject mesa;
	public GameObject walkingPoint;
	private Vector3 puntoDeReferencia;

	int px=2, pz=2;

    // Start is called before the first frame update
    void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
		puntoDeReferencia = this.transform.position;
		if(Input.GetKeyDown(KeyCode.Q))
		{
			
			if(px <= 14)
			{
				spawnTable(px, pz);
				pz+=2;
				if(pz >= 14 )
				{
					pz = 2;
					px += 2;
				}
			}
				
		}
    }

	public void spawnTable(float x, float z)
	{
		//gameObject piso = GameObject.Find("Piso(Clone)");
		float y = puntoDeReferencia.y + 0.0f;
		x = puntoDeReferencia.x + x;
		z = puntoDeReferencia.z + z;
		GameObject temp = Instantiate(mesa, new Vector3(x, y, z), Quaternion.identity);
		temp.tag = "Mesa";
	}

	public void spawnWalkingPoint(float x, float z)
	{
		//gameObject piso = GameObject.Find("Piso(Clone)");
		float y = puntoDeReferencia.y + 0.1f;
		x = puntoDeReferencia.x + x + 0.40f;
		z = puntoDeReferencia.z + z;
		GameObject temp = Instantiate(walkingPoint, new Vector3(x, y, z), Quaternion.identity);
		temp.tag = "Mesa";
	}
}
