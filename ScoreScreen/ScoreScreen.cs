using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ScoreScreen))]
public class ScoreScreenEditor: Editor
{
	List<bool> m_collapse;

	List<bool> collapse
	{
		get
		{
			if(m_collapse == null)
				m_collapse = new List<bool>( new bool[sequenceProp.arraySize]);

			return m_collapse;
		}

	}


	SerializedProperty sequenceProp;
	SerializedObject GetTarget;

	void OnEnable () {
		// Setup the SerializedProperties.

		GetTarget = new SerializedObject(target);
		sequenceProp = serializedObject.FindProperty ("sequence");


	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update ();
		Show(sequenceProp);
	

		serializedObject.ApplyModifiedProperties ();
	}

	public void Show (SerializedProperty list) {

		int ListSize = list.arraySize;
		 ListSize = EditorGUILayout.IntField ("List Size", ListSize);

		if(ListSize != list.arraySize){
			while(ListSize > list.arraySize){
				list.InsertArrayElementAtIndex(list.arraySize);
				m_collapse.Add(false);
			}
			while(ListSize < list.arraySize){
				list.DeleteArrayElementAtIndex(list.arraySize - 1);
				m_collapse.RemoveAt(m_collapse.Count - 1);

			}
		}

		 
		int DisplayFieldType = 1;


		for(int i = 0; i < list.arraySize; i++)
		{
			SerializedProperty MyListRef  = list.GetArrayElementAtIndex(i);
			SerializedProperty MyTag = MyListRef.FindPropertyRelative("tag");
			SerializedProperty MyType = MyListRef.FindPropertyRelative("type");
		

			string strFoldOut = string.IsNullOrEmpty( MyTag.stringValue) ? "New" : MyTag.stringValue;
			collapse[i] = EditorGUILayout.Foldout(collapse[i], strFoldOut);

			if(collapse[i])
				continue;
			
			EditorGUI.indentLevel += 1;

			MyTag.stringValue = EditorGUILayout.TextField("Tag", MyTag.stringValue);
			MyType.enumValueIndex = EditorGUILayout.Popup("Type", MyType.enumValueIndex, MyType.enumDisplayNames);

			SerializedProperty MyCurve1 = MyListRef.FindPropertyRelative("curve1");
			SerializedProperty MyCurve2 = MyListRef.FindPropertyRelative("curve2");
			SerializedProperty MyGradient = MyListRef.FindPropertyRelative("gradient");

			if(MyType.enumValueIndex == (int)SA_Type.SCALE)
			{
				SerializedProperty MyRectT = MyListRef.FindPropertyRelative("rectT");


				MyRectT.objectReferenceValue = EditorGUILayout.ObjectField("Obj", MyRectT.objectReferenceValue, typeof(RectTransform), true);
				MyCurve1.animationCurveValue = EditorGUILayout.CurveField("Curve X", MyCurve1.animationCurveValue);
				MyCurve2.animationCurveValue = EditorGUILayout.CurveField("Curve Y", MyCurve2.animationCurveValue);
			}
			else if(MyType.enumValueIndex == (int)SA_Type.TRANSLATE)
			{
				SerializedProperty MyRectT = MyListRef.FindPropertyRelative("rectT");
				SerializedProperty MyGoalT = MyListRef.FindPropertyRelative("goalT");



				MyRectT.objectReferenceValue = EditorGUILayout.ObjectField("Obj", MyRectT.objectReferenceValue, typeof(RectTransform), true);
				MyGoalT.objectReferenceValue = EditorGUILayout.ObjectField("Goal", MyGoalT.objectReferenceValue, typeof(RectTransform), true);

				MyCurve1.animationCurveValue = EditorGUILayout.CurveField("Curve", MyCurve1.animationCurveValue);
			}
			else if(MyType.enumValueIndex == (int)SA_Type.COLOR_IMAGE)
			{
				SerializedProperty MyImage = MyListRef.FindPropertyRelative("image");

				MyImage.objectReferenceValue = EditorGUILayout.ObjectField("Image", MyImage.objectReferenceValue, typeof(Image), true);
				EditorGUILayout.PropertyField(MyGradient, true);

				MyCurve1.animationCurveValue = EditorGUILayout.CurveField("Curve", MyCurve1.animationCurveValue);
			}
			else if(MyType.enumValueIndex == (int)SA_Type.COLOR_TEXT)
			{
				SerializedProperty MyText = MyListRef.FindPropertyRelative("text");

				MyText.objectReferenceValue = EditorGUILayout.ObjectField("Text", MyText.objectReferenceValue, typeof(Text), true);
				EditorGUILayout.PropertyField(MyGradient, true);


				MyCurve1.animationCurveValue = EditorGUILayout.CurveField("Curve", MyCurve1.animationCurveValue);
			}
			else if(MyType.enumValueIndex == (int)SA_Type.TICK_TEXT)
			{
				SerializedProperty MyText = MyListRef.FindPropertyRelative("text");

				MyText.objectReferenceValue = EditorGUILayout.ObjectField("Text", MyText.objectReferenceValue, typeof(Text), true);
				MyCurve1.animationCurveValue = EditorGUILayout.CurveField("Curve", MyCurve1.animationCurveValue);
			}

			SerializedProperty MyAsync = MyListRef.FindPropertyRelative("async");
			SerializedProperty MyDur = MyListRef.FindPropertyRelative("duration");
			SerializedProperty MyDelay = MyListRef.FindPropertyRelative("delay");

			MyAsync.boolValue = EditorGUILayout.Toggle("Async", MyAsync.boolValue);
			MyDur.floatValue = EditorGUILayout.FloatField("Dur", MyDur.floatValue);
			MyDelay.floatValue = EditorGUILayout.FloatField("Delay", MyDelay.floatValue);

		

			EditorGUILayout.Space ();

			EditorGUILayout.BeginHorizontal();


			if(GUILayout.Button("R")){

				MyCurve1.animationCurveValue = AnimationCurve.Linear(0,0,1,1);
				MyCurve2.animationCurveValue = AnimationCurve.Linear(0,0,1,1);
			}

			if(GUILayout.Button("▲")){
				list.MoveArrayElement(i, i-1);
			}
			if(GUILayout.Button("▼")){
				list.MoveArrayElement(i, i+1);
			}
			if(GUILayout.Button("Del")){
				list.DeleteArrayElementAtIndex(i);
				m_collapse.RemoveAt(m_collapse.Count - 1);
			}


			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();

			EditorGUI.indentLevel -= 1;
		}



	}
}
#endif

public enum SA_Type
{
	SCALE,
	TRANSLATE,
	COLOR_TEXT,
	COLOR_IMAGE,
	TICK_TEXT,

}

[System.Serializable]
public class SequentialEffect
{
	public string tag;
	public SA_Type type;

	public RectTransform rectT;
	public RectTransform goalT;
	public Image image;
	public Text text;
	public AnimationCurve curve1 = AnimationCurve.Linear(0,0,1,1);
	public AnimationCurve curve2 = AnimationCurve.Linear(0,0,1,1);
	public Gradient gradient;
	public bool async;
	public float delay;
	public float duration;
	public Vector3 origin;
}

public class ScoreScreen : MonoBehaviour {

	public int startNum = 0;
	public int sequenceNum = 0;
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

	public void Show()
	{
		transform.GetChild(0).gameObject.SetActive(true);
		StartCoroutine( iShow() );
	}

	IEnumerator iShow()
	{
		tickId = 0;
		startNum = Mathf.Clamp(startNum, 0, sequence.Count - 1);

		for(int i = 0; i < startNum; ++i)
		{
			ApplyEffect(sequence[i], sequence[i].duration);
		}

		for(int i = startNum; i < sequence.Count; ++i)
		{
			ApplyEffect(sequence[i], 0);
		}

		tickId = 0;

		for(int i = startNum; i < sequence.Count; ++i)
		{
			sequenceNum = i;
			Coroutine coro = StartCoroutine(iEffect(sequence[i]));

			if(!sequence[i].async)
				yield return coro;

			if(sequence[i].delay > 0)
				yield return new WaitForSeconds(sequence[i].delay);
		}
	}

	IEnumerator iEffect(SequentialEffect effect)
	{
		switch(effect.type)
		{
			case SA_Type.SCALE:
				return iScale( effect );
			case SA_Type.TRANSLATE:
				return iTranslate( effect );
			case SA_Type.COLOR_TEXT:
				return iColorText( effect );
			case SA_Type.COLOR_IMAGE:
				return iColorImage( effect );
			case SA_Type.TICK_TEXT:
				int tickCurr = tickId;
				++tickId;
				return iTickText( effect, tickCurr );
		}

		return iBlank();
	}

	IEnumerator iBlank()
	{
		yield return null;
	}

	IEnumerator iScale(SequentialEffect effect)
	{
		float t = 0;

		while(t < effect.duration)
		{
			ApplyScale(effect, t);
			t += Time.deltaTime;

			print(effect.tag + " " + t);
			yield return null;
		}

		ApplyScale(effect, effect.duration);
	}


	private void ApplyScale(SequentialEffect effect, float time)
	{
		float x = effect.curve1.Evaluate(time);
		float y = effect.curve2.Evaluate(time);
		Vector2 scale = new Vector2(x, y);
		effect.rectT.localScale = scale;
	}

	IEnumerator iTranslate(SequentialEffect effect)
	{
		float t = 0;

		effect.origin = effect.rectT.anchoredPosition;

		while(t < effect.duration)
		{
			ApplyTranslate(effect, t);
			t += Time.deltaTime;

			print(effect.tag + " " + t);
			yield return null;
		}

		ApplyTranslate(effect, effect.duration);
	}


	private void ApplyTranslate(SequentialEffect effect, float time)
	{
		if(effect.rectT != null && effect.goalT != null)
		{
			float curveVal = effect.curve1.Evaluate(time);
			effect.rectT.anchoredPosition = Vector2.Lerp(effect.origin, effect.goalT.anchoredPosition, curveVal);
		}
	}

	IEnumerator iColorText(SequentialEffect effect)
	{
		float t = 0;

		while(t < effect.duration)
		{
			ApplyColorText(effect, t);
			t += Time.deltaTime;

			print(effect.tag + " " + t);
			yield return null;
		}

		ApplyColorText(effect, effect.duration);
	}


	private void ApplyColorText(SequentialEffect effect, float time)
	{
		float curveVal = effect.curve1.Evaluate(time);
		effect.text.color = effect.gradient.Evaluate(curveVal);
	}

	IEnumerator iColorImage(SequentialEffect effect)
	{
		float t = 0;

		while(t < effect.duration)
		{
			ApplyColorImage(effect, t);
			t += Time.deltaTime;

			print(effect.tag + " " + t);
			yield return null;
		}

		ApplyColorImage(effect, effect.duration);
	}


	private void ApplyColorImage(SequentialEffect effect, float time)
	{
		float curveVal = effect.curve1.Evaluate(time);
		effect.image.color = effect.gradient.Evaluate(curveVal);
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

	private void ApplyEffect(SequentialEffect effect, float time)
	{
		switch(effect.type)
		{
			case SA_Type.SCALE:
				ApplyScale(effect, time);
				return;

			case SA_Type.TRANSLATE:
				ApplyTranslate( effect, time);
				return;

			case SA_Type.COLOR_IMAGE:
				ApplyColorImage(effect, time);
				return;

			case SA_Type.COLOR_TEXT:
				ApplyColorText( effect, time);
				return;

			case SA_Type.TICK_TEXT:
				ApplyTickText( effect, tickId, time);
				++tickId;
				return;

		}
	}

}
