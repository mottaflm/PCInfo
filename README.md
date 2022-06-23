# PCInfo

PCInfo é um Programa Winforms em C# (.NET Framework) para facilitar a análise e suporte ao cliente que está em uma máquina conectada à mesma rede (Active Directory).

![image](https://user-images.githubusercontent.com/21315200/175323930-33ad3d5a-f041-4393-a900-e602e3b84997.png)

Para realizar o acesso ao projeto basta realizar o clone: "git clone https://github.com/mottaflm/PCInfo.git NOME_PASTA", executar o .sln e compilar/executar o projeto (F5).

<h3>Requisitos</h3>

- Git para clone do projeto (Caso prefira, é possível baixar a solução em .zip)
- Visual Studio 2019 (ou compatível) com Módulos: 
- Possuir diretriz administrativa no computador alvo
   
<h3>Funcionalidades</h3>

- Buscar por dados do computador em mesma rede informando o nome ou ip do mesmo

  ![image](https://user-images.githubusercontent.com/21315200/175325222-3f0da5e9-82c2-442f-9e91-281fde931b86.png)
  
  - Os dados buscados são compostos em:
    1. Horário da Consulta
    2. Usuário logado
    3. Tempo de atividade
    4. Uso de Recursos (CPU, Memoria, Disco e Rede)
    5. Informações da Rede (IP e MAC)
    6. Dados da Placa Mãe (Fabricante, Modelo, ServiceTag)
    7. Dados do SO (Nome, Versão, Data de Instalação, Usuários Locais)

- Reinicialização Remota por Tempo

![image](https://user-images.githubusercontent.com/21315200/175327026-b04d584d-b5c4-4dee-a681-997471cd359d.png)

Mensagem de alerta ao usuário do computador alvo:

![image](https://user-images.githubusercontent.com/21315200/175327282-8b2e4eb7-baa6-4ab0-b3f8-ec87ebf5b64e.png)

- Assistência Remota

![image](https://user-images.githubusercontent.com/21315200/175327633-c1230253-7d22-462b-9030-3b1af8690a13.png)

![image](https://user-images.githubusercontent.com/21315200/175327532-b790cf45-5865-47f9-99e3-e39b11558a8a.png)

- Abertura de Tickets (Redireciona para o site desejado)




