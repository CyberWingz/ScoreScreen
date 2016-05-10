using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewScore : MonoBehaviour {
	
	[SerializeField] List<SequentialEffect> sequence;

	int tickId = 0;

	public List<KeyValuePair<int,int>> ListTick = new  List<KeyValuePair<int,int>>();

	public void AddTickValues(int a, int b)
	{
		ListTick.Add(new KeyValuePair<int,int>(a,b));
	}

	// Use this for initialization
	void Start () {


		AddTickValues(0, 100); 
		AddTickValues(100, 200); 

		Show();
	}


	// Update is called once per frame
	void Update () {
	
	}

	public void Show()
	{
		transform.GetChild(0).gameObject.SetActive(true);
		tickId = 0;
		GetComponent<Animator>().SetInteger("Phase", tickId);
	}

	public void StartTick()
	{
		StartCoroutine( iTick() );
	}

	IEnumerator iTick()
	{
		yield return StartCoroutine(iTickText(sequence[tickId], tickId));
		++tickId;
		GetComponent<Animator>().SetInteger("Phase", tickId);
	}


	IEnumerator iTickText(SequentialEffect effect, int tickId)
	{
		float t = 0;

		while(t < effect.duration)
		{
			ApplyTickText(effect, tickId, t);
			t += Time.deltaTime;

			print(effect.tag + " " + t);
			yield return null;
		}

		ApplyTickText(effect, tickId, effect.duration);
	}


	private void ApplyTickText(SequentialEffect effect, int tickId, float time)
	{
		float curveVal = effect.curve1.Evaluate(time);
		print(tickId);
		int value = (int)Mathf.Lerp(ListTick[tickId].Key, ListTick[tickId].Value, curveVal);

		effect.text.text = value.ToString();
	}
}
