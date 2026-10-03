using UnityEngine;
using UnityEngine.UI;

public class MusicToggleButton : MonoBehaviour
{
    [Header("UI")]
    public Button musicButton;         // Drag your button here
    public Image buttonImage;          // The Image component of the button
    public Sprite musicOnSprite;       // Icon for music ON
    public Sprite musicOffSprite;      // Icon for music OFF

    private bool isMusicMuted;

    void Start()
    {
        // Load saved state, default = unmuted
        isMusicMuted = PlayerPrefs.GetInt("MuteMusic", 0) == 1;

        // Set the correct sprite
        UpdateButtonSprite();

        // Add the click listener
        musicButton.onClick.AddListener(ToggleMusic);
    }

    void ToggleMusic()
    {
        // Toggle the state
        isMusicMuted = !isMusicMuted;

        // Save to PlayerPrefs
        PlayerPrefs.SetInt("MuteMusic", isMusicMuted ? 1 : 0);

        // Update button icon
        UpdateButtonSprite();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetMusicMute(isMusicMuted);
        }
    }

    void UpdateButtonSprite()
    {
        if (buttonImage != null)
        {
            buttonImage.sprite = isMusicMuted ? musicOffSprite : musicOnSprite;
        }
    }
}
