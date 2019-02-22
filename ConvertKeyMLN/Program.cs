using System.Collections.Generic;
using System.IO;

namespace ConvertKeyMLN
{
    class Program
    {
        static void Main(string[] args)
        {
            string pathOrigin = @"C:\Users\tanlu\source\repos\ConvertKeyMLN\ConvertKeyMLN\keyMln.txt";
            string pathDestination = @"C:\Users\tanlu\source\repos\ConvertKeyMLN\ConvertKeyMLN\keyMln_Fix1.txt";
            var questions = ConvertKeyMLN.ReadFile(pathOrigin, "^-^");
            using (TextWriter tw = new StreamWriter(pathDestination))
            {
                foreach (var question in questions)
                    tw.WriteLine($"{question.Quest} | {question.Result}");
            }

            ConvertKeyMLN.ConvertFileVietnamese(pathDestination);
        }
    }

    class Question
    {
        public string Quest { get; set; }
        public string Result { get; set; }
    }

    class ConvertKeyMLN
    {
        private static readonly string[] VietNamChar = new string[]
        {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ", 
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };

        public static List<Question> ReadFile(string path, string prefixResult)
        {
            var questions = new List<Question>();
            int counter = 0;
            string line;

            // Read the file and display it line by line.  
            var file = new StreamReader(path);
            var question = new Question();
            var result = string.Empty;
            while ((line = file.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line))
                {
                }
                else if (line.Contains(prefixResult))
                {
                    question.Quest = line;
                    result = line.Substring(0, 1);
                }
                else
                {
                    if (!string.IsNullOrEmpty(result) && line.StartsWith(result))
                    {
                        question.Result = line;
                        result = string.Empty;
                        questions.Add(question);
                        question = new Question();
                    }
                }

                counter++;
            }

            file.Close();

            return questions;
        }

        public static void ConvertFileVietnamese(string path)
        {
            string contents = File.ReadAllText(path);
            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                    contents = contents.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
            }

            File.WriteAllText(path, contents);
        }
    }
}
