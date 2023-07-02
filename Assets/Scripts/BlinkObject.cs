using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlinkObject : MonoBehaviour
{

    private SpriteRenderer sprite;

    private Color colorBase;

    public Color colorChanged;

    void Awake() 
    {
        sprite = transform.GetComponent<SpriteRenderer>();
        colorBase = sprite.color;
    }

    void OnEnable() 
    {
        Sequence mySequenceColor = DOTween.Sequence();
        mySequenceColor
        .Append(sprite.DOColor(colorChanged, 0.5f)
            .SetEase(Ease.Linear))
            .SetDelay(0.2f)
        .Append(sprite.DOColor(colorBase, 0.2f)
           .SetEase(Ease.Linear))   
        .SetLoops(-1);
    }

    void OnDisable() 
    {
        DOTween.KillAll(transform);
        sprite.color = colorBase;
    }
}
