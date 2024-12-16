using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class LabelManager : MonoBehaviour
{
    public List<GameObject> objectsToLabel;
    public List<Text> labels;

    private List<File> fls;

    private List<Folder> flds;

    private Text rootLbl;
    public Vector3 offset = new Vector3(0, 1.5f, 0);

    public Canvas canvas;
    public int rows = 2;
    public int cols = 3;
    public GameObject[,] grid;
    public float spacing;
    public float padding;

    void Start()
    {
        grid = new GameObject[rows,cols];
        Buffer();
        // Find all objects of type Image in the scene
        File[] allFiles = FindObjectsOfType<File>();
        // Debug.Log(allFiles.Length);
        Folder[] allFlds = FindObjectsOfType<Folder>();
        // Convert the array to a List
        fls = new List<File>(allFiles);
        flds = new List<Folder>(allFlds);

        foreach (File fl in fls)
        {
            GameObject go = fl.gameObject;
            objectsToLabel.Add(go);
        }
        // Debug.Log(objectsToLabel.Count);
        foreach (Folder fld in flds)
        {
            GameObject go = fld.gameObject;
            objectsToLabel.Add(go);
        }
        //canvas.transform.localScale = new Vector3(1.4375f, 1.4375f, 1);
        // Debug.Log(objectsToLabel.Count);
        RefreshLabels();
        // Debug.Log(objectsToLabel.Count);
    }

    public void CreateLabel(GameObject target)
    {
        // Text label;
        // // Debug.Log("CreateLabel called");
        // GameObject textObj = new GameObject(target.name + "_Label");
        // // Debug.Log("textObj created. Name of obj: " + textObj.name);
        // // Debug.Log("coords of obj: " + textObj.transform);
        // textObj.transform.SetParent(canvas.transform, false);
        // //textObj.transform.parent = target.transform;
        // label = textObj.AddComponent<Text>();
        // label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        // //label.alignment = TextAnchor.MiddleCenter;
        // label.text = target.name;
        // label.fontSize = 16;
        // label.color = Color.white;

        //  textObj.transform.localPosition = target.transform.position + offset;
        //  //textObj.transform.localScale = Vector3.one * 0.01f;

        //  labels.Add(label);


        Text label;
    // Create a new GameObject for the label
        GameObject textObj = new GameObject(target.name + "_Label");

        // Set the parent to the canvas, ensuring it's in the world space
        textObj.transform.SetParent(canvas.transform, false);  // 'false' keeps local position

        // Add the Text component and set up label
        label = textObj.AddComponent<Text>();
        label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        //label.alignment = TextAnchor.MiddleCenter;
        label.text = target.name;
        Transform trans = target.GetComponent<Button>().transform;
        label.fontSize = 16;
        label.color = Color.white;

        // Position the label relative to the target object
        // The target object's position is in world space, so we need to offset it

        //textObj.transform.localPosition = target.transform.localPosition;
        textObj.transform.localPosition = trans.localPosition;

        // Scale the text object if necessary (optional)
        textObj.transform.localScale = new Vector3(1.4375f, 1.4375f, 1);  // Make sure the scale is correct

        // Add the label to the list
        labels.Add(label);
    }

    public void RefreshLabels()
    {
        Debug.Log("Length of labels: " + labels.Count);
        List<Text> oldLabels = new List<Text>(labels);
        Debug.Log("Length of oldLabels: " + oldLabels.Count);
        labels.Clear();
        Debug.Log("New length of oldLabels: " + oldLabels.Count);
        foreach(Text lbl in oldLabels) {
            Debug.Log("Entered for loop through oldLabels");
            lbl.GetComponent<Text>().enabled = false;
            Destroy(lbl.gameObject);
            Destroy(lbl);
        }
        StartCoroutine(Buffer());
        SetLabels();
    }

    public void AddObject(GameObject obj)
    {
        if (!objectsToLabel.Contains(obj)) {
            objectsToLabel.Add(obj);
        }
    }

    public void SetLabels()
    {
        foreach (GameObject obj in objectsToLabel)
        {
            if (obj.GetComponent<Image>().enabled){
                CreateLabel(obj);
            }
        }

    }

    public void PositionLabels()
    {
        
    }
    IEnumerator Buffer()
    {
        yield return new WaitForSeconds(1.0f);
    }
}
