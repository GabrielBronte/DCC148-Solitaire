using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public bool topo = false;
    public string naipe;
    public int valor;
    public int fila;
    public bool viradoCima = false;
    public bool estaEmpilhadoBaralho;
    private string valorString;

    // Start is called before the first frame update
    void Start()
    {
        if(CompareTag("Card"))
        {
            naipe = transform.name[0].ToString();

            for(int i = 1; i < transform.name.Length; i++)
            {
                char c = transform.name[i];
                valorString = valorString + c.ToString();
            }

            if(valorString == "A")  valor = 1;
            if(valorString == "2")  valor = 2;
            if(valorString == "3")  valor = 3;
            if(valorString == "4")  valor = 4;
            if(valorString == "5")  valor = 5;
            if(valorString == "6")  valor = 6;
            if(valorString == "7")  valor = 7;
            if(valorString == "8")  valor = 8;
            if(valorString == "9")  valor = 9;
            if(valorString == "10")  valor = 10;
            if(valorString == "J")  valor = 11;
            if(valorString == "Q")  valor = 12;
            if(valorString == "K")  valor = 13;

            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
