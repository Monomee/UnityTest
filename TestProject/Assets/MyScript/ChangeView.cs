using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeView : MonoBehaviour
{
    public Camera FirstPersonView;
    public Camera ThirdPersonView;
    bool isToggled = false; // Biến trạng thái ban đầu là false

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) // Kiểm tra xem nút Q có được nhấn không
        {
            isToggled = !isToggled; // Đảo ngược trạng thái

            if (isToggled)
            {
                FirstPersonView.gameObject.SetActive(true);
                ThirdPersonView.gameObject.SetActive(false);
                Debug.Log("First Person View");
            }
            else
            {
                ThirdPersonView.gameObject.SetActive(true);
                FirstPersonView.gameObject.SetActive(false);
                Debug.Log("Third Person View");
            }
        }
    }
}
