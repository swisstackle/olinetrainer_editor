using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.EventSystems;
using SFB;
using System.Threading;
public class DrawLine : MonoBehaviour
{
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private GameObject currentLine;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private List<Vector2> fingerPositions;
    public List<GameObject> lines;


    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject currentPlayer;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Button newPlayerButton;

    [SerializeField] private UnityEngine.UI.Button clearButton;
    [SerializeField] private UnityEngine.UI.Button debugButton;
    [SerializeField] private UnityEngine.UI.Button exportButton;
    [SerializeField] private Button openButton;

    public List<GameObject> players;

    private EdgeCollider2D collider;



    /*
     * <summary>
     * Initializes button listeners and lists
     * </summary>
     */
    void Start()
    {
        players = new List<GameObject>();

        lines = new List<GameObject>();
        UnityEngine.UI.Button btn = clearButton.GetComponent<UnityEngine.UI.Button>();
        btn.onClick.AddListener(Clear);
        

        UnityEngine.UI.Button debug = debugButton.GetComponent<UnityEngine.UI.Button>();
        debug.onClick.AddListener(printLines);

        UnityEngine.UI.Button export = exportButton.GetComponent<UnityEngine.UI.Button>();
        export.onClick.AddListener(Export);

        Button open = openButton.GetComponent<UnityEngine.UI.Button>();
        open.onClick.AddListener(Open);


    }



    // Update is called once per frame

    /*
     * <summary>
     * Checks when to update a line or create a player
     * </summary>
     */
    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
           if (Input.GetMouseButton(0) && (Input.GetKey(KeyCode.LeftShift)) && !(Input.GetKey(KeyCode.LeftControl)))
            {
                try // try catch it because I cant check if mouseposition is on a player or not in this script. I know it's ugly, deal wit it.
                {
                    Vector2 tempFingerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (Vector2.Distance(tempFingerPos, fingerPositions[fingerPositions.Count - 1]) > .1f)
                    {
                        UpdateLine(tempFingerPos);
                    }
                }
                catch
                {

                }

            }
            else if ((Input.GetKey(KeyCode.LeftControl)) && Input.GetMouseButtonDown(0) && !((Input.GetKey(KeyCode.LeftShift))))
            {
                CreatePlayer();
            }


        }
            
    }
    /*
     * <summary>
     * Creates a path/line for a player and adds it to a list.
     * </summary>
     */

    public void CreateLine()
    {
        //Check first if mouse lays on one of the buttons; if so, then dont create the line obviously.
        
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
                currentLine.transform.tag = "Selectable";
                lineRenderer = currentLine.GetComponent<LineRenderer>();
                fingerPositions.Clear();

                fingerPositions.Add(mousePos);
                fingerPositions.Add(mousePos);
                lineRenderer.SetPosition(0, fingerPositions[0]);
                lineRenderer.SetPosition(1, fingerPositions[1]);
                
                collider = currentLine.GetComponent<EdgeCollider2D>();

                collider.SetPoints(fingerPositions);

                lines.Add(currentLine);
            }
        }
    /*
     * <summary>
     * Updates a line and adds all new positions to the line according to the mouseposition. 
     * </summary>
     */
    void UpdateLine(Vector2 newFingerPos)
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            fingerPositions.Add(newFingerPos);
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPos);
            addColliderPoints(newFingerPos);
        }
    }
    /*
     * <summary>
     * Adds collider points. Why do we need a collider? Because the line needs to be selectable so that the player can delete it if necessary.
     * </summary>
     */
    private void addColliderPoints(Vector2 pointToAdd)
    {
        List<Vector2> oldPoints = new List<Vector2>();
        collider.GetPoints(oldPoints);
        oldPoints.Add(pointToAdd);
        collider.SetPoints(oldPoints);

    }
    /*
     * <summary>
     * If you want to add multiple points to the collider. Warning: Not needed nor tested at the moment!
     * </summary>
     */
    private void addColliderPoints(Vector2[] pointsToAdd)
    {
        List<Vector2> oldPoints = new List<Vector2>();
        collider.GetPoints(oldPoints);
        int length = pointsToAdd.Length + collider.pointCount;
        Vector2[] newPoints = new Vector2[length];

        for(int i = 0; i < collider.pointCount; i++)
        {
            newPoints[i] = oldPoints.ElementAt(i);
        }
        for(int i = collider.pointCount; i < length; i++)
        {
            newPoints[i] = pointsToAdd[i - collider.pointCount];
        }

    }
    /*
     * <summary>
     * Delete every player and path
     * </summary>
     */
    void Clear()
    {
        fingerPositions.Clear();
        foreach(GameObject l in lines)
        {
            GameObject.Destroy(l);
        }
        lines.Clear();
        foreach (GameObject l in players)
        {
            GameObject.Destroy(l);
        }
    }
    /*
     * <summary>
     * For debugging.
     * </summary>
     */
    void printLines()
    {
        ClearLog();

        foreach (GameObject line in lines)
        {
            Debug.Log("New Line");
            LineRenderer ren = line.GetComponent<LineRenderer>();
            int count = ren.positionCount;

            for(int i = 0; i < count; i++)
            {
                Debug.Log("Position: "+ren.GetPosition(i)+" "+line.GetHashCode());
            }
        }
    }
    /*
    * <summary>
    * For debugging.
    * </summary>
    */
    public void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
    /*
    * <summary>
    * Export all paths/lines to a json file.
    * </summary>
    */
    void Export()
    {
        //Convert to vector 3 
        List<Vector3[]> tempLines = new List<Vector3[]>();
        foreach (GameObject line in lines){
            LineRenderer tempLineRenderer = line.GetComponent<LineRenderer>();
            int posAmount = tempLineRenderer.positionCount;

            Vector3[] tempPositions = new Vector3[posAmount];
            
            tempLineRenderer.GetPositions(tempPositions);
              
            tempLines.Add(tempPositions);
        }
        Vector3[][] tempArray = tempLines.ToArray();

        var settings = new Newtonsoft.Json.JsonSerializerSettings();
        
        // This tells your serializer that multiple references are okay.
        settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        string json = JsonConvert.SerializeObject(tempArray,settings);
        var extensionFilter = new[]
        {
            new ExtensionFilter("JSON","json")
        };
        string sfd = StandaloneFileBrowser.SaveFilePanel("Save Play", "C:\\Users\\alain\\Documents", "play.json",extensionFilter);
        if(sfd.Length > 0)
        {
            File.WriteAllText(sfd, json);
        }
        

    }

    /*
    * <summary>
    * Open an existing json file and draw the paths.
    * </summary>
    */
    void Open()
    {
        var extensionFilter = new[]
        {
            new ExtensionFilter("JSON","json")
        };
        var paths = StandaloneFileBrowser.OpenFilePanel("Open Play", "" , extensionFilter, false);
        if(paths.Length > 0)
        {
            string path = paths[0];
            var settings = new Newtonsoft.Json.JsonSerializerSettings();

            // This tells your serializer that multiple references are okay.
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            Vector3[][] mylines = JsonConvert.DeserializeObject<Vector3[][]>(File.ReadAllText(path), settings);

            for (int i = 0; i < mylines.GetLength(0); i++)
            {

                currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);


                lineRenderer = currentLine.GetComponent<LineRenderer>();



                lineRenderer.positionCount = 0;
                int innerLength = mylines[i].Length;

                for (int j = 0; j < innerLength; j++)
                {


                    lineRenderer.positionCount++;


                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, mylines[i][j]);



                }
                currentLine.SetActive(true);
            }
        }
    }
    /*
    * <summary>
    * Create a player...
    * </summary>
    */
    void CreatePlayer()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            currentPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            Transform transform = currentPlayer.transform;
            mousePos.z = 1f;
            transform.position = mousePos;
            transform.tag = "Selectable";
            players.Add(currentPlayer);
            
        }

  
    }



}
