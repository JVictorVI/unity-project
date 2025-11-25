using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProgressData
{
    public int paginasColetadas;
    public string cenaAtual;
    public float[] posicaoJogador;

    public List<string> itensColetados = new List<string>();

    // 🔥 Novas variáveis para salvar progresso do rádio e livro
    public bool collectedRadio;
    public bool collectedBook;

    public ProgressData(
        int paginas,
        string cena,
        Vector3 posicao,
        List<string> itens,
        bool radio,
        bool book
    )
    {
        paginasColetadas = paginas;
        cenaAtual = cena;
        
        posicaoJogador = new float[3]
        {
            posicao.x,
            posicao.y,
            posicao.z
        };

        itensColetados = itens;

        collectedRadio = radio;
        collectedBook = book;
    }
}
