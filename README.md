# API de Folha de Pagamento

## Introdução

Este projeto tem como objetivo desenvolver uma API RESTful para gerenciar o cadastro de funcionários e o cálculo da folha de pagamento de uma empresa. A aplicação foi desenvolvida utilizando C# com o SDK 8.0 (Minimal API), Entity Framework e SQLite, proporcionando uma solução eficiente e escalável para o cálculo da folha de pagamento.

## Arquitetura

### Visão Geral
[Diagrama de classes ou arquitetura UML]

A aplicação está estruturada em três camadas principais:

* **Apresentação:** Exposição das APIs RESTful utilizando o Minimal API.
* **Negócios:** Contém a lógica de negócio, incluindo os cálculos de folha de pagamento e as regras de negócio da empresa.
* **Dados:** Interação com o banco de dados SQLite utilizando Entity Framework.

### Diagrama de Sequência
[Diagrama de sequência para um dos endpoints, por exemplo, o cadastro de uma folha de pagamento]

## Banco de Dados

* **Nome:** Nome da dupla (ex: joao_pedro)
* **Esquema:**
  * **Funcionário:** Id (PK), Nome, CPF, Cargo, Salário por Hora
  * **Folha de Pagamento:** Id (PK), FuncionárioId (FK), Data, Horas Trabalhadas, Salário Bruto, IR, INSS, FGTS, Salário Líquido

## Endpoints

### Funcionários
* **POST /api/funcionario/cadastrar:** Cadastra um novo funcionário.
  * **Requisição:** JSON com as informações do funcionário.
  * **Resposta:** Status 201 (Criado) se o cadastro for bem-sucedido.
* **GET /api/funcionario/listar:** Lista todos os funcionários cadastrados.
  * **Resposta:** JSON com a lista de funcionários.

### Folhas de Pagamento
* **POST /api/folha/cadastrar:** Cadastra uma nova folha de pagamento para um funcionário.
  * **Requisição:** JSON com as informações da folha de pagamento, incluindo o CPF do funcionário.
  * **Resposta:** Status 201 (Criado) se o cadastro for bem-sucedido, caso contrário, status 404 (Não Encontrado) se o funcionário não existir.
* **GET /api/folha/listar:** Lista todas as folhas de pagamento cadastradas.
  * **Resposta:** JSON com a lista de folhas de pagamento.
* **GET /api/folha/buscar/{cpf}/{mes}/{ano}:** Busca uma folha de pagamento específica por CPF, mês e ano.
  * **Resposta:** JSON com a folha de pagamento encontrada, ou status 404 (Não Encontrado) se não for encontrada.

## Cálculos da Folha de Pagamento

* **Salário Bruto:** Calculado multiplicando o número de horas trabalhadas pelo valor da hora.
* **Imposto de Renda:** Calculado com base na tabela de IR fornecida e aplicado sobre o salário bruto.
* **INSS:** Calculado com base na tabela de INSS fornecida e aplicado sobre o salário bruto.
* **FGTS:** Calculado como 8% do salário bruto.
* **Salário Líquido:** Calculado subtraindo o IR e o INSS do salário bruto.

**Tabelas de Cálculo:**
[Incluir as tabelas de IR e INSS aqui, formatadas em Markdown]

## Testes
* **Unidade:** Testes unitários para as classes de negócio e de acesso a dados.
* **Integração:** Testes de integração para verificar a comunicação entre as diferentes camadas da aplicação.
* **Funcional:** Testes funcionais para garantir que os endpoints da API funcionam conforme o esperado.

## Melhorias Futuras
* **Implementação de outras leis trabalhistas:** Adicionar cálculos para férias, 13º salário, etc.
* **Integração com sistemas de ponto eletrônico:** Automatizar a coleta de horas trabalhadas.
* **Geração de relatórios:** Gerar relatórios personalizados para análise da folha de pagamento.

## Contribuições
* Contribuições são bem-vindas! Abra um pull request com suas alterações.

## Licença
* Este projeto está licenciado sob a licença MIT.
