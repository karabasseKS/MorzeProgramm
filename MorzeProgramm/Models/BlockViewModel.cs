namespace MorzeProgramm.Models
{
    public class BlockViewModel
    {
        public int BlockId { get; set; }  // ✅ ДОБАВЛЕНО
        public List<string> Letters { get; set; } = new();

        // Расшифровка в морзе
        public Dictionary<string, string> MorseMap => Letters
            .Where(c => MorseDictionary.ContainsKey(c))
            .ToDictionary(c => c, c => MorseDictionary[c]);

        // Морзе-словарь
        public static readonly Dictionary<string, string> MorseDictionary = new()
        {
            { "А", ".-" }, { "Б", "-..." }, { "В", ".--" }, { "Г", "--." },
            { "Д", "-.." }, { "Е", "." }, { "Ж", "...-" }, { "З", "--.." },
            { "И", ".." }, { "Й", ".---" }, { "К", "-.-" }, { "Л", ".-.." },
            { "М", "--" }, { "Н", "-." }, { "О", "---" }, { "П", ".--." },
            { "Р", ".-." }, { "С", "..." }, { "Т", "-" }, { "У", "..-" },
            { "Ф", "..-." }, { "Х", "...." }, { "Ц", "-.-." }, { "Ч", "---." },
            { "Ш", "----" }, { "Щ", "--.-" }, { "Ь", "-..-" }, { "Ы", "-.--" },
            { "Э", "..-.." }, { "Ю", "..--" }, { "Я", ".-.-" },
            { "1", ".----" }, { "2", "..---" }, { "3", "...--" },
            { "4", "....-" }, { "5", "....." }, { "6", "-...." },
            { "7", "--..." }, { "8", "---.." }, { "9", "----." }, { "0", "-----" }
        };
    }
}
