using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenuController : MonoBehaviour
{
    [Header("SO")] 
    [SerializeField] private PinsManagerSO pinsManager;
    [Space]
    [SerializeField] private RectTransform contentRect;
    [Header("UI Elements")] 
    [SerializeField] private TextMeshProUGUI textObjectives;
    [SerializeField] private TextMeshProUGUI textLocation;
    [SerializeField] private TextMeshProUGUI textDifficulty;
    [SerializeField] private TextMeshProUGUI textStatus;
    [SerializeField] private Image imagePreview;
    
    
    private void OnEnable()
    {
        pinsManager.pinSelected += UpdateContextMenu;
        pinsManager.pinDeselected += HideContentWindow;
    }

    private void OnDisable()
    {
        pinsManager.pinSelected -= UpdateContextMenu;
        pinsManager.pinDeselected -= HideContentWindow;
    }

    void Start()
    {
        contentRect.anchoredPosition = new Vector2(705f, 0f);
    }
    
    private void UpdateContextMenu(PinInfo info)
    {
        textObjectives.text = info.objectives;
        textLocation.text = info.location;
        textDifficulty.text = info.difficulty;
        textStatus.text = info.completed == true ? "Completa" : "Incompleta";
        imagePreview.sprite = info.imagePreview;
        
        ShowContentWindow();
    }
    
    private void HideContentWindow()
    {
        float xPos = contentRect.anchoredPosition.x;
        
        Sequence sequence = DOTween.Sequence();

        sequence.Append(DOTween.To(x => xPos = x, contentRect.anchoredPosition.x, 705.66f, 0.5f)).SetEase(Ease.InOutSine);
        sequence.OnUpdate(() =>
        {
            contentRect.anchoredPosition = new Vector2(xPos, 0f);
        });
    }

    private void ShowContentWindow()
    {
        
        float xPos = contentRect.anchoredPosition.x;
        
        Sequence sequence = DOTween.Sequence();

        sequence.Append(DOTween.To(x => xPos = x, contentRect.anchoredPosition.x, 0, 0.5f)).SetEase(Ease.InOutSine);
        sequence.OnUpdate(() =>
        {
            contentRect.anchoredPosition = new Vector2(xPos, 0f);
        });
    }
}
