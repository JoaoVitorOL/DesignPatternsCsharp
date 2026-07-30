# 🐧 Guia Técnico: Ubuntu Server do Zero ao Avançado

> **Nível:** Zero ao Avançado  
> **Sistema operacional:** Ubuntu Server  
> **Fontes de referência principais:** [Ubuntu Server documentation](https://ubuntu.com/server/docs/), [Ubuntu Server download](https://ubuntu.com/download/server), [Ubuntu release cycle](https://ubuntu.com/about/release-cycle) e [Ubuntu installation documentation](https://canonical-subiquity.readthedocs-hosted.com/en/latest/)  
> **Versão de referência:** Ubuntu Server 26.04 LTS, com observações compatíveis com Ubuntu Server 24.04 LTS quando relevante  
> **Atualizado em:** 30/07/2026

---

## Prefácio

[⬆️ Voltar ao Sumário](#sumário)

Ubuntu Server costuma ser aprendido de duas formas incompletas: como uma lista de comandos soltos ou como "Ubuntu sem interface gráfica". As duas ideias ajudam no primeiro contato, mas escondem o que realmente importa para operar um servidor: boot, rede, usuários, pacotes, serviços, logs, armazenamento, segurança, automação, backup, observabilidade e recuperação.

Este guia trata Ubuntu Server como uma plataforma de operação. O kernel Linux fornece as bases; o Ubuntu organiza distribuição, pacotes, suporte e ciclo de release; o `systemd` gerencia serviços e boot; o APT instala e atualiza software; o Netplan descreve rede; OpenSSH viabiliza acesso remoto; UFW, AppArmor e atualizações reduzem risco; cloud-init, autoinstall e ferramentas de infraestrutura tornam servidores reproduzíveis.

O objetivo não é decorar comandos. O objetivo é formar um modelo mental que permita responder, com segurança:

- o que este servidor está executando?
- quem consegue acessá-lo?
- quais portas estão expostas?
- como ele recebe atualizações?
- onde os dados estão?
- como ele volta ao ar depois de falha?
- o que muda quando ele está em bare metal, VM, cloud ou container?

Ubuntu Server é específico. Ele compartilha muito com Debian e com outras distribuições Linux, mas possui escolhas próprias de ciclo LTS, repositórios, instalador, suporte, ferramentas oficiais, defaults de rede e integração com o ecossistema Canonical. Este arquivo foca nele.

---

## Como usar este guia

[⬆️ Voltar ao Sumário](#sumário)

Há três trilhas possíveis:

1. **Trilha iniciante:** leia as Partes 1 a 12, instale uma VM descartável e pratique usuários, SSH, APT, serviços, logs e firewall.
2. **Trilha profissional:** avance pelas Partes 13 a 27, configurando serviços reais, backups, atualizações automáticas, hardening, observabilidade, containers e automação.
3. **Trilha de consulta:** use as Partes 28 e 29, anexos e glossário para lembrar comandos, camadas, arquivos e fontes oficiais.

Ao estudar qualquer recurso, responda sempre:

1. Isso pertence ao kernel, ao Ubuntu, ao `systemd`, ao pacote instalado, à cloud ou à aplicação?
2. O estado desejado está documentado em arquivo, comando, serviço, policy ou infraestrutura externa?
3. Qual risco aparece se o servidor reiniciar agora?
4. Como validar antes de aplicar uma mudança remota?
5. Como desfazer ou recuperar se a mudança falhar?
6. O que precisa estar em backup para reconstruir esta máquina?

> **Regra de laboratório:** pratique em VM antes de repetir em produção. Comandos sobre disco, firewall, SSH e upgrade de release podem derrubar acesso ou apagar dados.

---

<a id="sumário"></a>

## Sumário Geral

### Como o conteúdo está organizado

| Bloco | Partes | Assuntos centrais | Resultado esperado | Comece por |
|---|---:|---|---|---|
| 1. Base, release e instalação | 1-4 | Ubuntu Server, LTS, imagem ISO, Subiquity, cloud images e boot | entender o que está sendo instalado e qual estado inicial nasce no servidor | [Parte 1](#parte-1--introdução-e-contextualização) |
| 2. Sistema base e administração local | 5-8 | filesystem, usuários, permissões, processos, `systemd`, logs e shell | administrar o servidor sem depender de interface gráfica | [Parte 5](#parte-5--filesystem-e-hierarquia-do-sistema) |
| 3. Rede e acesso remoto | 9-12 | Netplan, IP, DNS, tempo, SSH, UFW e portas | acessar e proteger o servidor pela rede sem perder controle remoto | [Parte 9](#parte-9--rede-com-netplan-e-systemd-networkd) |
| 4. Software, serviços e aplicações | 13-16 | APT, snaps, repositórios, serviços web, bancos e filas | instalar, atualizar e operar software de servidor com fronteiras claras | [Parte 13](#parte-13--pacotes-repositórios-e-atualizações) |
| 5. Armazenamento e dados | 17-19 | discos, partições, LVM, RAID, mounts, backups e restore | tratar dados como estado crítico e recuperável | [Parte 17](#parte-17--discos-partições-lvm-e-raid) |
| 6. Segurança e produção | 20-23 | hardening, AppArmor, segredos, auditoria, upgrades e runbooks | reduzir exposição e manter o servidor evoluindo com risco controlado | [Parte 20](#parte-20--hardening-e-superfície-de-ataque) |
| 7. Automação, cloud e plataforma | 24-27 | cloud-init, autoinstall, containers, virtualização e observabilidade | criar servidores reproduzíveis e operáveis em escala | [Parte 24](#parte-24--cloud-init-autoinstall-e-servidores-reproduzíveis) |
| 8. Catálogo e ecossistema | 28-29 | comandos, arquivos, serviços, ferramentas e papéis de servidor | descobrir o que já existe antes de improvisar scripts frágeis | [Parte 28](#parte-28--catálogo-prático-do-ubuntu-server) |
| 9. Revisão | Anexos | trilhas oficiais, referências e glossário | aprofundar pela documentação oficial e revisar termos | [Anexo A](#anexo-a--trilhas-oficiais-de-estudo-e-prática) |

### Atalhos por pergunta prática

| Se você quer saber... | Consulte primeiro |
|---|---|
| qual versão instalar e por que LTS importa | [Partes 1](#parte-1--introdução-e-contextualização) e [2](#parte-2--releases-lts-suporte-e-ciclo-de-vida) |
| como instalar Ubuntu Server em VM ou máquina física | [Partes 3](#parte-3--instalação-com-subiquity) e [4](#parte-4--boot-firmware-iso-e-imagens-cloud) |
| como navegar no sistema e entender diretórios | [Parte 5](#parte-5--filesystem-e-hierarquia-do-sistema) |
| como criar usuários, usar `sudo` e proteger permissões | [Parte 6](#parte-6--usuários-grupos-permissões-e-sudo) |
| como controlar serviços | [Parte 7](#parte-7--processos-systemd-e-unidades) |
| como investigar logs e falhas | [Parte 8](#parte-8--logs-journalctl-e-diagnóstico) |
| como configurar IP fixo e DNS | [Partes 9](#parte-9--rede-com-netplan-e-systemd-networkd) e [10](#parte-10--dns-tempo-e-conectividade) |
| como habilitar SSH sem se trancar fora | [Parte 11](#parte-11--openssh-e-acesso-remoto) |
| como liberar portas com firewall | [Parte 12](#parte-12--firewall-com-ufw-e-camadas-de-rede) |
| como instalar, atualizar e remover software | [Parte 13](#parte-13--pacotes-repositórios-e-atualizações) |
| como publicar uma aplicação ou serviço | [Partes 14](#parte-14--serviços-de-aplicação-com-systemd), [15](#parte-15--web-reverse-proxy-e-tls) e [16](#parte-16--bancos-filas-e-serviços-de-estado) |
| como planejar disco, LVM e mounts | [Parte 17](#parte-17--discos-partições-lvm-e-raid) |
| como fazer backup e testar restore | [Parte 18](#parte-18--backup-restore-e-recuperação) |
| como endurecer o servidor | [Partes 20](#parte-20--hardening-e-superfície-de-ataque), [21](#parte-21--atualizações-segurança-e-ubuntu-pro) e [22](#parte-22--apparmor-segredos-e-auditoria) |
| como automatizar instalações | [Parte 24](#parte-24--cloud-init-autoinstall-e-servidores-reproduzíveis) |
| como usar containers ou VMs no Ubuntu Server | [Partes 25](#parte-25--containers-lxd-docker-e-imagens-oci) e [26](#parte-26--virtualização-com-kvm-qemu-e-libvirt) |
| quais comandos e arquivos lembrar | [Parte 28](#parte-28--catálogo-prático-do-ubuntu-server) |

### Índice detalhado

**Bloco 1 — Base, release e instalação**

- **[Parte 1 — Introdução e Contextualização](#parte-1--introdução-e-contextualização)**
  - [1.1 O que é Ubuntu Server?](#11-o-que-é-ubuntu-server)
  - [1.2 Ubuntu Server não é Ubuntu Desktop sem mouse](#12-ubuntu-server-não-é-ubuntu-desktop-sem-mouse)
  - [1.3 Camadas: kernel, distribuição, pacote, serviço e aplicação](#13-camadas-kernel-distribuição-pacote-serviço-e-aplicação)
  - [1.4 Onde Ubuntu Server é usado](#14-onde-ubuntu-server-é-usado)
- **[Parte 2 — Releases, LTS, Suporte e Ciclo de Vida](#parte-2--releases-lts-suporte-e-ciclo-de-vida)**
  - [2.1 LTS e interim releases](#21-lts-e-interim-releases)
  - [2.2 Ubuntu Pro, ESM e suporte estendido](#22-ubuntu-pro-esm-e-suporte-estendido)
  - [2.3 Como escolher versão em produção](#23-como-escolher-versão-em-produção)
- **[Parte 3 — Instalação com Subiquity](#parte-3--instalação-com-subiquity)**
  - [3.1 O instalador do Ubuntu Server](#31-o-instalador-do-ubuntu-server)
  - [3.2 Decisões durante a instalação](#32-decisões-durante-a-instalação)
  - [3.3 Checklist antes de instalar](#33-checklist-antes-de-instalar)
- **[Parte 4 — Boot, Firmware, ISO e Imagens Cloud](#parte-4--boot-firmware-iso-e-imagens-cloud)**
  - [4.1 Boot em bare metal e VM](#41-boot-em-bare-metal-e-vm)
  - [4.2 ISO de instalação versus cloud image](#42-iso-de-instalação-versus-cloud-image)
  - [4.3 Verificação de integridade](#43-verificação-de-integridade)

**Bloco 2 — Sistema base e administração local**

- **[Parte 5 — Filesystem e Hierarquia do Sistema](#parte-5--filesystem-e-hierarquia-do-sistema)**
- **[Parte 6 — Usuários, Grupos, Permissões e sudo](#parte-6--usuários-grupos-permissões-e-sudo)**
- **[Parte 7 — Processos, systemd e Unidades](#parte-7--processos-systemd-e-unidades)**
- **[Parte 8 — Logs, journalctl e Diagnóstico](#parte-8--logs-journalctl-e-diagnóstico)**

**Bloco 3 — Rede e acesso remoto**

- **[Parte 9 — Rede com Netplan e systemd-networkd](#parte-9--rede-com-netplan-e-systemd-networkd)**
- **[Parte 10 — DNS, Tempo e Conectividade](#parte-10--dns-tempo-e-conectividade)**
- **[Parte 11 — OpenSSH e Acesso Remoto](#parte-11--openssh-e-acesso-remoto)**
- **[Parte 12 — Firewall com UFW e Camadas de Rede](#parte-12--firewall-com-ufw-e-camadas-de-rede)**

**Bloco 4 — Software, serviços e aplicações**

- **[Parte 13 — Pacotes, Repositórios e Atualizações](#parte-13--pacotes-repositórios-e-atualizações)**
- **[Parte 14 — Serviços de Aplicação com systemd](#parte-14--serviços-de-aplicação-com-systemd)**
- **[Parte 15 — Web, Reverse Proxy e TLS](#parte-15--web-reverse-proxy-e-tls)**
- **[Parte 16 — Bancos, Filas e Serviços de Estado](#parte-16--bancos-filas-e-serviços-de-estado)**

**Bloco 5 — Armazenamento e dados**

- **[Parte 17 — Discos, Partições, LVM e RAID](#parte-17--discos-partições-lvm-e-raid)**
- **[Parte 18 — Backup, Restore e Recuperação](#parte-18--backup-restore-e-recuperação)**
- **[Parte 19 — Sistemas de Arquivos, Mounts e Quotas](#parte-19--sistemas-de-arquivos-mounts-e-quotas)**

**Bloco 6 — Segurança e produção**

- **[Parte 20 — Hardening e Superfície de Ataque](#parte-20--hardening-e-superfície-de-ataque)**
- **[Parte 21 — Atualizações, Segurança e Ubuntu Pro](#parte-21--atualizações-segurança-e-ubuntu-pro)**
- **[Parte 22 — AppArmor, Segredos e Auditoria](#parte-22--apparmor-segredos-e-auditoria)**
- **[Parte 23 — Upgrades, Mudanças e Runbooks](#parte-23--upgrades-mudanças-e-runbooks)**

**Bloco 7 — Automação, cloud e plataforma**

- **[Parte 24 — cloud-init, autoinstall e Servidores Reproduzíveis](#parte-24--cloud-init-autoinstall-e-servidores-reproduzíveis)**
- **[Parte 25 — Containers, LXD, Docker e Imagens OCI](#parte-25--containers-lxd-docker-e-imagens-oci)**
- **[Parte 26 — Virtualização com KVM, QEMU e libvirt](#parte-26--virtualização-com-kvm-qemu-e-libvirt)**
- **[Parte 27 — Observabilidade, Performance e Capacidade](#parte-27--observabilidade-performance-e-capacidade)**

**Bloco 8 — Catálogo e ecossistema**

- **[Parte 28 — Catálogo Prático do Ubuntu Server](#parte-28--catálogo-prático-do-ubuntu-server)**
- **[Parte 29 — Ecossistema, Papéis de Servidor e Ferramentas Externas](#parte-29--ecossistema-papéis-de-servidor-e-ferramentas-externas)**

**Revisão**

- **[Anexo A — Trilhas Oficiais de Estudo e Prática](#anexo-a--trilhas-oficiais-de-estudo-e-prática)**
- **[Anexo B — Referências Oficiais Consultadas](#anexo-b--referências-oficiais-consultadas)**
- **[Glossário](#glossário)**

---

## Parte 1 — Introdução e Contextualização

[⬆️ Voltar ao Sumário](#sumário)

### 1.1 O que é Ubuntu Server?

Ubuntu Server é a edição do Ubuntu voltada para execução de serviços. Ela não nasce para ser uma estação gráfica de trabalho, e sim para hospedar processos que precisam ficar disponíveis: APIs, bancos de dados, filas, servidores web, DNS, VPN, arquivos, virtualização, containers, automação, observabilidade e infraestrutura.

A diferença importante é de **perfil operacional**:

| Aspecto | Ubuntu Server |
|---|---|
| Interface padrão | Terminal e instalador textual |
| Foco | Serviços, rede, estabilidade e automação |
| Acesso comum | SSH, console remoto, serial, cloud console ou hypervisor |
| Estado crítico | Configuração, dados, chaves, serviços e logs |
| Atualização | Planejada por janelas, automação e política de segurança |

Um servidor não é definido apenas pelo hardware. Uma VM pequena em laboratório pode ser "servidor" se sua função é oferecer um serviço. Um equipamento físico potente pode ser mal operado se for tratado como um desktop sempre ligado.

### 1.2 Ubuntu Server não é Ubuntu Desktop sem mouse

Ubuntu Desktop e Ubuntu Server compartilham base, pacotes, kernel e repositórios. Mas a edição Server faz escolhas diferentes:

| Tema | Ubuntu Desktop | Ubuntu Server |
|---|---|---|
| Experiência inicial | interface gráfica, usuário local, aplicativos de uso diário | instalação textual, rede, storage e usuário administrativo |
| Rede | NetworkManager normalmente aparece com destaque | Netplan normalmente descreve rede para backend como `systemd-networkd` |
| Uso esperado | interação humana frequente | serviço contínuo, acesso remoto e automação |
| Risco típico | quebrar ambiente de uso pessoal | derrubar serviço, perder acesso remoto ou dados |

Instalar GNOME em Ubuntu Server é possível, mas muda o perfil da máquina. Para estudar serviços, é melhor manter o servidor simples e acessar por SSH.

### 1.3 Camadas: kernel, distribuição, pacote, serviço e aplicação

Uma confusão comum é culpar "o Ubuntu" por qualquer comportamento. Em administração real, é preciso localizar a camada.

| Camada | Exemplo | Pergunta útil |
|---|---|---|
| Hardware/firmware | UEFI, disco, NIC, RAID controller | o equipamento entrega o recurso ao sistema? |
| Kernel Linux | drivers, rede, processos, memória, filesystem | o kernel reconheceu e expôs o recurso? |
| Distribuição Ubuntu | release, repositórios, defaults, suporte | qual versão, pacote e política estão em uso? |
| Gerenciador de serviços | `systemd`, timers, units | quem inicia, reinicia e monitora o processo? |
| Pacote | `openssh-server`, `nginx`, `postgresql` | quais arquivos e serviços o pacote instalou? |
| Aplicação | API, banco, job, worker | qual contrato de negócio está rodando aqui? |
| Infra externa | cloud firewall, load balancer, DNS, storage | o tráfego e os dados passam por outra camada? |

Essa separação evita diagnóstico supersticioso. Se a API não responde, pode ser código, porta fechada, DNS errado, serviço parado, firewall da cloud, certificado vencido, disco cheio ou rota quebrada.

### 1.4 Onde Ubuntu Server é usado

Ubuntu Server aparece em contextos muito diferentes:

- laboratório local com VirtualBox, Hyper-V, VMware, Proxmox, KVM ou Multipass;
- cloud pública, como AWS, Azure, Google Cloud, Oracle Cloud e provedores regionais;
- servidores físicos em empresa;
- clusters Kubernetes;
- hosts de containers;
- appliances de rede, VPN, proxy e storage;
- ambientes de CI/CD;
- servidores de banco de dados e aplicações.

O ponto comum é o mesmo: Ubuntu Server precisa ser **instalável, atualizável, monitorável, recuperável e reproduzível**.

---

## Parte 2 — Releases, LTS, Suporte e Ciclo de Vida

[⬆️ Voltar ao Sumário](#sumário)

### 2.1 LTS e interim releases

Ubuntu tem dois ritmos principais:

| Tipo | Frequência | Suporte típico | Uso recomendado |
|---|---:|---:|---|
| LTS | a cada 2 anos | 5 anos de manutenção padrão de segurança | produção, servidores, projetos de longo prazo |
| Interim | a cada 6 meses | cerca de 9 meses | teste de recursos recentes, hardware novo, laboratório |

Na data deste guia, a página oficial de download apresenta **Ubuntu Server 26.04 LTS** como a versão LTS atual. Para produção, a escolha conservadora normalmente é LTS.

### 2.2 Ubuntu Pro, ESM e suporte estendido

O suporte padrão da LTS cobre um período inicial longo, mas nem todos os pacotes e cenários têm o mesmo nível de cobertura. Ubuntu Pro acrescenta recursos como Expanded Security Maintenance, Livepatch, hardening e suporte comercial conforme assinatura.

Modelo mental:

```text
Ubuntu LTS
  base estável para produção

Ubuntu Pro
  cobertura e recursos adicionais para segurança, compliance e suporte

Aplicação
  ainda precisa de ciclo próprio de patch, teste e deploy
```

Ubuntu Pro não substitui boas práticas de administração. Ele amplia cobertura, mas você continua precisando aplicar patches, testar mudanças, controlar acesso, monitorar e restaurar backups.

### 2.3 Como escolher versão em produção

Critérios práticos:

| Critério | Pergunta |
|---|---|
| Suporte | a versão estará suportada durante a vida do sistema? |
| Compatibilidade | pacotes necessários existem na versão escolhida? |
| Drivers | hardware, cloud image ou hypervisor são suportados? |
| Janela de upgrade | existe plano para próxima LTS? |
| Segurança | atualizações automáticas e ESM fazem sentido? |
| Equipe | a equipe conhece os defaults desta versão? |

Evite instalar uma interim release em produção apenas porque "é a mais nova". Em servidor, novidade sem plano de atualização pode virar dívida operacional em poucos meses.

---

## Parte 3 — Instalação com Subiquity

[⬆️ Voltar ao Sumário](#sumário)

### 3.1 O instalador do Ubuntu Server

Ubuntu Server usa o instalador Subiquity nas ISOs modernas. Ele conduz:

- idioma e teclado;
- rede;
- proxy e mirror;
- storage;
- usuário inicial;
- instalação de OpenSSH;
- snaps ou perfis opcionais;
- finalização e reinício.

O instalador não é apenas "um wizard". Ele materializa decisões de infraestrutura. Escolher LVM, apagar disco, configurar IP ou instalar SSH muda como o servidor será acessado e recuperado depois.

### 3.2 Decisões durante a instalação

| Decisão | Impacto |
|---|---|
| hostname | aparece em logs, prompt, DNS interno e inventário |
| usuário inicial | primeiro ponto administrativo com `sudo` |
| OpenSSH | habilita administração remota |
| senha versus chave SSH | define superfície de ataque inicial |
| DHCP versus IP estático | afeta previsibilidade de acesso |
| LVM | facilita expansão e snapshots, mas adiciona camada |
| disco inteiro | apaga estado anterior |
| mirror/proxy | afeta velocidade, compliance e atualização |

### 3.3 Checklist antes de instalar

```text
[ ] Sei se o alvo é VM, bare metal ou cloud.
[ ] Baixei a ISO oficial do Ubuntu Server.
[ ] Verifiquei a integridade do arquivo quando possível.
[ ] Sei se a máquina inicia por UEFI ou BIOS legado.
[ ] Fiz backup de qualquer disco que será reutilizado.
[ ] Defini hostname.
[ ] Tenho chave SSH pública para o usuário inicial.
[ ] Defini se a rede será DHCP ou IP fixo.
[ ] Sei qual disco será apagado ou particionado.
[ ] Tenho acesso ao console caso o SSH falhe.
```

---

## Parte 4 — Boot, Firmware, ISO e Imagens Cloud

[⬆️ Voltar ao Sumário](#sumário)

### 4.1 Boot em bare metal e VM

O boot simplificado é:

```text
Firmware ou hypervisor
  -> bootloader
  -> kernel Linux
  -> initramfs
  -> systemd
  -> targets e serviços
```

Em bare metal, UEFI, Secure Boot, controladora de disco e ordem de boot importam. Em VM, o hypervisor fornece firmware virtual, disco virtual, placa de rede virtual e console.

Comandos úteis após instalar:

```bash
hostnamectl
timedatectl
lsblk -f
findmnt /
systemctl --failed
journalctl -b -p warning
```

### 4.2 ISO de instalação versus cloud image

| Imagem | Uso |
|---|---|
| ISO Server | instalação interativa ou automatizada em bare metal/VM |
| Cloud image | imagem pronta para clouds e VMs com cloud-init |
| Container image | base mínima para processo containerizado, não servidor completo |

Uma cloud image não é "a ISO mais rápida". Ela presume um ambiente que fornece metadados, rede, chave SSH e configuração inicial via cloud-init.

### 4.3 Verificação de integridade

No Windows PowerShell:

```powershell
Get-FileHash .\ubuntu-26.04-live-server-amd64.iso -Algorithm SHA256
```

No Linux:

```bash
sha256sum ubuntu-26.04-live-server-amd64.iso
```

Compare com os hashes oficiais da versão baixada. Se o hash não bater, descarte o arquivo.

---

## Parte 5 — Filesystem e Hierarquia do Sistema

[⬆️ Voltar ao Sumário](#sumário)

Ubuntu Server segue a organização tradicional de sistemas Linux. O importante é entender o papel de cada região.

| Caminho | Função |
|---|---|
| `/` | raiz do sistema |
| `/bin`, `/usr/bin` | comandos executáveis |
| `/sbin`, `/usr/sbin` | comandos administrativos |
| `/etc` | configuração do sistema e serviços |
| `/home` | diretórios de usuários humanos |
| `/root` | home do usuário `root` |
| `/var` | dados variáveis: logs, spool, cache, bancos conforme pacote |
| `/var/log` | logs persistentes de vários serviços |
| `/tmp` | arquivos temporários |
| `/opt` | software externo instalado fora do empacotamento padrão |
| `/srv` | dados servidos pelo sistema, quando adotado pela equipe |
| `/run` | estado volátil de runtime |
| `/proc` | visão virtual de processos e kernel |
| `/sys` | visão virtual de dispositivos e kernel |
| `/dev` | dispositivos expostos como arquivos |
| `/mnt`, `/media` | montagem temporária ou removível |

Comandos de leitura:

```bash
pwd
ls -la
tree -L 2 /etc 2>/dev/null
df -h
du -sh /var/log
findmnt
```

Regra mental: quase tudo que precisa sobreviver a reboot está em disco; quase tudo que descreve comportamento do sistema está em `/etc`; quase tudo que explica o que aconteceu está em logs e journal.

---

## Parte 6 — Usuários, Grupos, Permissões e sudo

[⬆️ Voltar ao Sumário](#sumário)

Linux é multiusuário. Mesmo em um servidor pequeno, processos rodam com identidades diferentes para limitar danos.

### Usuários comuns e usuários de serviço

| Tipo | Exemplo | Uso |
|---|---|---|
| humano administrativo | `admin`, `joao` | acesso por SSH e `sudo` |
| humano sem administração | `deploy` | operação limitada |
| sistema/serviço | `www-data`, `postgres`, `syslog` | isolar processos |
| `root` | `root` | administração total |

Comandos:

```bash
id
whoami
getent passwd
getent group sudo
sudo -l
```

### Criar usuário administrativo

```bash
sudo adduser operador
sudo usermod -aG sudo operador
```

### Permissões

Modelo clássico:

```text
arquivo
  dono
  grupo
  outros

permissões
  r = leitura
  w = escrita
  x = execução ou travessia em diretório
```

Exemplos:

```bash
ls -l /etc/ssh/sshd_config
chmod 600 ~/.ssh/authorized_keys
chmod 700 ~/.ssh
chown -R app:app /srv/minha-api
```

### sudo

`sudo` permite executar comandos com privilégios elevados, normalmente registrando a ação. Edite regras com `visudo`, porque ele valida sintaxe antes de salvar:

```bash
sudo visudo
```

Evite login direto como `root` via SSH. Prefira usuário nominal, chave SSH e `sudo`.

---

## Parte 7 — Processos, systemd e Unidades

[⬆️ Voltar ao Sumário](#sumário)

Ubuntu Server usa `systemd` como sistema de init e gerenciador de serviços.

### Modelo mental

```text
systemd
  inicia o sistema
  organiza dependências
  inicia serviços
  executa timers
  coleta estado
  integra logs com journald
```

### Comandos essenciais

```bash
systemctl status ssh
systemctl start nginx
systemctl stop nginx
systemctl restart nginx
systemctl reload nginx
systemctl enable nginx
systemctl disable nginx
systemctl is-enabled nginx
systemctl --failed
```

### Unit file mínima

Exemplo para uma aplicação própria:

```ini
[Unit]
Description=Minha API
After=network-online.target
Wants=network-online.target

[Service]
Type=simple
User=app
Group=app
WorkingDirectory=/srv/minha-api
ExecStart=/usr/bin/dotnet /srv/minha-api/MinhaApi.dll
Restart=on-failure
RestartSec=5
Environment=ASPNETCORE_URLS=http://127.0.0.1:5000

[Install]
WantedBy=multi-user.target
```

Local comum:

```bash
/etc/systemd/system/minha-api.service
```

Após criar ou alterar:

```bash
sudo systemctl daemon-reload
sudo systemctl enable --now minha-api
systemctl status minha-api
journalctl -u minha-api -f
```

Regra prática: se uma aplicação precisa sobreviver a logout e reiniciar após falha, ela deve ser serviço, não processo solto em terminal.

---

## Parte 8 — Logs, journalctl e Diagnóstico

[⬆️ Voltar ao Sumário](#sumário)

Logs são parte do contrato operacional do servidor. Sem logs, uma falha vira adivinhação.

### Locais comuns

| Fonte | Consulta |
|---|---|
| journal do systemd | `journalctl` |
| logs tradicionais | `/var/log` |
| kernel | `dmesg`, `journalctl -k` |
| autenticação | `/var/log/auth.log` ou journal conforme versão/configuração |
| serviço específico | `/var/log/nginx`, `/var/log/postgresql`, journal da unit |

### Comandos úteis

```bash
journalctl -b
journalctl -b -p err
journalctl -u ssh
journalctl -u nginx --since "1 hour ago"
journalctl -f
journalctl -k
systemctl status nginx
```

### Diagnóstico por camadas

Quando algo falhar, pergunte:

1. O serviço existe?
2. Está ativo?
3. Escuta na porta esperada?
4. A aplicação logou erro?
5. O firewall local permite?
6. A rede externa permite?
7. DNS aponta para o host certo?
8. Certificado ou credencial expirou?
9. Disco, memória ou CPU estão saturados?

Comandos iniciais:

```bash
systemctl --failed
ss -tulpn
ip addr
ip route
df -h
free -h
top
```

---

## Parte 9 — Rede com Netplan e systemd-networkd

[⬆️ Voltar ao Sumário](#sumário)

Ubuntu Server usa Netplan para descrever configuração de rede em YAML. O Netplan gera configuração para um backend, como `systemd-networkd` ou NetworkManager.

### Ver estado atual

```bash
ip addr
ip route
resolvectl status
networkctl status
ls /etc/netplan
```

### DHCP simples

```yaml
network:
  version: 2
  renderer: networkd
  ethernets:
    enp1s0:
      dhcp4: true
```

### IP estático

```yaml
network:
  version: 2
  renderer: networkd
  ethernets:
    enp1s0:
      dhcp4: false
      addresses:
        - 192.0.2.10/24
      routes:
        - to: default
          via: 192.0.2.1
      nameservers:
        addresses:
          - 1.1.1.1
          - 8.8.8.8
```

Aplicação:

```bash
sudo netplan generate
sudo netplan try
sudo netplan apply
```

Em servidor remoto, prefira `netplan try`. Ele permite confirmar a configuração e reduz o risco de perder acesso por erro de IP, rota ou gateway.

---

## Parte 10 — DNS, Tempo e Conectividade

[⬆️ Voltar ao Sumário](#sumário)

Rede funcional não é só IP. Servidor precisa resolver nomes, manter hora correta e alcançar dependências.

### DNS

Comandos:

```bash
resolvectl status
resolvectl query ubuntu.com
getent hosts ubuntu.com
dig ubuntu.com
```

Arquivos e serviços comuns:

| Item | Papel |
|---|---|
| Netplan | declara nameservers |
| `systemd-resolved` | resolução local e cache conforme configuração |
| `/etc/resolv.conf` | ponto de compatibilidade para resolvedores |
| DNS externo/cloud | pode sobrescrever ou fornecer configuração via DHCP |

### Tempo

Hora errada quebra TLS, Kerberos, logs, backups, clusters e auditoria.

```bash
timedatectl
timedatectl list-timezones | grep Sao_Paulo
sudo timedatectl set-timezone America/Sao_Paulo
```

Verifique qual serviço de sincronização está em uso na sua versão e imagem:

```bash
systemctl status chrony --no-pager
systemctl status systemd-timesyncd --no-pager
```

### Conectividade

```bash
ping -c 4 1.1.1.1
ping -c 4 ubuntu.com
tracepath ubuntu.com
curl -I https://ubuntu.com
ss -tulpn
```

Se IP funciona e DNS falha, investigue resolvedor. Se DNS funciona e HTTP falha, investigue proxy, rota, firewall ou TLS.

---

## Parte 11 — OpenSSH e Acesso Remoto

[⬆️ Voltar ao Sumário](#sumário)

SSH é o principal canal de administração de um Ubuntu Server.

### Instalar servidor SSH

```bash
sudo apt update
sudo apt install openssh-server
systemctl status ssh
```

### Chaves

No cliente:

```bash
ssh-keygen -t ed25519 -C "admin@srv-lab"
ssh-copy-id usuario@servidor
```

No servidor, permissões esperadas:

```bash
chmod 700 ~/.ssh
chmod 600 ~/.ssh/authorized_keys
```

### Configuração segura

Prefira snippets em:

```bash
/etc/ssh/sshd_config.d/*.conf
```

Exemplo:

```text
PasswordAuthentication no
PermitRootLogin no
PubkeyAuthentication yes
```

Antes de reiniciar o SSH:

```bash
sudo sshd -t
sudo systemctl reload ssh
```

Regra de ouro: mantenha uma sessão SSH aberta enquanto testa outra. Se a nova sessão falhar, você ainda tem caminho de volta.

---

## Parte 12 — Firewall com UFW e Camadas de Rede

[⬆️ Voltar ao Sumário](#sumário)

UFW é a ferramenta padrão amigável do Ubuntu para firewall local.

### Fluxo mínimo seguro

```bash
sudo ufw status verbose
sudo ufw allow OpenSSH
sudo ufw enable
sudo ufw status numbered
```

Para servidor web:

```bash
sudo ufw allow 80/tcp
sudo ufw allow 443/tcp
```

Para remover regra:

```bash
sudo ufw status numbered
sudo ufw delete NUMERO
```

### Camadas de firewall

| Camada | Exemplo |
|---|---|
| aplicação | Nginx permitindo hosts específicos |
| firewall local | UFW/nftables no servidor |
| cloud firewall | security group, NSG, VCN rule |
| rede corporativa | ACL, roteador, VPN |
| load balancer | listeners e target groups |

Abrir UFW não garante acesso se a cloud bloqueia. Bloquear UFW não protege se um container publica porta por outra cadeia sem você entender o caminho. Audite com `ss -tulpn` e teste de fora.

---

## Parte 13 — Pacotes, Repositórios e Atualizações

[⬆️ Voltar ao Sumário](#sumário)

Ubuntu usa pacotes Debian (`.deb`) e APT como caminho principal de instalação e atualização do sistema.

### APT essencial

```bash
sudo apt update
apt list --upgradable
sudo apt upgrade
sudo apt install nginx
sudo apt remove nginx
apt search postgresql
apt show openssh-server
dpkg -l | grep nginx
```

### Diferença entre comandos

| Comando | Papel |
|---|---|
| `apt update` | atualiza índice de pacotes |
| `apt upgrade` | atualiza pacotes sem remover dependências importantes |
| `apt full-upgrade` | permite mudanças mais amplas de dependências |
| `apt install` | instala pacote |
| `apt remove` | remove pacote preservando configs |
| `apt purge` | remove pacote e configs do pacote |
| `apt autoremove` | remove dependências não usadas |
| `dpkg` | opera pacote local em nível mais baixo |

### Repositórios

Categorias comuns:

| Repositório | Ideia |
|---|---|
| Main | pacotes oficialmente mantidos pela Canonical |
| Restricted | software suportado com restrições de licença |
| Universe | software mantido pela comunidade |
| Multiverse | software com restrições legais/licença |

Repositório de terceiro deve ser exceção deliberada. Prefira formato `deb822` em `/etc/apt/sources.list.d/*.sources`, chave limitada ao repositório e documentação de remoção.

### Snaps

Snaps são outro formato de distribuição usado em Ubuntu. Podem ser úteis para ferramentas específicas, mas possuem ciclo, confinamento e atualização próprios.

```bash
snap list
sudo snap install lxd
sudo snap refresh
```

Regra prática: não misture instalação via APT, snap, script remoto e binário manual sem registrar ownership. O problema não é ter mais de um formato; é esquecer quem atualiza o quê.

---

## Parte 14 — Serviços de Aplicação com systemd

[⬆️ Voltar ao Sumário](#sumário)

Uma aplicação de servidor precisa ter dono operacional.

Checklist:

```text
[ ] Existe usuário de serviço sem login interativo desnecessário.
[ ] O binário/código está em caminho previsível.
[ ] Configuração não contém segredo exposto.
[ ] A unit systemd define WorkingDirectory, User, Restart e Environment.
[ ] Logs aparecem em journal ou arquivo conhecido.
[ ] Health check existe.
[ ] Deploy consegue reiniciar ou recarregar com segurança.
```

### Criar usuário de serviço

```bash
sudo adduser --system --group --home /srv/minha-api app
```

### Estrutura simples

```text
/srv/minha-api/
  app
  appsettings.json
  releases/
  current -> releases/2026-07-30/

/etc/systemd/system/minha-api.service
/etc/minha-api/minha-api.env
```

### Variáveis de ambiente

Em unit:

```ini
EnvironmentFile=/etc/minha-api/minha-api.env
```

Proteja o arquivo:

```bash
sudo chown root:app /etc/minha-api/minha-api.env
sudo chmod 640 /etc/minha-api/minha-api.env
```

Segredo em variável de ambiente ainda é segredo em memória e pode aparecer em diagnósticos. Para produção sensível, use cofre de segredos ou mecanismo da plataforma.

---

## Parte 15 — Web, Reverse Proxy e TLS

[⬆️ Voltar ao Sumário](#sumário)

O padrão comum para aplicações web é:

```text
internet
  -> firewall/cloud
  -> Nginx ou Apache
  -> aplicação local em 127.0.0.1:PORTA
  -> banco/cache/fila privados
```

### Nginx como reverse proxy

Instalação:

```bash
sudo apt install nginx
sudo systemctl enable --now nginx
```

Exemplo de site:

```nginx
server {
    listen 80;
    server_name exemplo.com;

    location / {
        proxy_pass http://127.0.0.1:5000;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

Validação:

```bash
sudo nginx -t
sudo systemctl reload nginx
```

### TLS

TLS não é decoração. Ele define autenticação do servidor e criptografia do tráfego.

Checklist:

```text
[ ] DNS aponta para o servidor ou load balancer correto.
[ ] Portas 80 e 443 estão liberadas onde necessário.
[ ] Certificado renova automaticamente.
[ ] Aplicação entende proxy headers se precisar gerar URLs.
[ ] HTTP redireciona para HTTPS.
[ ] Logs de acesso e erro são coletados.
```

Use Certbot, ACME do provedor, load balancer gerenciado ou outra ferramenta adequada à arquitetura. O importante é existir renovação testada.

---

## Parte 16 — Bancos, Filas e Serviços de Estado

[⬆️ Voltar ao Sumário](#sumário)

Serviços de estado exigem mais cuidado que serviços stateless. Um processo web pode ser recriado; um banco sem backup pode perder o sistema.

### Exemplos

| Serviço | Estado crítico |
|---|---|
| PostgreSQL | diretório de dados, WAL, roles, configs, backups |
| MySQL/MariaDB | datadir, binlogs, usuários, configs |
| Redis/Valkey | persistência RDB/AOF, memória, eviction |
| RabbitMQ | filas, mensagens, usuários, cluster metadata |
| MinIO | objetos, credenciais, política, discos |

### Perguntas antes de instalar

1. O serviço será local ou gerenciado fora da máquina?
2. Quais dados precisam de backup consistente?
3. Como restaurar em outro servidor?
4. Qual porta precisa ficar privada?
5. Quem administra usuários e senhas?
6. Há criptografia em trânsito?
7. O disco foi dimensionado para crescimento e logs?
8. Como será monitorado?

### Porta pública quase nunca é a resposta

Banco de dados exposto diretamente na internet é um erro comum. Prefira:

- acesso apenas pela rede privada;
- firewall local e cloud firewall;
- VPN/bastion para administração;
- TLS e autenticação forte quando houver tráfego remoto;
- usuário de aplicação com privilégios mínimos.

---

## Parte 17 — Discos, Partições, LVM e RAID

[⬆️ Voltar ao Sumário](#sumário)

Storage é onde a teoria vira consequência. Antes de mexer, observe.

### Comandos de inventário

```bash
lsblk -f
blkid
findmnt
df -h
du -sh /var/*
pvs
vgs
lvs
```

### Conceitos

| Termo | Ideia |
|---|---|
| disco | dispositivo físico ou virtual |
| partição | divisão do disco |
| filesystem | formato lógico para arquivos |
| mount point | caminho onde filesystem aparece |
| LVM PV | volume físico usado pelo LVM |
| LVM VG | grupo de volumes |
| LVM LV | volume lógico montável |
| RAID | redundância ou combinação de discos |

### LVM

LVM adiciona flexibilidade:

- expandir volume lógico;
- mover dados entre discos;
- organizar volumes por função;
- usar snapshots em alguns cenários.

Custo:

- mais uma camada para diagnosticar;
- recuperação exige entender PV/VG/LV;
- snapshot não substitui backup.

### RAID

RAID ajuda contra falha de disco, mas não protege contra:

- `rm` acidental;
- corrupção lógica;
- ransomware;
- bug de aplicação;
- perda do servidor inteiro;
- erro de migration.

RAID não é backup. Essa frase é curta porque precisa ser lembrada.

---

## Parte 18 — Backup, Restore e Recuperação

[⬆️ Voltar ao Sumário](#sumário)

Backup não é "ter uma cópia". Backup é conseguir restaurar.

### O que salvar

| Categoria | Exemplos |
|---|---|
| configuração | `/etc`, units customizadas, Netplan, SSH, Nginx |
| dados de aplicação | `/srv`, uploads, volumes |
| bancos | dumps, backups físicos, WAL/binlog conforme engine |
| segredos | chaves SSH, certificados, tokens, senhas |
| inventário | pacotes, versão, disco, serviços habilitados |
| automação | Ansible, Terraform, cloud-init, scripts |

### Comandos úteis

Inventário:

```bash
hostnamectl
lsblk -f
findmnt
systemctl list-unit-files --state=enabled
apt-mark showmanual
```

Arquivos:

```bash
sudo rsync -aHAX --numeric-ids /etc/ /backup/etc/
sudo rsync -aHAX --numeric-ids /srv/ /backup/srv/
```

PostgreSQL lógico:

```bash
pg_dump -Fc nome_banco > nome_banco.dump
pg_restore -d nome_banco_restaurado nome_banco.dump
```

### Regra 3-2-1

Modelo simples:

```text
3 cópias
2 mídias ou locais diferentes
1 cópia fora do servidor principal
```

### Restore drill

Faça periodicamente:

1. criar VM limpa;
2. instalar mesma versão base;
3. restaurar configuração e dados;
4. subir serviços;
5. executar teste funcional;
6. medir tempo;
7. documentar lacunas.

Se nunca foi restaurado, é esperança, não backup.

---

## Parte 19 — Sistemas de Arquivos, Mounts e Quotas

[⬆️ Voltar ao Sumário](#sumário)

Ubuntu Server normalmente usa `ext4` em instalações simples, mas pode trabalhar com outros filesystems conforme necessidade.

### `/etc/fstab`

`/etc/fstab` define mounts persistentes.

Exemplo:

```text
UUID=1111-2222 /data ext4 defaults,nofail 0 2
```

Antes de reiniciar:

```bash
sudo findmnt --verify
sudo mount -a
```

`mount -a` ajuda a descobrir erro de sintaxe ou dispositivo ausente antes do reboot.

### Opções úteis

| Opção | Uso |
|---|---|
| `defaults` | conjunto padrão |
| `nofail` | não falha boot se o mount estiver ausente |
| `noexec` | impede execução direta |
| `nodev` | ignora devices |
| `nosuid` | ignora bits setuid/setgid |
| `ro` | somente leitura |

Não aplique opções de hardening mecanicamente. `noexec` em um diretório usado por instalador ou runtime pode quebrar aplicação.

---

## Parte 20 — Hardening e Superfície de Ataque

[⬆️ Voltar ao Sumário](#sumário)

Hardening é reduzir caminhos de ataque sem quebrar a função do servidor.

### Checklist inicial

```text
[ ] Instalei só pacotes necessários.
[ ] Usuários administrativos são nominais.
[ ] Login SSH de root está desativado.
[ ] Senha por SSH está desativada quando chaves forem suficientes.
[ ] UFW libera apenas portas necessárias.
[ ] Serviços escutam em 127.0.0.1 quando não precisam ser públicos.
[ ] Atualizações de segurança estão planejadas.
[ ] Logs de autenticação são revisados.
[ ] Backups são criptografados e testados.
[ ] Segredos não estão em repositório Git.
```

### Ver o que está exposto

```bash
ss -tulpn
sudo ufw status verbose
systemctl --type=service --state=running
```

### Menor privilégio

Aplicações devem rodar como usuário próprio, não como `root`, salvo necessidade clara.

Em units systemd, avalie:

```ini
User=app
Group=app
NoNewPrivileges=true
PrivateTmp=true
ProtectSystem=full
ProtectHome=true
```

Essas opções podem quebrar aplicações que escrevem em locais indevidos. Teste antes.

---

## Parte 21 — Atualizações, Segurança e Ubuntu Pro

[⬆️ Voltar ao Sumário](#sumário)

Atualização é operação contínua, não evento raro.

### Atualização manual

```bash
sudo apt update
apt list --upgradable
sudo apt upgrade
sudo reboot
```

Verificar reboot pendente:

```bash
test -f /var/run/reboot-required && cat /var/run/reboot-required
```

### unattended-upgrades

Ubuntu Server pode aplicar atualizações de segurança automaticamente com `unattended-upgrades`.

Arquivos importantes:

| Arquivo | Papel |
|---|---|
| `/etc/apt/apt.conf.d/50unattended-upgrades` | comportamento da ferramenta |
| `/etc/apt/apt.conf.d/20auto-upgrades` | habilitação e frequência |
| `/var/log/unattended-upgrades` | logs das execuções |

Comandos:

```bash
sudo apt install unattended-upgrades
systemctl status apt-daily-upgrade.timer
```

Trade-off: aplicar atualização automaticamente reduz janela de vulnerabilidade, mas pode introduzir mudança em momento ruim. Sistemas críticos precisam de estratégia: staging, janela de manutenção, monitoramento e rollback.

### Upgrade de release

Atualizar pacotes não é o mesmo que trocar de release.

```bash
sudo do-release-upgrade
```

Para LTS, o caminho conservador é atualizar para a próxima LTS sequencial, normalmente após o primeiro point release. Não trate upgrade de release como comando rotineiro em produção sem snapshot, backup, teste e janela.

---

## Parte 22 — AppArmor, Segredos e Auditoria

[⬆️ Voltar ao Sumário](#sumário)

### AppArmor

AppArmor é um mecanismo de controle de acesso mandatório usado no Ubuntu para confinar programas por perfis.

Comandos:

```bash
sudo aa-status
sudo apt install apparmor-profiles
```

Modos:

| Modo | Significado |
|---|---|
| complain | violações são permitidas e registradas |
| enforce | violações são bloqueadas |

Diagnóstico:

```bash
journalctl -k | grep -i apparmor
```

### Segredos

Segredo é qualquer valor que permite acesso:

- chave SSH privada;
- token de API;
- senha de banco;
- certificado privado;
- arquivo `.env`;
- chave GPG;
- cookie de sessão;
- credencial de cloud.

Regras práticas:

```text
[ ] Não commitar segredos.
[ ] Evitar segredo em linha de comando.
[ ] Restringir leitura com owner/grupo/permissão.
[ ] Rotacionar após exposição.
[ ] Preferir vault/secret manager quando houver escala.
[ ] Registrar quem usa cada segredo.
```

### Auditoria mínima

```bash
last
lastlog
sudo journalctl _COMM=sudo
grep sudo /var/log/auth.log 2>/dev/null
```

Em ambientes maiores, use coleta centralizada de logs e políticas de retenção.

---

## Parte 23 — Upgrades, Mudanças e Runbooks

[⬆️ Voltar ao Sumário](#sumário)

Servidor de produção precisa de mudança controlada.

### Checklist de mudança

```text
[ ] Qual serviço será afetado?
[ ] Qual é o estado atual?
[ ] Existe backup ou snapshot?
[ ] Como validar depois?
[ ] Como voltar atrás?
[ ] Quem precisa ser avisado?
[ ] Qual janela de manutenção?
[ ] Quais logs serão observados?
```

### Runbook curto

Um runbook útil responde:

1. sintoma;
2. impacto;
3. comandos de diagnóstico;
4. ação segura;
5. escalonamento;
6. rollback;
7. validação.

Exemplo:

```text
Serviço web fora

Diagnóstico:
  systemctl status nginx
  journalctl -u nginx -n 100
  ss -tulpn | grep ':80\|:443'
  df -h

Ação:
  sudo nginx -t
  sudo systemctl reload nginx

Rollback:
  restaurar arquivo anterior em /etc/nginx/sites-enabled/
  sudo nginx -t
  sudo systemctl reload nginx
```

Runbook bom é aquele que funciona às 3 da manhã, quando ninguém quer interpretar arquitetura em pânico.

---

## Parte 24 — cloud-init, autoinstall e Servidores Reproduzíveis

[⬆️ Voltar ao Sumário](#sumário)

Automação evita servidores artesanais.

### cloud-init

cloud-init aplica configuração inicial em imagens cloud:

- usuário e chaves SSH;
- pacotes;
- comandos iniciais;
- arquivos;
- hostname;
- rede em alguns ambientes;
- integração com metadados da cloud.

Exemplo:

```yaml
#cloud-config
hostname: srv-lab-01
users:
  - name: admin
    groups: sudo
    shell: /bin/bash
    ssh_authorized_keys:
      - ssh-ed25519 AAAA... admin@notebook
package_update: true
packages:
  - nginx
  - unattended-upgrades
```

### autoinstall

autoinstall automatiza a instalação do Ubuntu Server com Subiquity.

Exemplo didático:

```yaml
#cloud-config
autoinstall:
  version: 1
  locale: pt_BR.UTF-8
  keyboard:
    layout: br
  identity:
    hostname: srv-lab-01
    username: admin
    password: "$y$j9T$HASH_EXEMPLO_SUBSTITUA"
  ssh:
    install-server: true
    allow-pw: false
    authorized-keys:
      - ssh-ed25519 AAAA... admin@notebook
  storage:
    layout:
      name: lvm
  packages:
    - qemu-guest-agent
    - unattended-upgrades
  updates: security
```

Não use senha em texto puro. Gere hash adequado e proteja o arquivo. Um autoinstall com erro pode apagar o disco errado tão eficientemente quanto um humano.

### Infraestrutura reproduzível

Meta:

```text
servidor novo + automação + backup
  -> estado equivalente ao antigo
```

Ferramentas comuns:

- cloud-init;
- autoinstall;
- Ansible;
- Terraform/OpenTofu;
- Packer;
- MAAS;
- scripts idempotentes;
- templates de systemd, Nginx e Netplan.

---

## Parte 25 — Containers, LXD, Docker e Imagens OCI

[⬆️ Voltar ao Sumário](#sumário)

Container não é VM pequena. Container compartilha kernel com o host e isola processos por namespaces, cgroups e políticas.

### LXD

LXD é forte para containers de sistema e VMs gerenciadas.

```bash
sudo snap install lxd
sudo lxd init
lxc launch ubuntu:26.04 teste
lxc exec teste -- bash
```

### Docker

Docker é comum para empacotar aplicações em imagens OCI.

```bash
sudo apt update
sudo apt install docker.io
sudo systemctl enable --now docker
docker ps
```

### Perguntas operacionais

| Tema | Pergunta |
|---|---|
| storage | onde volumes persistentes ficam? |
| rede | qual porta é publicada e por qual interface? |
| logs | quem coleta stdout/stderr? |
| atualização | quem reconstrói e troca imagens? |
| segredos | segredo entra por env, arquivo, vault ou orchestrator? |
| firewall | UFW e regras de container estão coerentes? |
| backup | volumes e bancos têm backup consistente? |

Container facilita empacotamento, mas não elimina administração do host.

---

## Parte 26 — Virtualização com KVM, QEMU e libvirt

[⬆️ Voltar ao Sumário](#sumário)

Ubuntu Server pode ser host de virtualização com KVM/QEMU e libvirt.

### Conceitos

| Peça | Papel |
|---|---|
| KVM | aceleração de virtualização no kernel |
| QEMU | emulação/execução de máquinas virtuais |
| libvirt | API e daemon para gerenciar VMs |
| `virsh` | CLI de administração libvirt |
| cloud image | base rápida para VM com cloud-init |
| bridge | rede que conecta VM à rede externa |

### Instalação básica

```bash
sudo apt update
sudo apt install qemu-system-x86 libvirt-daemon-system libvirt-clients virtinst
systemctl status libvirtd
```

Verificação:

```bash
egrep -c '(vmx|svm)' /proc/cpuinfo
lsmod | grep kvm
virsh list --all
```

### Cuidados

- planeje storage de imagens;
- monitore I/O;
- entenda bridge e firewall;
- evite overcommit sem métrica;
- use snapshots com consciência;
- mantenha backup fora do host.

Host de virtualização concentra risco: se ele cai, várias VMs caem juntas.

---

## Parte 27 — Observabilidade, Performance e Capacidade

[⬆️ Voltar ao Sumário](#sumário)

Observabilidade responde: o que está acontecendo, por quê e com qual impacto?

### Sinais básicos

| Sinal | Comandos |
|---|---|
| CPU | `top`, `htop`, `mpstat` |
| memória | `free -h`, `vmstat` |
| disco | `df -h`, `iostat`, `iotop` |
| rede | `ss -s`, `ip -s link`, `iftop` |
| logs | `journalctl`, `/var/log` |
| serviços | `systemctl`, health checks |

Instalar ferramentas:

```bash
sudo apt install sysstat htop iotop
```

### Métricas, logs e alertas

Um stack comum:

```text
node exporter / telegraf
  -> Prometheus
  -> Alertmanager
  -> Grafana

journald / rsyslog / agent
  -> armazenamento central de logs
```

Alertas devem apontar para ação. "CPU alta" pode ser normal. "fila crescendo e latência acima do SLO" costuma ser melhor.

### Capacidade

Capacidade não é só média:

- pico de CPU;
- memória livre real;
- IOPS e latência de disco;
- conexões simultâneas;
- crescimento de logs;
- tempo de backup;
- tempo de restore;
- janela de atualização.

Servidor bom não é o que nunca falha; é o que falha de forma observável e recuperável.

---

## Parte 28 — Catálogo Prático do Ubuntu Server

[⬆️ Voltar ao Sumário](#sumário)

### 28.1 Comandos por tarefa

| Tarefa | Comandos |
|---|---|
| identificar sistema | `lsb_release -a`, `hostnamectl`, `uname -a` |
| ver boot atual | `journalctl -b`, `systemd-analyze` |
| serviços | `systemctl status`, `systemctl --failed` |
| logs | `journalctl`, `tail -f /var/log/syslog` |
| rede | `ip addr`, `ip route`, `ss -tulpn` |
| DNS | `resolvectl status`, `dig`, `getent hosts` |
| firewall | `ufw status verbose`, `ufw allow`, `ufw delete` |
| pacotes | `apt update`, `apt install`, `apt show`, `dpkg -l` |
| disco | `lsblk -f`, `df -h`, `findmnt`, `du -sh` |
| LVM | `pvs`, `vgs`, `lvs` |
| usuários | `id`, `getent passwd`, `adduser`, `usermod` |
| permissões | `chmod`, `chown`, `stat` |
| processos | `ps aux`, `top`, `pgrep`, `kill` |
| hardware | `lscpu`, `lsmem`, `lspci`, `lsusb` |

### 28.2 Arquivos e diretórios de configuração

| Caminho | Uso |
|---|---|
| `/etc/netplan/*.yaml` | rede declarativa |
| `/etc/ssh/sshd_config` | configuração principal SSH |
| `/etc/ssh/sshd_config.d/*.conf` | snippets SSH |
| `/etc/fstab` | mounts persistentes |
| `/etc/systemd/system/*.service` | units customizadas |
| `/etc/apt/sources.list.d/` | repositórios adicionais |
| `/etc/apt/apt.conf.d/` | configuração APT |
| `/var/log` | logs tradicionais |
| `/var/lib` | estado de serviços empacotados |
| `/srv` | dados de serviços locais, se adotado |

### 28.3 Portas comuns

| Porta | Serviço |
|---:|---|
| 22 | SSH |
| 53 | DNS |
| 80 | HTTP |
| 123 | NTP |
| 443 | HTTPS |
| 5432 | PostgreSQL |
| 3306 | MySQL/MariaDB |
| 6379 | Redis/Valkey |
| 5672 | AMQP/RabbitMQ |
| 9090 | Prometheus, quando exposto localmente |

Porta comum não significa porta segura para internet. Banco e métricas geralmente devem ficar privados.

---

## Parte 29 — Ecossistema, Papéis de Servidor e Ferramentas Externas

[⬆️ Voltar ao Sumário](#sumário)

### 29.1 Papéis comuns

| Papel | Pacotes/ferramentas comuns |
|---|---|
| web server | Nginx, Apache |
| aplicação | systemd, runtime da linguagem, reverse proxy |
| banco relacional | PostgreSQL, MySQL, MariaDB |
| cache | Redis, Valkey, Memcached |
| fila | RabbitMQ, Kafka, NATS |
| arquivos | Samba, NFS |
| VPN | WireGuard, OpenVPN |
| containers | LXD, Docker, containerd |
| virtualização | KVM, QEMU, libvirt |
| observabilidade | Prometheus, Grafana, Telegraf, Logwatch |
| automação | cloud-init, autoinstall, Ansible, MAAS |

### 29.2 Como avaliar uma ferramenta

Antes de instalar:

1. existe pacote oficial no repositório da versão?
2. o pacote vem de Main, Universe, snap ou terceiro?
3. quem atualiza?
4. qual usuário o serviço usa?
5. quais portas abre?
6. onde grava dados?
7. como faz backup?
8. como atualiza sem downtime?
9. como remover?
10. existe documentação oficial da versão?

### 29.3 Antipadrões

| Antipadrão | Risco |
|---|---|
| rodar aplicação como `root` por conveniência | dano amplo após bug ou invasão |
| abrir banco para internet | exploração e vazamento |
| editar config sem backup | rollback difícil |
| desligar firewall para "testar rápido" | exposição esquecida |
| usar script `curl | sudo bash` sem revisão | supply chain opaca |
| confiar em snapshot como backup único | perda junto com host/provedor |
| atualizar release sem teste | indisponibilidade longa |
| ignorar logs até incidente | diagnóstico tardio |

---

## Anexo A — Trilhas Oficiais de Estudo e Prática

[⬆️ Voltar ao Sumário](#sumário)

### Trilha A1 — Primeira semana

1. Instale Ubuntu Server em uma VM.
2. Crie usuário administrativo com `sudo`.
3. Habilite SSH com chave.
4. Configure UFW liberando apenas SSH.
5. Instale Nginx.
6. Leia logs com `journalctl`.
7. Configure IP fixo com Netplan em uma VM descartável.
8. Escreva um pequeno runbook de recuperação de SSH.

### Trilha A2 — Servidor de aplicação

1. Crie usuário de serviço.
2. Publique uma API atrás de Nginx.
3. Crie unit systemd.
4. Configure TLS.
5. Configure logs e rotação.
6. Adicione health check.
7. Faça backup de `/etc`, `/srv` e dados.
8. Restaure em outra VM.

### Trilha A3 — Produção e automação

1. Crie cloud-init para VM.
2. Crie autoinstall de laboratório.
3. Automatize configuração com Ansible.
4. Configure unattended-upgrades conforme política.
5. Adicione Prometheus/Grafana ou stack equivalente.
6. Faça teste de restore.
7. Documente runbooks.
8. Planeje upgrade para próxima LTS.

---

## Anexo B — Referências Oficiais Consultadas

[⬆️ Voltar ao Sumário](#sumário)

### Ubuntu Server e releases

- [Ubuntu Server documentation](https://ubuntu.com/server/docs/)
- [Get Ubuntu Server](https://ubuntu.com/download/server)
- [Ubuntu release cycle](https://ubuntu.com/about/release-cycle)
- [Ubuntu Server how-to guides](https://ubuntu.com/server/docs/how-to/)

### Instalação e automação

- [Ubuntu installation documentation](https://canonical-subiquity.readthedocs-hosted.com/en/latest/)
- [Basic server installation](https://canonical-subiquity.readthedocs-hosted.com/en/latest/howto/basic-server-installation.html)
- [Autoinstall configuration reference](https://ubuntu.com/server/docs/install/autoinstall-reference/)
- [Providing autoinstall configuration](https://canonical-subiquity.readthedocs-hosted.com/en/latest/tutorial/providing-autoinstall.html)

### Rede, SSH e segurança

- [Configuring networks](https://ubuntu.com/server/docs/explanation/networking/configuring-networks/)
- [Netplan documentation](https://netplan.readthedocs.io/)
- [OpenSSH server](https://ubuntu.com/server/docs/how-to/security/openssh-server/)
- [Firewall](https://ubuntu.com/server/docs/how-to/security/firewalls/)
- [Security suggestions](https://ubuntu.com/server/docs/explanation/security/security_suggestions/)
- [AppArmor](https://ubuntu.com/server/docs/how-to/security/apparmor/)

### Pacotes, updates e operação

- [Package management](https://ubuntu.com/server/docs/how-to/software/package-management/)
- [Managing your software](https://ubuntu.com/server/docs/tutorial/managing-software/)
- [Automatic updates](https://ubuntu.com/server/docs/how-to/software/automatic-updates/)
- [Upgrade your release](https://ubuntu.com/server/docs/how-to/software/upgrade-your-release/)
- [Third-party repository usage](https://ubuntu.com/server/docs/explanation/software/third-party-repository-usage/)

### Storage, containers, virtualização e observabilidade

- [Manage logical volumes](https://ubuntu.com/server/docs/how-to/storage/manage-logical-volumes/)
- [Containers](https://ubuntu.com/server/docs/how-to/containers/)
- [Docker for system admins](https://ubuntu.com/server/docs/how-to/containers/docker-for-system-admins/)
- [Virtualisation](https://ubuntu.com/server/docs/how-to/virtualisation/)
- [QEMU](https://ubuntu.com/server/docs/how-to/virtualisation/qemu/)
- [Libvirt](https://ubuntu.com/server/docs/how-to/virtualisation/libvirt/)
- [Set up your LMA stack](https://ubuntu.com/server/docs/how-to/observability/set-up-your-lma-stack/)
- [Install Logwatch](https://ubuntu.com/server/docs/how-to/observability/install-logwatch/)

---

## Glossário

[⬆️ Voltar ao Sumário](#sumário)

| Termo | Definição resumida |
|---|---|
| APT | ferramenta principal para instalar e atualizar pacotes `.deb` no Ubuntu |
| AppArmor | mecanismo de confinamento por perfis usado para restringir programas |
| autoinstall | formato de instalação automatizada do Ubuntu Server via Subiquity |
| backup | cópia recuperável de estado importante |
| cloud image | imagem pronta para cloud/VM, normalmente inicializada com cloud-init |
| cloud-init | ferramenta de inicialização que aplica configuração fornecida pela plataforma |
| deb | formato de pacote Debian usado pelo Ubuntu |
| ESM | Expanded Security Maintenance, cobertura estendida disponível via Ubuntu Pro |
| filesystem | estrutura lógica usada para armazenar arquivos |
| firewall | política que filtra tráfego de rede |
| hostname | nome do sistema na rede e nos logs |
| journal | mecanismo de logs do `systemd` |
| KVM | virtualização acelerada pelo kernel Linux |
| LTS | Long-Term Support, release com ciclo longo de manutenção |
| LVM | camada de gerenciamento flexível de volumes lógicos |
| Netplan | ferramenta declarativa de configuração de rede em YAML |
| OpenSSH | implementação de SSH usada para acesso remoto seguro |
| RAID | técnica de combinar discos para redundância ou desempenho |
| restore | recuperação efetiva a partir de backup |
| runbook | procedimento operacional para diagnóstico, ação e rollback |
| snap | formato de empacotamento com ciclo próprio de distribuição e atualização |
| SSH | protocolo seguro para acesso remoto e transferência |
| Subiquity | instalador moderno do Ubuntu Server |
| systemd | init system e gerenciador de serviços do Ubuntu |
| UFW | Uncomplicated Firewall, interface amigável para firewall local |
| Ubuntu Pro | assinatura Canonical com segurança e suporte adicionais |
| unit | arquivo de configuração do `systemd` para serviço, timer, mount etc. |

---

> **Encerramento:** dominar Ubuntu Server é aprender a transformar uma máquina em serviço confiável. A instalação é só o começo; o valor real está em configurar, atualizar, proteger, observar, automatizar e recuperar o servidor com clareza.
