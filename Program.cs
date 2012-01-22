using System;
using System.IO;

namespace SubSync
{
    class Program
    {
        private static string nomeArq;
        private static int offsetSegs, offsetMSegs;
        private static string[] separadores = { " --> " };
        private static string[] separadoresTempo = { ":", "," };

        public static void Main(string[] args)
        {
            obterDados();
            string nomeArq = "leg.srt";
            string nomeSaida = "editado_" + nomeArq;
            StreamReader arq = new StreamReader(nomeArq, System.Text.Encoding.Default);
            StreamWriter saida = File.CreateText(nomeSaida);
            string lido;
            Console.WriteLine("Processando...");
            try
            {
                while (!arq.EndOfStream)
                {
                    lido = arq.ReadLine();
                    saida.WriteLine(lido);
                    lido = arq.ReadLine();
                    saida.WriteLine(processaTempos(lido));
                    lido = arq.ReadLine();
                    saida.WriteLine(lido);
                    lido = arq.ReadLine();
                    saida.WriteLine(lido);
                    if (!lido.Equals(""))
                    {
                        lido = arq.ReadLine();
                        saida.WriteLine(lido);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ocorreu um erro, processo interrompido.\n" + e.Message);
                saida.Close();
                arq.Close();
                return;
            }
            saida.Close();
            arq.Close();
            Console.WriteLine("Processo terminado com sucesso.");
        }

        private static void obterDados()
        {
            Console.WriteLine("SubSync Versão 1.0 por Mateus Azis (Lord_X)");
            Console.Write("Digite o nome do arquivo de legenda: ");
            nomeArq = Console.ReadLine();
            Console.Write("Digite quantos milissegundos deseja avançar (negativo para voltar): ");
            offsetMSegs = Int32.Parse(Console.ReadLine());
            Console.Write("Digite quantos segundos deseja avançar (negativo para voltar): ");
            offsetSegs = Int32.Parse(Console.ReadLine());
        }

        private static string processaTempos(string entrada)
        {
            string [] tempos = entrada.Split(separadores, StringSplitOptions.None);
            return processa(tempos[0]) + " --> " + processa(tempos[1]);
        }

        private static string processa(string entrada)
        {
            string[] tempos = entrada.Split(separadoresTempo, StringSplitOptions.None);
            int [] iTempos = new int[4];
            for(int i = 0; i < 4; i++)
                iTempos[i] = Int32.Parse(tempos[i]);
            iTempos[3] += offsetMSegs;
            if(iTempos[3] >= 1000){
                iTempos[3] %= 1000;
                iTempos[2]++;
            }
            iTempos[2] += offsetSegs;
            if (iTempos[2] >= 60)
            {
                iTempos[2] %= 60;
                iTempos[1]++;
            }
            if (iTempos[1] >= 60)
            {
                iTempos[1] %= 60;
                iTempos[0]++;
            }
            
            return string.Format("{0}:{1}:{2},{3}", formata(iTempos[0], 2) , formata(iTempos[1], 2) , formata(iTempos[2], 2) , formata(iTempos[3], 3));
        }

        private static string formata(int entrada, int casas)
        {
            string resp = entrada + "";
            while (resp.Length < casas)
                resp = "0" + resp;
            return resp;
        }
    }
}
