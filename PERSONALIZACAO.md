# Guia de Personalização do App Base VR Meta Quest 3

Este guia fornece instruções detalhadas para personalizar o aplicativo base de visualização de conteúdo 360° para Meta Quest 3.

## Índice
1. [Adicionando Seu Conteúdo](#adicionando-seu-conteúdo)
2. [Personalizando a Interface](#personalizando-a-interface)
3. [Modificando a Navegação](#modificando-a-navegação)
4. [Expandindo Funcionalidades](#expandindo-funcionalidades)
5. [Dicas de Otimização](#dicas-de-otimização)

## Adicionando Seu Conteúdo

### Fotos 360°

Para adicionar suas próprias fotos panorâmicas em 360°:

1. **Prepare suas imagens**:
   - Use formato equiretangular (proporção 2:1)
   - Resolução recomendada: 4K (4096x2048) ou superior
   - Formatos suportados: JPG, PNG
   - Otimize o tamanho dos arquivos para melhor desempenho

2. **Adicione ao projeto**:
   ```
   Assets/
   └── Resources/
       └── Photos/
           ├── categoria1/
           │   ├── foto1.jpg
           │   └── foto2.jpg
           └── categoria2/
               └── foto3.jpg
   ```

3. **Crie miniaturas**:
   - Prepare versões reduzidas (256x256 pixels) para o menu
   - Salve na pasta `Assets/Resources/Thumbnails/`
   - Use o mesmo nome do arquivo original

4. **Atualize o arquivo de configuração**:
   - Edite `menu_config.json` para incluir suas novas fotos
   - Exemplo:
     ```json
     {
       "categories": [
         {
           "name": "Paisagens",
           "items": [
             {
               "title": "Praia Tropical",
               "path": "Photos/categoria1/foto1",
               "type": "photo"
             }
           ]
         }
       ]
     }
     ```

### Vídeos 360°

Para adicionar vídeos em 360°:

1. **Prepare seus vídeos**:
   - Formato equiretangular (proporção 2:1)
   - Codificação recomendada: H.264 em MP4
   - Resolução ideal: 4K (3840x1920)
   - Bitrate: 30-60 Mbps para melhor qualidade

2. **Adicione ao projeto**:
   ```
   Assets/
   └── Resources/
       └── Videos/
           ├── categoria1/
           │   ├── video1.mp4
           │   └── video2.mp4
           └── categoria2/
               └── video3.mp4
   ```

3. **Crie frames de pré-visualização**:
   - Capture um frame representativo de cada vídeo
   - Salve como miniatura na pasta `Assets/Resources/Thumbnails/`

4. **Atualize o arquivo de configuração**:
   - Adicione entradas para seus vídeos em `menu_config.json`
   - Exemplo:
     ```json
     {
       "categories": [
         {
           "name": "Tours Virtuais",
           "items": [
             {
               "title": "Tour Paris",
               "path": "Videos/categoria1/video1",
               "type": "video"
             }
           ]
         }
       ]
     }
     ```

## Personalizando a Interface

### Cores e Tema

Para modificar as cores e o tema visual:

1. Abra a cena `MainMenu.unity`
2. Selecione o objeto `ThemeManager` no Hierarchy
3. No Inspector, você encontrará as seguintes opções:
   - `Primary Color`: Cor principal da interface
   - `Secondary Color`: Cor de destaque
   - `Background Color`: Cor de fundo
   - `Text Color`: Cor do texto
   - `Highlight Color`: Cor de seleção

### Fontes e Textos

Para alterar fontes e textos:

1. Importe suas fontes personalizadas em `Assets/Fonts/`
2. Selecione os elementos de texto no Hierarchy
3. No Inspector, modifique:
   - Font Asset: Sua fonte personalizada
   - Font Size: Tamanho do texto
   - Font Style: Estilo (negrito, itálico, etc.)

### Ambiente de Fundo

Para personalizar o ambiente de fundo:

1. Selecione o objeto `Environment` no Hierarchy
2. Substitua o Skybox Material por um personalizado
3. Ajuste a iluminação conforme necessário
4. Para um ambiente mais imersivo, adicione modelos 3D simples

## Modificando a Navegação

### Layout do Menu

Para alterar o layout do menu:

1. Abra a cena `MainMenu.unity`
2. Selecione o objeto `MenuContainer` no Hierarchy
3. Modifique:
   - Posição e escala dos painéis
   - Espaçamento entre botões
   - Organização das categorias

### Interação com Controladores

Para personalizar a interação:

1. Abra o script `VRMenuManager.cs`
2. Localize a seção de mapeamento de botões
3. Modifique as ações associadas a cada botão
4. Exemplo:
   ```csharp
   // Alterar o botão para selecionar itens
   if (rightController.inputDevice.TryGetFeatureValue(
       UnityEngine.XR.CommonUsages.primaryButton, out bool pressed) && pressed)
   {
       // Sua ação personalizada aqui
   }
   ```

### Gestos e Movimentos

Para adicionar gestos personalizados:

1. Crie um novo script derivado de `XRGestureInteractor`
2. Implemente a detecção de gestos desejados
3. Associe ações aos gestos reconhecidos
4. Adicione o script ao objeto `XR Rig` na cena

## Expandindo Funcionalidades

### Adicionando Novos Tipos de Conteúdo

Para suportar novos tipos de conteúdo:

1. Atualize o enum `ContentType` em `VRMenuManager.cs`:
   ```csharp
   public enum ContentType
   {
       Photo,
       Video,
       Model3D,  // Novo tipo para modelos 3D
       Audio     // Novo tipo para áudio espacial
   }
   ```

2. Crie visualizadores para os novos tipos:
   - `Model3DViewer.cs` para modelos 3D
   - `AudioPlayer360.cs` para áudio espacial

3. Atualize o método `OpenContent` para lidar com os novos tipos

### Implementando Interatividade

Para adicionar elementos interativos:

1. Crie objetos interativos usando `XRGrabInteractable`
2. Adicione scripts de comportamento personalizado
3. Exemplo para um objeto informativo:
   ```csharp
   public class InfoPoint : MonoBehaviour
   {
       public string title;
       public string description;
       
       public void OnSelect()
       {
           // Mostrar painel de informações
           UIManager.Instance.ShowInfoPanel(title, description);
       }
   }
   ```

### Integrando Áudio Espacial

Para adicionar áudio espacial:

1. Importe arquivos de áudio em `Assets/Resources/Audio/`
2. Adicione componentes `AudioSource` aos objetos
3. Configure:
   - Spatial Blend: 1 (totalmente espacial)
   - Min/Max Distance: Ajuste conforme necessário
   - Rolloff: Logarítmico para maior realismo

## Dicas de Otimização

### Desempenho Gráfico

Para otimizar o desempenho:

1. **Texturas**:
   - Use compressão adequada (ASTC para Quest)
   - Reduza resolução de texturas secundárias
   - Implemente mip-mapping para texturas distantes

2. **Shaders**:
   - Use shaders otimizados para mobile VR
   - Evite efeitos de pós-processamento pesados
   - Simplifique materiais quando possível

3. **Geometria**:
   - Mantenha baixa contagem de polígonos
   - Use LOD (Level of Detail) para objetos complexos
   - Combine meshes estáticos quando possível

### Otimização de Memória

Para gerenciar o uso de memória:

1. **Carregamento Assíncrono**:
   - Implemente carregamento assíncrono para conteúdo grande
   - Use `Resources.UnloadUnusedAssets()` regularmente

2. **Pooling de Objetos**:
   - Reutilize objetos em vez de criar/destruir
   - Implemente um sistema de object pooling para UI

3. **Streaming de Vídeo**:
   - Use streaming progressivo para vídeos grandes
   - Ajuste qualidade de vídeo dinamicamente

---

## Suporte e Recursos Adicionais

Para mais informações e suporte:

- **Documentação Meta Quest**: [developer.oculus.com](https://developer.oculus.com)
- **Fóruns Unity XR**: [forum.unity.com/forums/xr-platform-ar-vr.80/](https://forum.unity.com/forums/xr-platform-ar-vr.80/)
- **Tutoriais VR**: [learn.unity.com/pathway/vr-development](https://learn.unity.com/pathway/vr-development)

---

© 2025 [Sua Empresa]. Todos os direitos reservados.
