using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] bool isPlayableUnit;
    [SerializeField] BattleHud hud;
    public bool IsPlayerUnit { get { return isPlayableUnit; } }
    public BattleHud Hud { get { return hud; } }
    public Pokemon Pokemon { get; set; }
    Image image;
    Vector3 originalPos;
    Color originalColor;
    public Conditions Status { get; private set; }
    private void Awake()
    {
        image = GetComponent<Image>();
        originalPos = image.transform.localPosition;
        originalColor = image.color;
    }
    //setting up images and playing entering animation
    public void Setup(Pokemon pokemon)
    {
        Pokemon = pokemon;
        if (isPlayableUnit)
        {
            image.sprite = Pokemon.Base.BackSprite;
        }
        else
        {
            image.sprite = Pokemon.Base.FrontSprite;
        }
        image.color = originalColor;
        PlayEnterAnimation();
        hud.SetData(pokemon);
    }
    //all the various animations using DOTween   
    public void PlayEnterAnimation()
    {
        if (isPlayableUnit)
        {
            image.transform.localPosition = new Vector3(-500f, originalPos.y);
        }
        else
        {
            image.transform.localPosition = new Vector3(500f, originalPos.y);
        }
        image.transform.DOLocalMoveX(originalPos.x, 2f);
    }
    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        if (isPlayableUnit)
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x + 50f, 0.25f));
        }
        else
        {
            sequence.Append(image.transform.DOLocalMoveX(originalPos.x - 50f, 0.25f));
        }
        sequence.Append(image.transform.DOLocalMoveX(originalPos.x, 0.25f));
    }
    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.black, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
        sequence.Append(image.DOColor(Color.black, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
        sequence.Append(image.DOColor(Color.black, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }
    public void PlayBoostAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.green, 0.5f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }
    public void PlayDebuffAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.cyan, 0.5f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }
    public void PlayPoisonAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.magenta, 0.5f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }
    public void PlayBurnAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.red, 0.5f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }
    public void PlayPrlzAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.yellow, 0.5f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }
    public void PlayFrzAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.blue, 0.5f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }
    public void PlaySlpAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(Color.gray, 0.5f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
    }
    public void PlayFaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY((originalPos.y - 150f), 0.5f));
        sequence.Join(image.DOFade(0, 0.5f));
    }
    //setting up visual effects for status conditions
    public void SetStatusEffect(ConditionID conditionID)
    {
        Status = ConditionsDB.StatusConditions[conditionID];
    }
    public void ShowStatusEffects(bool notPlayed)
    {
        if(notPlayed == true)
        {
            Status?.VisualEffect?.Invoke(this);
        }
    }
}
