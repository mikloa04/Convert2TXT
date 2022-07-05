using EpubSharp;
using Spire.Doc;

namespace Convert2TXT
{
    class ConvertTool
    {
        public static string Convert(string FilePath, string OutputFolder, bool bOther)
        {
            string result = "OK";
            string stFileName = System.IO.Path.GetFileName(FilePath);
            string stExt = System.IO.Path.GetExtension(FilePath);
            if (bOther)
            {
                stFileName = OutputFolder + "\\" + stFileName;
            }
            else
            {
                stFileName = FilePath;
            }
            switch(stExt)
            {
                case ".doc":
                    {
                        result = ConvertDoc(stFileName);
                        break;
                    }

                case ".epub":
                    {
                        result = ConvertEpub(stFileName);
                        break;
                    }
                default:
                    result = "File format isn't supported!";
                    break;
            }
            return result;
        }
        private static string ConvertDoc(string FilePath)
        {
            string FileName = FilePath.Trim();
            string ConvertName = FileName.Replace(".doc", ".txt");
            string result = "OK";
            //Create word document
            try
            {
                Document document = new Document();
                document.LoadFromFile(FileName);
                document.SaveToTxt(ConvertName, System.Text.Encoding.UTF8);
            }
            catch
            {
                result = "Error";
            }
            return result;
        }

        private static string ConvertEpub(string FilePath)
        {
            string FileName = FilePath.Trim();
            string ConvertName = FileName.Replace(".epub", ".txt");
            string result = "OK";
            //Create word document
            try
            {
                //Create epub document
                EpubBook book = EpubReader.Read(FileName);
                // Convert to plain text
                string text = book.ToPlainText();
                System.IO.File.WriteAllText(ConvertName, text, System.Text.Encoding.UTF8);
            }
            catch
            {
                result = "Error";
            }
            return result;
        }
    }
}
