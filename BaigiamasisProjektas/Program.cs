using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaigiamasisProjektas
{
    class Program
    {
        static void Main(string[] args)
        {
			Console.InputEncoding = Encoding.Unicode;
			Console.OutputEncoding = Encoding.Unicode;

			var darZaidziama = true;

			//Pradnis konsolės langas
			Console.ForegroundColor = ConsoleColor.DarkGreen;
			Console.WriteLine("-----------------------");
			Console.WriteLine("     Tic Tac Toe!");
			Console.WriteLine("-----------------------\n");
			Console.ResetColor();

			while (darZaidziama)
			{
				Console.WriteLine("Ką norite daryti?:");
				Console.WriteLine("1. Pradėti žaidimą");
				Console.WriteLine("2. Išeiti\n");

				Console.Write("Pasirinkite skaičiu ir spauskite <enter>: ");

				var pasirinkimas = GetUserInput("[12]");

				switch (pasirinkimas)
				{
					case "1":
						PlayGame();
						Console.Clear();
						break;
					case "2":
						darZaidziama = false;
						break;
				}
			}
		}

		// Vartojo ivestis konsoleje metodas
		private static string GetUserInput(string validPattern = null)
		{
			var ivestis = Console.ReadLine();
			ivestis = ivestis.Trim();
			//Tikrina ar vartotojas iveda skaiciu o ne simboli
			if (validPattern != null && !System.Text.RegularExpressions.Regex.IsMatch(ivestis, validPattern))
			{
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.WriteLine("\"" + ivestis + "\" netinkama ivestis.\n");
				Console.ResetColor();
				return null;
			}

			return ivestis;
		}

		// Pradedamas zaidimas
		private static void PlayGame()
		{
			string skaiElitesPasirinkimas = null;
			while (skaiElitesPasirinkimas == null)
			{
				Console.Write("Su kiek eilučių norite žaisit? (3, 4, ar 5) ");
				skaiElitesPasirinkimas = GetUserInput("[345]");
			}
			var lentosDydis = (int)Math.Pow(int.Parse(skaiElitesPasirinkimas), 2);
			var lenta = new string[lentosDydis];

			var turn = "X";
			while (true)
			{
				Console.Clear();

				var winner = WhoWins(lenta);
				if (winner != null)
				{
					AnnounceResult(winner[0] + " LAIMĖJO!!!", lenta);
					break;
				}
				if (IsBoardFull(lenta))
				{
					AnnounceResult("Lygiosios!", lenta);
					break;
				}

				Console.WriteLine("Dėkite savo " + turn + ":");

				DrawBoard(lenta);

				Console.WriteLine("\nNaudokite rodyklės ir spauskite <enter>");

				var xoLoc = GetXOLocation(lenta);
				lenta[xoLoc] = turn;

				turn = turn == "X" ? "O" : "X";
			}
		}

		//Pranesimas rezultatui
		private static void AnnounceResult(string zinute, string[] lenta)
		{
			Console.WriteLine();
			DrawBoard(lenta);

			Console.ForegroundColor = ConsoleColor.DarkGreen;
			Console.WriteLine(zinute);
			Console.ResetColor();

			Console.Write("\nPress any key to continue...");
			Console.CursorVisible = false;
			Console.ReadKey();
			Console.CursorVisible = true;
		}

		// X ir O lokacija ir navigacija
		private static int GetXOLocation(string[] lenta)
		{
			int numRows = (int)Math.Sqrt(lenta.Length);

			int curRow = 0, curCol = 0;

			for (int i = 0; i < lenta.Length; i++)
			{
				if (lenta[i] == null)
				{
					curRow = i / numRows;
					curCol = i % numRows;
					break;
				}
			}

			while (true)
			{
				// SetCursorPosition numato kurioje pozicijoje bus nurodoma pele ten atisras ir pasirinkimas
				Console.SetCursorPosition(curCol * 4 + 2, curRow * 4 + 3);
				var keyInfo = Console.ReadKey();
				Console.SetCursorPosition(curCol * 4 + 2, curRow * 4 + 3);
				Console.Write(lenta[curRow * numRows + curCol] ?? " ");

				switch (keyInfo.Key)
				{
					case ConsoleKey.LeftArrow:
						if (curCol > 0)
							curCol--;
						break;
					case ConsoleKey.RightArrow:
						if (curCol + 1 < numRows)
							curCol++;
						break;
					case ConsoleKey.UpArrow:
						if (curRow > 0)
							curRow--;
						break;
					case ConsoleKey.DownArrow:
						if (curRow + 1 < numRows)
							curRow++;
						break;
					case ConsoleKey.Spacebar:
					case ConsoleKey.Enter:
						if (lenta[curRow * numRows + curCol] == null)
							return curRow * numRows + curCol;
						break;
				}
			}
		}

		private static void DrawBoard(string[] board)
		{
			var numRows = (int)Math.Sqrt(board.Length);

			Console.WriteLine();

			for (int row = 0; row < numRows; row++)
			{
				if (row != 0)
					Console.WriteLine(" " + string.Join("|", Enumerable.Repeat("---", numRows)));

				Console.Write(" " + string.Join("|", Enumerable.Repeat("   ", numRows)) + "\n ");

				for (int col = 0; col < numRows; col++)
				{
					if (col != 0)
						Console.Write("|");
					var space = board[row * numRows + col] ?? " ";
					if (space.Length > 1)
						Console.ForegroundColor = ConsoleColor.DarkGreen;
					Console.Write(" " + space[0] + " ");
					Console.ResetColor();
				}

				Console.WriteLine("\n " + string.Join("|", Enumerable.Repeat("   ", numRows)));
			}

			Console.WriteLine();
		}

		private static bool IsBoardFull(IEnumerable<string> board)
		{
			return board.All(space => space != null);
		}

		private static string WhoWins(string[] board)
		{
			var numRows = (int)Math.Sqrt(board.Length);

			// Tikrina eilutes
			for (int row = 0; row < numRows; row++)
			{
				if (board[row * numRows] != null)
				{
					bool hasTicTacToe = true;
					for (int col = 1; col < numRows && hasTicTacToe; col++)
					{
						if (board[row * numRows + col] != board[row * numRows])
							hasTicTacToe = false;
					}
					if (hasTicTacToe)
					{
						// Į lentelę įdėkite indikatorių, kad sužinotumėte, kurie yra tikimojo tako dalis
						for (int col = 0; col < numRows; col++)
							board[row * numRows + col] += "!";
						return board[row * numRows];
					}
				}
			}

			// Tikrina stulpelius
			for (int col = 0; col < numRows; col++)
			{
				if (board[col] != null)
				{
					bool hasTicTacToe = true;
					for (int row = 1; row < numRows && hasTicTacToe; row++)
					{
						if (board[row * numRows + col] != board[col])
							hasTicTacToe = false;
					}
					if (hasTicTacToe)
					{
						// Į lentelę įdėkite indikatorių, kad sužinotumėte, kurie yra tikimojo tako dalis
						for (int row = 0; row < numRows; row++)
							board[row * numRows + col] += "!";
						return board[col];
					}
				}
			}

			// Patikrinkite viršutinę kairę -> apatinę dešinę įstrižainę
			if (board[0] != null)
			{
				bool hasTicTacToe = true;
				for (int row = 1; row < numRows && hasTicTacToe; row++)
				{
					if (board[row * numRows + row] != board[0])
						hasTicTacToe = false;
				}
				if (hasTicTacToe)
				{
					// Į lentelę įdėkite indikatorių, kad sužinotumėte, kurie yra tikimojo tako dalis
					for (int row = 0; row < numRows; row++)
						board[row * numRows + row] += "!";
					return board[0];
				}
			}

			// Patikrinkite viršutinę dešinę -> apatinę kairę įstrižainę
			if (board[numRows - 1] != null)
			{
				bool hasTicTacToe = true;
				for (int row = 1; row < numRows && hasTicTacToe; row++)
				{
					if (board[row * numRows + (numRows - 1 - row)] != board[numRows - 1])
						hasTicTacToe = false;
				}
				if (hasTicTacToe)
				{
					// Į lentelę įdėkite indikatorių, kad sužinotumėte, kurie yra tikimojo tako dalis
					for (int row = 0; row < numRows; row++)
						board[row * numRows + (numRows - 1 - row)] += "!";
					return board[numRows - 1];
				}
			}

			return null;
		}
	}
}
