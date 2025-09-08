using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldUI : MonoBehaviour
{
    public Field field;

    public void Regist(Card card)
    {

        field.card = card;

    }

    public void Init(Field field)
    {
        this.field=field;
    }
}
