using UnityEngine;

namespace _Game_Scenes.SchoolRoomScene.Scripts
{
    [CreateAssetMenu(fileName = "SchoolRoomQuest", menuName = "ScriptableObjects/SchoolRoomScene/SchoolRoomQuestSO")]
    public class SchoolRoomQuestsSO : ScriptableObject
    {
        [field:TextArea(3,5)] [field: SerializeField] public string[] Description { get; set; }
        [field: SerializeField] public ResearchTypeSO ResearchTypeCorrectAnswer { get; set; }
    }
    
}

public static class NpcDialogs
{
    private static string[] _possibleNpcIntroductions  = 
    {
        "Pode me ajudar com essa pergunta? Diz assim:",
        "Hmmm, essa parece um pouco complicada... Está escrito assim:",
        "?!, essa eu preciso pesquisar sobre... O enunciado é:",
        "Essa eu REALMENTE preciso de uma ajudinha. Aqui diz o seguinte:",
        "Preciso pensar sobre essa... Diz assim:",
        "Essa é bem interessante. Segue a pergunta:"
    };

    public static string GetRandomDialogIntroduction() =>
        _possibleNpcIntroductions[Random.Range(0, _possibleNpcIntroductions.Length)];

    private static string[] _possibleNpcSuccessDialogs =
    {
        "Exatamente, resposta correta, obrigado!",
        "ISSO, certinho, muito obrigado!",
        "Ah, que ótimo, está correto!",
        "Perfeito, sabia que você iria conseguir me ajudar!",
        "Hmm, não tinha pensado nisso, muito obrigado!",
        "Perfeita resposta!",
        "Conferi aqui nos livros e está correta a resposta! Muito obrigado!"
    };
    public static string GetRandomDialogSuccessAnswer() =>
        _possibleNpcSuccessDialogs[Random.Range(0, _possibleNpcSuccessDialogs.Length)];
    
    private static string[] _possibleNpcErrorDialogs =
    {
        "Hmm, pesquisei aqui e essa não parece ser a resposta correta...",
        "Que pena, parece que essa não era a resposta correta...",
        "Acho que erramos, aqui diz que a resposta correta era outra.",
        "Pesquisei sem parar e a resposta não parece ser essa...",
        "Ponto de vista interessante, mas esta não é a resposta correta..."
    };
    
    public static string GetRandomDialogErrorAnswer() =>
        _possibleNpcErrorDialogs[Random.Range(0, _possibleNpcErrorDialogs.Length)];
}
