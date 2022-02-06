using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Solitaire : MonoBehaviour
{
    public Sprite[] cartas;
    public GameObject cartaPrefab;
    public GameObject cartaBaralho;
    public GameObject[] bottomPos;
    public GameObject[] topPos;
    public static string[] naipe = { "P", "O", "C", "E" };
    public static string[] valor = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
    public List<string>[] bottoms;
    public List<string>[] tops;
    public List<string> jogadaNaTela = new List<string>();
    public List<List<string>> jogadasBaralho = new List<List<string>>();
    private List<string> bottom0 = new List<string>();
    private List<string> bottom1 = new List<string>();
    private List<string> bottom2 = new List<string>();
    private List<string> bottom3 = new List<string>();
    private List<string> bottom4 = new List<string>();
    private List<string> bottom5 = new List<string>();
    private List<string> bottom6 = new List<string>();
    public List<string> baralho;
    public List<string> pilhaDescarte = new List<string>();
    private int localizacaoBaralho;
    private int jogadas;
    private int jogadasRestantes;


    // Start is called before the first frame update
    void Start()
    {
        bottoms = new List<string>[] {bottom0, bottom1, bottom2, bottom3, bottom4, bottom5, bottom6};
        Jogar();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Jogar()
    {
        baralho = GerarBaralho();
        Embaralhar(baralho);
        foreach (string card in baralho) print(card);
        OrdenarJogo();
        StartCoroutine(GerarCartas());
        EmbaralharPorJogada();
    }

    public static List<string> GerarBaralho()
    {
        List<string> novoBaralho = new List<string>();
        foreach (string n in naipe)
        {
            foreach (string v in valor)
            {
                novoBaralho.Add(n + v);
            }
        }
        return novoBaralho;
    }

    public void Embaralhar<T>(List<T> baralho)
    {
        System.Random random = new System.Random();
        int n = baralho.Count;
        while (n > 1)
        {
            int k = random.Next(n);
            n--;
            T temp = baralho[k];
            baralho[k] = baralho[n];
            baralho[n] = temp;
        }
    }

    IEnumerator GerarCartas()
    {
        for(int i = 0; i < 7; i++)
        {
            float yOffset = 0;
            float zOffset = 0.03f;

            foreach(string carta in bottoms[i])
            {
                yield return new WaitForSeconds(0.01f);
                GameObject novaCarta = Instantiate(
                    cartaPrefab,
                    new Vector3(bottomPos[i].transform.position.x,bottomPos[i].transform.position.y - yOffset, bottomPos[i].transform.position.z - zOffset),
                    Quaternion.identity,
                    bottomPos[i].transform
                );
                novaCarta.name = carta;
                novaCarta.GetComponent<Selectable>().fila = i;

                if(carta == bottoms[i][bottoms[i].Count -1])    novaCarta.GetComponent<Selectable>().viradoCima = true;

                yOffset = yOffset + 0.3f;
                zOffset = zOffset + 0.03f;
                pilhaDescarte.Add(carta);
            }
        }

        foreach(string carta in pilhaDescarte)
        {
            if(baralho.Contains(carta))
            {
                baralho.Remove(carta);
            }
        }
        pilhaDescarte.Clear();
    }
    void OrdenarJogo()
    {
        for(int i = 0; i < 7; i++)
        {
            for(int j=i; j < 7 ; j++)
            {
                bottoms[j].Add(baralho.Last<string>());
                baralho.RemoveAt(baralho.Count - 1);
            }
        }
    }
    public void EmbaralharPorJogada()
    {
        jogadas = baralho.Count / 3;
        jogadasRestantes = baralho.Count % 3;
        jogadasBaralho.Clear();
        int modifier = 0;
        for (int i=0; i < jogadas; i++)
        {
            List<string> minhaJogada = new List<string>();
            for (int j=0; j < 3; j++)
            {
                minhaJogada.Add(baralho[j + modifier]);
            }
            jogadasBaralho.Add(minhaJogada);
            modifier += 3;
        }
        if(jogadasRestantes != 0)
        {
            List<string> jogadaRestantes = new List<string>();
            modifier = 0;
            for(int k=0; k < jogadasRestantes; k++)
            {
                jogadaRestantes.Add(baralho[baralho.Count - jogadasRestantes + modifier]);
                modifier++;
            }
            jogadasBaralho.Add(jogadaRestantes);
            jogadas++;
        }
        localizacaoBaralho = 0;
    }

    public void DealFromDeck()
    {
        foreach(Transform filho in cartaBaralho.transform)
        {
            if(filho.CompareTag("Card"))
            {
                baralho.Remove(filho.name);
                pilhaDescarte.Add(filho.name);
                Destroy(filho.gameObject);
            }
        }
        if(localizacaoBaralho < jogadas)
        {
            jogadaNaTela.Clear();
            float xOffset = 2.5f;
            float zOffset = -0.2f;

            foreach(string carta in jogadasBaralho[localizacaoBaralho])
            {
                GameObject novaCarta = Instantiate(cartaPrefab, new Vector3(cartaBaralho.transform.position.x + xOffset, cartaBaralho.transform.position.y, cartaBaralho.transform.position.z + zOffset), Quaternion.identity, cartaBaralho.transform);
                xOffset += 0.5f;
                zOffset -= 0.2f; 
                novaCarta.name = carta;
                jogadaNaTela.Add(carta);
                novaCarta.GetComponent<Selectable>().viradoCima = true; 
                novaCarta.GetComponent<Selectable>().estaEmpilhadoBaralho = true; 
            }
            localizacaoBaralho++;
        }
        else
        {
            ReiniciarBaralho();
        }
    }
    void ReiniciarBaralho()
    {
        baralho.Clear();
        foreach(string carta in pilhaDescarte)
        {
            baralho.Add(carta);
        }
        pilhaDescarte.Clear();
        EmbaralharPorJogada();
    }
}
