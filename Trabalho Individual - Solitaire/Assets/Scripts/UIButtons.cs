using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtons : MonoBehaviour
{
    public GameObject melhorPontuacao;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ReiniciarJogo()
    {
        UpdateSprite[] cartas = FindObjectsOfType<UpdateSprite>();
        foreach (UpdateSprite carta in cartas)
        {
            Destroy(carta.gameObject);
        }
        LimparValoresTopo();
        FindObjectOfType<Solitaire>().Jogar();
    }

    public void LimparValoresTopo()
    {
        Selectable[] selectables = FindObjectsOfType<Selectable>();
        foreach (Selectable selectable in selectables)
        {
            if(selectable.CompareTag("Top"))
            {
                selectable.naipe = null;
                selectable.valor = 0;
            }
        }
    }

    public void JogarNovamente()
    {
        melhorPontuacao.SetActive(false);
        ReiniciarJogo();
    }
}
