using UnityEngine;
using UnityEngine.UI;

public class SFXToggleButton : MonoBehaviour
{
    [Header("UI")]
    public Button sfxButton;        // Drag your SFX button here
    public Image buttonImage;       // The Image component of the button
    public Sprite sfxOnSprite;      // Icon for SFX ON
    public Sprite sfxOffSprite;     // Icon for SFX OFF

    private bool isSFXMuted;

    void Start()
    {
        // Load saved state, default = unmuted
        isSFXMuted = PlayerPrefs.GetInt("MuteSFX", 0) == 1;

        // Set the correct sprite
        UpdateButtonSprite();

        // Add the click listener
        sfxButton.onClick.AddListener(ToggleSFX);
    }

    void ToggleSFX()
    {
        // Toggle the state
        isSFXMuted = !isSFXMuted;

        // Save to PlayerPrefs
        PlayerPrefs.SetInt("MuteSFX", isSFXMuted ? 1 : 0);

        // Update button icon
        UpdateButtonSprite();

        // Apply mute/unmute to all SFX via GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetSFXMute(isSFXMuted);
        }
    }

    void UpdateButtonSprite()
    {
        if (buttonImage != null)
        {
            buttonImage.sprite = isSFXMuted ? sfxOffSprite : sfxOnSprite;
        }
    }
}
