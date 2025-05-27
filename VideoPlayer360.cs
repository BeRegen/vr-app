using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

/// <summary>
/// Gerencia a reprodução de vídeos em 360° no ambiente VR
/// </summary>
public class VideoPlayer360 : MonoBehaviour
{
    [Header("Player Configuration")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Material videoMaterial;
    [SerializeField] private Transform playerSphere;
    
    [Header("UI References")]
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private TMPro.TextMeshProUGUI titleText;
    [SerializeField] private TMPro.TextMeshProUGUI timeText;
    [SerializeField] private UnityEngine.UI.Slider progressSlider;
    [SerializeField] private UnityEngine.UI.Slider volumeSlider;
    
    // Controles de interação
    private XRController leftController;
    private XRController rightController;
    private string currentVideoPath;
    private bool isPlaying = false;
    private bool isDraggingSlider = false;
    
    private void Start()
    {
        // Obter referências aos controladores
        leftController = FindObjectOfType<XRController>(selectLeftHand: true);
        rightController = FindObjectOfType<XRController>(selectLeftHand: false);
        
        // Configurar o VideoPlayer
        if (videoPlayer != null)
        {
            videoPlayer.playOnAwake = false;
            videoPlayer.isLooping = true;
            videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
            videoPlayer.targetMaterialRenderer = playerSphere.GetComponent<Renderer>();
            videoPlayer.targetMaterialProperty = "_MainTex";
        }
        
        // Configurar o slider de volume
        if (volumeSlider != null)
        {
            volumeSlider.value = 0.8f;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
        
        // Configurar o slider de progresso
        if (progressSlider != null)
        {
            progressSlider.onValueChanged.AddListener(SetVideoProgress);
        }
        
        // Carregar o vídeo selecionado (em uma implementação real, viria de PlayerPrefs ou similar)
        string selectedVideo = PlayerPrefs.GetString("SelectedVideo", "");
        if (!string.IsNullOrEmpty(selectedVideo))
        {
            LoadVideo(selectedVideo);
        }
        
        // Configurar a câmera para o modo 360
        Camera.main.fieldOfView = 90f;
        Camera.main.nearClipPlane = 0.01f;
    }
    
    private void Update()
    {
        // Atualizar UI de tempo e progresso
        UpdateTimeDisplay();
        
        // Verificar controles de reprodução
        CheckPlaybackControls();
        
        // Verificar botão de retorno ao menu
        CheckReturnToMenu();
    }
    
    /// <summary>
    /// Carrega um vídeo 360° do caminho especificado
    /// </summary>
    public void LoadVideo(string videoPath)
    {
        if (string.IsNullOrEmpty(videoPath) || videoPlayer == null) return;
        
        currentVideoPath = videoPath;
        
        // Exibir mensagem de carregamento
        if (titleText != null)
        {
            titleText.text = "Carregando...";
        }
        
        // Em uma implementação real, usaria Resources.Load ou similar
        // videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, videoPath);
        
        // Para este exemplo, simulamos o carregamento
        StartCoroutine(SimulateVideoLoading(videoPath));
    }
    
    private IEnumerator SimulateVideoLoading(string videoPath)
    {
        // Simular tempo de carregamento
        yield return new WaitForSeconds(1.0f);
        
        // Atualizar o título
        if (titleText != null)
        {
            string videoName = System.IO.Path.GetFileNameWithoutExtension(videoPath);
            titleText.text = videoName;
        }
        
        // Iniciar reprodução
        PlayVideo();
    }
    
    /// <summary>
    /// Inicia a reprodução do vídeo
    /// </summary>
    public void PlayVideo()
    {
        if (videoPlayer == null) return;
        
        videoPlayer.Play();
        isPlaying = true;
    }
    
    /// <summary>
    /// Pausa a reprodução do vídeo
    /// </summary>
    public void PauseVideo()
    {
        if (videoPlayer == null) return;
        
        videoPlayer.Pause();
        isPlaying = false;
    }
    
    /// <summary>
    /// Alterna entre reprodução e pausa
    /// </summary>
    public void TogglePlayPause()
    {
        if (isPlaying)
        {
            PauseVideo();
        }
        else
        {
            PlayVideo();
        }
    }
    
    /// <summary>
    /// Define o volume do vídeo
    /// </summary>
    public void SetVolume(float volume)
    {
        if (videoPlayer == null) return;
        
        videoPlayer.SetDirectAudioVolume(0, volume);
    }
    
    /// <summary>
    /// Define a posição de reprodução do vídeo
    /// </summary>
    public void SetVideoProgress(float progress)
    {
        if (videoPlayer == null || !isDraggingSlider) return;
        
        videoPlayer.time = progress * videoPlayer.length;
    }
    
    /// <summary>
    /// Atualiza a exibição de tempo e progresso
    /// </summary>
    private void UpdateTimeDisplay()
    {
        if (videoPlayer == null || !videoPlayer.isPlaying) return;
        
        // Atualizar texto de tempo
        if (timeText != null)
        {
            float currentTime = (float)videoPlayer.time;
            float totalTime = (float)videoPlayer.length;
            
            string currentTimeStr = FormatTime(currentTime);
            string totalTimeStr = FormatTime(totalTime);
            
            timeText.text = $"{currentTimeStr} / {totalTimeStr}";
        }
        
        // Atualizar slider de progresso
        if (progressSlider != null && !isDraggingSlider)
        {
            progressSlider.value = (float)(videoPlayer.time / videoPlayer.length);
        }
    }
    
    /// <summary>
    /// Formata o tempo em minutos e segundos
    /// </summary>
    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        
        return $"{minutes:00}:{seconds:00}";
    }
    
    /// <summary>
    /// Verifica os controles de reprodução dos controladores
    /// </summary>
    private void CheckPlaybackControls()
    {
        if (rightController == null) return;
        
        // Verificar botão A/X para play/pause
        bool primaryButtonPressed = false;
        if (rightController.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out primaryButtonPressed) && primaryButtonPressed)
        {
            TogglePlayPause();
        }
        
        // Verificar botão B/Y para mostrar/ocultar controles
        bool secondaryButtonPressed = false;
        if (rightController.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out secondaryButtonPressed) && secondaryButtonPressed)
        {
            ToggleControlPanel();
        }
    }
    
    /// <summary>
    /// Verifica se o botão de retorno ao menu foi pressionado
    /// </summary>
    private void CheckReturnToMenu()
    {
        if (leftController == null) return;
        
        // Verificar botão menu
        bool menuButtonPressed = false;
        if (leftController.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.menuButton, out menuButtonPressed) && menuButtonPressed)
        {
            ReturnToMainMenu();
        }
    }
    
    /// <summary>
    /// Retorna ao menu principal
    /// </summary>
    public void ReturnToMainMenu()
    {
        // Em uma implementação real, isso carregaria a cena do menu principal
        // SceneManager.LoadScene("MainMenu");
        Debug.Log("Retornando ao menu principal");
        
        // Parar o vídeo antes de sair
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }
    }
    
    /// <summary>
    /// Alterna a visibilidade do painel de controle
    /// </summary>
    public void ToggleControlPanel()
    {
        if (controlPanel != null)
        {
            controlPanel.SetActive(!controlPanel.activeSelf);
        }
    }
    
    /// <summary>
    /// Chamado quando o usuário começa a arrastar o slider de progresso
    /// </summary>
    public void OnProgressDragStart()
    {
        isDraggingSlider = true;
    }
    
    /// <summary>
    /// Chamado quando o usuário termina de arrastar o slider de progresso
    /// </summary>
    public void OnProgressDragEnd()
    {
        isDraggingSlider = false;
    }
}
