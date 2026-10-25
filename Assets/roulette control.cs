using UnityEngine;
using UnityEngine.InputSystem;

public class roulettecontrol : MonoBehaviour
{
    float rotSpeed = 0; // 旋轉速度

    [Header("音效檔案 (可以直接拖入 MP3)")]
    public AudioClip spinClip; // 轉動中的 MP3 檔案
    public AudioClip stopClip; // 停下來的 MP3 檔案

    private AudioSource audioSource;
    private bool hasPlayedStopAudio = true;
    private bool shouldDecelerate = false; // 控制現在是否可以開始減速

    void Start()
    {
        Application.targetFrameRate = 60;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        var pointer = Pointer.current;

        if (pointer != null)
        {
            if (pointer.press.wasPressedThisFrame)
            {
                this.rotSpeed = 100;
                hasPlayedStopAudio = false;
                shouldDecelerate = false; // 鎖定速度，手還沒放開前不准減速

                // 播放轉動音效
                audioSource.clip = spinClip;
                audioSource.loop = true;
                audioSource.Play();
            }

            if (pointer.press.isPressed)
            {
                this.rotSpeed = 100;
            }

            if (pointer.press.wasReleasedThisFrame)
            {
                shouldDecelerate = true;
            }
        }

        if (shouldDecelerate && this.rotSpeed > 0)
        {
            this.rotSpeed *= 0.96f;

            if (this.rotSpeed < 0.5f)
            {
                this.rotSpeed = 0;
                shouldDecelerate = false; // 停止減速狀態

                // 停止轉動音效
                audioSource.Stop();

                // 播放一次停止音效
                if (!hasPlayedStopAudio && stopClip != null)
                {
                    audioSource.PlayOneShot(stopClip);
                    hasPlayedStopAudio = true;
                }
            }
        }

        // 旋轉輪盤
        transform.Rotate(0, 0, this.rotSpeed);
    }
}