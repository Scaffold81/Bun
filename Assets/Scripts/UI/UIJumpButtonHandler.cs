using System;
using UnityEngine;
using UnityEngine.UI;

public class UIJumpButtonHandler :MonoBehaviour
{
    private Button jumpButton;
    public Action Jump;

  
    private void Awake()
    {
        jumpButton = GetComponent<Button>();
    }

    private void OnEnable()
    {
        jumpButton?.onClick.AddListener(OnJump);
    }

    public void OnJump() 
    {
        Jump?.Invoke();
    }

    private void OnDisable()
    {
        jumpButton?.onClick.RemoveListener(OnJump);
    }
}
