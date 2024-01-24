using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        List<Titulo> titulos = new List<Titulo>();

        while (true)
        {
            Console.WriteLine("Escolha uma opção:");
            Console.WriteLine("1. Adicionar título");
            Console.WriteLine("2. Remover título");
            Console.WriteLine("3. Remover todos os títulos");
            Console.WriteLine("4. Simular antecipação de todos os títulos");
            Console.WriteLine("5. Sair");

            int opcao;
            if (int.TryParse(Console.ReadLine(), out opcao))
            {
                switch (opcao)
                {
                    case 1:
                        AdicionarTitulo(titulos);
                        break;
                    case 2:
                        RemoverTitulo(titulos);
                        break;
                    case 3:
                        RemoverTodosTitulos(titulos);
                        break;
                    case 4:
                        SimularAntecipacao(titulos);
                        break;
                    case 5:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }
            }

        }
    }

    static void AdicionarTitulo(List<Titulo> titulos)
    {
        Console.Write("Informe o valor do título: ");
        decimal valor = Convert.ToDecimal(Console.ReadLine());

        Console.Write("Informe a data de vencimento do título (yyyy-MM-dd): ");
        DateTime vencimento = DateTime.Parse(Console.ReadLine());

        // Verifica se o prazo de antecipação é menor ou igual a 90 dias
        if ((vencimento - DateTime.Now).Days > 90)
        {
            Console.WriteLine("O prazo de antecipação não pode ser superior a 90 dias.");
            return;
        }

        Console.Write("Informe a taxa de antecipação mensal: ");
        decimal taxaAntecipacao = Convert.ToDecimal(Console.ReadLine());

        Titulo novoTitulo = new Titulo(valor, vencimento, taxaAntecipacao);
        titulos.Add(novoTitulo);

        Console.WriteLine("Título adicionado com sucesso!");
    }

    static void RemoverTitulo(List<Titulo> titulos)
    {
        if (titulos.Count == 0)
        {
            Console.WriteLine("Nenhum título disponível para remover.");
            return;
        }

        Console.Write("Informe o índice do título a ser removido: ");
        if (int.TryParse(Console.ReadLine(), out int indice))
        {
            if (indice >= 0 && indice < titulos.Count)
            {
                titulos.RemoveAt(indice);
                Console.WriteLine("Título removido com sucesso!");
            }
            else
            {
                Console.WriteLine("Índice inválido.");
            }
        }
        else
        {
            Console.WriteLine("Entrada inválida. Tente novamente.");
        }
    }

    static void RemoverTodosTitulos(List<Titulo> titulos)
    {
        titulos.Clear();
        Console.WriteLine("Todos os títulos foram removidos.");
    }

    static void SimularAntecipacao(List<Titulo> titulos)
    {
        if (titulos.Count == 0)
        {
            Console.WriteLine("Nenhum título disponível para antecipação.");
            return;
        }

        decimal valorTotalAntecipado = 0;
        decimal valorTotalJuros = 0;

        Console.WriteLine("Simulando antecipação de todos os títulos:");

        foreach (Titulo titulo in titulos)
        {
            int diasAntecipacao = (titulo.Vencimento - DateTime.Now).Days;

            decimal valorAntecipado = Math.Round(titulo.CalcularValorAntecipadoDiario(diasAntecipacao), 2);
            decimal juros = Math.Round(titulo.CalcularValorJuros(), 2);

            Console.WriteLine($"Título com vencimento em {titulo.Vencimento}: Valor Antecipado: {valorAntecipado}, Juros: {juros}");

            valorTotalAntecipado += valorAntecipado;
            valorTotalJuros += juros;
        }

        valorTotalAntecipado = Math.Round(valorTotalAntecipado, 2);
        valorTotalJuros = Math.Round(valorTotalJuros, 2);

        Console.WriteLine($"Valor Total Antecipado: {valorTotalAntecipado}, Valor Total de Juros: {valorTotalJuros}");
    }
}

class Titulo
{
    public decimal Valor { get; set; }
    public DateTime Vencimento { get; set; }
    public decimal TaxaAntecipacao { get; set; }

    public Titulo(decimal valor, DateTime vencimento, decimal taxaAntecipacao)
    {
        Valor = valor;
        Vencimento = vencimento;
        TaxaAntecipacao = taxaAntecipacao;
    }

    public decimal CalcularValorAntecipadoDiario(int diasAntecipacao)
    {
        // Lógica para calcular o valor antecipado com base na taxa de antecipação diária
        return Valor - (Valor * (TaxaAntecipacao / 30) / 100) * diasAntecipacao;
    }

    public decimal CalcularValorJuros()
    {
        // Lógica para calcular o valor de juros
        int diasAntecipacao = (Vencimento - DateTime.Now).Days;
        return Valor * (TaxaAntecipacao / 30) * diasAntecipacao / 100;
    }
}