using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    public GameObject slot1;
    private Solitaire solitaire;
    private float cronometro;
    private float duploCliqueTempo;
    private int contadorCliques = 0;

    // Start is called before the first frame update
    void Start()
    {
        solitaire = FindObjectOfType<Solitaire>();
        slot1 = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(contadorCliques == 1)
        {
            cronometro += Time.deltaTime; 
        }
        if(contadorCliques == 3)
        {
            cronometro = 0;
            contadorCliques = 1;
        }
        if(cronometro > duploCliqueTempo)
        {
            cronometro = 0;
            contadorCliques = 0;
        }
        
        GetMouseClick();
    }
    
    void GetMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            contadorCliques++;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                if (hit.collider.CompareTag("Deck"))
                {
                    Baralho();
                }
                else if (hit.collider.CompareTag("Card"))
                {
                    Carta(hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag("Top"))
                {
                    Topo(hit.collider.gameObject);              
                }
                else if (hit.collider.CompareTag("Bottom"))
                {
                    Fundo(hit.collider.gameObject);
                }
            }
        }
    }

    void Baralho()
    {
        solitaire.AcoesBaralho();
        slot1 = this.gameObject;
    }

    void Carta(GameObject cartaSelecionada)
    {
        if(!cartaSelecionada.GetComponent<Selectable>().viradoCima)
        {
            if(!Bloqueado(cartaSelecionada))
            {
                cartaSelecionada.GetComponent<Selectable>().viradoCima = true;
                slot1 = this.gameObject;
            }
        }
        else if(cartaSelecionada.GetComponent<Selectable>().estaEmpilhadoBaralho)
        {
            if(!Bloqueado(cartaSelecionada))
            {
                if(slot1 == cartaSelecionada)
                {
                    if(DuploClique())
                    {
                        AutoEmpilhar(cartaSelecionada);
                    }
                }
                else
                {
                    slot1 = cartaSelecionada;
                }
            }
        }
        else
        {
            if (slot1 == this.gameObject)
            {
                slot1 = cartaSelecionada;
            }

            else if (slot1 != cartaSelecionada)
            {
                if (Empilhavel(cartaSelecionada))
                {
                    Empilhar(cartaSelecionada);
                }
                else
                {
                    slot1 = cartaSelecionada;
                }
            }
            else if(slot1 == cartaSelecionada)
            {
                if(DuploClique())
                {

                }
            }
        }
    }
    
    void Fundo(GameObject cartaSelecionada)
    {
        if(slot1.CompareTag("Card"))
        {
            if(slot1.GetComponent<Selectable>().valor == 13)
            {
                Empilhar(cartaSelecionada);
            }
        }
    }
    
    void Topo(GameObject cartaSelecionada)
    {
        if(slot1.CompareTag("Card"))
        {
            if(slot1.GetComponent<Selectable>().valor == 1)
            {
                Empilhar(cartaSelecionada);
            }
        }
    }

    bool Empilhavel(GameObject cartaSelecionada)
    {
        Selectable s1 = slot1.GetComponent<Selectable>();
        Selectable s2 = cartaSelecionada.GetComponent<Selectable>();
        if(!s2.estaEmpilhadoBaralho)
        {
            if (s2.topo)
            {
                if (s1.naipe == s2.naipe || (s1.valor == 1 && s2.naipe == null))
                {
                    if (s1.valor == s2.valor + 1)
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (s1.valor == (s2.valor - 1))
                {
                    bool carta1 = true;
                    bool carta2 = true;
                    if (s1.naipe == "P" || s1.naipe == "E")
                    {
                        carta1 = false;
                    }
                    if (s2.naipe == "P" || s2.naipe == "E")
                    {
                        carta2 = false;
                    }

                    if (carta1 == carta2)
                    {
                        print("Não empilhavel");
                        return false;
                    }
                    else
                    {
                        print("Empilhado");
                        return true;
                    }
                }
            }
        }
        return false;
    }

    void Empilhar(GameObject cartaSelecionada)
    {
        Selectable s1 = slot1.GetComponent<Selectable>();
        Selectable s2 = cartaSelecionada.GetComponent<Selectable>();
        float yOffset = 0.3f;

        if (s2.topo || (!s2.topo && s1.valor == 13))
        {
            yOffset = 0;
        }

        slot1.transform.position = new Vector3(cartaSelecionada.transform.position.x, cartaSelecionada.transform.position.y - yOffset, cartaSelecionada.transform.position.z - 0.01f);
        slot1.transform.parent = cartaSelecionada.transform;

        if (s1.estaEmpilhadoBaralho)
        {
            solitaire.jogadaNaTela.Remove(slot1.name);
        }
        else if (s1.topo && s2.topo && s1.valor == 1)
        {
            solitaire.topPos[s1.fila].GetComponent<Selectable>().valor = 0;
            solitaire.topPos[s1.fila].GetComponent<Selectable>().naipe = null;
        }
        else if (s1.topo)
        {
            solitaire.topPos[s1.fila].GetComponent<Selectable>().valor = s1.valor - 1;
        }
        else
        {
            solitaire.bottoms[s1.fila].Remove(slot1.name);
        }

        s1.estaEmpilhadoBaralho = false;
        s1.fila = s2.fila;

        if (s2.topo)
        {
            solitaire.topPos[s1.fila].GetComponent<Selectable>().valor = s1.valor;
            solitaire.topPos[s1.fila].GetComponent<Selectable>().naipe = s1.naipe;
            s1.topo = true;
        }
        else
        {
            s1.topo = false;
        }

        slot1 = this.gameObject;
    }
    
    bool Bloqueado(GameObject cartaSelecionada)
    {
        Selectable s2 = cartaSelecionada.GetComponent<Selectable>();
        if(s2.estaEmpilhadoBaralho == true)
        {
            if(s2.name == solitaire.jogadaNaTela.Last())
            {
                return false;
            }
            else
            {
                print(s2.name + " está bloqueado por " + solitaire.jogadaNaTela.Last());
                return true;
            }
        }
        else
        {
            if(s2.name == solitaire.bottoms[s2.fila].Last())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    bool DuploClique()
    {
        if(cronometro < duploCliqueTempo && contadorCliques == 2)
        {
            print("Duplo clique");
            return true;
        }
        else
        {
            return false;
        }
    }

    void AutoEmpilhar(GameObject cartaSelecionada)
    {
        for(int i = 0; i < solitaire.topPos.Length; i++ )
        {
            Selectable pilha = solitaire.topPos[i].GetComponent<Selectable>();
            if(cartaSelecionada.GetComponent<Selectable>().valor == 1)
            {
                if(solitaire.topPos[i].GetComponent<Selectable>().valor == 0)
                {
                    slot1 = cartaSelecionada;
                    Empilhar(pilha.gameObject);
                    break;
                }
            }
            else
            {
                if((solitaire.topPos[i].GetComponent<Selectable>().naipe == slot1.GetComponent<Selectable>().naipe) && (solitaire.topPos[i].GetComponent<Selectable>().valor == slot1.GetComponent<Selectable>().valor - 1))
                {
                    if(NaoPossuiFilho(slot1))
                    {
                        
                        slot1 = cartaSelecionada;
                        string ultimaCartaNome = pilha.naipe + pilha.valor.ToString();
                        if(pilha.valor == 1)
                        {
                            ultimaCartaNome = pilha.naipe + "A";
                        }
                        if(pilha.valor == 11)
                        {
                            ultimaCartaNome = pilha.naipe + "J";
                        }
                        if(pilha.valor == 12)
                        {
                            ultimaCartaNome = pilha.naipe + "Q";
                        }
                        if(pilha.valor == 13)
                        {
                            ultimaCartaNome = pilha.naipe + "K";
                        }
                        GameObject ultimaCarta = GameObject.Find(ultimaCartaNome);
                        Empilhar(ultimaCarta);
                        break;
                    }
                }
            }
        }
    }

    bool NaoPossuiFilho(GameObject carta)
    {
        int i = 0;
        foreach(Transform filho in carta.transform)
        {
            i++;
        }
        if(i==0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
