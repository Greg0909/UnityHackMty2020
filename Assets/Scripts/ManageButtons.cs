using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ManageButtons : MonoBehaviour
{
	public CreateRoom cr;
	public SpawnObjects spawner;
	public InputField largo;
	public InputField ancho;
	public InputField inputMesa;
	public InputField inputUso;
	public InputField inputSusana;
	public Text porcentajeTexto;
	private string jsonResponse="";
	private int mesasPedidas = 0;

	// Start is called before the first frame update
    void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
        if(jsonResponse != "")
        {
			cr.destroyRoom();
			cr.buildRoom(CreateRoom.X, CreateRoom.Y);
			PackRecive data = new PackRecive(jsonResponse);
			

			foreach (var cordenada in data.tables)
			{
				spawner.spawnTable(cordenada.Item1, cordenada.Item2);
			}

			foreach (var puntos in data.wpoints)
			{
				spawner.spawnWalkingPoint(puntos.Item1, puntos.Item2);
			}

			float p = ( (data.tables.Count*1.0f) / mesasPedidas) * 100;
			
			porcentajeTexto.text = "Porcentaje de uso: " + p.ToString("#.##") + "%";
			jsonResponse = "";
		}
    }




	IEnumerator Upload(string json)
	{
		//WWWForm form = new WWWForm();
		//form.AddField("Body", json);

		using (UnityWebRequest www = UnityWebRequest.Post("https://artificialhacks-grdpxbotjq-ue.a.run.app/generate", json))
		{
			yield return www.SendWebRequest();

			if (www.isNetworkError || www.isHttpError)
			{
				Debug.Log(www.error);
			}
			else
			{
				Debug.Log("Form upload complete!");
			}
		}
	}

	IEnumerator Post(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
		
        Debug.Log("Status Code: " + request.responseCode);
		jsonResponse = request.downloadHandler.text;
		Debug.Log("Respuesta " + request.downloadHandler.text);
	}

	// Crea el Json
	/*
	{
	"tableSize" : 0.8,
	"tableNumber": 15,
	"threshold": 100,
	"distance": 1.5,
	"walkingPath":"E",
	"polygonName":"box1"
	}
	*/
	public async void calculateThreshold()
	{
		try
		{
			int numeroMesas = int.Parse(inputMesa.text);
			float porcentajeUso = float.Parse(inputUso.text);
			float minimaDistancia = float.Parse(inputSusana.text);
			mesasPedidas = numeroMesas;

			PackSender info = new PackSender();
			info.tableSize = 1.6f;
			info.tableNumber = numeroMesas;
			info.threshold = porcentajeUso;
			info.distance = minimaDistancia + 1.0f;
			info.walkingPath = "E";
			info.polygonName = "box1";
			info.scaleX = CreateRoom.X;
			info.scaleY = CreateRoom.Y;
			string jsonString;
			jsonString = JsonUtility.ToJson(info);
			Debug.Log(jsonString);
			// Envia los parametros a Mario XD
			StartCoroutine(Post("https://artificialhacks-grdpxbotjq-ue.a.run.app/generate", jsonString));
		}
		catch
		{
			showToast("Error en los Parametros del\n Calculo", 2);
		}
	}

	public void roomCreation()
	{
		try
		{
			float x = float.Parse(ancho.text);
			float y = float.Parse(largo.text);
			cr.destroyRoom();
			cr.buildRoom(x,y);
		}
		catch
		{
			showToast("Error en los Parametros de\nReformacion", 2);
		}

	}

	public Text txt;

	void showToast(string text,
		int duration)
	{
		StartCoroutine(showToastCOR(text, duration));
	}

	private IEnumerator showToastCOR(string text,
		int duration)
	{
		Color orginalColor = txt.color;

		txt.text = text;
		txt.enabled = true;

		//Fade in
		yield return fadeInAndOut(txt, true, 0.5f);

		//Wait for the duration
		float counter = 0;
		while (counter < duration)
		{
			counter += Time.deltaTime;
			yield return null;
		}

		//Fade out
		yield return fadeInAndOut(txt, false, 0.5f);

		txt.enabled = false;
		txt.color = orginalColor;
	}

	IEnumerator fadeInAndOut(Text targetText, bool fadeIn, float duration)
	{
		//Set Values depending on if fadeIn or fadeOut
		float a, b;
		if (fadeIn)
		{
			a = 0f;
			b = 1f;
		}
		else
		{
			a = 1f;
			b = 0f;
		}

		Color currentColor = Color.clear;
		float counter = 0f;

		while (counter < duration)
		{
			counter += Time.deltaTime;
			float alpha = Mathf.Lerp(a, b, counter / duration);

			targetText.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
			yield return null;
		}
	}
}


[Serializable]
public class PackSender
{
	public float tableSize;
	public int tableNumber;
	public float threshold;
	public float distance;
	public string walkingPath;
	public string polygonName;
	public float scaleX;
	public float scaleY;
}

[Serializable]
public class PackRecive
{
	//"tables":[[11.50164255932024,11.18923924474236],[8.480896001032553,10.852999796028474],
	//[6.20160288895177,11.405218416348783],[3.557414130105881,10.824376848452209],[11.35381171418713,3.6015504194327534]]
	public List<Tuple<float,float>> tables = new List<Tuple<float, float>>();
	public List<Tuple<float, float>> wpoints = new List<Tuple<float, float>>();
	public PackRecive(string jsonString)
	{
		walkingPath(jsonString);
		int p1 = jsonString.IndexOf("tables") + 10;
		int p2 = jsonString.IndexOf(']', p1)-1;
		int end = jsonString.IndexOf("]]", p1);

		while(p2 < end)
        {
			string temp = jsonString.Substring(p1, p2 - p1);
			string num1 = temp.Substring(0, temp.IndexOf(','));
			string num2 = temp.Substring(temp.IndexOf(',')+1);


			tables.Add(new Tuple<float, float>(float.Parse(num1), float.Parse(num2)));

			

			p1 = jsonString.IndexOf('[', p1) + 1;
			p2 = jsonString.IndexOf(']', p1) -1;
			

		}
	}

	public void walkingPath(string jsonString)
    {
		int p1 = jsonString.IndexOf("walkingPath") + 15;
		int p2 = jsonString.IndexOf(']', p1) - 1;
		int end = jsonString.IndexOf("]]", p1);

		while (p2+1 < end)
		{
			string temp = jsonString.Substring(p1, p2 - p1+1);
			Debug.Log(temp);
			string num1 = temp.Substring(0, temp.IndexOf(','));
			string num2 = temp.Substring(temp.IndexOf(',') + 1);


			wpoints.Add(new Tuple<float, float>(float.Parse(num1), float.Parse(num2)));



			p1 = jsonString.IndexOf('[', p1) + 1;
			p2 = jsonString.IndexOf(']', p1) - 1;


		}
	}
}