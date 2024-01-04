
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public string ID;

    private void Awake()
    {
        ID = gameObject.name; //+ transform.position.ToString();
    }

    private void Start()
    {
        for(int i = 0; i < Object.FindObjectsOfType<DontDestroy>().Length; i++)
        {
            if (Object.FindObjectsOfType<DontDestroy>()[i].ID == ID && Object.FindObjectsOfType<DontDestroy>()[i] != this)
            {
                Destroy(this.gameObject);
            }
        }

        DontDestroyOnLoad(this.gameObject);
    }

}
