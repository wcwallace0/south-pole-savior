using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    public Font font;

    public int rows;
    public int cols;
    public GameObject[,] grid;

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
        rectTrans.anchorMin = targTrans.anchorMin; 
        rectTrans.anchorMax = targTrans.anchorMax;
        rectTrans.pivot = targTrans.pivot; 

        //rectTrans.anchoredPosition = targTrans.anchoredPosition + offset;
        float targX = targTrans.position.x;
        float targY = targTrans.position.y;
       // Debug.Log("Target X: " + targX + " Target Y: " + targY);
        Vector2 targPos = new Vector2 (targX, targY);
        float offsX = offset.x;
        float offsY = offset.y;

        float tranX = targX + offsX;
        float tranY = targY + offsY;

        Vector2 pos = new Vector2(tranX, tranY);

        //rectTrans.position = canvas.transform.TransformPoint(pos);
        //rectTrans.anchoredPosition = new Vector2(tranX, tranY);
        rectTrans.position = pos;
        

        Text label;
        label = textObj.AddComponent<Text>();

        label.alignment = TextAnchor.UpperLeft;
        label.font = font;//Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        label.text = target.name;
        label.fontSize = 12;
        label.color = Color.white;
        textObj.AddComponent<UnityEngine.UI.Button>();
        textObj.GetComponent<UnityEngine.UI.Button>().interactable = false;

        ContentSizeFitter fitter = label.GetComponent<ContentSizeFitter>();
        if (fitter == null)
        {
            fitter = label.gameObject.AddComponent<ContentSizeFitter>();
        }
        fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        rectTrans.sizeDelta = new Vector2(rectTrans.sizeDelta.x, rectTrans.sizeDelta.y);

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

    private void PositionLabels() {
        for(int i = 0; i<rows; i++) {
            for(int j = 0; j<cols; j++) {
                GameObject file = grid[i,j];
                if(file != null) {
                    RectTransform fileRect = file.GetComponent<RectTransform>();
                    float step = spacing + fileRect.rect.width;
                    fileRect.anchoredPosition = new Vector2((j*step) + padding, (-i*step) - padding);
                }
            }
        }
    }

    IEnumerator Buffer()
    {
        yield return new WaitForSeconds(1.0f);
    }
}
