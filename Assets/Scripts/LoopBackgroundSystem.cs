using TMPro;
using UnityEngine;

[System.Serializable]
public class Layer
{
    [SerializeField] private float layerSpeed = 5;
    [SerializeField] private GameObject obj;

    public Bounds bounds;
    private Transform[] elements;
    
    public void LoadLayer(Bounds screenBounds, Transform parent = null)
    {
        var boundsComponent = obj.GetComponent<BoundsComponent>();
        
        var numElement = Mathf.Max(2,Mathf.CeilToInt(screenBounds.size.y * 2 / boundsComponent.size.y));
        var initialPosition = obj.transform.position;

        bounds = new Bounds(boundsComponent.offset, boundsComponent.size);
        elements = new Transform[numElement];
        for (var i = 0; i < numElement; ++i)
        {
            var element = Object.Instantiate(obj.gameObject, initialPosition + bounds.size.y * i * Vector3.up,
                Quaternion.identity, parent);
            element.name = obj.name + i;
            
            elements[i] = element.transform;
        }
        
        Object.Destroy(obj);
    }

    public void Update(Bounds screenBounds, float speed)
    {
        if (elements == null || elements.Length == 0)
            return;
        
        var delta = (Time.deltaTime * layerSpeed * speed) * Vector3.down;
        
        for (var i = 0; i < elements.Length; ++i)
        {
            var newPosition = elements[i].transform.position + delta;

            if (speed > 0 && (screenBounds.center - newPosition).y > (bounds.extents.y + screenBounds.size.y))
            {
                newPosition.y += bounds.size.y * elements.Length;
            } 
            else if (speed < 0 && (newPosition - screenBounds.center).y > (bounds.extents.y + screenBounds.size.y))
            {
                newPosition.y -= bounds.size.y * elements.Length;
            }
            
            elements[i].position = newPosition;
        }
    }
}

public class LoopBackgroundSystem : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Layer[] layers;
    
    [SerializeField] private float damping = 1;
    [SerializeField] private float newSpeed = 0;

    private float speed;
    public TextMeshProUGUI Speed_TM;

    private Bounds screenBounds;
    
    private void Start()
    {
        if (!mainCamera)
        {
            mainCamera = Camera.main;
        }

        screenBounds = new Bounds(mainCamera.transform.position,
            mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z)));

        
        
        for (var i = 0; i < layers.Length; ++i)
        {
            layers[i]?.LoadLayer(screenBounds, transform);
        }
    }

    public void SetSpeed(float s)
    {
        newSpeed = s;
        Speed_TM.text = "Speed: " + newSpeed;
    }
    
    void Update()
    {
        speed = Mathf.Lerp(speed, newSpeed, damping * Time.deltaTime);
        for (var i = 0; i < layers.Length; ++i)
        {
            layers[i]?.Update(screenBounds, speed);
        }
    }
}
