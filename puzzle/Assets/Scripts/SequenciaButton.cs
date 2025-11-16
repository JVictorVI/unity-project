using UnityEngine;

public class SequenciaButton : MonoBehaviour
{
    public int id; // número deste botão
    public SequenciaPuzzle puzzle; // referência ao puzzle principal

    void OnMouseDown()
    {
        if (puzzle != null)
            puzzle.Press(id);
    }
}
