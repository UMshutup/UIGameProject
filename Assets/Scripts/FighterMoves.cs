using UnityEngine;

public class FighterMoves : MonoBehaviour
{
    [HideInInspector] public string chosenMove;

    public string[] moves;

    public void SelectMove(string _chosenMove)
    {
        for (int i = 0; i < moves.Length; i++) {
            if (moves[i].Equals(_chosenMove))
            {
                chosenMove = moves[i];
            }
        }
    }
    
}
