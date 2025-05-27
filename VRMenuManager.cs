using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Gerencia o menu principal de navegação em VR, permitindo a seleção de categorias e conteúdos
/// </summary>
public class VRMenuManager : MonoBehaviour
{
    [Header("Menu Configuration")]
    [SerializeField] private Transform menuContainer;
    [SerializeField] private GameObject categoryButtonPrefab;
    [SerializeField] private GameObject contentButtonPrefab;
    [SerializeField] private float buttonSpacing = 0.2f;
    
    [Header("Content References")]
    [SerializeField] private Transform contentPanel;
    [SerializeField] private Text titleText;
    
    // Lista de categorias disponíveis
    private List<Category> categories = new List<Category>();
    private Category currentCategory;
    
    private void Start()
    {
        // Carregar categorias do arquivo de configuração
        LoadCategories();
        
        // Criar botões de categoria
        CreateCategoryButtons();
        
        // Selecionar a primeira categoria por padrão
        if (categories.Count > 0)
        {
            SelectCategory(categories[0]);
        }
    }
    
    /// <summary>
    /// Carrega as categorias do arquivo de configuração
    /// </summary>
    private void LoadCategories()
    {
        // Em uma implementação real, isso carregaria de um JSON ou outro formato
        // Para este exemplo, criamos algumas categorias de demonstração
        
        Category photosCategory = new Category("Fotos 360°");
        photosCategory.AddContent(new ContentItem("Praia", "Photos/beach", ContentType.Photo));
        photosCategory.AddContent(new ContentItem("Montanha", "Photos/mountain", ContentType.Photo));
        photosCategory.AddContent(new ContentItem("Cidade", "Photos/city", ContentType.Photo));
        categories.Add(photosCategory);
        
        Category videosCategory = new Category("Vídeos 360°");
        videosCategory.AddContent(new ContentItem("Tour Virtual", "Videos/tour", ContentType.Video));
        videosCategory.AddContent(new ContentItem("Natureza", "Videos/nature", ContentType.Video));
        categories.Add(videosCategory);
    }
    
    /// <summary>
    /// Cria os botões de categoria no menu
    /// </summary>
    private void CreateCategoryButtons()
    {
        // Limpar botões existentes
        foreach (Transform child in menuContainer)
        {
            Destroy(child.gameObject);
        }
        
        // Criar novos botões para cada categoria
        float posX = 0;
        foreach (Category category in categories)
        {
            GameObject buttonObj = Instantiate(categoryButtonPrefab, menuContainer);
            buttonObj.transform.localPosition = new Vector3(posX, 0, 0);
            
            // Configurar texto e evento de clique
            Text buttonText = buttonObj.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = category.Name;
            }
            
            // Adicionar interação VR
            XRSimpleInteractable interactable = buttonObj.GetComponent<XRSimpleInteractable>();
            if (interactable != null)
            {
                Category cat = category; // Variável local para captura no lambda
                interactable.selectEntered.AddListener((args) => {
                    SelectCategory(cat);
                });
            }
            
            posX += buttonSpacing;
        }
    }
    
    /// <summary>
    /// Seleciona uma categoria e exibe seu conteúdo
    /// </summary>
    public void SelectCategory(Category category)
    {
        currentCategory = category;
        titleText.text = category.Name;
        
        // Limpar painel de conteúdo
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }
        
        // Criar botões para cada item de conteúdo
        float posY = 0;
        foreach (ContentItem item in category.Contents)
        {
            GameObject buttonObj = Instantiate(contentButtonPrefab, contentPanel);
            buttonObj.transform.localPosition = new Vector3(0, posY, 0);
            
            // Configurar texto e miniatura
            Text buttonText = buttonObj.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = item.Title;
            }
            
            // Adicionar imagem de miniatura (em implementação real, carregaria do Resources)
            Image thumbnail = buttonObj.GetComponentInChildren<Image>();
            // thumbnail.sprite = Resources.Load<Sprite>(item.ThumbnailPath);
            
            // Adicionar interação VR
            XRSimpleInteractable interactable = buttonObj.GetComponent<XRSimpleInteractable>();
            if (interactable != null)
            {
                ContentItem contentItem = item; // Variável local para captura no lambda
                interactable.selectEntered.AddListener((args) => {
                    OpenContent(contentItem);
                });
            }
            
            posY -= buttonSpacing;
        }
    }
    
    /// <summary>
    /// Abre o conteúdo selecionado (foto ou vídeo)
    /// </summary>
    public void OpenContent(ContentItem item)
    {
        Debug.Log($"Abrindo conteúdo: {item.Title} ({item.Type})");
        
        // Em uma implementação real, isso carregaria a cena apropriada
        // ou abriria o visualizador correto
        
        if (item.Type == ContentType.Photo)
        {
            // Carregar cena do visualizador de fotos
            // SceneManager.LoadScene("PhotoViewer");
            // PlayerPrefs.SetString("SelectedPhoto", item.ResourcePath);
        }
        else if (item.Type == ContentType.Video)
        {
            // Carregar cena do player de vídeo
            // SceneManager.LoadScene("VideoPlayer");
            // PlayerPrefs.SetString("SelectedVideo", item.ResourcePath);
        }
    }
}

/// <summary>
/// Representa uma categoria de conteúdo
/// </summary>
[System.Serializable]
public class Category
{
    public string Name { get; private set; }
    public List<ContentItem> Contents { get; private set; } = new List<ContentItem>();
    
    public Category(string name)
    {
        Name = name;
    }
    
    public void AddContent(ContentItem item)
    {
        Contents.Add(item);
    }
}

/// <summary>
/// Representa um item de conteúdo (foto ou vídeo)
/// </summary>
[System.Serializable]
public class ContentItem
{
    public string Title { get; private set; }
    public string ResourcePath { get; private set; }
    public ContentType Type { get; private set; }
    public string ThumbnailPath => $"Thumbnails/{Title.ToLower()}";
    
    public ContentItem(string title, string resourcePath, ContentType type)
    {
        Title = title;
        ResourcePath = resourcePath;
        Type = type;
    }
}

/// <summary>
/// Tipos de conteúdo suportados
/// </summary>
public enum ContentType
{
    Photo,
    Video
}
