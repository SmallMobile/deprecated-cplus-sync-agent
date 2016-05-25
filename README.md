# cplus-sync-agent

Sincronizador de ordem de serviços e atividades entre o C-Plus e o Field Control

### Download
Realize o [download](https://github.com/FieldControl/cplus-sync-agent/releases/download/v1.2/Release.zip) da última versão do sincronizador no menu [Release](https://github.com/FieldControl/cplus-sync-agent/releases/).

### Instalação

A recomendação é que o sincronizador seja uma tarefa no [Task Scheduler do Windows](https://technet.microsoft.com/en-us/library/cc721931(v=ws.11).aspx) com um intervalo configurado de *20 minutos* ou mais.

### Execução

Os dados sempre serão sincronizados em período de ```7 dias```, sendo que a data de início pode ser especificada via paramêtro.
Caso nenhuma data seja fornecida como paramêtro, serão sincronizados 7 dias a partir da ```data atual``` (valor padrão)

Por exemplo:
  - Dado que hoje é dia ```01/06/2016```, serão sincronizados dados de ```01/06``` até ```07/06```.

### Configurações

É necessario configurar algumas variavéis antes de executar o sicronizador, essas variavéis estão no arquivo ```FieldControl.CPlusSync.ConsoleApp.exe.config``` e são elas:

  - ```logging.folderPath``` - Pasta de destino para os logs do sincronizador
  - ```google.geoCoderKey``` - Google ApiKey para decodificação de endereços (transformar ruas e números em latitude e longitude)
    você pode criar uma no seguinte endereço: `https://console.developers.google.com/`
  - ```fieldcontrol.username``` - Usuário/E-mail da sua Field Control
  - ```fieldcontrol.password``` - ApiKey do Field Control (entre em contato com _integracoes@fieldcontrol.com.br_)
  - ```cplus.connectionString``` - String de conexão do banco de dados do C-Plus

Exemplo:

```
  <appSettings>
    <add key="logging.folderPath" value="C:\temp\" />
    <add key="google.geoCoderKey" value="AIzaSyAtFnN2wIUnUeCzKhbo" />
    <add key="fieldcontrol.baseUrl" value="http://api.fieldcontrol.com.br/" />
    <add key="fieldcontrol.username" value="email@dominio.com.br" />
    <add key="fieldcontrol.password" value="password" />
    <add key="cplus.connectionString" value="User=SYSDBA;Password=masterkey;Database=C:\CPlus\CPlus.FDB;DataSource=localhost;Port=3050;Dialect=3;Charset=NONE;Role =;Connection lifetime=15;Pooling=true;MinPoolSize=0;MaxPoolSize=50;Packet Size=8192;ServerType=0;" />
  </appSettings>
```

### Convenções e conversões

As conversões das informações do C-Plus para o Field Control são todas baseadas em _convenções_.

| C-Plus        | Field Control | Correspondência | Cadastro automático |
| ------------- | ------------- | ---------------: | :------------------: |
| Técnico       | Colaborador |              Nome | Não |
| Serviço/Produto     | Tipo de Atividade |   Nome | Não |
| Status     | Situação |   Nome | Não |
| Cliente       | Cliente     |    Nome | Sim |

Ou seja, essas informações devem ser *exatamente* *iguais* em ambos os sistemas. Um técnico com nome de `Neymar Junior` no C-Plus será correspondido por um Colaborador com nome de `Neymar Junior` no Field Control.

`Técnico` e `Colaborador`, `Serviço/Produto` e `Tipo de Atividade` devem estar previamente cadastrados em ambos os sistemas, caso contrário, a ordem de serviço não será enviada para o Field Control.

Para o dado `Cliente`, caso não exista um cliente com o Nome exatamente _igual_ em ambos os sistemas, o sincronizador criará um novo cliente no Field Control.

O Field Control não permite a criação de situação para as atividades, por isso, é preciso criar e usar os seguintes status no C-Plus:
 - Agendada
 - Em andamento
 - Concluída
 - Reportada como problema
 - Cancelada

### Informações sincronizadas

Atualmente as informações são:

##### C-Plus para Field Control
  - Ordem de serviço para Atividade (Descrição, Data, Técnico, Serviço, Cliente)

Ordens de serviço criadas como `Externa` são enviadas como atividades para o Field Control.

##### Field Control para C-Plus
  - Situação da atividade para status da ordem de serviço

Atividades que tem sua situação alterada no Field Control são atualizadas com seu novo Status no C-Plus.

### Parâmetros

O executável `FieldControl.CPlusSync.ConsoleApp.exe` aceita dois parametros

| Parâmetro        | Valor padrão | Formato | Exemplo |
| ------------- | ------------- | :---------------: | :-------------: |
| Data inicio para sincronização       | Data atual |              yyyy-MM-dd | 2015-05-15 |
| Verbose logging    | false |   true/false | true |

Exemplo de sincronização com dia de inicio específico

```code
  FieldControl.CPlusSync.ConsoleApp.exe 2015-05-15 true
```
