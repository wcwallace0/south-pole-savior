using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LabelManager : MonoBehaviour
{
    public List<GameObject> objectsToLabel;
    public List<Text> labels;

    private List<File> fls;

    private List<Folder> flds;

    private Text rootLbl;
    public Vector2 offset;

    public Canvas canvas;
    public float spacing;
    public float padding;

    void Start()
    {
       // Debug.Log("X Offset: " + offset.x + "Y Offset: " + offset.y);

        Buffer();

        File[] allFiles = FindObjectsOfType<File>();
        Folder[] allFlds = FindObjectsOfType<Folder>();
        fls = new List<File>(allFiles);
        flds = new List<Folder>(allFlds);

        foreach (File fl in fls)
        {
            GameObject go = fl.gameObject;
            objectsToLabel.Add(go);
        }

        foreach (Folder fld in flds)
        {
            GameObject go = fld.gameObject;
            objectsToLabel.Add(go);
        }
        canvas.transform.localScale = new Vector3(1.4375f, 1.4375f, 1.4375f);

        RefreshLabels();

    }

    public void CreateLabel(GameObject target)
    {
        GameObject textObj = new GameObject(target.name + "_Label");
        RectTransform rectTrans = textObj.AddComponent<RectTransform>();
        RectTransform targTrans = target.GetComponent<RectTransform>();

        rectTrans.AddComponent<CanvasRenderer>();

        rectTrans.localScale = targTrans.localScale;
        rectTrans.SetParent(canvas.transform, false);
        rectTrans.anchorMin = targTrans.anchorMin; //new Vector2(0,1);
        rectTrans.anchorMax = targTrans.anchorMax;
        rectTrans.pivot = targTrans.pivot; //new Vector2(0,1);

        //rectTrans.anchoredPosition = targTrans.anchoredPosition + offset;
        float targX = targTrans.position.x;
        float targY = targTrans.position.y;
       // Debug.Log("Target X: " + targX + " Target Y: " + targY);
        Vector2 targPos = new Vector2 (targX, targY);
        float offsX = offset.x;
        float offsY = offset.y;

        float tranX = targX + offsX;
        float tranY = targY + offsY;
        Debug.Log("Label X: " + tranX + " Label Y: " + tranY);

        Vector2 pos = new Vector2(tranX, tranY);

        //rectTrans.position = canvas.transform.TransformPoint(pos);
        //rectTrans.anchoredPosition = new Vector2(tranX, tranY);
        rectTrans.position = pos;
        

        Text label;
        label = textObj.AddComponent<Text>();

        label.alignment = TextAnchor.UpperLeft;
        label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        label.text = target.name;
        label.fontSize = 16;
        label.color = Color.white;

        labels.Add(label);
    }

    public void RefreshLabels()
    {
       // Debug.Log("Length of labels: " + labels.Count);
        List<Text> oldLabels = new List<Text>(labels);
       // Debug.Log("Length of oldLabels: " + oldLabels.Count);
        labels.Clear();
       // Debug.Log("New length of oldLabels: " + oldLabels.Count);
        foreach(Text lbl in oldLabels) {
            //Debug.Log("Entered for loop through oldLabels");
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
            if (obj.GetComponent<UnityEngine.UI.Image>().enabled){
                CreateLabel(obj);
            }
        }

    }
    IEnumerator Buffer()
    {
        yield return new WaitForSeconds(1.0f);
    }
}
