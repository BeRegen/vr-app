using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

/// <summary>
/// Gerencia a visualização de imagens panorâmicas em 360°
/// </summary>
public class PhotoViewer360 : MonoBehaviour
{
    [Header("Viewer Configuration")]
    [SerializeField] private Material panoramaMaterial;
    [SerializeField] private Transform viewerSphere;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float zoomSpeed = 0.5f;
    
    [Header("UI References")]
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private TMPro.TextMeshProUGUI titleText;
    
    // Controles de interação
    private XRController leftController;
    private XRController rightController;
    private string currentPhotoPath;
    private float currentZoom = 10f;
    private bool isLoading = false;
    
    private void Start()
    {
        // Obter referências aos controladores
        leftController = FindObjectOfType<XRController>(selectLeftHand: true);
        rightController = FindObjectOfType<XRController>(selectLeftHand: false);
        
        // Carregar a foto selecionada (em uma implementação real, viria de PlayerPrefs ou similar)
        string selectedPhoto = PlayerPrefs.GetString("SelectedPhoto", "");
        if (!string.IsNullOrEmpty(selectedPhoto))
        {
            LoadPhoto(selectedPhoto);
        }
        
        // Configurar a câmera para o modo 360
        Camera.main.fieldOfView = 90f;
        Camera.main.nearClipPlane = 0.01f;
    }
    
    private void Update()
    {
        // Não processar input durante carregamento
        if (isLoading) return;
        
        // Rotação baseada no movimento da cabeça (já implementado pelo XR Rig)
        
        // Zoom com os controladores
        HandleZoom();
        
        // Verificar botão de retorno ao menu
        CheckReturnToMenu();
    }
    
    /// <summary>
    /// Carrega uma foto panorâmica 360° do caminho especificado
    /// </summary>
    public void LoadPhoto(string photoPath)
    {
        if (isLoading || string.IsNullOrEmpty(photoPath)) return;
        
        StartCoroutine(LoadPhotoCoroutine(photoPath));
    }
    
    private IEnumerator LoadPhotoCoroutine(string photoPath)
    {
        isLoading = true;
        currentPhotoPath = photoPath;
        
        // Exibir mensagem de carregamento
        if (titleText != null)
        {
            titleText.text = "Carregando...";
        }
        
        // Carregar a textura da foto (em uma implementação real, usaria Resources.Load)
        Texture2D photoTexture = Resources.Load<Texture2D>(photoPath);
        
        // Simular tempo de carregamento
        yield return new WaitForSeconds(0.5f);
        
        if (photoTexture != null)
        {
            // Aplicar a textura ao material
            if (panoramaMaterial != null)
            {
                panoramaMaterial.mainTexture = photoTexture;
            }
            
            // Atualizar o título
            if (titleText != null)
            {
                string photoName = System.IO.Path.GetFileNameWithoutExtension(photoPath);
                titleText.text = photoName;
            }
        }
        else
        {
            Debug.LogError($"Não foi possível carregar a foto: {photoPath}");
            if (titleText != null)
            {
                titleText.text = "Erro ao carregar foto";
            }
        }
        
        isLoading = false;
    }
    
    /// <summary>
    /// Gerencia o zoom com os controladores
    /// </summary>
    private void HandleZoom()
    {
        if (leftController == null || rightController == null) return;
        
        // Obter input dos joysticks
        Vector2 leftStick = Vector2.zero;
        Vector2 rightStick = Vector2.zero;
        
        if (leftController.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out leftStick) &&
            rightController.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out rightStick))
        {
            // Usar eixo Y do joystick direito para zoom
            float zoomInput = rightStick.y;
            
            // Aplicar zoom
            currentZoom += zoomInput * zoomSpeed * Time.deltaTime;
            currentZoom = Mathf.Clamp(currentZoom, 5f, 20f);
            
            // Atualizar campo de visão da câmera
            Camera.main.fieldOfView = currentZoom;
        }
    }
    
    /// <summary>
    /// Verifica se o botão de retorno ao menu foi pressionado
    /// </summary>
    private void CheckReturnToMenu()
    {
        if (leftController == null) return;
        
        // Verificar botão B/Y (dependendo do controlador)
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
}
