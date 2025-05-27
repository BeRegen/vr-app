# Aplicativo Base para Meta Quest 3
## Navegação Intuitiva com Visualização de Conteúdo 360°

Este projeto contém um aplicativo base para Meta Quest 3, desenvolvido em Unity, que permite navegação intuitiva em ambiente VR e visualização de conteúdo em 360° (fotos e vídeos).

## Requisitos

- Unity 2022.3 LTS ou superior
- Meta Quest SDK (v60.0 ou superior)
- Meta Quest 3 para testes (ou emulador)
- Visual Studio 2022 ou JetBrains Rider

## Configuração do Ambiente

### 1. Instalação do Unity

1. Baixe e instale o Unity Hub em [unity.com/download](https://unity.com/download)
2. Instale a versão Unity 2022.3 LTS através do Unity Hub
3. Durante a instalação, selecione os seguintes módulos:
   - Android Build Support
   - OpenJDK
   - Android SDK & NDK Tools

### 2. Configuração do SDK Meta Quest

1. Acesse o [Portal de Desenvolvedores Meta](https://developer.oculus.com/downloads/package/meta-quest-sdk/)
2. Baixe o Meta Quest SDK mais recente
3. Extraia o SDK em um local de fácil acesso

### 3. Importação do Projeto

1. Abra o Unity Hub
2. Clique em "Adicionar" e selecione a pasta deste projeto
3. Abra o projeto com a versão Unity 2022.3 LTS
4. Aguarde a importação dos pacotes e a compilação inicial

### 4. Configuração para Meta Quest 3

1. No Unity, vá para Edit > Project Settings
2. Selecione "XR Plugin Management" no menu lateral
3. Na aba "Android", marque "OpenXR" e "Meta XR"
4. Clique em "OpenXR" no menu lateral
5. Adicione "Meta Quest Touch Pro Controller Profile"
6. Em "Player Settings", configure:
   - Company Name: [Seu nome ou empresa]
   - Product Name: [Nome do seu aplicativo]
   - Package Name: com.[empresa].[app] (ex: com.mycompany.vrapp)

## Estrutura do Projeto

```
Assets/
├── Scenes/
│   ├── MainMenu.unity        # Menu principal com navegação
│   ├── PhotoViewer.unity     # Visualizador de fotos 360°
│   └── VideoPlayer.unity     # Player de vídeos 360°
├── Scripts/
│   ├── Navigation/           # Scripts de navegação VR
│   ├── UI/                   # Interface de usuário
│   └── Viewers/              # Visualizadores de conteúdo 360°
├── Prefabs/                  # Componentes reutilizáveis
├── Materials/                # Materiais e shaders
└── Resources/                # Conteúdo carregado dinamicamente
```

## Funcionalidades Principais

### Menu de Navegação

O menu principal oferece uma interface intuitiva com:
- Painéis flutuantes organizados em grade
- Navegação por apontamento e seleção
- Miniaturas para pré-visualização de conteúdo
- Categorias personalizáveis

### Visualizador de Fotos 360°

Permite a visualização de imagens panorâmicas com:
- Rotação por movimento da cabeça
- Zoom por gestos dos controladores
- Controles de brilho e contraste
- Retorno fácil ao menu principal

### Player de Vídeos 360°

Reproduz vídeos em formato 360° com:
- Controles de reprodução (play, pause, avançar)
- Ajuste de volume
- Opções de qualidade
- Suporte a vídeos locais e streaming

## Personalização

### Adicionando Conteúdo

1. **Fotos 360°**:
   - Coloque suas imagens panorâmicas na pasta `Assets/Resources/Photos`
   - Formate as imagens como equiretangulares (proporção 2:1)
   - Formatos suportados: JPG, PNG (recomendado 4K ou superior)

2. **Vídeos 360°**:
   - Adicione vídeos na pasta `Assets/Resources/Videos`
   - Formatos suportados: MP4 com codificação H.264
   - Resolução recomendada: 4K (3840x1920) ou superior

3. **Configuração do Menu**:
   - Edite o arquivo `Assets/Resources/menu_config.json` para organizar categorias
   - Adicione miniaturas na pasta `Assets/Resources/Thumbnails`

### Personalização Visual

1. Abra a cena `MainMenu.unity`
2. Selecione o objeto `EnvironmentSettings` no Hierarchy
3. No Inspector, você pode modificar:
   - Cores do tema
   - Ambiente de fundo
   - Fontes e tamanhos de texto
   - Efeitos visuais

## Compilação e Publicação

### Compilação para Teste

1. Conecte seu Meta Quest 3 ao computador via USB
2. Ative o modo de desenvolvedor no headset
3. No Unity, vá para File > Build Settings
4. Selecione "Android" como plataforma
5. Clique em "Build and Run"

### Preparação para Publicação

1. Atualize os ícones do aplicativo em `Player Settings`
2. Configure a versão e o número de build
3. Adicione sua chave de assinatura para Android
4. Gere o APK final através de "Build"

### Distribuição

Para publicação na Meta Quest Store:
1. Crie uma conta de desenvolvedor em [developer.oculus.com](https://developer.oculus.com)
2. Siga as diretrizes de submissão da Meta
3. Envie seu APK para revisão

Para distribuição privada (sideloading):
1. Compartilhe o APK diretamente
2. Os usuários podem instalar via SideQuest ou similar

## Solução de Problemas

### Problemas Comuns

1. **Erro de compilação para Android**:
   - Verifique se o Android SDK está corretamente configurado
   - Confirme que o JDK está instalado e configurado

2. **Desempenho baixo no headset**:
   - Reduza a resolução das texturas
   - Simplifique os shaders
   - Otimize a iluminação

3. **Problemas com controladores**:
   - Verifique se o perfil correto está selecionado nas configurações OpenXR
   - Teste com diferentes mapeamentos de botões

## Próximos Passos

Este aplicativo base pode ser expandido de várias formas:

1. Adicione interatividade aos ambientes 360°
2. Implemente sistema de áudio espacial
3. Adicione suporte a múltiplos usuários
4. Integre com serviços de armazenamento em nuvem
5. Adicione animações e efeitos visuais personalizados

## Suporte

Para dúvidas ou suporte adicional, entre em contato:
- Email: beregenerative.life@gmail.com      
- Website: https://github.com/BeRegen

---

© 2025 BeRegen. Todos os direitos reservados.
