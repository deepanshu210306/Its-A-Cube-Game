using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI scoreText;   // <-- TextMeshPro UI type

    void Update()
    {
        scoreText.text = Mathf.FloorToInt(player.position.z).ToString();
        // or: scoreText.text = player.position.z.ToString("0");
    }
}

