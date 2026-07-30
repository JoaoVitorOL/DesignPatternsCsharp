# Guia Técnico: NGINX Proxy do Zero ao Avançado

> **Nível:** Zero ao Avançado  
> **Tema:** NGINX como proxy reverso, balanceador HTTP, gateway TCP/UDP e ponto de terminação TLS  
> **Fontes de referência:** apenas documentação oficial do NGINX/F5 NGINX e páginas oficiais `nginx.org`/`docs.nginx.com`  
> **Versões oficiais conferidas em 30/07/2026:** NGINX Open Source mainline `1.31.3` e stable `1.30.4`, conforme página oficial de download  
> **Atualizado em:** 30/07/2026

---

## Nota de Escopo

[Voltar ao Sumário](#sumário)

Este guia trata de **NGINX Proxy** no sentido técnico de configurar o NGINX como:

- proxy reverso HTTP/HTTPS;
- balanceador de carga para aplicações HTTP;
- proxy para WebSocket;
- cache de conteúdo proxied;
- terminador TLS;
- proxy TCP/UDP no contexto `stream`;
- ponto de entrada para serviços internos.

Não é um guia sobre:

- NGINX Proxy Manager;
- ferramentas gráficas de terceiros;
- tutoriais baseados em blogs não oficiais;
- receitas de cloud provider sem documentação oficial do NGINX;
- proxy genérico de navegação corporativa, exceto a seção curta sobre HTTP CONNECT no NGINX Plus.

Sempre que houver diferença entre **NGINX Open Source** e **F5 NGINX Plus**, ela será indicada. O foco principal é NGINX Open Source como proxy reverso.

---

## Prefácio

[Voltar ao Sumário](#sumário)

NGINX é muito usado como porta de entrada de aplicações web porque fica em um ponto privilegiado: recebe a conexão do cliente, decide qual bloco de servidor e qual `location` tratam a requisição, aplica regras de cabeçalho, TLS, cache, compressão, limites, logs e encaminha a requisição para um backend.

Aprender NGINX Proxy não é decorar `proxy_pass`. O que realmente importa é entender a cadeia:

1. cliente abre conexão;
2. NGINX escolhe o `server`;
3. NGINX escolhe o `location`;
4. diretivas herdadas entram em jogo;
5. headers são preservados, removidos ou reescritos;
6. URI pode ser preservada ou remapeada;
7. upstream é escolhido;
8. timeouts, buffering, cache e retries definem o comportamento sob carga e falha;
9. logs revelam o que de fato aconteceu.

O proxy reverso simples cabe em poucas linhas. O proxy reverso confiável exige escolhas explícitas: nome do host repassado, IP real do cliente, limites, TLS, estratégia de reload, upstreams, comportamento em deploy, endpoints de health, observabilidade e tratamento de falhas.

Este guia foi escrito para ser prático sem perder o modelo mental. A documentação oficial do NGINX é a fonte primária para sintaxe, contexto e comportamento das diretivas.

---

## Como usar este guia

[Voltar ao Sumário](#sumário)

Há quatro trilhas úteis:

1. **Trilha iniciante:** leia as Partes 1 a 8 para entender proxy reverso, configuração mínima, `server`, `location`, `proxy_pass` e headers.
2. **Trilha operacional:** leia as Partes 9 a 16 para lidar com TLS, upstream, balanceamento, timeouts, buffering, WebSocket, cache e logs.
3. **Trilha de produção:** leia as Partes 17 a 23 para hardening, real IP, rate limit, deploy seguro, troubleshooting e checklist.
4. **Trilha de consulta:** use as Partes 24 a 26, anexos e glossário quando precisar lembrar diretivas e padrões.

Ao escrever qualquer configuração de proxy, responda:

1. Este bloco pertence a `http`, `server`, `location`, `upstream` ou `stream`?
2. A URI deve ser preservada ou remapeada?
3. O backend precisa receber o `Host` original, um host interno ou um host fixo?
4. O backend precisa saber o IP real do cliente?
5. O tráfego entre NGINX e backend será HTTP, HTTPS, gRPC, FastCGI, uwsgi, SCGI ou TCP/UDP?
6. O que deve acontecer quando o backend demora, cai ou retorna erro?
7. O comportamento desejado é streaming, buffering ou cache?
8. Como a mudança será testada e recarregada sem derrubar conexões?

> **Regra de laboratório:** antes de qualquer reload, rode `nginx -t`. Em produção, trate `reload` como deploy de configuração.

---

<a id="sumário"></a>

## Sumário Geral

### Como o conteúdo está organizado

| Bloco | Partes | Assuntos centrais | Resultado esperado |
|---|---:|---|---|
| 1. Base conceitual | 1-4 | proxy reverso, arquitetura, instalação e configuração | entender o papel do NGINX e localizar diretivas |
| 2. HTTP proxy | 5-10 | `server`, `location`, `proxy_pass`, headers, URI e upstream | configurar proxy reverso previsível |
| 3. Produção HTTP | 11-16 | TLS, WebSocket, timeouts, buffering, cache, logs e erros | operar proxy sob tráfego real |
| 4. Segurança e limites | 17-20 | real IP, rate limit, acesso, headers e corpo da requisição | reduzir risco e diagnosticar abuso |
| 5. TCP/UDP e NGINX Plus | 21-23 | `stream`, PROXY protocol, forward proxy HTTP CONNECT | separar proxy reverso, L4 e recursos comerciais |
| 6. Operação | 24-26 | deploy, troubleshooting, catálogo e checklists | manter configuração auditável |
| 7. Revisão | Anexos | templates, referências e glossário | consultar rapidamente |

### Atalhos por pergunta prática

| Pergunta | Vá para |
|---|---|
| "Como faço o proxy reverso mais simples?" | [Parte 6](#parte-6--proxy-reverso-http-mínimo) |
| "Qual a diferença entre `proxy_pass` com e sem barra?" | [Parte 7](#parte-7--proxy_pass-uri-e-remapeamento) |
| "Como repassar o IP real do cliente?" | [Parte 8](#parte-8--headers-proxy-e-identidade-do-cliente) |
| "Como balancear entre vários backends?" | [Parte 10](#parte-10--upstream-e-balanceamento-http) |
| "Como configurar HTTPS na frente?" | [Parte 11](#parte-11--tls-https-e-terminação-ssl) |
| "Como fazer proxy de WebSocket?" | [Parte 12](#parte-12--websocket-e-conexões-upgrade) |
| "Quando desligar buffering?" | [Parte 14](#parte-14--buffering-streaming-e-upload) |
| "Como cachear respostas do backend?" | [Parte 15](#parte-15--cache-de-proxy) |
| "Como limitar requisições por IP?" | [Parte 18](#parte-18--rate-limit-e-proteção-básica) |
| "Como fazer proxy TCP ou UDP?" | [Parte 21](#parte-21--proxy-tcpudp-com-stream) |
| "NGINX serve como forward proxy?" | [Parte 23](#parte-23--forward-proxy-http-connect-e-nginx-plus) |
| "Como validar antes de publicar?" | [Parte 24](#parte-24--deploy-seguro-reload-e-rollback) |

### Índice detalhado

1. [Introdução e Contextualização](#parte-1--introdução-e-contextualização)
2. [Proxy Reverso, Forward Proxy e Load Balancer](#parte-2--proxy-reverso-forward-proxy-e-load-balancer)
3. [Arquitetura do NGINX](#parte-3--arquitetura-do-nginx)
4. [Instalação, Versões e Comandos Essenciais](#parte-4--instalação-versões-e-comandos-essenciais)
5. [Modelo de Configuração](#parte-5--modelo-de-configuração)
6. [Proxy Reverso HTTP Mínimo](#parte-6--proxy-reverso-http-mínimo)
7. [`proxy_pass`, URI e Remapeamento](#parte-7--proxy_pass-uri-e-remapeamento)
8. [Headers Proxy e Identidade do Cliente](#parte-8--headers-proxy-e-identidade-do-cliente)
9. [Server Blocks, Locations e Roteamento](#parte-9--server-blocks-locations-e-roteamento)
10. [Upstream e Balanceamento HTTP](#parte-10--upstream-e-balanceamento-http)
11. [TLS, HTTPS e Terminação SSL](#parte-11--tls-https-e-terminação-ssl)
12. [WebSocket e Conexões Upgrade](#parte-12--websocket-e-conexões-upgrade)
13. [Timeouts, Retries e Falhas](#parte-13--timeouts-retries-e-falhas)
14. [Buffering, Streaming e Upload](#parte-14--buffering-streaming-e-upload)
15. [Cache de Proxy](#parte-15--cache-de-proxy)
16. [Logs, Métricas e Diagnóstico](#parte-16--logs-métricas-e-diagnóstico)
17. [Real IP, PROXY Protocol e Cadeia de Proxies](#parte-17--real-ip-proxy-protocol-e-cadeia-de-proxies)
18. [Rate Limit e Proteção Básica](#parte-18--rate-limit-e-proteção-básica)
19. [Controle de Acesso e Autenticação](#parte-19--controle-de-acesso-e-autenticação)
20. [Headers de Resposta, Cookies e Redirecionamentos](#parte-20--headers-de-resposta-cookies-e-redirecionamentos)
21. [Proxy TCP/UDP com Stream](#parte-21--proxy-tcpudp-com-stream)
22. [Proxy para Upstreams HTTPS](#parte-22--proxy-para-upstreams-https)
23. [Forward Proxy HTTP CONNECT e NGINX Plus](#parte-23--forward-proxy-http-connect-e-nginx-plus)
24. [Deploy Seguro, Reload e Rollback](#parte-24--deploy-seguro-reload-e-rollback)
25. [Troubleshooting](#parte-25--troubleshooting)
26. [Catálogo Prático de Diretivas](#parte-26--catálogo-prático-de-diretivas)
27. [Anexo A — Templates Prontos](#anexo-a--templates-prontos)
28. [Anexo B — Referências Oficiais Consultadas](#anexo-b--referências-oficiais-consultadas)
29. [Glossário](#glossário)

---

## Parte 1 — Introdução e Contextualização

[Voltar ao Sumário](#sumário)

### 1.1 O que é NGINX Proxy?

No uso mais comum, "NGINX Proxy" significa configurar NGINX como **proxy reverso**: ele fica na frente de uma aplicação e encaminha requisições para servidores internos.

Fluxo:

```text
cliente -> NGINX -> aplicação
```

Exemplo:

```text
navegador -> https://app.exemplo.com -> NGINX -> http://127.0.0.1:3000
```

O cliente conversa com o NGINX. A aplicação conversa com o NGINX. O NGINX vira a borda técnica entre rede pública e serviço interno.

### 1.2 Por que usar NGINX na frente?

NGINX como proxy reverso ajuda a centralizar:

- TLS/HTTPS;
- nomes de domínio;
- roteamento por caminho;
- roteamento por host;
- balanceamento de carga;
- cache;
- compressão;
- logs;
- rate limit;
- controle de acesso;
- integração com backends em portas privadas;
- deploy sem expor diretamente a aplicação.

Sem proxy:

```text
cliente -> aplicação:3000
```

Com proxy:

```text
cliente -> NGINX:443 -> aplicação:3000
```

O segundo desenho permite que a aplicação não saiba lidar com certificado TLS, portas públicas, múltiplos domínios, limites de requisição ou cache.

### 1.3 O que NGINX não corrige sozinho

NGINX não transforma uma aplicação instável em aplicação estável. Ele consegue limitar, isolar e tornar o tráfego mais previsível, mas ainda depende de:

- backend saudável;
- timeouts coerentes;
- logs úteis;
- capacidade de CPU/RAM/rede;
- configuração de sistema operacional;
- certificado válido;
- DNS correto;
- firewall correto;
- deploy controlado.

Um proxy mal configurado pode esconder erros por alguns minutos, mas depois costuma transformá-los em `502`, `504`, cache indevido ou headers incorretos.

---

## Parte 2 — Proxy Reverso, Forward Proxy e Load Balancer

[Voltar ao Sumário](#sumário)

### 2.1 Proxy reverso

Proxy reverso protege e publica servidores.

```text
cliente -> proxy reverso -> servidores internos
```

O cliente acessa `app.exemplo.com`; ele não precisa saber que por trás existem `app1:3000`, `app2:3000` e `app3:3000`.

### 2.2 Forward proxy

Forward proxy representa clientes.

```text
cliente -> forward proxy -> internet
```

É o modelo usado para controlar saída de clientes para recursos externos. Na documentação oficial atual, o HTTP CONNECT forward proxy é recurso documentado para NGINX Plus, usando `tunnel_pass`.

### 2.3 Load balancer

Load balancer distribui tráfego entre backends.

```text
cliente -> NGINX -> backend A
                 -> backend B
                 -> backend C
```

Na configuração HTTP, isso normalmente é feito com `upstream` e `proxy_pass`.

### 2.4 Camadas

NGINX pode atuar em camadas diferentes:

| Camada | Contexto | Exemplo |
|---|---|---|
| HTTP/HTTPS | `http`, `server`, `location` | proxy reverso para API |
| Aplicação especializada | módulos `fastcgi`, `uwsgi`, `grpc` | gateway para PHP-FPM, uWSGI ou gRPC |
| TCP/UDP | `stream` | proxy para banco, SMTP, Redis, DNS |
| Mail proxy | `mail` | proxy IMAP/POP3/SMTP |

Este guia foca em HTTP/HTTPS e inclui uma parte dedicada a `stream`.

---

## Parte 3 — Arquitetura do NGINX

[Voltar ao Sumário](#sumário)

### 3.1 Processo master e workers

O NGINX roda com um processo master e processos worker. O master lê configuração, gerencia workers e aplica sinais como reload. Os workers processam conexões.

Modelo mental:

```text
master
  worker
  worker
  worker
```

Diretivas globais comuns:

```nginx
user nginx;
worker_processes auto;

events {
    worker_connections 1024;
}
```

`worker_processes auto` costuma ser uma base simples para servidores modernos. `worker_connections` define quantas conexões um worker pode abrir, mas o limite real também depende do sistema operacional.

### 3.2 Contextos

A configuração NGINX é hierárquica.

```nginx
main;

events {
    # conexões
}

http {
    upstream app_backend {
        server 127.0.0.1:3000;
    }

    server {
        listen 80;
        server_name app.exemplo.com;

        location / {
            proxy_pass http://app_backend;
        }
    }
}
```

Os contextos mais importantes para proxy HTTP:

| Contexto | Serve para |
|---|---|
| main | diretivas globais |
| `events` | processamento de conexões |
| `http` | configuração HTTP compartilhada |
| `server` | virtual host por porta/nome |
| `location` | regra por URI |
| `upstream` | grupo de servidores backend |

Para proxy TCP/UDP:

```nginx
stream {
    server {
        listen 5432;
        proxy_pass 10.0.0.10:5432;
    }
}
```

### 3.3 Herança de diretivas

Muitas diretivas podem ser definidas em `http`, sobrescritas em `server` e refinadas em `location`.

Exemplo:

```nginx
http {
    proxy_connect_timeout 5s;
    proxy_send_timeout 30s;
    proxy_read_timeout 30s;

    server {
        location /stream/ {
            proxy_read_timeout 1h;
            proxy_pass http://stream_backend;
        }
    }
}
```

Neste exemplo, `/stream/` herda alguns timeouts, mas ajusta o tempo de leitura por causa de respostas longas.

---

## Parte 4 — Instalação, Versões e Comandos Essenciais

[Voltar ao Sumário](#sumário)

### 4.1 Versões oficiais

Na página oficial de download consultada em 30/07/2026:

| Linha | Versão |
|---|---|
| Mainline | `1.31.3` |
| Stable | `1.30.4` |

A documentação oficial descreve:

- **mainline:** versão de desenvolvimento atual, com novos recursos, correções e atualizações frequentes;
- **stable:** linha atualizada com menor frequência, indicada quando há exigência forte de estabilidade.

Para produção, a documentação oficial recomenda usar o repositório oficial do NGINX quando você precisa de versão atualizada e controle de updates.

### 4.2 Instalação simples em Debian/Ubuntu

Para laboratório ou teste:

```bash
sudo apt update -y
sudo apt install nginx
```

Depois:

```bash
nginx -v
nginx -V
```

`nginx -v` mostra a versão. `nginx -V` mostra argumentos de compilação, módulos e caminhos úteis.

### 4.3 Comandos essenciais

```bash
sudo nginx -t
sudo nginx -s reload
sudo nginx -s quit
sudo nginx -s stop
```

Significado:

| Comando | Função |
|---|---|
| `nginx -t` | testa sintaxe e validade da configuração |
| `nginx -s reload` | recarrega configuração |
| `nginx -s quit` | encerramento gracioso |
| `nginx -s stop` | encerramento rápido |

Em sistemas com systemd:

```bash
sudo systemctl status nginx
sudo systemctl reload nginx
sudo systemctl restart nginx
sudo journalctl -u nginx
```

Use `reload` para aplicar configuração sem encerrar abruptamente workers ativos. Use `restart` quando realmente precisar reiniciar o serviço.

### 4.4 Localização de arquivos

Os caminhos variam por pacote e distribuição. Descubra com:

```bash
nginx -V 2>&1
```

Procure por:

- `--conf-path=`;
- `--error-log-path=`;
- `--http-log-path=`.

Em muitos ambientes Linux, você verá algo próximo de:

```text
/etc/nginx/nginx.conf
/var/log/nginx/error.log
/var/log/nginx/access.log
```

Não assuma caminhos sem conferir no pacote instalado.

---

## Parte 5 — Modelo de Configuração

[Voltar ao Sumário](#sumário)

### 5.1 Sintaxe básica

Diretivas simples terminam com `;`.

```nginx
worker_processes auto;
```

Blocos usam `{}`.

```nginx
http {
    server {
        listen 80;
    }
}
```

Comentários começam com `#`.

```nginx
# comentário
```

### 5.2 Arquivo mínimo didático

```nginx
events {
    worker_connections 1024;
}

http {
    server {
        listen 80;
        server_name exemplo.local;

        location / {
            proxy_pass http://127.0.0.1:3000;
        }
    }
}
```

Este arquivo diz:

1. aceite conexões;
2. trate HTTP;
3. escute porta `80`;
4. responda por `exemplo.local`;
5. encaminhe `/` para `127.0.0.1:3000`.

### 5.3 Includes

É comum dividir arquivos:

```nginx
http {
    include /etc/nginx/conf.d/*.conf;
}
```

Em produção, prefira arquivos por aplicação:

```text
/etc/nginx/conf.d/app.conf
/etc/nginx/conf.d/api.conf
/etc/nginx/conf.d/admin.conf
```

O importante é evitar um `nginx.conf` gigante sem fronteiras.

### 5.4 Teste de configuração

Depois de editar:

```bash
sudo nginx -t
```

Se passar:

```bash
sudo nginx -s reload
```

Se falhar, não recarregue. Corrija o erro indicado.

---

## Parte 6 — Proxy Reverso HTTP Mínimo

[Voltar ao Sumário](#sumário)

### 6.1 Backend local

Imagine uma aplicação escutando:

```text
127.0.0.1:3000
```

Configuração NGINX:

```nginx
server {
    listen 80;
    server_name app.exemplo.com;

    location / {
        proxy_pass http://127.0.0.1:3000;
    }
}
```

Isso encaminha requisições recebidas por `app.exemplo.com` ao backend local.

### 6.2 Headers mínimos recomendados

Um proxy reverso costuma precisar preservar informações do cliente e do host.

```nginx
server {
    listen 80;
    server_name app.exemplo.com;

    location / {
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;

        proxy_pass http://127.0.0.1:3000;
    }
}
```

Leitura:

| Header | Ideia |
|---|---|
| `Host` | host público recebido pelo NGINX |
| `X-Real-IP` | IP remoto visto pelo NGINX |
| `X-Forwarded-For` | cadeia de IPs encaminhados |
| `X-Forwarded-Proto` | `http` ou `https` no lado cliente |

### 6.3 O que testar

```bash
curl -I http://app.exemplo.com/
curl -H "Host: app.exemplo.com" http://127.0.0.1/
```

No backend, verifique:

- se o host recebido é o esperado;
- se URLs absolutas geradas usam `https` quando há TLS na frente;
- se IPs aparecem corretamente nos logs;
- se redirecionamentos não apontam para `127.0.0.1:3000`.

---

## Parte 7 — `proxy_pass`, URI e Remapeamento

[Voltar ao Sumário](#sumário)

`proxy_pass` é a diretiva central do proxy HTTP. Ela define protocolo, endereço e, opcionalmente, uma URI.

### 7.1 Sem URI no `proxy_pass`

```nginx
location /api/ {
    proxy_pass http://127.0.0.1:3000;
}
```

Requisição:

```text
/api/users
```

Backend recebe, em termos práticos:

```text
/api/users
```

A URI original é preservada.

### 7.2 Com URI no `proxy_pass`

```nginx
location /api/ {
    proxy_pass http://127.0.0.1:3000/;
}
```

Requisição:

```text
/api/users
```

Backend recebe:

```text
/users
```

A parte que casou com `location /api/` é substituída pela URI do `proxy_pass`.

### 7.3 Remapeamento explícito

```nginx
location /v1/ {
    proxy_pass http://127.0.0.1:3000/api/;
}
```

Requisição:

```text
/v1/users
```

Backend:

```text
/api/users
```

Esse detalhe da barra final é uma das fontes mais comuns de bugs em NGINX.

### 7.4 Regra prática

Use sem URI quando o backend espera o mesmo caminho público:

```nginx
location /api/ {
    proxy_pass http://api_backend;
}
```

Use com URI quando você quer remapear prefixos:

```nginx
location /api/ {
    proxy_pass http://api_backend/;
}
```

### 7.5 `proxy_pass` com upstream

```nginx
upstream api_backend {
    server 127.0.0.1:3000;
    server 127.0.0.1:3001;
}

server {
    listen 80;
    server_name api.exemplo.com;

    location / {
        proxy_pass http://api_backend;
    }
}
```

`api_backend` é um grupo de servidores. O NGINX escolhe um servidor conforme a estratégia de balanceamento.

### 7.6 `proxy_pass` com socket Unix

```nginx
location / {
    proxy_pass http://unix:/run/app.sock:;
}
```

Com URI:

```nginx
location /app/ {
    proxy_pass http://unix:/run/app.sock:/;
}
```

Socket Unix reduz exposição de porta TCP local e pode simplificar comunicação com aplicações no mesmo host.

---

## Parte 8 — Headers Proxy e Identidade do Cliente

[Voltar ao Sumário](#sumário)

### 8.1 O problema

Sem headers adequados, o backend pode acreditar que:

- todo cliente é o NGINX;
- o esquema é `http`, mesmo quando o cliente usou `https`;
- o host é o nome interno do upstream;
- redirects devem apontar para a porta interna.

### 8.2 Conjunto comum

```nginx
proxy_set_header Host $host;
proxy_set_header X-Real-IP $remote_addr;
proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
proxy_set_header X-Forwarded-Proto $scheme;
proxy_set_header X-Forwarded-Host $host;
proxy_set_header X-Forwarded-Port $server_port;
```

Nem toda aplicação precisa de todos, mas estes são comuns em proxies HTTP.

### 8.3 `$host`, `$http_host` e `$proxy_host`

| Variável | Uso típico |
|---|---|
| `$host` | host normalizado da requisição ou nome primário do servidor |
| `$http_host` | valor bruto do header `Host`, se enviado |
| `$proxy_host` | host extraído do `proxy_pass` |

Para preservar o host público, use:

```nginx
proxy_set_header Host $host;
```

Para forçar host interno:

```nginx
proxy_set_header Host backend.internal;
```

Para deixar o padrão do upstream:

```nginx
proxy_set_header Host $proxy_host;
```

### 8.4 Headers vazios

A documentação oficial do `proxy_set_header` indica que header com valor vazio não é passado ao backend.

Exemplo:

```nginx
proxy_set_header Accept-Encoding "";
```

Isso pode ser útil quando você quer impedir compressão no backend para permitir que o NGINX manipule resposta de forma previsível.

### 8.5 Cuidado com confiança

`X-Forwarded-For` pode vir do cliente. Se o NGINX está na borda pública, ele deve construir a cadeia. Se há outro proxy antes dele, configure `real_ip` somente para endereços confiáveis.

Nunca confie cegamente em um header de IP vindo da internet.

---

## Parte 9 — Server Blocks, Locations e Roteamento

[Voltar ao Sumário](#sumário)

### 9.1 `server_name`

`server_name` define quais nomes aquele bloco atende.

```nginx
server {
    listen 80;
    server_name app.exemplo.com;

    location / {
        proxy_pass http://127.0.0.1:3000;
    }
}
```

Você pode ter múltiplos blocos:

```nginx
server {
    listen 80;
    server_name app.exemplo.com;
    location / { proxy_pass http://app_backend; }
}

server {
    listen 80;
    server_name api.exemplo.com;
    location / { proxy_pass http://api_backend; }
}
```

### 9.2 Default server

Use um default explícito para tráfego sem host esperado.

```nginx
server {
    listen 80 default_server;
    server_name _;

    return 444;
}
```

`444` é um código especial do NGINX que fecha a conexão sem resposta HTTP. Use com cuidado: para ambientes internos, um `404` pode ser mais claro.

### 9.3 Locations por caminho

```nginx
location /api/ {
    proxy_pass http://api_backend;
}

location /admin/ {
    proxy_pass http://admin_backend;
}

location / {
    proxy_pass http://frontend_backend;
}
```

O bloco mais específico geralmente deve aparecer antes por legibilidade, mesmo que a regra de seleção do NGINX não seja simplesmente "primeiro bloco vence".

### 9.4 Locations exatos

```nginx
location = /health {
    access_log off;
    return 200 "ok\n";
}
```

Útil para health checks locais.

### 9.5 Locations regex

```nginx
location ~ \.php$ {
    proxy_pass http://legacy_php_gateway;
}
```

Evite regex quando prefixo simples resolve. Regex em roteamento de proxy costuma dificultar manutenção.

---

## Parte 10 — Upstream e Balanceamento HTTP

[Voltar ao Sumário](#sumário)

### 10.1 Grupo básico

```nginx
upstream app_backend {
    server 10.0.0.11:3000;
    server 10.0.0.12:3000;
    server 10.0.0.13:3000;
}

server {
    listen 80;
    server_name app.exemplo.com;

    location / {
        proxy_pass http://app_backend;
    }
}
```

Sem método explícito, o balanceamento HTTP usa round-robin ponderado.

### 10.2 Peso

```nginx
upstream app_backend {
    server 10.0.0.11:3000 weight=3;
    server 10.0.0.12:3000;
}
```

Neste exemplo, `10.0.0.11` recebe mais tráfego que `10.0.0.12`.

### 10.3 `least_conn`

```nginx
upstream app_backend {
    least_conn;

    server 10.0.0.11:3000;
    server 10.0.0.12:3000;
}
```

Ajuda quando algumas requisições demoram mais que outras, pois tende a escolher o backend com menos conexões ativas.

### 10.4 `ip_hash`

```nginx
upstream app_backend {
    ip_hash;

    server 10.0.0.11:3000;
    server 10.0.0.12:3000;
}
```

Usa IP do cliente para tentar manter o mesmo cliente no mesmo backend. Não é substituto ideal para sessão distribuída, mas pode ajudar em sistemas legados.

### 10.5 Falhas passivas

```nginx
upstream app_backend {
    server 10.0.0.11:3000 max_fails=3 fail_timeout=30s;
    server 10.0.0.12:3000 max_fails=3 fail_timeout=30s;
}
```

`max_fails` e `fail_timeout` definem quando um backend é considerado temporariamente indisponível por falhas de comunicação.

### 10.6 Backup

```nginx
upstream app_backend {
    server 10.0.0.11:3000;
    server 10.0.0.12:3000;
    server 10.0.0.99:3000 backup;
}
```

O servidor marcado como `backup` recebe requisições quando os primários estão indisponíveis.

### 10.7 Keepalive

Em versões atuais, conexões keepalive para upstream HTTP usam HTTP/1.1 por padrão desde `1.29.7`, conforme documentação oficial. Ainda assim, muitos ambientes antigos usam configuração explícita:

```nginx
upstream app_backend {
    server 10.0.0.11:3000;
    keepalive 32;
}

server {
    location / {
        proxy_http_version 1.1;
        proxy_set_header Connection "";
        proxy_pass http://app_backend;
    }
}
```

Em versões recentes, essa configuração pode ser redundante, mas é comum em bases existentes.

### 10.8 Health checks

NGINX Open Source possui comportamento de falha passiva por meio de tentativas malsucedidas. Health checks periódicos ativos são documentados como recurso do NGINX Plus em módulos específicos.

Para NGINX Open Source, combine:

- endpoint `/health` na aplicação;
- `max_fails`;
- `fail_timeout`;
- logs;
- monitoramento externo.

---

## Parte 11 — TLS, HTTPS e Terminação SSL

[Voltar ao Sumário](#sumário)

### 11.1 Terminação TLS

Terminar TLS significa:

```text
cliente --HTTPS--> NGINX --HTTP ou HTTPS--> backend
```

Configuração básica:

```nginx
server {
    listen 443 ssl;
    server_name app.exemplo.com;

    ssl_certificate     /etc/nginx/certs/app.exemplo.com.crt;
    ssl_certificate_key /etc/nginx/certs/app.exemplo.com.key;
    ssl_protocols       TLSv1.2 TLSv1.3;
    ssl_ciphers         HIGH:!aNULL:!MD5;

    location / {
        proxy_set_header Host $host;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_pass http://127.0.0.1:3000;
    }
}
```

### 11.2 Redirecionar HTTP para HTTPS

```nginx
server {
    listen 80;
    server_name app.exemplo.com;

    return 301 https://$host$request_uri;
}
```

Use redirecionamento apenas quando o serviço HTTPS já estiver funcionando.

### 11.3 Certificado e chave

O certificado público é enviado ao cliente. A chave privada deve ter acesso restrito e precisa ser legível pelo processo master do NGINX.

Exemplo de permissão conceitual:

```bash
sudo chown root:root /etc/nginx/certs/app.exemplo.com.key
sudo chmod 600 /etc/nginx/certs/app.exemplo.com.key
```

### 11.4 Backend HTTP versus HTTPS

Backend HTTP:

```nginx
proxy_pass http://127.0.0.1:3000;
```

Backend HTTPS:

```nginx
proxy_pass https://backend.internal:8443;
```

Se o backend é HTTPS, leia também a [Parte 22](#parte-22--proxy-para-upstreams-https), porque validação de certificado e SNI podem importar.

---

## Parte 12 — WebSocket e Conexões Upgrade

[Voltar ao Sumário](#sumário)

WebSocket usa upgrade de protocolo. A documentação oficial alerta que `Upgrade` é header hop-by-hop e requer configuração especial no proxy reverso.

### 12.1 Configuração comum

```nginx
map $http_upgrade $connection_upgrade {
    default upgrade;
    ''      close;
}

server {
    listen 80;
    server_name ws.exemplo.com;

    location / {
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection $connection_upgrade;
        proxy_set_header Host $host;

        proxy_pass http://127.0.0.1:3000;
    }
}
```

### 12.2 Timeouts para WebSocket

Conexões WebSocket podem ficar abertas por muito tempo.

```nginx
location /socket/ {
    proxy_http_version 1.1;
    proxy_set_header Upgrade $http_upgrade;
    proxy_set_header Connection $connection_upgrade;
    proxy_read_timeout 1h;

    proxy_pass http://socket_backend;
}
```

### 12.3 Sinais de erro

| Sintoma | Suspeita |
|---|---|
| conexão fecha logo após abrir | `Upgrade`/`Connection` ausentes |
| `400` no backend | backend não entende rota ou headers |
| `502` | upstream indisponível |
| fecha após tempo fixo | `proxy_read_timeout` |

---

## Parte 13 — Timeouts, Retries e Falhas

[Voltar ao Sumário](#sumário)

### 13.1 Timeouts principais

```nginx
proxy_connect_timeout 5s;
proxy_send_timeout 30s;
proxy_read_timeout 30s;
```

| Diretiva | Controla |
|---|---|
| `proxy_connect_timeout` | tempo para estabelecer conexão com backend |
| `proxy_send_timeout` | tempo entre escritas ao backend |
| `proxy_read_timeout` | tempo entre leituras da resposta do backend |

`proxy_read_timeout` não é tempo total da resposta. Ele mede o intervalo entre operações sucessivas de leitura.

### 13.2 Próximo upstream

```nginx
proxy_next_upstream error timeout http_502 http_503 http_504;
proxy_next_upstream_tries 3;
proxy_next_upstream_timeout 10s;
```

Use com cautela. Repetir requisições pode ser perigoso quando métodos não são idempotentes.

### 13.3 POST e retries

Por padrão, requests não idempotentes como `POST` não devem ser repetidos depois que já começaram a ser enviados ao upstream.

Regra operacional:

- `GET` pode tolerar retry em muitos sistemas;
- `POST`, `PATCH`, `LOCK` exigem cuidado;
- pagamentos, criação de pedidos e mutações devem ser idempotentes no backend antes de aceitar retry agressivo.

### 13.4 Erros comuns

| Código | Interpretação comum no proxy |
|---|---|
| `502 Bad Gateway` | NGINX não conseguiu obter resposta válida do upstream |
| `503 Service Unavailable` | serviço indisponível ou sem upstream utilizável |
| `504 Gateway Timeout` | timeout esperando backend |
| `499` | cliente fechou conexão antes da resposta |

`499` é particularmente útil em logs NGINX: muitas vezes aponta cliente, rede, browser, timeout de load balancer anterior ou usuário cancelando requisição.

---

## Parte 14 — Buffering, Streaming e Upload

[Voltar ao Sumário](#sumário)

### 14.1 Buffering de resposta

Por padrão, `proxy_buffering` fica ligado.

```nginx
proxy_buffering on;
```

Com buffering ligado, NGINX lê a resposta do backend o quanto antes e armazena em buffers. Se não couber em memória, pode usar arquivo temporário.

Isso costuma ser bom para:

- proteger backend contra clientes lentos;
- melhorar uso de conexões upstream;
- habilitar cache;
- suavizar entrega.

### 14.2 Streaming

Para streaming, SSE ou respostas que devem chegar imediatamente:

```nginx
location /events/ {
    proxy_buffering off;
    proxy_cache off;
    proxy_read_timeout 1h;

    proxy_pass http://events_backend;
}
```

Com buffering desligado, a resposta é passada ao cliente conforme chega.

### 14.3 Uploads grandes

Request buffering controla o corpo vindo do cliente.

```nginx
location /upload/ {
    client_max_body_size 100m;
    proxy_request_buffering on;
    proxy_pass http://upload_backend;
}
```

Quando `proxy_request_buffering on`, NGINX lê o corpo inteiro antes de enviar ao backend. Quando desligado, transmite conforme recebe, mas reduz a capacidade de trocar para outro upstream depois que começou a enviar.

### 14.4 Arquivos temporários

Se respostas grandes não couberem nos buffers, NGINX pode usar arquivo temporário. Diretivas relevantes:

```nginx
proxy_buffer_size 16k;
proxy_buffers 16 16k;
proxy_busy_buffers_size 32k;
proxy_max_temp_file_size 1024m;
```

Não copie esses valores sem medir. Ajuste por perfil de resposta, memória e I/O.

---

## Parte 15 — Cache de Proxy

[Voltar ao Sumário](#sumário)

### 15.1 Cache básico

No contexto `http`:

```nginx
proxy_cache_path /var/cache/nginx/app
    levels=1:2
    keys_zone=app_cache:10m
    max_size=1g
    inactive=60m
    use_temp_path=off;
```

No `location`:

```nginx
location /assets/ {
    proxy_cache app_cache;
    proxy_cache_valid 200 302 10m;
    proxy_cache_valid 404 1m;

    proxy_pass http://app_backend;
}
```

### 15.2 Cache key

O padrão é próximo de:

```nginx
proxy_cache_key $scheme$proxy_host$uri$is_args$args;
```

Você pode customizar:

```nginx
proxy_cache_key "$host$request_uri";
```

Cache key errada causa vazamento de conteúdo entre usuários. Cuidado especial com:

- cookies;
- Authorization;
- idioma;
- tenant;
- sessão;
- query string;
- conteúdo personalizado.

### 15.3 Bypass e no-cache

```nginx
proxy_cache_bypass $http_authorization;
proxy_no_cache $http_authorization;
```

Diferença:

| Diretiva | Efeito |
|---|---|
| `proxy_cache_bypass` | não lê do cache quando condição é verdadeira |
| `proxy_no_cache` | não grava no cache quando condição é verdadeira |

### 15.4 Stale cache

```nginx
proxy_cache_use_stale error timeout http_500 http_502 http_503 http_504 updating;
proxy_cache_lock on;
```

`proxy_cache_use_stale` pode servir resposta antiga quando o backend falha ou enquanto cache está sendo atualizado, dependendo dos parâmetros.

### 15.5 Header de debug

```nginx
add_header X-Cache-Status $upstream_cache_status always;
```

Valores comuns:

| Valor | Sentido |
|---|---|
| `MISS` | não havia item válido |
| `HIT` | resposta veio do cache |
| `BYPASS` | cache ignorado por condição |
| `EXPIRED` | item expirado exigiu atualização |
| `STALE` | resposta antiga servida |

Não exponha headers de debug sem avaliar segurança e privacidade.

---

## Parte 16 — Logs, Métricas e Diagnóstico

[Voltar ao Sumário](#sumário)

### 16.1 Access log com upstream

```nginx
log_format proxy_main
    '$remote_addr - $remote_user [$time_local] '
    '"$request" $status $body_bytes_sent '
    '"$http_referer" "$http_user_agent" '
    'rt=$request_time '
    'uct=$upstream_connect_time '
    'uht=$upstream_header_time '
    'urt=$upstream_response_time '
    'upstream=$upstream_addr '
    'cache=$upstream_cache_status';

access_log /var/log/nginx/access.log proxy_main;
```

Campos úteis:

| Campo | Ajuda a ver |
|---|---|
| `$request_time` | tempo total percebido pelo NGINX |
| `$upstream_connect_time` | tempo para conectar ao backend |
| `$upstream_header_time` | tempo até primeiro header do backend |
| `$upstream_response_time` | tempo total de resposta upstream |
| `$upstream_addr` | backend escolhido |
| `$upstream_cache_status` | status do cache |

### 16.2 Error log

```nginx
error_log /var/log/nginx/error.log warn;
```

Para diagnóstico temporário:

```nginx
error_log /var/log/nginx/error.log info;
```

Evite deixar debug/verbose ligado sem necessidade.

### 16.3 Diagnóstico com `curl`

```bash
curl -v http://app.exemplo.com/
curl -I http://app.exemplo.com/
curl -H "Host: app.exemplo.com" http://127.0.0.1/
```

Teste backend diretamente:

```bash
curl -v http://127.0.0.1:3000/
```

Se backend direto falha, o problema não é o proxy.

### 16.4 Logs e privacidade

Não registre tokens, cookies sensíveis ou payloads completos sem uma política clara. Proxy é ponto central e pode virar ponto central de vazamento.

---

## Parte 17 — Real IP, PROXY Protocol e Cadeia de Proxies

[Voltar ao Sumário](#sumário)

### 17.1 Quando usar `real_ip`

Use `real_ip` quando há outro proxy confiável antes do NGINX:

```text
cliente -> load balancer -> NGINX -> backend
```

Nesse cenário, `$remote_addr` pode ser o IP do load balancer, não do cliente.

### 17.2 Configuração por header

```nginx
set_real_ip_from 10.0.0.0/8;
real_ip_header X-Forwarded-For;
real_ip_recursive on;
```

Use apenas ranges confiáveis em `set_real_ip_from`.

### 17.3 Passar IP ao backend

Depois de resolver o IP real:

```nginx
proxy_set_header X-Real-IP $remote_addr;
proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
```

### 17.4 PROXY protocol na entrada

Alguns load balancers passam metadados por PROXY protocol.

```nginx
server {
    listen 80 proxy_protocol;

    real_ip_header proxy_protocol;

    location / {
        proxy_pass http://app_backend;
    }
}
```

Só habilite `proxy_protocol` em uma porta quando o emissor realmente fala PROXY protocol. Caso contrário, requisições HTTP comuns podem falhar.

### 17.5 PROXY protocol para upstream em `stream`

No contexto `stream`, `proxy_protocol on` envia PROXY protocol para o servidor proxied.

```nginx
stream {
    server {
        listen 5432;
        proxy_protocol on;
        proxy_pass database_backend;
    }
}
```

O backend precisa entender esse protocolo.

---

## Parte 18 — Rate Limit e Proteção Básica

[Voltar ao Sumário](#sumário)

### 18.1 Limitar taxa por IP

No contexto `http`:

```nginx
limit_req_zone $binary_remote_addr zone=per_ip:10m rate=5r/s;
```

No `server` ou `location`:

```nginx
location /api/ {
    limit_req zone=per_ip burst=20 nodelay;
    proxy_pass http://api_backend;
}
```

### 18.2 Dry run

Para medir antes de bloquear:

```nginx
limit_req_dry_run on;
```

Isso contabiliza excessos sem aplicar bloqueio real.

### 18.3 Limitar métodos

```nginx
location /api/ {
    limit_except GET POST {
        deny all;
    }

    proxy_pass http://api_backend;
}
```

Permitir `GET` também permite `HEAD`.

### 18.4 Limitar tamanho de corpo

```nginx
client_max_body_size 10m;
```

Para upload específico:

```nginx
location /upload/ {
    client_max_body_size 100m;
    proxy_pass http://upload_backend;
}
```

### 18.5 Rate limit não substitui autenticação

Rate limit reduz abuso volumétrico simples. Ele não prova identidade, não resolve autorização e não protege endpoint logicamente vulnerável.

---

## Parte 19 — Controle de Acesso e Autenticação

[Voltar ao Sumário](#sumário)

### 19.1 Allow/Deny por IP

```nginx
location /admin/ {
    allow 10.0.0.0/8;
    allow 192.168.0.0/16;
    deny all;

    proxy_pass http://admin_backend;
}
```

Ordem importa: a primeira regra que casa define o resultado.

### 19.2 Basic auth

```nginx
location /internal/ {
    auth_basic "restricted";
    auth_basic_user_file /etc/nginx/htpasswd;

    proxy_pass http://internal_backend;
}
```

Use basic auth com TLS. Senha básica sem HTTPS é uma péssima ideia.

### 19.3 Autorização por subrequest

```nginx
location /private/ {
    auth_request /auth;
    proxy_pass http://private_backend;
}

location = /auth {
    internal;
    proxy_pass http://auth_backend/check;
    proxy_pass_request_body off;
    proxy_set_header Content-Length "";
    proxy_set_header X-Original-URI $request_uri;
}
```

Se `/auth` retorna `2xx`, acesso permitido. Se retorna `401` ou `403`, acesso negado.

### 19.4 Combinar métodos

```nginx
location /admin/ {
    satisfy all;

    allow 10.0.0.0/8;
    deny all;

    auth_basic "admin";
    auth_basic_user_file /etc/nginx/htpasswd;

    proxy_pass http://admin_backend;
}
```

`satisfy all` exige todas as condições. `satisfy any` permite acesso se qualquer uma permitir.

---

## Parte 20 — Headers de Resposta, Cookies e Redirecionamentos

[Voltar ao Sumário](#sumário)

### 20.1 Adicionar headers

```nginx
add_header X-Proxy "nginx" always;
```

Para headers de segurança:

```nginx
add_header X-Content-Type-Options "nosniff" always;
add_header X-Frame-Options "SAMEORIGIN" always;
```

Avalie cada header conforme a aplicação. Não jogue headers prontos sem entender impacto.

### 20.2 Esconder headers do upstream

```nginx
proxy_hide_header X-Powered-By;
```

Por padrão, NGINX já não passa alguns headers do upstream, como `Date`, `Server`, `X-Pad` e `X-Accel-*`.

### 20.3 Reescrever redirects

Se backend retorna redirect interno:

```text
Location: http://localhost:3000/login
```

Você pode reescrever:

```nginx
proxy_redirect http://localhost:3000/ /;
```

### 20.4 Reescrever cookie domain

```nginx
proxy_cookie_domain localhost app.exemplo.com;
```

### 20.5 Reescrever cookie path

```nginx
proxy_cookie_path / /app/;
```

### 20.6 Flags de cookie

```nginx
proxy_cookie_flags ~ secure httponly samesite=lax;
```

Teste com cuidado. Alterar cookie em proxy pode resolver compatibilidade ou quebrar login.

---

## Parte 21 — Proxy TCP/UDP com Stream

[Voltar ao Sumário](#sumário)

### 21.1 Contexto `stream`

HTTP usa `http`. TCP/UDP usa `stream`.

```nginx
stream {
    server {
        listen 5432;
        proxy_pass 10.0.0.20:5432;
    }
}
```

Isso não inspeciona HTTP. É encaminhamento de fluxo.

### 21.2 TCP com upstream

```nginx
stream {
    upstream postgres_backend {
        server 10.0.0.21:5432;
        server 10.0.0.22:5432 backup;
    }

    server {
        listen 5432;
        proxy_connect_timeout 5s;
        proxy_timeout 10m;
        proxy_pass postgres_backend;
    }
}
```

### 21.3 UDP

```nginx
stream {
    server {
        listen 53 udp reuseport;
        proxy_timeout 20s;
        proxy_pass 10.0.0.53:53;
    }
}
```

UDP é sem conexão, então pense em datagramas e timeout de sessão, não em requisição HTTP.

### 21.4 Quando usar `stream`

Use `stream` para:

- TCP puro;
- UDP;
- TLS passthrough;
- protocolos não HTTP.

Não use `stream` quando você precisa:

- ler URI;
- roteamento por path;
- headers HTTP;
- cache HTTP;
- autenticação HTTP;
- `proxy_set_header`.

Esses recursos pertencem ao contexto `http`.

---

## Parte 22 — Proxy para Upstreams HTTPS

[Voltar ao Sumário](#sumário)

### 22.1 Proxy para backend TLS

```nginx
location / {
    proxy_pass https://backend.internal:8443;
}
```

### 22.2 SNI para upstream

Se o backend HTTPS usa SNI:

```nginx
proxy_ssl_server_name on;
proxy_ssl_name backend.internal;
```

### 22.3 Verificar certificado do upstream

Por padrão, `proxy_ssl_verify` é `off`. Para validar:

```nginx
proxy_ssl_server_name on;
proxy_ssl_name backend.internal;
proxy_ssl_trusted_certificate /etc/nginx/certs/backend-ca.pem;
proxy_ssl_verify on;
proxy_ssl_verify_depth 2;

proxy_pass https://backend.internal:8443;
```

Se você habilita verificação, o certificado precisa bater com o nome esperado e a cadeia precisa ser confiável.

### 22.4 mTLS com upstream

Quando backend exige certificado de cliente:

```nginx
proxy_ssl_certificate     /etc/nginx/certs/proxy-client.crt;
proxy_ssl_certificate_key /etc/nginx/certs/proxy-client.key;
proxy_ssl_server_name on;
proxy_ssl_verify on;
proxy_ssl_trusted_certificate /etc/nginx/certs/backend-ca.pem;

proxy_pass https://backend.internal:8443;
```

### 22.5 Erros típicos

| Erro | Causa provável |
|---|---|
| `certificate verify failed` | CA ausente ou nome incompatível |
| `SSL_do_handshake() failed` | protocolo/cipher/SNI incompatível |
| `502` com backend HTTPS | handshake falhou ou upstream indisponível |

---

## Parte 23 — Forward Proxy HTTP CONNECT e NGINX Plus

[Voltar ao Sumário](#sumário)

### 23.1 O ponto importante

O proxy reverso protege servidores. O forward proxy representa clientes.

Na documentação oficial atual, HTTP CONNECT forward proxy é documentado como recurso do **NGINX Plus**, com `tunnel_pass`.

### 23.2 Exemplo conceitual documentado

```nginx
server {
    listen 10.10.1.11:3128;

    tunnel_pass;
}
```

Esse modelo permite que clientes estabeleçam túneis via método `CONNECT`.

### 23.3 Quando não confundir

Esta configuração:

```nginx
location / {
    proxy_pass http://backend;
}
```

é proxy reverso.

Esta ideia:

```text
cliente -> proxy -> qualquer destino externo
```

é forward proxy.

São problemas diferentes, com riscos diferentes. Um forward proxy aberto para a internet vira abuso rapidamente.

---

## Parte 24 — Deploy Seguro, Reload e Rollback

[Voltar ao Sumário](#sumário)

### 24.1 Fluxo de alteração

1. Edite arquivo novo ou cópia.
2. Rode teste de sintaxe.
3. Confira diff.
4. Recarregue.
5. Teste endpoint público.
6. Observe logs.

Comandos:

```bash
sudo nginx -t
sudo nginx -s reload
curl -I https://app.exemplo.com/
tail -f /var/log/nginx/error.log
```

### 24.2 Rollback simples

Mantenha cópia da configuração anterior:

```bash
sudo cp /etc/nginx/conf.d/app.conf /etc/nginx/conf.d/app.conf.bak
```

Depois de alterar:

```bash
sudo nginx -t
```

Se algo falhar após reload:

```bash
sudo cp /etc/nginx/conf.d/app.conf.bak /etc/nginx/conf.d/app.conf
sudo nginx -t
sudo nginx -s reload
```

### 24.3 Evite alterações opacas

Não edite direto em produção sem:

- controle de versão;
- backup;
- teste de sintaxe;
- janela de rollback;
- logs acompanhados.

### 24.4 Validação operacional

Teste:

```bash
curl -k -I https://app.exemplo.com/
curl -H "Host: app.exemplo.com" http://127.0.0.1/
```

Valide:

- código HTTP;
- headers;
- redirects;
- cookies;
- latência;
- backend escolhido;
- logs;
- certificado;
- cache.

---

## Parte 25 — Troubleshooting

[Voltar ao Sumário](#sumário)

### 25.1 `502 Bad Gateway`

Checklist:

```bash
sudo nginx -t
sudo tail -n 100 /var/log/nginx/error.log
curl -v http://127.0.0.1:3000/
ss -lntp
```

Possíveis causas:

- backend não está escutando;
- porta errada;
- firewall local;
- DNS do upstream falhando;
- resposta inválida do backend;
- TLS upstream mal configurado;
- socket Unix sem permissão.

### 25.2 `504 Gateway Timeout`

Possíveis causas:

- backend lento;
- `proxy_read_timeout` baixo;
- fila no backend;
- banco de dados lento;
- rede instável;
- conexão aceita mas sem resposta.

Teste:

```bash
curl -v http://backend:3000/rota-lenta
```

### 25.3 Redirect errado

Sintoma:

```text
Location: http://127.0.0.1:3000/login
```

Ajustes possíveis:

```nginx
proxy_set_header Host $host;
proxy_set_header X-Forwarded-Proto $scheme;
proxy_redirect http://127.0.0.1:3000/ /;
```

Também pode ser necessário configurar a aplicação para confiar no proxy.

### 25.4 Backend vê IP errado

Use:

```nginx
proxy_set_header X-Real-IP $remote_addr;
proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
```

Se há proxy antes do NGINX:

```nginx
set_real_ip_from 10.0.0.0/8;
real_ip_header X-Forwarded-For;
real_ip_recursive on;
```

### 25.5 WebSocket falha

Verifique:

```nginx
proxy_http_version 1.1;
proxy_set_header Upgrade $http_upgrade;
proxy_set_header Connection $connection_upgrade;
```

E:

```nginx
proxy_read_timeout 1h;
```

### 25.6 Cache servindo resposta errada

Verifique:

- `proxy_cache_key`;
- cookies;
- header `Authorization`;
- query string;
- tenant;
- headers `Vary`;
- regras `proxy_no_cache`;
- regras `proxy_cache_bypass`.

Adicione temporariamente:

```nginx
add_header X-Cache-Status $upstream_cache_status always;
```

### 25.7 Configuração não aplica

Verifique:

```bash
sudo nginx -t
sudo nginx -T
```

`nginx -T` imprime configuração completa carregada, incluindo includes. É excelente para descobrir arquivo não incluído ou bloco duplicado.

---

## Parte 26 — Catálogo Prático de Diretivas

[Voltar ao Sumário](#sumário)

### 26.1 Proxy HTTP

| Diretiva | Contexto | Uso |
|---|---|---|
| `proxy_pass` | `location` | encaminhar para backend HTTP/HTTPS |
| `proxy_set_header` | `http`, `server`, `location` | alterar headers enviados ao backend |
| `proxy_http_version` | `http`, `server`, `location` | definir versão HTTP para upstream |
| `proxy_redirect` | `http`, `server`, `location` | reescrever `Location`/`Refresh` |
| `proxy_hide_header` | `http`, `server`, `location` | ocultar header de resposta upstream |
| `proxy_pass_header` | `http`, `server`, `location` | permitir header normalmente oculto |

### 26.2 Timeouts e falhas

| Diretiva | Uso |
|---|---|
| `proxy_connect_timeout` | timeout de conexão ao backend |
| `proxy_send_timeout` | timeout entre escritas ao backend |
| `proxy_read_timeout` | timeout entre leituras do backend |
| `proxy_next_upstream` | condições para tentar próximo backend |
| `proxy_next_upstream_tries` | limite de tentativas |
| `proxy_next_upstream_timeout` | janela total de retry |

### 26.3 Buffering e corpo

| Diretiva | Uso |
|---|---|
| `proxy_buffering` | buffering de resposta |
| `proxy_buffer_size` | buffer da primeira parte da resposta |
| `proxy_buffers` | buffers por conexão |
| `proxy_busy_buffers_size` | buffers ocupados enviando resposta |
| `proxy_request_buffering` | buffering do corpo da requisição |
| `client_max_body_size` | tamanho máximo do corpo |

### 26.4 Cache

| Diretiva | Uso |
|---|---|
| `proxy_cache_path` | define caminho e zona de cache |
| `proxy_cache` | habilita zona em `location` |
| `proxy_cache_key` | define chave de cache |
| `proxy_cache_valid` | TTL por status |
| `proxy_cache_bypass` | condição para não ler cache |
| `proxy_no_cache` | condição para não gravar cache |
| `proxy_cache_use_stale` | permite cache antigo em falhas |
| `proxy_cache_lock` | reduz múltiplos preenchimentos simultâneos |

### 26.5 Upstream

| Diretiva | Uso |
|---|---|
| `upstream` | grupo de servidores |
| `server` | backend dentro de `upstream` |
| `weight` | peso de backend |
| `max_fails` | falhas antes de marcar indisponível |
| `fail_timeout` | janela de falha e indisponibilidade |
| `backup` | servidor reserva |
| `down` | servidor fora |
| `least_conn` | menor número de conexões |
| `ip_hash` | afinidade por IP |
| `hash` | balanceamento por hash |
| `keepalive` | conexões idle ao upstream |

### 26.6 TLS

| Diretiva | Uso |
|---|---|
| `listen 443 ssl` | servidor HTTPS |
| `ssl_certificate` | certificado público |
| `ssl_certificate_key` | chave privada |
| `ssl_protocols` | versões TLS aceitas |
| `ssl_ciphers` | ciphers aceitos |
| `proxy_ssl_server_name` | SNI para upstream HTTPS |
| `proxy_ssl_name` | nome usado para SNI/verificação |
| `proxy_ssl_verify` | valida certificado do upstream |
| `proxy_ssl_trusted_certificate` | CA para verificar upstream |

### 26.7 Acesso e limites

| Diretiva | Uso |
|---|---|
| `allow`/`deny` | controle por IP |
| `auth_basic` | Basic auth |
| `auth_request` | autorização por subrequest |
| `satisfy` | combinar controles |
| `limit_req_zone` | zona de rate limit |
| `limit_req` | aplica rate limit |
| `limit_req_dry_run` | mede sem bloquear |
| `limit_except` | restringe métodos HTTP |

### 26.8 Stream TCP/UDP

| Diretiva | Contexto | Uso |
|---|---|---|
| `stream` | main | bloco L4 |
| `server` | `stream` | listener TCP/UDP |
| `listen ... udp` | `server` | listener UDP |
| `proxy_pass` | `server` | destino TCP/UDP |
| `proxy_connect_timeout` | `stream`, `server` | timeout de conexão |
| `proxy_timeout` | `stream`, `server` | timeout entre leituras/escritas |
| `proxy_protocol` | `stream`, `server` | envia PROXY protocol ao backend |

---

## Anexo A — Templates Prontos

[Voltar ao Sumário](#sumário)

### A1 — Proxy reverso HTTP simples

```nginx
server {
    listen 80;
    server_name app.exemplo.com;

    location / {
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;

        proxy_pass http://127.0.0.1:3000;
    }
}
```

### A2 — HTTPS na frente, HTTP no backend

```nginx
server {
    listen 80;
    server_name app.exemplo.com;

    return 301 https://$host$request_uri;
}

server {
    listen 443 ssl;
    server_name app.exemplo.com;

    ssl_certificate     /etc/nginx/certs/app.exemplo.com.crt;
    ssl_certificate_key /etc/nginx/certs/app.exemplo.com.key;
    ssl_protocols       TLSv1.2 TLSv1.3;
    ssl_ciphers         HIGH:!aNULL:!MD5;

    location / {
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;

        proxy_pass http://127.0.0.1:3000;
    }
}
```

### A3 — API com upstream e timeouts

```nginx
upstream api_backend {
    least_conn;
    server 10.0.0.11:3000 max_fails=3 fail_timeout=30s;
    server 10.0.0.12:3000 max_fails=3 fail_timeout=30s;
}

server {
    listen 443 ssl;
    server_name api.exemplo.com;

    ssl_certificate     /etc/nginx/certs/api.exemplo.com.crt;
    ssl_certificate_key /etc/nginx/certs/api.exemplo.com.key;

    location / {
        proxy_connect_timeout 5s;
        proxy_send_timeout 30s;
        proxy_read_timeout 30s;
        proxy_next_upstream error timeout http_502 http_503 http_504;
        proxy_next_upstream_tries 2;

        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;

        proxy_pass http://api_backend;
    }
}
```

### A4 — WebSocket

```nginx
map $http_upgrade $connection_upgrade {
    default upgrade;
    ''      close;
}

upstream socket_backend {
    server 127.0.0.1:3000;
}

server {
    listen 443 ssl;
    server_name socket.exemplo.com;

    ssl_certificate     /etc/nginx/certs/socket.exemplo.com.crt;
    ssl_certificate_key /etc/nginx/certs/socket.exemplo.com.key;

    location / {
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection $connection_upgrade;
        proxy_set_header Host $host;
        proxy_read_timeout 1h;

        proxy_pass http://socket_backend;
    }
}
```

### A5 — Cache para assets

```nginx
proxy_cache_path /var/cache/nginx/assets
    levels=1:2
    keys_zone=assets_cache:20m
    max_size=2g
    inactive=60m
    use_temp_path=off;

server {
    listen 80;
    server_name cdn.exemplo.com;

    location /assets/ {
        proxy_cache assets_cache;
        proxy_cache_key "$host$request_uri";
        proxy_cache_valid 200 302 10m;
        proxy_cache_valid 404 1m;
        proxy_cache_lock on;
        add_header X-Cache-Status $upstream_cache_status always;

        proxy_pass http://assets_backend;
    }
}
```

### A6 — Rate limit por IP

```nginx
limit_req_zone $binary_remote_addr zone=per_ip:10m rate=5r/s;

server {
    listen 80;
    server_name api.exemplo.com;

    location / {
        limit_req zone=per_ip burst=20 nodelay;
        proxy_pass http://api_backend;
    }
}
```

### A7 — TCP proxy

```nginx
stream {
    upstream tcp_backend {
        server 10.0.0.11:9000;
        server 10.0.0.12:9000;
    }

    server {
        listen 9000;
        proxy_connect_timeout 5s;
        proxy_timeout 10m;
        proxy_pass tcp_backend;
    }
}
```

---

## Anexo B — Referências Oficiais Consultadas

[Voltar ao Sumário](#sumário)

### Documentação geral e versões

- [NGINX official documentation](https://nginx.org/en/docs/)
- [NGINX download](https://nginx.org/en/download.html)
- [NGINX changelog](https://nginx.org/en/CHANGES)
- [Installing NGINX Open Source](https://docs.nginx.com/nginx/admin-guide/installing-nginx/installing-nginx-open-source/)
- [Beginner's Guide](https://nginx.org/en/docs/beginners_guide.html)

### Proxy HTTP e web server

- [NGINX Reverse Proxy](https://docs.nginx.com/nginx/admin-guide/web-server/reverse-proxy/)
- [Module ngx_http_proxy_module](https://nginx.org/en/docs/http/ngx_http_proxy_module.html)
- [Module ngx_http_core_module](https://nginx.org/en/docs/http/ngx_http_core_module.html)
- [Server names](https://nginx.org/en/docs/http/server_names.html)
- [WebSocket proxying](https://nginx.org/en/docs/http/websocket.html)
- [Module ngx_http_headers_module](https://nginx.org/en/docs/http/ngx_http_headers_module.html)

### Upstream, load balancing e cache

- [Using nginx as HTTP load balancer](https://nginx.org/en/docs/http/load_balancing.html)
- [Module ngx_http_upstream_module](https://nginx.org/en/docs/http/ngx_http_upstream_module.html)
- [NGINX Content Caching](https://docs.nginx.com/nginx/admin-guide/content-cache/content-caching/)

### TLS, segurança e identidade do cliente

- [Configuring HTTPS servers](https://nginx.org/en/docs/http/configuring_https_servers.html)
- [NGINX SSL Termination](https://docs.nginx.com/nginx/admin-guide/security-controls/terminating-ssl-http/)
- [Module ngx_http_ssl_module](https://nginx.org/en/docs/http/ngx_http_ssl_module.html)
- [Module ngx_http_realip_module](https://nginx.org/en/docs/http/ngx_http_realip_module.html)
- [Module ngx_http_limit_req_module](https://nginx.org/en/docs/http/ngx_http_limit_req_module.html)
- [Module ngx_http_access_module](https://nginx.org/en/docs/http/ngx_http_access_module.html)
- [Module ngx_http_auth_basic_module](https://nginx.org/en/docs/http/ngx_http_auth_basic_module.html)
- [Module ngx_http_auth_request_module](https://nginx.org/en/docs/http/ngx_http_auth_request_module.html)

### Logs e stream

- [Module ngx_http_log_module](https://nginx.org/en/docs/http/ngx_http_log_module.html)
- [Module ngx_stream_proxy_module](https://nginx.org/en/docs/stream/ngx_stream_proxy_module.html)
- [Module ngx_stream_upstream_module](https://nginx.org/en/docs/stream/ngx_stream_upstream_module.html)
- [TCP and UDP Load Balancing](https://docs.nginx.com/nginx/admin-guide/load-balancer/tcp-udp-load-balancer/)
- [Accepting the PROXY Protocol](https://docs.nginx.com/nginx/admin-guide/load-balancer/using-proxy-protocol/)

### NGINX Plus

- [Installing NGINX Plus](https://docs.nginx.com/nginx/admin-guide/installing-nginx/installing-nginx-plus/)
- [HTTP CONNECT forward proxy](https://docs.nginx.com/nginx/admin-guide/web-server/http-connect-proxy/)

---

## Glossário

[Voltar ao Sumário](#sumário)

| Termo | Definição resumida |
|---|---|
| access log | log de requisições processadas |
| backend | servidor de aplicação por trás do NGINX |
| buffering | armazenamento temporário de request/response pelo NGINX |
| cache key | chave usada para identificar resposta em cache |
| default server | bloco `server` usado quando não há correspondência melhor |
| forward proxy | proxy que representa clientes acessando destinos externos |
| health check | verificação de saúde de backend |
| host header | header HTTP que indica o host solicitado |
| keepalive | reutilização de conexão |
| location | bloco que seleciona tratamento por URI |
| mainline | linha mais recente de desenvolvimento do NGINX Open Source |
| NGINX Plus | produto comercial da F5 NGINX com recursos adicionais |
| PROXY protocol | protocolo para transportar informações originais de conexão |
| proxy reverso | proxy que representa servidores diante dos clientes |
| `proxy_pass` | diretiva que encaminha requisição ou fluxo ao destino |
| rate limit | limitação de taxa de requisições |
| real IP | IP real do cliente após cadeia de proxies confiáveis |
| reload | recarregamento de configuração sem parada abrupta |
| server block | virtual host NGINX |
| SNI | indicação de nome de servidor durante handshake TLS |
| stable | linha estável do NGINX Open Source |
| stream | contexto NGINX para TCP/UDP |
| upstream | grupo de servidores backend |
| WebSocket | protocolo que usa upgrade de HTTP para conexão persistente |

---

> **Encerramento:** NGINX Proxy é simples na primeira configuração e exigente nos detalhes. Domine `server`, `location`, `proxy_pass`, headers, upstreams, timeouts, buffering, cache e logs. O resto fica bem menos misterioso.
