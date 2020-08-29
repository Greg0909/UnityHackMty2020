using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
	public GameObject mesa;
	private Vector3 puntoDeReferencia;

	int px=2, pz=2;

    // Start is called before the first frame update
    void Start()
    {
		puntoDeReferencia = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
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

	void spawnTable(float x, float z)
	{
		float y = 5.3f;
		x = puntoDeReferencia.x + x;
		z = puntoDeReferencia.z + z;
		Instantiate(mesa, new Vector3(x, y, z), Quaternion.identity);
	}
}
