using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NpcsNames
{
    public static List<string> names = new();

    public static string GetRandomName()
    {
        List<string> allNames = new()
        {
            "Bot", "Droid", "Tron", "Gadget", "Ciborgue", "Bit", "Drone", "Dispositivo",
            "Robo", "Máquina", "Ton", "Fuse", "Plug", "Cabo", "Clamp", "Byte", "Bolt", "Cyd", "Andy Roid", "Qwerty",
            "Anetron", "Ive", "Golem", "Earl", "Sparkle", "Jet", "Ihocator", "Owit", "Rob Bott", "Crowby", "Spud", 
            "Screwie", "Otuc", "Eptx",
        };
        names = new(allNames);
        
        int randomNumber = UnityEngine.Random.Range(0, allNames.Count);
        string randomName = names[randomNumber];
        names.RemoveAt(randomNumber);
        
        return randomName;
    }
}

public static class BlankDialogs
{
    public static List<string> texts = new();
    
    public static string GetRandomBlankDialog()
    {
        List<string> allTexts = new() {"Olá, bom dia!", 
            "Oi, tudo bem?", 
            "Estou pensando em ir na cafeteria hoje...", 
            "... Que vontade de velejar...",
            "Que calor!",
            "A maresia está começando a me enferrujar..."
        };

        texts = new(allTexts);
        
        int randomNumber = UnityEngine.Random.Range(0, texts.Count);
        return texts[randomNumber];
    }
}

public static class FindObjectQuestTypeDialogs
{
    public static List<string> GetRandomFindObjectQuestGiverDialog()
    {
        List<List<string>> questGiverTexts = new()
        {
            new List<string>(){"Oh, que bom que você chegou!", "Pode me ajudar em uma coisinha?",
                "Acabei perdendo algo muito importante para mim", "Acho que deixei com meu irmão \nPoderia encontrá-lo?",
                "Enviarei os detalhes para você!"
            },
            new List<string>(){"Ah, nãããããão...", "Acabei esquecendo algo que iria trazer para cá...",
                "Você poderia me ajudar a encontrá-lo? \nAcho que está com meu amigo", 
                "Oba, que ótimo!! \nEnviarei os detalhes para você!",},
            new List<string>(){"Ah, que belo dia!", "Só ficaria melhor com meu item preferido...",
                "Se você puder buscá-lo para mim seria muito legal!", "Enviarei os detalhes para você!",}
        };
        
        int randomNumber = UnityEngine.Random.Range(0, questGiverTexts.Count);
        return questGiverTexts[randomNumber];
    }
    
    public static List<string> GetRandomFindObjectQuestInteractorDialog()
    { 
        List<List<string>> questInteractorTexts = new()
        {
            new List<string>(){"Oh... \nOlá, do que você precisa?"},
            new List<string>(){"Hmm... \nAcho que tenho justo o que você precisa!",},
            new List<string>(){"Opa, tudo bem? \nProcurando algo?",}
        };
        
        int randomNumber = UnityEngine.Random.Range(0, questInteractorTexts.Count);
        return questInteractorTexts[randomNumber];
    }
}
