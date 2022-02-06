using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{
    public Sprite cartaFrente;
    public Sprite cartaVerso;
    private SpriteRenderer spriteRenderer;
    private Selectable selectable;
    private Solitaire paciencia;
    private UserInput userInput;

    // Start is called before the first frame update
    void Start()
    {
        List<string> baralho = Solitaire.GerarBaralho();
        paciencia = FindObjectOfType<Solitaire>();
        userInput = FindObjectOfType<UserInput>();

        int i = 0;

        foreach(string card in baralho)
        {
            if (this.name == card)
            {
                cartaFrente = paciencia.cartas[i];
                break;
            }
            i++;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        selectable = GetComponent<Selectable>();
    }

    // Update is called once per frame
    void Update()
    {
        if(selectable.viradoCima == true)
        {
            spriteRenderer.sprite = cartaFrente;
        }
        else
        {
            spriteRenderer.sprite = cartaVerso;
        }

        if(userInput.slot1)
        {

            if(name == userInput.slot1.name)
            {
                spriteRenderer.color = Color.yellow;
            }
            else
            {
                spriteRenderer.color = Color.white;
            }
        }   
    }
}
