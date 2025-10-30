using UnityEngine;

public static class ReadCardData
{
    public static Card ReadCard(GameObject cardModel)
    {
        Card card= new Card();
        if (cardModel.GetComponent<HandCardDisplay>()!=null)
        {
            card = cardModel.GetComponent<HandCardDisplay>().card;
        }
        else if (cardModel.GetComponent<UnitCardDisplay>() != null)
        {
            card = cardModel.GetComponent<UnitCardDisplay>().card;
        }else if (cardModel.GetComponent<SelectCardDisplay>()!=null)
        {
            card = cardModel.GetComponent<SelectCardDisplay>().card;
        }

        return card;
    }
}