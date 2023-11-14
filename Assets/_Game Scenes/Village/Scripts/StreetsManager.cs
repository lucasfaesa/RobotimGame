using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StreetsManager : MonoBehaviour
{
    [SerializeField] private List<StreetInfoHolder> streetInfoHolders;
    [Space]
    [SerializeField] private UnityEvent<string> playerCurrentStreets;
    
    private List<string> randomStreetsName = new() {"Sao Sebastiao", "Parana", "Bela Vista", "M", "7", "Santa Luzia",
        "Sao Jorge", "Dezenove", "Castro Alves", "Duque De Caxias", "Projetada", "Rui Barbosa", "Santa Catarina",
        "Minas Gerais", "N", "Santos Dumont", "8", "Espirito Santo", "Vinte e Um", "Vinte e Dois", "Da Paz", "Treze De Maio",
        "K", "Rio De Janeiro", "Goias", "Ceara", "10", "Belo Horizonte", "Das Flores", "Sergipe", "Vitoria" };
    
    private List<string> currentPlayerStreets = new List<string>();
    
    void Start()
    {
        playerCurrentStreets?.Invoke("");
        AssignRandomStreetNames();        
    }

    private void AssignRandomStreetNames()
    {
        foreach (var streetInfoHolder in streetInfoHolders)
        {
            int randomNumber = Random.Range(0, randomStreetsName.Count);
            streetInfoHolder.SetStreetName(randomStreetsName[randomNumber]);
            
            randomStreetsName.RemoveAt(randomNumber);
        }
    }

    public void PlayerEnteredStreet(string streetName)
    {
       currentPlayerStreets.Add(streetName);
       UpdatePlayerCurrentStreetUI();
    }

    public void PlayerLeftStreet(string streetName)
    {
        currentPlayerStreets.Remove(streetName);
        UpdatePlayerCurrentStreetUI();
    }

    private void UpdatePlayerCurrentStreetUI()
    {
        string streetsName = "";
        int cont = 0;
        
        foreach (var currentPlayerStreet in currentPlayerStreets)
        {
            if (cont == 0)
                streetsName += "Rua " + currentPlayerStreet;
            if (cont > 0 && cont < currentPlayerStreets.Count)
                streetsName += " x " + "Rua " + currentPlayerStreet;
            if (cont > 0 && cont >= currentPlayerStreets.Count)
                streetsName += " " + "Rua " + currentPlayerStreet;
            
            cont++;
        }
        
        playerCurrentStreets?.Invoke(streetsName);
    }

}
