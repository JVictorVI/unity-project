using UnityEngine;

[CreateAssetMenu(fileName = "NovaNota", menuName = "Itens/Nota")]
public class Note : ScriptableObject
{

    [TextArea(4, 10)]
    public string conteudo;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
