<!Doctype HTML>
<html>
  <head>
  <h1>Asp.NET-Core-Identity-Parte-1 e Parte -2   </h1> 
    <p>Curso <a href=https://www.udemy.com/course/aspnet-core-identity/> Asp.Net Coire Identity </a>:pushpin:.</p>
  </head>
      <body>
  <h2>Identity com Dapper - MVC (Parte-1) </h2>       
          <p>
          Utilizei métodos CRUD com Dapper, realizei conexão com banco de dados (SQL Server) em servidor doméstico e para além da ministração do curso, pude experimentar
          alterar Views MVC para que os erros de Login fossem apresentados via View.
          </p>
        <h2>Identity EntityFramework Core - MVC (Curso Parte-2) </h2>  
            :heavy_check_mark:Nesta etapa, o aprendizado se deu pela extensibilidade da classe "Users". Onde é possível herdar de IdentityUser e acrescentar campos na                    propriedade. Como o campo "NomeCompleto", por exemplo. 
            </br>:heavy_check_mark: Houve uma nova Migração e Update do Db (excluindo Db que havia em que utilizei micro-ORM Dapper).
            </br> :heavy_check_mark: Houve ainda, alteração de startup para que a injeção de dependência fosse acrescentada pelas classes UsersDbContext (que herda de IdentityDbContext) e Users (que herda de IdentityUser). </p> 
     
</htm>





