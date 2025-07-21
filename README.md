# 🔐 ASP.NET Core Auth API

API de autenticação desenvolvida com **ASP.NET Core**, implementando funcionalidades completas para gerenciamento de usuários, incluindo **login**, **registro**, **recuperação de senha por e-mail** e **redefinição com código**.

---

## 📦 Funcionalidades

- ✅ Registro de usuários
- 🔐 Login com senha criptografada (BCrypt)
- 📧 Envio de e-mail para recuperação de senha
- 🔁 Redefinição de senha com código único e data de expiração
- 🧱 Arquitetura em camadas com separação de responsabilidades (Controllers, Services, Repositories, DTOs)

---

## 🛠️ Tecnologias e Ferramentas

- ASP.NET Core 8
- Entity Framework Core
- SQLite (pode ser substituído por outro banco)
- BCrypt.Net (hash de senha)
- FluentValidation (validações)
- MailKit (ou `System.Net.Mail` para envio de e-mails)
- AutoMapper (opcional para DTOs)
- Swagger para documentação de endpoints
