using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextController : MonoBehaviour {

    public GameObject textBox;
    GameObject textPrefab = null;
    public float textDelay;
    public static string textToDisplay;
    public float yOffset;

    Player player;
    
    // Use this for initialization
	void Start () {
        player = GameObject.FindObjectOfType<Player>();
	}
	
	// Update is called once per frame
	void Update () {
        if (textPrefab != null)
        {
            UpdateTextBoxPosition(textPrefab);

            if (textPrefab.transform.rotation != player.transform.rotation)
                RotateTextBox(textPrefab);
        }
    }

    public void DisplayText()
    {
        if (textPrefab)
            Destroy(textPrefab);

        StartCoroutine(DisplayText(textBox, textToDisplay, textDelay));
    }

    IEnumerator DisplayText(GameObject textObj, string text, float waitTime)
    {
        Vector3 offsetPlayerPos = new Vector3(player.transform.position.x, player.transform.position.y + yOffset, player.transform.position.z);
        textObj = Instantiate(textObj, offsetPlayerPos, Quaternion.identity);
        textPrefab = textObj;
        textPrefab.GetComponent<TextMeshPro>().text = textToDisplay;
        yield return new WaitForSeconds(waitTime);
        Destroy(textObj);
    }

    void RotateTextBox(GameObject textObj)
    {
        textObj.transform.rotation = player.transform.rotation;
    }

    void UpdateTextBoxPosition(GameObject textObj)
    {
        Vector3 offsetPlayerPos = new Vector3(player.transform.position.x, player.transform.position.y + yOffset, player.transform.position.z);
        textObj.transform.position = offsetPlayerPos;
    }
}
