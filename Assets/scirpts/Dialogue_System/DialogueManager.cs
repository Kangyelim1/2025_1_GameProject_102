using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;



public class DialogueManager : MonoBehaviour
{
    [Header("UI 요소 - Instector에서 연결")]
    public GameObject DialoguePanel;
    public Image characterImage;
    public TextMeshProUGUI characternameText;
    public TextMeshProUGUI dialogueText;
    public Button nexButton;

    [Header("기본 설정")]
    public Sprite defaultcharacterImage;

    [Header("타이핀 효과 설정")]
    public float typingSpeed = 0.05f;
    public bool skipTypingOnClick = true;

    private DialogueDataSO currentDialogue;
    private int currentLinelndex = 0;
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    IEnumerator TypeText(string textToType)
    {
        isTyping = true;
        dialogueText.text = "";

        for (int i = 0; i < textToType.Length; i++)
        {
            dialogueText.text += textToType[i];
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    private void CompleteTyping()
    {
        if (typingCoroutine != null)
        {
           StopCoroutine(typingCoroutine);
        }

        isTyping = false;

        if (currentDialogue != null && currentLinelndex < currentDialogue.dialogueLines.Count)
        {
            dialogueText.text = currentDialogue.dialogueLines[currentLinelndex];
        }
    }

    void ShowCurrentLine()
    {
        if (currentDialogue != null && currentLinelndex < currentDialogue.dialogueLines.Count)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            string currentText = currentDialogue.dialogueLines[currentLinelndex];
            typingCoroutine = StartCoroutine(TypeText(currentText));
        }
    }

    public void ShowNextLine()
    {
        currentLinelndex++;

        if (currentLinelndex >= currentDialogue.dialogueLines.Count)
        {
            EndDialogue();
        }
        else
        {
            ShowCurrentLine();
        }
    }

    void EndDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isDialogueActive = false;
        isTyping = false;
        DialoguePanel.SetActive(false);
        currentLinelndex = 0;
    }

    public void HandleNextInput()
    {
        if (isTyping && skipTypingOnClick)
        {
            CompleteTyping();
        }
        else if (!isTyping)
        {
            ShowNextLine();
        }
    }

    public void SkipDalogue()
    {
        EndDialogue();
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }

    public void StarDialogue(DialogueDataSO dialogue)
    {
        if (dialogue == null || dialogue.dialogueLines.Count == 0) return;

        currentDialogue = dialogue;
        currentLinelndex = 0;
        isDialogueActive = true;

        DialoguePanel.SetActive(true);
        characternameText.text = dialogue.characterName;

        if (characterImage != null)
        {
            if (dialogue.characterImage != null)
            {
                characterImage.sprite = dialogue.characterImage;
            }
            else
            {
                characterImage.sprite = defaultcharacterImage;
            }
        }
        ShowCurrentLine();
    }

    void Start()
    {
        DialoguePanel.SetActive(false);
        nexButton.onClick.AddListener(HandleNextInput);
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            HandleNextInput();
        }
    }
}
