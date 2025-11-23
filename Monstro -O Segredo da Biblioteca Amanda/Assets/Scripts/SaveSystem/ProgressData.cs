using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProgressData
{
    public int paginasColetadas;
    public string cenaAtual;
    public float[] posicaoJogador;

    public List<string> itensColetados = new List<string>();

    public ProgressData(int paginas, string cena, Vector3 posicao, List<string> itens)
    {
        paginasColetadas = paginas;
        cenaAtual = cena;
        posicaoJogador = new float[3] { posicao.x, posicao.y, posicao.z };
        itensColetados = itens;
    }
}
