using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public GameObject melhorPontuacao;
    public Selectable[] topStacks;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(JogadorVenceu())
        {
            Vitoria();
        }
    }

    public bool JogadorVenceu()
    {
        int i=0;
        foreach(Selectable topstack in topStacks)
        {
            i+= topstack.valor;
        }
        if(i>=52)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void Vitoria(){
        melhorPontuacao.SetActive(true);
        print("Parabéns, você venceu");
    }
}
