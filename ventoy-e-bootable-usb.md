# Migração Windows <-> Linux com Ventoy e USB inicializável

Documento de referência técnica para entender e executar, com segurança, a migração de uma máquina que usa Windows para Linux, o caminho inverso de Linux para Windows e as diferenças práticas entre Ubuntu e outras distribuições.

Contexto prático: preparar uma mídia USB inicializável com Ventoy para instalar Ubuntu em uma máquina física. O foco principal deste guia continua sendo **Windows -> Ubuntu**, mas o mesmo modelo é generalizado para **Windows -> outras distros Linux** e para o retorno **Linux -> Windows**.

Fontes consultadas: Ubuntu/Canonical, Ventoy, Microsoft, Fedora Project, Debian, Linux Mint, openSUSE e Arch Linux. Links oficiais estão reunidos no fim do documento.

> Data da revisão: 2026-07-29. Nesta data, as páginas oficiais do Ubuntu indicam Ubuntu 26.04 LTS como a versão LTS atual para Desktop e Server.

---

## 1. Ideia central: migração não é conversão

Migrar de Windows para Ubuntu **não transforma** uma instalação Windows em uma instalação Linux. O que acontece, tecnicamente, é uma destas operações:

| Estratégia | O que acontece no disco interno | Quando usar |
|---|---|---|
| Substituição total | As partições do Windows são removidas ou sobrescritas; o Ubuntu passa a ocupar o disco | Quando a máquina será Linux e os dados já foram salvos fora dela |
| Dual boot | Windows e Ubuntu ficam em partições diferentes; o firmware/bootloader escolhe qual iniciar | Quando você ainda precisa manter Windows localmente |
| Teste live | O Ubuntu roda a partir do pendrive sem instalar no disco interno | Quando você quer testar hardware, rede, teclado, vídeo e Wi-Fi antes de decidir |
| Máquina virtual | Ubuntu roda como convidado dentro do Windows, ou Windows roda dentro do Ubuntu | Quando o objetivo é estudo/desenvolvimento sem alterar o boot físico da máquina |

Para engenharia de software, pense no processo como uma troca de **estado-alvo do disco**:

1. Você preserva dados importantes fora da máquina.
2. Você prepara uma mídia confiável de instalação.
3. O firmware inicializa essa mídia.
4. O instalador escreve uma nova estrutura de partições, bootloader e sistema operacional no disco interno.
5. Você restaura dados, configura ferramentas e valida hardware.

O pendrive não é "a migração". Ele é apenas o meio de transporte do instalador.

---

## 2. Modelo mental do boot

Ao ligar um PC, a sequência simplificada é:

1. O firmware da placa-mãe inicia o hardware.
2. O firmware procura um dispositivo inicializável, seguindo a ordem configurada ou o menu de boot.
3. O firmware encontra um bootloader.
4. O bootloader carrega o kernel ou o instalador do sistema operacional.
5. O sistema operacional assume o controle.

No caso de uma instalação por USB:

```text
Firmware (BIOS/UEFI)
        |
        v
Pendrive inicializável
        |
        v
Bootloader do pendrive
        |
        v
Instalador do Ubuntu ou Windows
        |
        v
Disco interno da máquina
```

O ponto crítico: **dar boot pelo pendrive não altera o disco interno por si só**. A alteração acontece quando você, dentro do instalador, escolhe instalar, apagar disco, criar partições ou formatar.

---

## 3. BIOS, UEFI, MBR, GPT e Secure Boot

Esses termos aparecem juntos, mas representam camadas diferentes.

| Termo | Camada | Função |
|---|---|---|
| BIOS legado | Firmware | Modo antigo de inicialização; normalmente associado a MBR |
| UEFI | Firmware | Modo moderno de inicialização; normalmente associado a GPT e partição EFI |
| MBR | Tabela de partição | Estrutura antiga; também contém código de boot no primeiro setor em sistemas BIOS |
| GPT | Tabela de partição | Estrutura moderna; usada em instalações UEFI |
| ESP | Partição | EFI System Partition; partição FAT32 onde ficam arquivos `.efi` de boot |
| Secure Boot | Política de segurança UEFI | Só permite bootloaders assinados ou autorizados |

Em máquinas modernas, o caminho mais comum é:

```text
UEFI -> ESP -> arquivo .efi -> bootloader -> kernel/instalador
```

Em máquinas antigas, o caminho típico é:

```text
BIOS -> MBR -> bootloader -> kernel/instalador
```

### Implicação prática

Antes da instalação, descubra se a máquina de destino inicializa em UEFI ou BIOS legado. Isso afeta:

- como o pendrive aparece no menu de boot;
- se Secure Boot pode interferir;
- como o Ubuntu instalará o bootloader;
- como um eventual dual boot com Windows será organizado.

---

## 4. O que é uma imagem ISO

Uma imagem ISO é um arquivo que representa uma mídia de instalação. No caso do Ubuntu, a ISO contém o sistema live, o instalador e os arquivos necessários para iniciar a máquina pelo pendrive.

Importante:

- copiar uma ISO para um pendrive comum **não** torna o pendrive bootável;
- ferramentas como Rufus, balenaEtcher e `dd` gravam a imagem de modo especial;
- o Ventoy é diferente: ele instala um bootloader próprio no pendrive e depois permite copiar ISOs como arquivos comuns.

Outros formatos comuns:

| Formato | Uso típico |
|---|---|
| `.iso` | Instaladores de sistemas operacionais, como Ubuntu e Windows |
| `.img` | Imagem bruta de disco ou cartão SD |
| `.wim` / `.esd` | Imagens internas usadas pelo instalador do Windows |
| `.vhd` / `.vhdx` | Discos virtuais, muito usados com Hyper-V |
| `.efi` | Executável de boot reconhecido por firmware UEFI |

---

## 5. Ventoy em uma frase

Ventoy é uma ferramenta open source para criar um pendrive inicializável capaz de carregar arquivos como ISO, WIM, IMG, VHD/VHDx e EFI.

O fluxo tradicional é:

```text
Baixar ISO -> gravar pendrive -> usar -> apagar pendrive -> gravar outra ISO
```

Com Ventoy:

```text
Instalar Ventoy uma vez -> copiar ISOs para o pendrive -> escolher a ISO no menu de boot
```

Isso é útil para estudo e laboratório porque o mesmo pendrive pode carregar, por exemplo:

- Ubuntu Desktop;
- Ubuntu Server;
- Windows 11;
- ferramentas de recuperação;
- outras distribuições Linux.

### Atenção importante

Instalar o Ventoy no pendrive **formata o pendrive**. Depois que o Ventoy já está instalado, atualizar o Ventoy tende a preservar os arquivos da primeira partição, conforme a documentação oficial, mas a instalação inicial apaga os dados.

---

## 6. Antes de migrar Windows -> Ubuntu

Não pule esta etapa. Ela é a diferença entre uma migração controlada e uma perda de dados com cara de surpresa.

### 6.1 Fazer backup

Salve fora da máquina:

- `Desktop`;
- `Documents`;
- `Downloads`;
- `Pictures`;
- projetos de código;
- chaves SSH;
- arquivos `.env`;
- bancos locais;
- favoritos do navegador;
- perfis/exportações de ferramentas;
- instaladores ou licenças de programas pagos;
- arquivos de máquinas virtuais.

No Windows, a Microsoft oferece o Windows Backup para salvar arquivos, configurações e preferências associadas à conta Microsoft/OneDrive. Para engenharia e desenvolvimento, isso não substitui um backup manual de projetos, chaves e bancos locais.

Checklist mínimo:

```text
[ ] Meus arquivos pessoais foram copiados para outro disco ou nuvem.
[ ] Meus projetos Git foram enviados para um remoto ou copiados.
[ ] Minhas chaves SSH/GPG foram salvas de forma segura.
[ ] Bancos locais e arquivos de configuração foram exportados.
[ ] Eu sei reinstalar minhas ferramentas depois.
```

### 6.2 Conferir BitLocker

Se o Windows usa BitLocker ou criptografia de dispositivo, salve a chave de recuperação antes de alterar boot, firmware ou partições. A Microsoft documenta que mudanças de hardware, firmware ou software podem fazer o Windows pedir a chave de recuperação.

Checklist:

```text
[ ] Verifiquei se o BitLocker/Device Encryption está ativo.
[ ] Salvei a chave de recuperação fora do disco interno.
[ ] Confirmei que consigo acessar essa chave sem depender da máquina que será formatada.
```

### 6.3 Conferir licença do Windows

Mesmo migrando para Ubuntu, registre o estado do Windows antes:

- edição instalada: Home, Pro, Enterprise etc.;
- status de ativação;
- conta Microsoft vinculada;
- product key, se houver.

A Microsoft informa que uma licença digital fica associada ao hardware e pode permitir reinstalar a mesma edição do Windows sem digitar product key. Ainda assim, anote a edição correta. Reinstalar Windows Home em uma máquina licenciada para Pro, ou o contrário, costuma gerar problema de ativação.

### 6.4 Escolher Ubuntu Desktop ou Ubuntu Server

| Versão | Interface | Uso ideal |
|---|---|---|
| Ubuntu Desktop | Interface gráfica GNOME | Estudo, uso diário, desenvolvimento local, navegador, IDEs |
| Ubuntu Server | Instalação em modo texto; sem desktop por padrão | Servidores, laboratório de rede, serviços, máquinas sem monitor |

Se a máquina será sua estação de estudo/desenvolvimento, use **Ubuntu Desktop**.

Se a máquina será um servidor de testes, use **Ubuntu Server**.

Em 2026-07-29, as páginas oficiais indicam Ubuntu 26.04 LTS como LTS atual. LTS é a escolha conservadora para estabilidade: a Canonical descreve LTS como versões com 5 anos de manutenção de segurança padrão, com extensão possível via Ubuntu Pro.

---

## 7. Baixar e verificar a ISO do Ubuntu

### 7.1 Download oficial

Baixe somente das páginas oficiais:

- Ubuntu Desktop: `https://ubuntu.com/download/desktop`
- Ubuntu Server: `https://ubuntu.com/download/server`

Para PCs comuns com processador Intel ou AMD de 64 bits, escolha a imagem `amd64` ou "Intel/AMD 64-bit architecture".

### 7.2 Verificar integridade

A verificação responde a duas perguntas:

1. O arquivo baixou sem corromper?
2. O arquivo corresponde ao que o Ubuntu publicou?

No Linux:

```bash
sha256sum ubuntu-26.04-desktop-amd64.iso
```

No Windows PowerShell:

```powershell
Get-FileHash .\ubuntu-26.04-desktop-amd64.iso -Algorithm SHA256
```

Compare o hash com o arquivo `SHA256SUMS` publicado pelo Ubuntu para a versão baixada. Para uma verificação mais forte, valide também a assinatura `SHA256SUMS.gpg` com GPG, como descrito no tutorial oficial do Ubuntu.

Regra prática: se o checksum não bater, **não instale**. Baixe novamente.

---

## 8. Preparar o pendrive com Ventoy

### 8.1 O que você precisa

- Um pendrive vazio ou que possa ser apagado.
- Ventoy baixado do site oficial: `https://www.ventoy.net/en/download.html`
- ISO oficial do Ubuntu já verificada.
- Acesso ao menu de boot da máquina de destino.

Use um pendrive de pelo menos 8 GB. Para manter várias ISOs, prefira 32 GB ou mais.

### 8.2 Instalar Ventoy pelo Windows

1. Baixe o pacote Windows do Ventoy.
2. Extraia o `.zip`.
3. Execute `Ventoy2Disk.exe` ou uma variante adequada da pasta `altexe`, se necessário.
4. Selecione o pendrive correto.
5. Clique em `Install`.
6. Confirme sabendo que o pendrive será apagado.

A documentação oficial informa que o Ventoy lista apenas dispositivos USB por padrão para evitar erro operacional. Existe opção para mostrar todos os discos, mas ela deve ser usada com extremo cuidado.

### 8.3 Instalar Ventoy pelo Linux

Primeiro identifique o pendrive:

```bash
lsblk
```

Exemplo de instalação:

```bash
sudo sh Ventoy2Disk.sh -i /dev/sdX
```

Onde `/dev/sdX` é o dispositivo inteiro do pendrive, como `/dev/sdb`, e não uma partição como `/dev/sdb1`.

Comandos relevantes segundo a documentação do Ventoy:

| Opção | Significado |
|---|---|
| `-i` | instala o Ventoy se ele ainda não estiver instalado |
| `-I` | força a instalação mesmo se já houver Ventoy |
| `-u` | atualiza o Ventoy |
| `-l` | lista informações do Ventoy no dispositivo |
| `-s` | habilita suporte a Secure Boot |
| `-g` | usa GPT em vez do MBR padrão |

### 8.4 Copiar a ISO

Depois da instalação, o pendrive terá uma partição principal de dados. Copie a ISO para essa partição:

```text
Ventoy/
  ubuntu-26.04-desktop-amd64.iso
  ubuntu-26.04-live-server-amd64.iso
  Win11_*.iso
```

Não é preciso gravar a ISO de novo. Copiar o arquivo basta.

O Ventoy procura imagens em diretórios e subdiretórios e as lista no menu de boot.

---

## 9. Secure Boot com Ventoy e Ubuntu

Secure Boot é uma política do UEFI para bloquear bootloaders não autorizados. O Ubuntu oficial normalmente lida bem com Secure Boot em instalações UEFI comuns. Com Ventoy, há uma camada a mais: o firmware precisa aceitar o bootloader do Ventoy.

Segundo a documentação do Ventoy, há suporte a Secure Boot e uma opção específica para habilitá-lo. Em Windows, ela aparece no menu de opções do `Ventoy2Disk.exe`. Em Linux, pode ser usada com `-s`.

Fluxo recomendado:

1. Tente manter Secure Boot habilitado.
2. Se o Ventoy não iniciar, confira se o Ventoy foi instalado com suporte a Secure Boot.
3. Se a tela de inscrição/registro de chave do Ventoy aparecer no primeiro boot, siga a documentação oficial do Ventoy.
4. Desabilite Secure Boot apenas como diagnóstico ou decisão consciente, entendendo que isso reduz uma proteção de boot.

---

## 10. Instalar Ubuntu no lugar do Windows

Esta é a rota de migração total: Windows sai, Ubuntu entra.

### 10.1 Boot pelo pendrive

1. Conecte o pendrive Ventoy na máquina de destino.
2. Ligue ou reinicie.
3. Abra o menu de boot. Teclas comuns: `F12`, `Esc`, `F2`, `F10` ou `Del`.
4. Escolha o pendrive.
5. No menu do Ventoy, selecione a ISO do Ubuntu.

O tutorial oficial do Ubuntu Desktop menciona `F12` como tecla comum de menu de boot, com `Esc`, `F2` e `F10` como alternativas frequentes.

### 10.2 Testar antes de instalar

No Ubuntu Desktop, escolha a opção de testar quando disponível. Verifique:

- teclado;
- mouse/touchpad;
- vídeo;
- Wi-Fi ou Ethernet;
- áudio;
- suspensão/retorno;
- reconhecimento do disco interno.

No Ubuntu Server, o foco é validar:

- rede;
- detecção do disco;
- teclado;
- acesso ao instalador em modo texto.

### 10.3 Escolher o tipo de instalação

No Ubuntu Desktop, o instalador pode oferecer opções como:

| Opção | Efeito |
|---|---|
| Try Ubuntu | Testa sem instalar |
| Install Ubuntu | Inicia instalação |
| Erase disk and install Ubuntu | Remove o sistema existente e instala Ubuntu |
| Install alongside | Instala Ubuntu ao lado do sistema existente, quando suportado |
| Manual installation | Permite controlar partições manualmente |

Para migração total, escolha a opção equivalente a **Erase disk and install Ubuntu**.

Para dual boot, escolha **Install alongside** quando o instalador oferecer essa opção, ou faça particionamento manual com cuidado.

### 10.4 O que acontece no disco

Em uma instalação UEFI típica, o Ubuntu cria ou usa:

| Partição | Sistema de arquivos | Montagem | Função |
|---|---|---|---|
| EFI System Partition | FAT32 | `/boot/efi` | Arquivos de boot UEFI |
| Root | ext4, ou outra escolha suportada | `/` | Sistema Ubuntu |
| Swap | arquivo ou partição, conforme instalação | swap | Memória auxiliar |
| Home opcional | ext4, ou outra escolha suportada | `/home` | Dados de usuários |

Em substituição total, o instalador pode apagar partições antigas do Windows, incluindo `C:`, recuperação e partições de sistema. Por isso o backup precisa estar pronto antes.

### 10.5 Finalização

Após instalar:

1. Remova o pendrive quando o instalador pedir.
2. Reinicie.
3. Entre no Ubuntu instalado.
4. Atualize o sistema:

```bash
sudo apt update
sudo apt upgrade
```

5. Instale drivers adicionais, se aplicável:

```bash
software-properties-gtk
```

No Ubuntu Server, use ferramentas de terminal e pacotes apropriados ao hardware.

---

## 11. Dual boot: Windows e Ubuntu na mesma máquina

Dual boot é mais delicado que substituição total porque dois sistemas passam a dividir firmware, disco e boot.

Checklist antes:

```text
[ ] Backup completo feito.
[ ] BitLocker suspenso/desativado ou chave de recuperação salva.
[ ] Espaço livre criado para Ubuntu.
[ ] Windows desligado completamente, sem hibernação.
[ ] Modo de boot consistente: ambos em UEFI ou ambos em Legacy, se possível.
```

### 11.1 Criar espaço livre pelo Windows

Use o Gerenciamento de Disco do Windows para reduzir a partição principal e deixar espaço não alocado. A Microsoft documenta o Gerenciamento de Disco como ferramenta nativa para criar, formatar, estender e reduzir volumes.

Não formate esse espaço como NTFS para o Ubuntu. Deixe como **não alocado** e permita que o instalador do Ubuntu use esse espaço.

### 11.2 Instalar Ubuntu ao lado do Windows

No instalador, escolha a opção de instalar ao lado do Windows se ela aparecer. Se não aparecer, pare e investigue antes de usar particionamento manual.

Possíveis causas para a opção não aparecer:

- Windows e pendrive foram iniciados em modos diferentes, por exemplo Windows em UEFI e USB em Legacy;
- BitLocker ou hibernação interferindo;
- disco configurado em modo RAID/RST em vez de AHCI;
- tabela de partição ou espaço livre não reconhecidos como esperado.

### 11.3 Depois da instalação

Ao reiniciar, você pode ver:

- menu do GRUB com Ubuntu e Windows;
- boot direto no Ubuntu;
- boot direto no Windows.

Se um sistema não aparece, primeiro confira a ordem de boot no firmware. Em dual boot UEFI, é comum existirem entradas separadas para `ubuntu` e `Windows Boot Manager`.

---

## 12. Caminho inverso: Linux -> Windows

Migrar de Linux para Windows segue o mesmo princípio da ida Windows -> Linux: não há conversão do sistema instalado. Há backup, mídia de instalação, boot por USB e reescrita de partições no disco interno.

Esta seção vale para Ubuntu, Debian, Fedora, Linux Mint, openSUSE, Arch e outras distribuições. O que muda entre elas é o modo de inventariar pacotes e configurações antes de apagar o sistema.

### 12.1 Decidir o alvo

| Alvo | O que acontece |
|---|---|
| Substituir Linux por Windows | O instalador do Windows remove ou ignora as partições Linux e cria as partições necessárias para Windows |
| Manter Linux e instalar Windows em dual boot | É preciso deixar espaço livre, instalar Windows e depois validar ou reparar o bootloader Linux |
| Voltar para Windows e preservar dados Linux em outro disco | Os dados precisam estar em formato que o Windows leia, como exFAT ou NTFS, ou em backup externo/nuvem |

O Windows não usa ext4, Btrfs ou XFS como sistemas de arquivos nativos para instalação comum. Portanto, não conte com o instalador do Windows para "aproveitar" diretamente uma partição Linux existente como se fosse `C:`.

### 12.2 Inventariar o Linux antes de apagar

Salve:

- `/home`;
- arquivos ocultos importantes em `/home/usuario`, como `.ssh`, `.gnupg`, `.config`, `.gitconfig`;
- projetos Git;
- chaves SSH/GPG;
- arquivos `.env`;
- bancos locais;
- containers, volumes Docker e imagens importantes;
- configurações de IDE;
- lista de pacotes instalados;
- configurações manuais de serviços em `/etc`, quando a máquina funcionava como servidor.

Comandos úteis para entender a máquina:

```bash
lsblk -f
df -h
test -d /sys/firmware/efi && echo UEFI || echo BIOS
```

Se o pacote `mokutil` estiver instalado, você também pode conferir Secure Boot:

```bash
mokutil --sb-state
```

Para registrar pacotes instalados, use o gerenciador da sua distro:

| Família | Comandos úteis |
|---|---|
| Ubuntu, Debian, Linux Mint | `apt-mark showmanual > pacotes-manuais.txt` e `dpkg --get-selections > pacotes-dpkg.txt` |
| Fedora | `dnf repoquery --userinstalled > pacotes-fedora.txt` ou `rpm -qa > pacotes-rpm.txt` |
| openSUSE | `zypper search --installed-only > pacotes-opensuse.txt` |
| Arch | `pacman -Qqe > pacotes-arch-explicitos.txt` |
| Flatpak | `flatpak list --app > flatpaks.txt` |

Para backup de diretórios, prefira preservar atributos:

```bash
rsync -aHAX --info=progress2 /home/usuario/ /media/backup/home-usuario/
```

Se usa Docker, liste o que existe antes de exportar:

```bash
docker ps -a
docker image ls
docker volume ls
```

Exporte volumes ou dados de bancos de forma própria. Copiar apenas `/var/lib/docker` sem entender o estado do daemon pode gerar backup difícil de restaurar.

### 12.3 Criar mídia oficial do Windows

A rota mais alinhada à documentação da Microsoft é criar a mídia de instalação pelo site oficial de download do Windows. Para Windows 11, o fluxo oficial em um PC Windows é baixar e executar o `MediaCreationTool.exe`, que guia a criação da mídia.

Requisitos importantes documentados pela Microsoft:

- conexão confiável com a internet;
- pendrive vazio com pelo menos 8 GB;
- conteúdo do pendrive será apagado.

Se você só tem Linux disponível, há duas rotas práticas:

1. Usar outro PC com Windows para executar o Media Creation Tool e gerar o pendrive.
2. Baixar a ISO oficial do Windows no site da Microsoft e inicializá-la com Ventoy.

A segunda opção é conveniente em laboratório porque o Ventoy suporta arquivos ISO/WIM/IMG/VHD(x)/EFI. Se a instalação do Windows falhar a partir do Ventoy em algum hardware específico, volte para a rota mais conservadora: mídia criada pelo Media Creation Tool em um PC Windows.

Em 2026-07-29, a documentação oficial da Microsoft informa que o suporte do Windows 10 terminou em 2025-10-14. Para uma instalação nova, prefira Windows 11 quando o hardware for compatível e a licença permitir.

### 12.4 Instalar Windows apagando Linux

1. Dê boot pela mídia oficial do Windows ou pelo Ventoy com a ISO oficial do Windows.
2. Escolha idioma, teclado e edição correta.
3. Quando o instalador perguntar o tipo de instalação, escolha a opção personalizada/avançada para instalação limpa.
4. Se a intenção for substituir Linux totalmente, selecione as partições Linux do disco interno e apague-as.
5. Deixe espaço não alocado para o instalador criar as partições do Windows.
6. Prossiga com a instalação.
7. Após reiniciar, rode Windows Update.
8. Instale drivers ausentes pelo fabricante do notebook/placa-mãe, quando necessário.
9. Ative o Windows com licença digital ou product key.
10. Restaure dados do backup.

A Microsoft descreve instalação limpa como uma opção avançada e alerta que ela remove arquivos pessoais, aplicativos, customizações do fabricante e alterações de configuração.

### 12.5 Instalar Windows mantendo Linux em dual boot

Essa rota é mais trabalhosa porque o instalador do Windows tende a priorizar o Windows Boot Manager.

Fluxo recomendado:

1. No Linux, reduza uma partição com ferramenta confiável ou use um live USB com GParted.
2. Deixe espaço **não alocado** para o Windows.
3. Dê boot no instalador do Windows no mesmo modo do Linux existente, preferencialmente UEFI.
4. Instale Windows no espaço não alocado.
5. Depois da instalação, confira no firmware se ainda existe uma entrada de boot da distro Linux.
6. Se o firmware iniciar direto no Windows, altere a ordem de boot para a entrada Linux, quando ela existir.
7. Se a entrada Linux sumiu ou o GRUB não aparece, use um live USB da distro para reparar o bootloader.

Em UEFI, Windows e Linux normalmente podem conviver na mesma EFI System Partition, cada um com sua pasta de boot. O conflito mais comum não é "um sistema apagar o outro", e sim a ordem de boot mudar para `Windows Boot Manager`.

### 12.6 Depois que o Windows voltar

Valide:

- ativação;
- rede;
- áudio;
- vídeo;
- leitor biométrico, se houver;
- suspensão/retorno;
- Windows Update;
- drivers do fabricante;
- restauração dos dados.

Se a máquina será usada para desenvolvimento, reinstale ferramentas como Git, VS Code, SDKs, Docker Desktop/WSL e gerenciadores de pacotes. Se quiser continuar estudando Linux dentro do Windows, WSL é uma alternativa sem alterar partições.

---

## 13. Windows -> outras distros Linux

O processo Windows -> Ubuntu é apenas um caso específico do processo Windows -> Linux. A arquitetura geral é a mesma para outras distribuições:

```text
Backup -> ISO oficial -> verificação -> USB bootável -> boot pelo firmware -> instalador -> partições -> pós-instalação
```

O que muda:

- site oficial de download;
- forma de verificar a ISO;
- ferramenta recomendada para criar USB;
- instalador;
- gerenciador de pacotes;
- política de versões;
- defaults de sistema de arquivos, drivers e Secure Boot.

### 13.1 Padrão universal de migração

1. Faça backup do Windows e salve chave BitLocker, se houver.
2. Escolha a distro com base no objetivo.
3. Baixe a ISO somente do site oficial.
4. Verifique checksum e assinatura quando a distro oferecer esse fluxo.
5. Copie a ISO para o Ventoy ou grave a ISO com a ferramenta oficial/recomendada da distro.
6. Dê boot pelo pendrive.
7. Teste o modo live, quando existir.
8. Escolha substituir Windows, instalar ao lado ou particionar manualmente.
9. Instale.
10. Atualize o sistema.
11. Instale drivers, codecs e ferramentas de desenvolvimento.
12. Restaure dados.

O Ventoy ajuda especialmente na etapa 5: você pode manter várias ISOs no mesmo pendrive e comparar instaladores antes de tocar no disco interno.

### 13.2 Diferenças por distro

| Distro | Perfil | Instalação e mídia | Gerenciador | Observações na migração |
|---|---|---|---|---|
| Ubuntu | Equilíbrio entre facilidade, comunidade e suporte LTS | ISO oficial; Desktop com instalador gráfico; Server com instalador em modo texto | `apt` | Boa primeira escolha para estudo, desenvolvimento e servidores simples |
| Fedora | Software recente, forte integração com GNOME e ecossistema Red Hat | Fedora Media Writer é recomendado oficialmente; imagens live permitem testar antes | `dnf` | Bom para aprender tecnologias próximas de Red Hat/RHEL; ciclos mais rápidos |
| Debian | Estabilidade, controle e base técnica de muitas distros | Debian Installer; imagens netinst/live; documentação detalhada por arquitetura | `apt` | Excelente para entender Linux com menos camadas de customização; pode exigir atenção a firmware conforme hardware |
| Linux Mint | Desktop amigável para quem vem do Windows | Guia oficial recomenda USB bootável; ISO pode iniciar em EFI ou BIOS | `apt` | Boa transição para desktop, especialmente pela familiaridade da interface Cinnamon |
| openSUSE Leap/Tumbleweed | Forte ferramenta de administração YaST; Leap mais conservador, Tumbleweed rolling release | Instalação com YaST; documentação oficial menciona USB/DVD e suporte a Secure Boot em UEFI | `zypper` | Boa para estudar administração de sistema e snapshots quando configurados |
| Arch Linux | Sistema minimalista e manual | ISO oficial para novas instalações; instalação guiada pela ArchWiki | `pacman` | Melhor para aprender profundamente particionamento, bootloader, pacotes e montagem do sistema |

### 13.3 Escolha por objetivo de engenharia de software

| Objetivo | Distros candidatas |
|---|---|
| Primeira instalação Linux sem muita fricção | Ubuntu Desktop, Linux Mint, Fedora Workstation |
| Servidor de laboratório | Ubuntu Server, Debian, Fedora Server, openSUSE Leap |
| Aprender fundamentos de Linux a fundo | Debian, Arch |
| Ambiente próximo de Red Hat/RHEL | Fedora |
| Desktop estável e familiar | Linux Mint, Ubuntu LTS |
| Rolling release para pacotes recentes | Arch, openSUSE Tumbleweed |

### 13.4 Cuidados ao sair do Windows para qualquer distro

Esses pontos independem da distro:

- Faça backup antes de particionar.
- Salve a chave BitLocker.
- Desative inicialização rápida/hibernação do Windows antes de dual boot.
- Reduza a partição do Windows pelo Gerenciamento de Disco do próprio Windows quando for instalar ao lado.
- Deixe o espaço para Linux como **não alocado**.
- Inicialize o pendrive no mesmo modo do Windows instalado, de preferência UEFI.
- Evite misturar Windows em UEFI com Linux em Legacy/CSM.
- Verifique se RAID/RST precisa ser alterado para AHCI antes da instalação.
- Teste Wi-Fi, vídeo, touchpad e suspensão no live USB quando possível.
- Leia a tela de particionamento com calma: "erase disk", "use entire disk" e equivalentes apagam o sistema existente.

### 13.5 Exemplos de atualização pós-instalação

Depois de instalar, cada família tem comandos próprios:

```bash
# Ubuntu, Debian, Linux Mint
sudo apt update
sudo apt upgrade
```

```bash
# Fedora
sudo dnf upgrade
```

```bash
# openSUSE
sudo zypper refresh
sudo zypper update
```

```bash
# Arch
sudo pacman -Syu
```

Esses comandos não substituem a leitura da documentação da distro, mas ajudam a fixar a diferença central: Linux não é uma coisa única; é uma família de sistemas que compartilham kernel e conceitos, mas variam em empacotamento, instalador, ciclo de release e escolhas de configuração.

---

## 14. Método antigo: gravação direta com `dd`

Antes de ferramentas como Rufus, balenaEtcher e Ventoy, era comum gravar uma ISO diretamente no dispositivo.

Exemplo em Linux:

```bash
sudo dd if=ubuntu-26.04-desktop-amd64.iso of=/dev/sdX bs=4M status=progress conv=fsync
```

Significado:

| Parte | Explicação |
|---|---|
| `if=` | arquivo de entrada, a ISO |
| `of=` | destino, o dispositivo inteiro do pendrive |
| `bs=4M` | tamanho dos blocos copiados |
| `status=progress` | mostra progresso |
| `conv=fsync` | força sincronização da escrita antes de encerrar |

Risco principal: escolher o disco errado em `of=` destrói dados. `dd` não pergunta se você tem certeza de que `/dev/sdX` é mesmo o pendrive.

Depois de usar `dd`, o pendrive fica dedicado àquela imagem. Para trocar a ISO, você regrava tudo.

---

## 15. Método tradicional no Windows: Rufus ou Media Creation Tool

Para Ubuntu, o tutorial oficial do Ubuntu Desktop usa balenaEtcher como exemplo de gravador de imagem, porque roda em Windows, Linux e macOS. Rufus também é comum no Windows, embora não seja a ferramenta usada nesse tutorial específico.

Para Windows, a rota oficial da Microsoft é o Media Creation Tool ou a ISO oficial do site de download.

Comparação rápida:

| Ferramenta | Melhor uso |
|---|---|
| Ventoy | Laboratório multi-ISO, várias imagens no mesmo pendrive |
| balenaEtcher | Gravar uma ISO simples seguindo o tutorial Ubuntu |
| Rufus | Gravar uma ISO específica com muitas opções no Windows |
| Media Creation Tool | Criar mídia oficial do Windows |
| `dd` | Gravação bruta em Linux/macOS, com alto risco se escolher disco errado |

---

## 16. Problemas comuns

### 16.1 O pendrive não aparece no boot

Verifique:

- o pendrive foi instalado com Ventoy corretamente;
- a ISO está copiada na partição de dados;
- você abriu o menu de boot correto;
- testou outra porta USB;
- o modo UEFI/Legacy está compatível;
- Secure Boot não está bloqueando o Ventoy;
- a máquina realmente suporta boot por USB.

### 16.2 O Ventoy aparece, mas a ISO não

Verifique:

- extensão do arquivo;
- se a ISO está corrompida;
- se o arquivo terminou de copiar;
- se o nome do arquivo não contém caracteres estranhos;
- se a ISO está em uma partição suportada pelo Ventoy.

### 16.3 O instalador não vê o disco interno

Causas comuns:

- controladora em modo RAID/RST;
- driver/controlador de armazenamento incomum;
- disco com falha;
- firmware antigo;
- modo de boot inconsistente.

Em notebook com Windows pré-instalado, alterar modo RAID/RST para AHCI pode impedir o Windows existente de iniciar. Faça isso somente depois de backup e entendendo o impacto.

### 16.4 O BitLocker pediu chave

Isso pode acontecer após mudanças percebidas como alteração de ambiente de boot. Use a chave de recuperação salva antes da migração.

### 16.5 Instalei Linux e quero voltar ao Windows

Use a mídia oficial da Microsoft, instale a mesma edição licenciada quando depender de licença digital e restaure os dados do backup. O retorno também é uma reinstalação, não uma reversão automática.

---

## 17. Checklist completo: Windows -> Ubuntu ou outra distro Linux

```text
[ ] Decidi qual distro atende ao objetivo: Ubuntu, Fedora, Debian, Mint, openSUSE, Arch etc.
[ ] Li a documentação oficial da distro escolhida.
[ ] Fiz backup de arquivos, projetos, chaves e bancos locais.
[ ] Salvei chave BitLocker, se houver.
[ ] Anotei edição e ativação do Windows.
[ ] Baixei a ISO oficial da distro.
[ ] Verifiquei SHA256/GPG ou, no mínimo, SHA256.
[ ] Instalei Ventoy no pendrive correto.
[ ] Copiei a ISO para o pendrive Ventoy.
[ ] Dei boot pelo pendrive.
[ ] Testei hardware no modo live, quando a distro oferecer essa opção.
[ ] Escolhi conscientemente "apagar disco" ou "instalar ao lado".
[ ] Concluí instalação.
[ ] Atualizei o sistema pelo gerenciador de pacotes correto.
[ ] Restaurei dados.
[ ] Validei rede, vídeo, áudio, suspensão e ferramentas de desenvolvimento.
```

---

## 18. Checklist completo: Linux -> Windows

```text
[ ] Fiz backup de /home, projetos, chaves e bancos.
[ ] Registrei pacotes, serviços e containers importantes.
[ ] Conferi se o sistema atual inicializa em UEFI ou BIOS.
[ ] Confirmei licença/edição do Windows que será instalada.
[ ] Criei mídia oficial do Windows ou baixei ISO oficial para usar com Ventoy.
[ ] Dei boot pela mídia.
[ ] Apaguei/preparei partições conscientemente.
[ ] Instalei Windows.
[ ] Ativei Windows.
[ ] Rodei Windows Update.
[ ] Reinstalei ferramentas.
[ ] Restaurei dados.
```

---

## 19. Resumo de entendimento

Se você entender estes pontos, entendeu o processo:

1. O firmware decide de onde iniciar.
2. O pendrive precisa conter um bootloader reconhecível.
3. Uma ISO é uma imagem de instalação, não um programa comum.
4. Ventoy separa bootloader e arquivos ISO: instala uma vez, copia imagens depois.
5. Migrar de Windows para Linux normalmente significa reinstalar o sistema no disco.
6. O risco real está em dados, partições, criptografia e licença, não no ato de "dar boot".
7. Dual boot preserva dois sistemas, mas aumenta complexidade.
8. Ubuntu, Fedora, Debian, Mint, openSUSE e Arch compartilham a lógica geral, mas diferem em instalador, empacotamento e ciclo de release.
9. O caminho Linux -> Windows também é reinstalação com backup e mídia oficial.

---

## 20. Fontes oficiais

- Ubuntu Desktop download: <https://ubuntu.com/download/desktop>
- Ubuntu Server download: <https://ubuntu.com/download/server>
- Ciclo de releases do Ubuntu: <https://ubuntu.com/about/release-cycle>
- Instalação do Ubuntu Desktop: <https://ubuntu.com/tutorials/install-ubuntu-desktop>
- Instalação do Ubuntu Server: <https://ubuntu.com/tutorials/install-ubuntu-server>
- Verificação da ISO Ubuntu: <https://ubuntu.com/tutorials/how-to-verify-ubuntu>
- Ventoy - página principal: <https://www.ventoy.net/>
- Ventoy - primeiros passos: <https://www.ventoy.net/en/doc_start.html>
- Ventoy - Secure Boot: <https://www.ventoy.net/en/doc_secure.html>
- Microsoft - Backup do Windows: <https://support.microsoft.com/en-us/windows/experience/backup-recovery/back-up-and-restore-with-windows-backup>
- Microsoft - BitLocker recovery key: <https://support.microsoft.com/en-us/windows/security/encryption/find-your-bitlocker-recovery-key>
- Microsoft - criar mídia de instalação do Windows: <https://support.microsoft.com/en-us/windows/deployment/install-upgrade/create-installation-media-for-windows>
- Microsoft - reinstalar Windows com mídia de instalação: <https://support.microsoft.com/en-us/windows/deployment/install-upgrade/reinstall-windows-with-the-installation-media>
- Microsoft - ativação do Windows: <https://support.microsoft.com/en-us/windows/activation/activate-windows>
- Microsoft - Gerenciamento de Disco: <https://support.microsoft.com/en-us/windows/experience/storage-filemanagement/disk-management-in-windows>
- Fedora Workstation download e Fedora Media Writer: <https://fedoraproject.org/workstation/download/>
- Fedora - preparação de mídia de boot: <https://docs.fedoraproject.org/en-US/fedora/latest/preparing-boot-media/>
- Debian GNU/Linux Installation Guide: <https://www.debian.org/releases/stable/amd64/>
- Linux Mint Installation Guide: <https://linuxmint-installation-guide.readthedocs.io/en/latest/>
- Linux Mint - criar mídia bootável: <https://linuxmint-installation-guide.readthedocs.io/en/latest/burn.html>
- Linux Mint - verificar ISO: <https://linuxmint-installation-guide.readthedocs.io/en/latest/verify.html>
- openSUSE Leap Installation Quick Start: <https://doc.opensuse.org/documentation/leap/startup/html/book-startup/art-opensuse-installquick.html>
- Arch Linux Downloads: <https://archlinux.org/download/>
- Arch Linux Installation Guide: <https://wiki.archlinux.org/title/Installation_guide>
