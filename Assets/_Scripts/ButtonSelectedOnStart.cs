using UnityEngine;
using UnityEngine.UI;

public class ButtonSelectedOnStart : MonoBehaviour
{
    Button button;
    void Start()
    {
        button = GetComponent<Button>();
        button.Select();
    }


}
