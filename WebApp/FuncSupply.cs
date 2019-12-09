using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp
{
    class FuncSupply
    {
        static char[] characters = new char[] { 'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и',
                                                'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с',
                                                'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь',
                                                'э', 'ю', 'я' };
        static int N = characters.Length;
        public static string Encrypt(string input, string keyword)
        {
            char[] m = input.ToLower().ToCharArray();
            char[] k = keyword.ToLower().ToCharArray();

            int keyword_index = 0;
            for (int i = 0; i < m.Length; i++)
            {
                if (characters.Contains(m[i]))
                {
                    int p = (Array.IndexOf(characters, m[i]) +
                        Array.IndexOf(characters, k[keyword_index])) % N;

                    m[i] = characters[p];

                    keyword_index++;

                    if ((keyword_index + 0) == keyword.Length)
                        keyword_index = 0;
                }
            }

            return new string(m);
        }
        public static string Decrypt(string input, string keyword)
        {
            char[] m = input.ToLower().ToCharArray();
            char[] k = keyword.ToLower().ToCharArray();
            int keyword_index = 0;

            for (int i = 0; i < m.Length; i++)
            {
                if (characters.Contains(m[i]))
                {
                    int p = (Array.IndexOf(characters, m[i]) + N -
                        Array.IndexOf(characters, k[keyword_index])) % N;

                    m[i] = characters[p];

                    keyword_index++;

                    if ((keyword_index + 0) == keyword.Length)
                        keyword_index = 0;
                }
            }
            return new string(m);
        }
        public static string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        
        public static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}
