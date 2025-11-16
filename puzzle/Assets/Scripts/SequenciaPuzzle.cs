using UnityEngine;

public class SequenciaPuzzle : MonoBehaviour
{
    [Header("Sequência correta")]
    public int[] sequence = { 2, 4, 1, 3 };

    [Header("Som")]
    public AudioSource sfx;
    public AudioClip clickClip;
    public AudioClip errorClip;
    public AudioClip okClip;

    [Header("Ação final")]
    public DoorController doorToUnlock;

    int index = 0;

    public void Press(int n)
    {
        Debug.Log("[Puzzle] Pressionado: " + n);

        if (sfx && clickClip) sfx.PlayOneShot(clickClip);

        if (n == sequence[index])
        {
            index++;

            // terminou a sequência
            if (index >= sequence.Length)
            {
                Debug.Log("[Puzzle] Sequência correta!");

                if (sfx && okClip) sfx.PlayOneShot(okClip);

                if (doorToUnlock != null)
                    doorToUnlock.Open(false);

                index = 0;
            }
        }
        else
        {
            Debug.Log("[Puzzle] ERRO. Reiniciando sequência.");

            if (sfx && errorClip) sfx.PlayOneShot(errorClip);

            index = 0;
        }
    }
}
